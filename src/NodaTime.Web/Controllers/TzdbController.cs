﻿// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.AspNetCore.Mvc;
using NodaTime.Helpers;
using NodaTime.Web.Models;
using NodaTime.Web.Services;

namespace NodaTime.Web.Controllers
{
    [AddHeader("X-Robots-Tag", "noindex")]
    public class TzdbController : Controller
    {
        private const string ContentType = "application/octet-stream";
        private readonly TzdbRepository tzdbRepository;

        public TzdbController(TzdbRepository tzdbRepository)
        {
            this.tzdbRepository = tzdbRepository;
        }

        [Route(@"/tzdb/{id:regex(^.*\.nzd$)}")]
        public IActionResult Get(string id)
        {
            var release = tzdbRepository.GetRelease(id);
            if (release == null)
            {
                return NotFound();
            }
            return File(release.GetContent(), ContentType, release.Name);
        }

        [Route("/tzdb/latest.txt")]
        public IActionResult Latest()
        {
            var download = tzdbRepository.GetReleases().First();
            return new ContentResult
            {
                ContentType = "text/plain",
                Content = GetDownloadUrl(download),
                StatusCode = 200
            };
        }

        [Route("/tzdb/index.txt")]
        public IActionResult Index()
        {
            // We've previous had this "oldest first", so let's honour that.
            var releaseUrls = tzdbRepository.GetReleases().OrderBy(x => x.Name, StringComparer.Ordinal).Select(GetDownloadUrl);
            return new ContentResult
            {
                ContentType = "text/plain",
                Content = string.Join("\r\n", releaseUrls),
                StatusCode = 200
            };
        }

        private string GetDownloadUrl(TzdbDownload download) =>
            $"https://{Request.Host}/tzdb/{download.Name}";
    }
}
