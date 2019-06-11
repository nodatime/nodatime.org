// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.AspNetCore.Mvc;
using NodaTime.Web.Configuration;
using NodaTime.Web.Services;
using NodaTime.Web.ViewModels;

namespace NodaTime.Web.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly MarkdownLoader loader;
        private readonly TryDotNetOptions tryDotNetOptions;

        public DocumentationController(MarkdownLoader loader, TryDotNetOptions tryDotNetOptions)
        {
            this.loader = loader;
            this.tryDotNetOptions = tryDotNetOptions;
        }

        public IActionResult ViewDocumentation(string bundle, string url)
        {           
            if (url == null || url.EndsWith("/"))
            {
                url += "index";
            }

            var page = loader.TryGetBundle(bundle)?.TryGetPage(url);
            if (page != null)
            {
                return View("Docs", new MarkdownPageViewModel(tryDotNetOptions.IFrameSrc, page));
            }
            var resource = loader.TryGetBundle(bundle)?.TryGetResource(url);
            if (resource != null)
            {
                return File(resource.GetContent(), resource.ContentType);
            }
            return NotFound();
        }
    }
}
