using DocfxYamlLoader;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace DocfxAnnotationGenerator
{
    class Program
    {
        private readonly string root;
        private readonly string package;
        private readonly IReadOnlyList<string> versions;
        private readonly Dictionary<string, IImmutableList<BuildAssembly>> reflectionDataByVersion;
        private readonly IReadOnlyList<Release> releases;

        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Arguments: <package directories>");
                Console.WriteLine("The package directories are expected to each multiple versions, with the nuget package within each version directory");
                return 1;
            }

            foreach (var package in args)
            {
                Console.WriteLine($"Annotating {Path.GetFileName(package)}");
                var instance = new Program(package);
                /* TODO: Reinstate this once we've fixed the Json.NET package docs; just do it for unstable though.
                if (!instance.CheckMembers())
                {
                    return 1;
                }*/
                instance.CreateDirectories();
                instance.WriteSinceAnnotations();
                instance.WriteAvailabilityAnnotations();
                instance.ModifyYamlFiles();
            }

            return 0;
        }

        private Program(string root)
        {
            this.root = root;
            package = Path.GetFileName(root);
            versions = Directory.GetDirectories(root).Select(dir => Path.GetRelativePath(root, dir)).ToList();
            var packageName = Path.GetFileName(root);

            Console.WriteLine("Loading docfx metadata");
            releases = versions.Select(v => Release.Load(Path.Combine(root, v, "api"), v)).ToList();
            Console.WriteLine("Loading assemblies");
            reflectionDataByVersion = versions
                .ToDictionary(v => v, v => NuGetPackage.Load(Directory.GetFiles(Path.Combine(root, v), "*.nupkg").Single()).Assemblies);
        }

        private bool CheckMembers()
        {
            // TODO: Either fix these in historical versions, or at least stop ignoring them from now on.
            var expectedMissingUids = new[]
            {
                "NodaTime.TimeZones.BclDateTimeZoneSource.#ctor",
                "NodaTime.Text.CompositePatternBuilder`1.#ctor",
                "NodaTime.Serialization.JsonNet.NodaConverterBase`1.#ctor",

            };

            bool result = true;
            foreach (var release in releases)
            {
                var reflectionData = reflectionDataByVersion[release.Version];
                var missing = reflectionData
                    .SelectMany(rd => rd.Members)
                    .Select(member => member.DocfxUid)
                    .Where(uid => !release.MembersByUid.ContainsKey(uid))
                    .Where(uid => !expectedMissingUids.Contains(uid))
                    .Distinct()
                    .ToList();
                if (missing.Count != 0)
                {
                    Console.WriteLine($"Release {release.Version} is missing members in docfx:");
                    foreach (var uid in missing)
                    {
                        Console.WriteLine($"  {uid}");
                    }
                    result = false;
                }
            }
            return result;
        }

        private void CreateDirectories()
        {
            foreach (var release in releases)
            {
                Directory.CreateDirectory(GetOverwriteDirectory(release));
            }
        }

        private void WriteAvailabilityAnnotations()
        {
            Console.WriteLine("Generating 'availability' annotations");
            foreach (var release in releases)
            {
                var assemblies = reflectionDataByVersion[release.Version];
                // I'm sure there's a cleaner way of doing this, but it should work...
                var frameworksByUid = 
                    assemblies.SelectMany(asm => asm.Members.Select(mem => new { Uid = mem.DocfxUid, Framework = asm.TargetFramework }))
                              .ToLookup(pair => pair.Uid, pair => pair.Framework);
                var file = Path.Combine(GetOverwriteDirectory(release), $"{package}-availability.md");
                using (var writer = File.CreateText(file))
                {
                    foreach (var uid in release.Members.Where(m => m.Type != DocfxMember.TypeKind.Namespace).Select(m => m.Uid))
                    {
                        string availability = string.Join(", ", frameworksByUid[uid].OrderBy(f => f));
                        if (availability == "")
                        {
                            // We can refine this later...
                            throw new Exception($"No reflection metadata for {uid} in release {release.Version}");
                        }
                        writer.WriteLine("---");
                        writer.WriteLine($"uid: {uid}");
                        writer.WriteLine($"availability: '{availability}'");
                        writer.WriteLine("---");
                        writer.WriteLine();
                    }
                }
            }
        }

        private void WriteSinceAnnotations()
        {
            Console.WriteLine("Generating 'since' annotations");
            var uidsToVersions = new Dictionary<string, string>();

            foreach (Release release in releases)
            {
                var file = Path.Combine(GetOverwriteDirectory(release), $"{package}-since.md");
                using (var writer = File.CreateText(file))
                {
                    foreach (var uid in release.Members.Select(m => m.Uid))
                    {
                        if (!uidsToVersions.TryGetValue(uid, out string version))
                        {
                            version = release.Version;
                            uidsToVersions[uid] = version;
                        }
                        writer.WriteLine("---");
                        writer.WriteLine($"uid: {uid}");
                        writer.WriteLine($"since: '{version}'");
                        writer.WriteLine("---");
                        writer.WriteLine();
                    }
                }
                // Effectively clear out any versions removed by this release.
                // (e.g. if a member is in 1.3.x, not in 2.0.x, then in 2.1.x,
                // we want the 2.1.x docs to say it's since 2.1.x).
                uidsToVersions = release.Members
                    .Select(m => m.Uid)
                    .ToDictionary(uid => uid, uid => uidsToVersions[uid]);
            }
        }

        private void ModifyYamlFiles()
        {
            // We don't want to load and save the YAML files over and over again, so we perform
            // potentially-multiple mutations, then resave. We only load documents as and when we need to.
            foreach (var release in releases)
            {
                Console.WriteLine($"Modifying YAML files for {release.Version} with {release.MembersByUid.Count} members");
                var files = new Dictionary<string, YamlStream>();

                AnnotateNotNullParameters(release, files);
                AnnotateNotNullReturns(release, files);

                foreach (var pair in files)
                {
                    using (var writer = File.CreateText(pair.Key))
                    {
                        writer.WriteLine("### YamlMime:ManagedReference");
                        pair.Value.Save(writer, false);
                    }
                }
            }
        }

        private void AnnotateNotNullParameters(Release release, Dictionary<string, YamlStream> files)
        {
            var members = reflectionDataByVersion[release.Version]
                .SelectMany(asm => asm.Members)
                .Where(m => m.NotNullParameters.Any());

            foreach (var member in members)
            {
                var document = FindDocument(release, files, member.DocfxUid);
                var node = FindChildByUid(document, "items", member.DocfxUid);

                if (!node.Children.TryGetValue("exceptions", out YamlNode exceptions))
                {
                    exceptions = new YamlSequenceNode();
                    node.Add("exceptions", exceptions);
                }
                YamlSequenceNode exceptionsSequence = (YamlSequenceNode) exceptions;
                var currentArgumentNullException = exceptionsSequence.Children
                    .Cast<YamlMappingNode>()
                    .SingleOrDefault(e => ((YamlScalarNode)e.Children["type"]).Value == "System.ArgumentNullException");
                if (currentArgumentNullException != null)
                {
                    exceptionsSequence.Children.Remove(currentArgumentNullException);
                }

                var names = member.NotNullParameters.ToList();
                string message;
                
                if (names.Count == 1)
                {
                    message = $"{ParamRef(names[0])} is null.";
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < names.Count - 2; i++)
                    {
                        builder.Append($"{ParamRef(names[i])}, ");
                    }
                    builder.Append($"{ParamRef(names[names.Count - 2])} or {ParamRef(names.Last())} is null");
                    message = builder.ToString();
                }
                exceptionsSequence.Children.Add(new YamlMappingNode
                {
                    { "type", "System.ArgumentNullException" },
                    { "commentId", "T:System.ArgumentNullException" },
                    { "description", message }
                });

                // Make sure the reference to ArgumentNullException is present
                var reference = FindChildByUid(document, "references", "System.ArgumentNullException");
                if (reference == null)
                {
                    ((YamlSequenceNode)document.Children["references"]).Add(new YamlMappingNode
                    {
                        { "uid", "System.ArgumentNullException" },
                        { "commentId", "T:System.ArgumentNullException" },
                        { "parent", "System" },
                        { "isExternal", "true" },
                        { "name", "ArgumentNullException" },
                        { "nameWithType", "ArgumentNullException" },
                        { "fullName", "System.ArgumentNullException" }
                    });
                }
            }

            string ParamRef(string name) => $"<span class=\"paramref\">{name}</span>"; ;
        }

        private void AnnotateNotNullReturns(Release release, Dictionary<string, YamlStream> files)
        {
            var errors = new List<string>();
            var members = reflectionDataByVersion[release.Version]
                .SelectMany(asm => asm.Members)
                .Where(m => m.NotNullReturn)
                .DistinctBy(m => m.DocfxUid);

            foreach (var member in members)
            {
                var document = FindDocument(release, files, member.DocfxUid);
                var node = FindChildByUid(document, "items", member.DocfxUid);

                var returnElement = (YamlMappingNode) node["syntax"]["return"];
                if (!returnElement.Children.ContainsKey("description"))
                {
                    // If the method overrides another but we don't have a specific description, that's okay.
                    if (!member.IsOverride)
                    {
                        errors.Add(member.DocfxUid);
                    }
                    continue;
                }
                var description = (YamlScalarNode) returnElement["description"];
                var suffix = " (The value returned is never null.)";
                if (!description.Value.EndsWith(suffix))
                {
                    description.Value += suffix;
                }
            }
            /* TODO: Reinstate this, if we can get doc inheritance to work properly.
             * At the moment, we can't inherit documentation from one project to another, which is annoying :(
             * This is a docfx limitation.
            if (errors.Count != 0)
            {
                throw new Exception($"UIDs with no return description:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
            }
            */
        }

        private YamlMappingNode FindDocument(Release release, Dictionary<string, YamlStream> files, string uid)
        {
            var docfxMember = release.MembersByUid[uid];
            string file = docfxMember.YamlFile;
            if (!files.TryGetValue(file, out YamlStream document))
            {
                document = LoadYamlFile(file);
                files[file] = document;
            }
            return (YamlMappingNode)document.Documents[0].RootNode;
        }

        private YamlMappingNode FindChildByUid(YamlMappingNode parent, string sequenceName, string uid)
        {
            var items = (YamlSequenceNode) parent[sequenceName];
            return items.Cast<YamlMappingNode>().SingleOrDefault(node => ((YamlScalarNode)node["uid"]).Value == uid);
        }
        
        private static YamlStream LoadYamlFile(string file)
        {
            var stream = new YamlStream();
            using (var reader = File.OpenText(file))
            {
                stream.Load(reader);
            }
            return stream;
        }

        private string GetOverwriteDirectory(Release release)
            => Path.Combine(root, release.Version, "overwrite");
    }
}
