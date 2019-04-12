using Microsoft.AspNetCore.Mvc;
using NodaTime.TimeZones;
using NodaTime.Web.Models;
using NodaTime.Web.Services;
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
            markdownBundle = markdownLoader.TryGetBundle("root");
            if (markdownBundle == null)
            {
                throw new ArgumentException("Couldn't get root bundle", nameof(markdownLoader));
            }
            this.repository = repository;
        }

        public IActionResult Versions() => View("Docs", markdownBundle.TryGetPage("versions"));

        public IActionResult Index() => View();

        public IActionResult Error() => View();

        private static readonly Regex NamePattern = new Regex(@"tzdb(\d+.)\.nzd");
        public IActionResult IanaTimeZones(string? version = null)
        {
            var releases = repository.GetReleases()
                .Select(release => NamePattern.Match(release.Name))
                .Where(m => m.Success)
                .Select(m => m.Groups[1].Value)
                .ToList();
            var source = TzdbDateTimeZoneSource.Default;
            if (version != null)
            {
                var release = repository.GetRelease($"tzdb{version}.nzd");
                if (release == null)
                {
                    return BadRequest("Unknown version");
                }
                source = TzdbDateTimeZoneSource.FromStream(release.GetContent());
            }

            var model = (releases, source);
            return View(model);
        }
    }
}
