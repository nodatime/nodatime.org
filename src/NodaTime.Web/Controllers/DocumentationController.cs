﻿// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.AspNetCore.Mvc;
using NodaTime.Web.Services;
using NodaTime.Web.ViewModels;

namespace NodaTime.Web.Controllers;

public class DocumentationController : Controller
{
    private readonly MarkdownLoader loader;

    public DocumentationController(MarkdownLoader loader)
    {
        this.loader = loader;
    }

    [Route("/developer/{*url}")]
    [Route("/{bundle}/userguide/{*url}")]
    public IActionResult ViewDocumentation(string bundle, string url)
    {
        if (bundle == null)
        {
            bundle = "developer";
        }
        if (url == null || url.EndsWith("/"))
        {
            url += "index";
        }
        string origin = $"https://{Request.Host}";
        var page = loader.TryGetBundle(bundle)?.TryGetPage(url);
        if (page != null)
        {
            return View("Docs", new MarkdownPageViewModel(origin, page));
        }
        var resource = loader.TryGetBundle(bundle)?.TryGetResource(url);
        if (resource != null)
        {
            return File(resource.GetContent(), resource.ContentType);
        }
        return NotFound();
    }
}
