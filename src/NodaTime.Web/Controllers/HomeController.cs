// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.AspNetCore.Mvc;
using NodaTime.TimeZones;
using NodaTime.Web.Models;
using NodaTime.Web.Services;
using NodaTime.Web.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NodaTime.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MarkdownBundle markdownBundle;
        private readonly ITzdbRepository repository;

        public HomeController(MarkdownLoader markdownLoader, ITzdbRepository repository)
        {
            
            markdownBundle = markdownLoader.TryGetBundle("root")
                ?? throw new ArgumentException("Couldn't get root bundle", nameof(markdownLoader));
            this.repository = repository;
        }

        public IActionResult Versions() => View("Docs", markdownBundle.TryGetPage("versions"));

        public IActionResult Index() => View();

        public IActionResult Error() => View();

        private static readonly Regex NzdNamePattern = new Regex(@"tzdb(\d+.)\.nzd");
        public IActionResult TimeZones(string? version = null, string? format = null)
        {
            var releases = repository.GetReleases()
                .Select(release => NzdNamePattern.Match(release.Name))
                .Where(m => m.Success)
                .Select(m => m.Groups[1].Value)
                .ToList();
            // Default to the most recent release
            version ??= releases.First();

            var release = repository.GetRelease($"tzdb{version}.nzd");
            if (release == null)
            {
                return BadRequest("Unknown version");
            }
            var source = TzdbDateTimeZoneSource.FromStream(release.GetContent());
            var releaseModel = IanaRelease.FromTzdbDateTimeZoneSource(source);
            if (format == "json")
            {
                return Json(releaseModel);
            }

            var model = (releases, releaseModel);
            return View(model);
        }
    }
}
