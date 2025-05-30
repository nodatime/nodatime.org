﻿// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.AspNetCore.Mvc;
using NodaTime.Helpers;
using NodaTime.TimeZones;
using NodaTime.TzValidate.NodaDump;
using NodaTime.Web.Services;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace NodaTime.Web.Controllers
{
    [AddHeader("X-Robots-Tag", "noindex")]
    public class TzValidateController : Controller
    {
        private readonly TzdbRepository repository;

        public TzValidateController(TzdbRepository repository) =>
            this.repository = repository;

        private static readonly Regex NzdNamePattern = new Regex(@"tzdb(\d+.)\.nzd");
        [Route("/tzvalidate/generate")]
        public IActionResult Generate(int startYear = 1, int endYear = 2035, string? zone = null, string? version = null)
        {
            if (startYear < 1 || endYear > 3000 || startYear > endYear)
            {
                return BadRequest("Invalid start/end year combination");
            }

            // TODO: Remove the duplication between this and HomeController
            version ??= repository.GetReleases().Select(release => NzdNamePattern.Match(release.Name))
                .Where(m => m.Success)
                .Select(m => m.Groups[1].Value)
                .First();

            var release = repository.GetRelease($"tzdb{version}.nzd");
            if (release == null)
            {
                return BadRequest("Unknown version");
            }
            // TODO: Cache these. There's no point in reparsing on each request.
            var source = TzdbDateTimeZoneSource.FromStream(release.GetContent());

            if (zone != null && !source.GetIds().Contains(zone))
            {
                return BadRequest("Unknown zone");
            }

            var writer = new StringWriter();
            var options = new Options { FromYear = startYear, ToYear = endYear, ZoneId = zone };
            var dumper = new ZoneDumper(source, options);
            dumper.Dump(writer);

            return new ContentResult
            {
                Content = writer.ToString(),
                ContentType = "text/plain",
                StatusCode = (int) HttpStatusCode.OK
            };
        }
    }
}
