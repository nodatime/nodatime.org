﻿// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NodaTime.Web.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NodaTime.Web.Services
{
    public class MarkdownLoader
    {
        private const string NodaTypePrefix = "noda-type://";
        private const string SerializationNodaTypePrefix = "noda-type://NodaTime.Serialization";
        private const string NodaPropertyPrefix = "noda-property://";
        private const string NodaNamespacePrefix = "noda-ns://";
        private static readonly Regex IssuePlaceholderPattern = new Regex(@"^issue \d+$");

        private readonly IFileProvider fileProvider;
        private readonly CommonMarkSettings commonMarkSettings;
        private readonly Dictionary<string, MarkdownBundle> bundles;

        public MarkdownLoader(IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            fileProvider = environment.ContentRootFileProvider;
            commonMarkSettings = CommonMarkSettings.Default.Clone();
            bundles = new Dictionary<string, MarkdownBundle>();

            // Lets us resolve numbers to issue links.
            commonMarkSettings.AdditionalFeatures = CommonMarkAdditionalFeatures.PlaceholderBracket;
            commonMarkSettings.OutputDelegate = FormatDocument;
            // May become obsolete, if we can resolve [NodaTime.LocalDateTime] to the right type link etc.
            commonMarkSettings.UriResolver = ResolveUrl;

            var logger = loggerFactory.CreateLogger<MarkdownLoader>();
            var stopwatch = Stopwatch.StartNew();
            // TODO: Make the root location configurable
            LoadBundleMetadata("Markdown");
            PopulateParentBundles();
            LoadBundleContent();
            
            var totalPages = bundles.Values.SelectMany(b => b.Categories).Sum(c => c.Pages.Count);
            logger.LogInformation("Loaded {bundleCount} bundles totalling {pageCount} pages in {durationMs}ms",
                bundles.Count, totalPages, stopwatch.ElapsedMilliseconds);
        }

        private void LoadBundleMetadata(string directory)
        {
            IDirectoryContents rootContents = fileProvider.GetDirectoryContents(directory);
            foreach (var fileInfo in rootContents)
            {
                LoadBundleMetadata($"{directory}/{fileInfo.Name}");
            }
            var index = fileProvider.GetFileInfo($"{directory}/index.json");
            if (index.Exists)
            {
                var bundle = JsonConvert.DeserializeObject<MarkdownBundle>(index.ReadAllText())!;
                bundle.ContentDirectory = directory;
                bundles.Add(bundle.Name, bundle);
            }
        }

        private void PopulateParentBundles()
        {
            foreach (var bundle in bundles.Values)
            {
                string parentName = bundle.Parent;
                if (parentName != null)
                {
                    if (!bundles.TryGetValue(parentName, out var parentBundle))
                    {
                        throw new Exception($"Invalid parent {parentName} for bundle {bundle.Name}");
                    }
                    bundle.ParentBundle = parentBundle;
                }
            }
        }

        private void LoadBundleContent()
        {
            // TODO: Simpler way of expressing this "work out a tree" code.
            var loadedBundleNames = new HashSet<string>();
            var remainingBundles = bundles.Values.ToList();
            // On each iteration, load everything which either has no parent, or
            // whose parent has already been loaded.
            while (remainingBundles.Count > 0)
            {
                var bundlesToLoadThisIteration = remainingBundles
                    .Where(b => b.Parent == null || loadedBundleNames.Contains(b.Parent))
                    .ToList();
                if (bundlesToLoadThisIteration.Count == 0)
                {
                    string bundleNames = string.Join(", ", remainingBundles.Select(b => b.Name));
                    throw new Exception($"Unable to make progress loading bundles. Remaining bundles: {bundleNames}");
                }
                foreach (var bundle in bundlesToLoadThisIteration)
                {
                    bundle.LoadContent(fileProvider, commonMarkSettings);
                    remainingBundles.Remove(bundle);
                    loadedBundleNames.Add(bundle.Name);
                }
            }
        }

        private static string ResolveUrl(string url)
        {
            if (url.StartsWith(SerializationNodaTypePrefix))
            {
                var (type, anchor) = SplitAnchorUrl(NodaTypePrefix, url);
                return $"/serialization/api/{type}.html{anchor}";
            }
            if (url.StartsWith(NodaTypePrefix))
            {
                var (type, anchor) = SplitAnchorUrl(NodaTypePrefix, url);
                return $"../api/{type}.html{anchor}";
            }
            else if (url.StartsWith(NodaNamespacePrefix))
            {
                var (ns, anchor) = SplitAnchorUrl(NodaNamespacePrefix, url);
                return $"../api/{ns}.html{anchor}";
            }
            else if (url.StartsWith(NodaPropertyPrefix))
            {
                url = url.Substring(NodaPropertyPrefix.Length);
                int nameIndex = url.LastIndexOf('.');
                string type = url.Substring(0, nameIndex);
                return $"../api/{type}.html#{url.Replace(".", "_")}";
            }

            (string, string) SplitAnchorUrl(string prefix, string url)
            {
                url = url.Substring(prefix.Length);
                int hashIndex = url.IndexOf('#');
                string type = hashIndex < 0 ? url : url.Substring(0, hashIndex);
                string anchor = hashIndex < 0 ? "" : url.Substring(hashIndex);
                return (type, anchor);
            }
            return url;
        }

        public MarkdownBundle? TryGetBundle(string bundle)
        {
            bundles.TryGetValue(bundle, out var ret);
            return ret;
        }

        private string ResolvePlaceholder(string placeholder)
        {
            if (IssuePlaceholderPattern.IsMatch(placeholder))
            {
                string url = placeholder.Replace("issue ", "https://github.com/nodatime/nodatime/issues/");
                return $"<a href=\"{url}\">{placeholder}</a>";
            }
            throw new Exception($"Unhandled placeholder: '{placeholder}'");
        }

        private void FormatDocument(Block block, TextWriter writer, CommonMarkSettings settings)
        {
            var formatter = new HtmlFormatter(writer, settings) { PlaceholderResolver = ResolvePlaceholder };
            formatter.WriteDocument(block);
        }
    }

    internal static class FileInfoExtensions
    {
        internal static TextReader CreateReader(this IFileInfo file) => new StreamReader(file.CreateReadStream());
        internal static string ReadAllText(this IFileInfo file)
        {
            using (var reader = file.CreateReader())
            {
                return reader.ReadToEnd();
            }
        }
    }
}
