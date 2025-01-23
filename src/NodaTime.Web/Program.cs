// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Google.Cloud.Logging.Console;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using NodaTime;
using NodaTime.Web.Configuration;
using NodaTime.Web.DataProtection;
using NodaTime.Web.Middleware;
using NodaTime.Web.Services;
using NReco.Logging.File;

// Note: old-style Program declaration for use in NodaTime.Web.SmokeTest.
public class Program
{
    public const string SmokeTestEnvironment = "SmokeTests";
    private static readonly MediaTypeHeaderValue TextHtml = new MediaTypeHeaderValue("text/html");

    internal static async Task Main(string[] args)
    {
        var app = await CreateWebApplication(new WebApplicationOptions { Args = args });
        // Now start serving.
        app.Run();
    }

    public static async Task<WebApplication> CreateWebApplication(WebApplicationOptions options)
    {
        var builder = WebApplication.CreateBuilder(options);

        // Add services to the container.
        builder.Services
            .AddMvc()
            .Services
            .AddDataProtection()
            .DisableDataProtection()
            .Services
            .AddHealthChecks()
            .Services
            .AddResponseCompression()
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddHttpClient()
            // This isn't needed normally, but when the smoke tests launch the server,
            // we need to say that the controllers are still in *this* assembly.
            .AddControllers().AddApplicationPart(typeof(Program).Assembly).Services
            .AddSingleton<MarkdownLoader>();
        ;
        var storageOptions = builder.Configuration.GetSection("Storage").Get<StorageOptions>() ?? throw new ArgumentException("Must have storage options");
        storageOptions.ConfigureServices(builder.Services);

        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.AddSimpleConsole(options => { options.SingleLine = true; options.UseUtcTimestamp = true; options.TimestampFormat = "yyyy-MM-dd'T'HH:mm:ss.fffZ "; });
        }
        else
        {
            builder.Logging.AddGoogleCloudConsole(options => { options.IncludeScopes = true; options.TraceGoogleCloudProjectId = "nodatime"; });
        }

        // It's handy to get a log file when running the smoke tests, but otherwise the console is fine.
        if (builder.Environment.EnvironmentName == SmokeTestEnvironment)
        {
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddFile("web-app-smoke-test.log", append: false));
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseResponseCompression();
        }

        app.UseCacheRefreshingMiddleware(options => options
            .Add<BenchmarkRepository>(Duration.FromMinutes(15))
            .Add<TzdbRepository>(Duration.FromMinutes(6))
            .Add<ReleaseRepository>(Duration.FromMinutes(7)));

        app.UseHealthChecks("/health");
        app.UseReferralNotFoundLogging();
        app.UseDefaultFiles();
        // Default content, e.g. CSS.
        // Even though we don't normally host the nzd files locally, it's useful to be able
        // to in case of emergency.
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = new FileExtensionContentTypeProvider
            {
                Mappings = { [".nzd"] = "application/octet-stream" }
            },
            OnPrepareResponse = context => SetCacheControlHeaderForStaticContent(app.Environment, context.Context)
        });

        // API documentation, if present. (It normally will be in production, but making this optional
        // allows the github repository to be cloned and then immediately used.)
        var docfxDirectory = Path.Combine(app.Environment.ContentRootPath, "docfx");
        if (Directory.Exists(docfxDirectory))
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(docfxDirectory),
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".yml"] = "text/x-yaml" }
                },
                OnPrepareResponse = context => SetCacheControlHeaderForStaticContent(app.Environment, context.Context)
            });
        }

        // Fetch the latest release so we can use it in rewrite options.
        // The fact that the rewrite options are fixed after initialization means
        // that until the next web site push, we'll end up redirecting /api and /userguide
        // to the previous latest release, but that's probably okay. (It'll only be temporary.)
        var releaseRepository = app.Services.GetRequiredService<ReleaseRepository>();
        await releaseRepository.Refresh(default);
        var latestRelease = releaseRepository.CurrentMinorVersions.First(); // e.g. 2.4.x

        // Captures "unstable" or a specific version - used several times below.
        // This includes versions that don't exist at the moment, but it just means they'll
        // fail after the redirect instead of before.
        string anyVersion = @"((?:\d\.\d\.x)|(?:unstable))";
        var rewriteOptions = new RewriteOptions()
            // Docfx wants index.html to exist, which is annoying... just redirect.
            .AddRedirect($@"^index.html$", "/")
            // We don't have an index.html or equivalent for the APIs, so let's go to NodaTime.html
            .AddRedirect($@"^{anyVersion}/api/?$", "$1/api/NodaTime.html")
            // For serialization, go to NodaTime.Serialization.JsonNet.html
            .AddRedirect($@"^serialization/api/?$", "serialization/api/NodaTime.Serialization.JsonNet.html")
            // Compatibility with old links
            .AddRedirect($@"^{anyVersion}/userguide/([^.]+)\.html$", "$1/userguide/$2")
            .AddRedirect($@"^developer/([^.]+)\.html$", "developer/$1")
            // Avoid links from userguide/unstable from going to userguide/core-concepts etc
            // (There are no doubt better ways of doing this...)
            .AddRedirect($@"^{anyVersion}/userguide$", "$1/userguide/")
            .AddRedirect($@"^developer$", "developer/")
            // Make /api and /userguide links to the latest stable release.
            .AddRedirect("^(api|userguide)(/.*)?$", $"{latestRelease}/$1$2");
        app.UseRewriter(rewriteOptions);
        app.MapControllers();
        // Documentation suggests this probably shouldn't be necessary - but without this, you can either have
        // static content or controllers working, but not both.
        app.UseRouting();

        // Force the set of benchmarks to start being loaded on startup.
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
        {
            await app.Services.GetRequiredService<BenchmarkRepository>().Refresh(cts.Token);
        }
        // Force the TZDB repository to be downloaded on startup.
        await app.Services.GetRequiredService<TzdbRepository>().Refresh(default);
        // Force all the Markdown to be loaded on startup.
        // (This loads pages synchronously; start it running after prodding the repositories,
        // which load asynchronously.)
        app.Services.GetRequiredService<MarkdownLoader>();
        return app;
    }

    private static void SetCacheControlHeaderForStaticContent(IWebHostEnvironment env, HttpContext context)
    {
        var headers = new ResponseHeaders(context.Response.Headers);

        // Don't set Cache-Control for HTML files (e.g. /tzdb/). The browser can figure out when to revalidate
        // (Which it can do easily, since we send an ETag with all static content responses).
        // Also don't set cache control for build.txt and commit.txt, which are diagnostic files designed
        // to show "the version being served" and should never be cached.
        if (headers.ContentType is MediaTypeHeaderValue contentType && contentType.IsSubsetOf(TextHtml) ||
            context.Request.Path.Value == "/build.txt" ||
            context.Request.Path.Value == "/commit.txt")
        {
            return;
        }

        // Otherwise if the request was made with a file version query parameter (?v=..., as sent by
        // asp-append-version=true), then we can always use the response 'indefinitely', even in development mode.
        if (context.Request.Query.ContainsKey("v"))
        {
            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(365)
            };
            return;
        }

        // Otherwise, the remaining content (/favicon.ico, /fonts/, /robots.txt, /styles/docfx.js etc) should be
        // good to use for a while without revalidation. When running in the Development environment, we'll use a
        // much shorter time, since we might be iterating on it (in particular, this also covers the unminified
        // JS/CSS).
        headers.CacheControl = new CacheControlHeaderValue
        {
            Public = true,
            MaxAge = env.IsDevelopment() ? TimeSpan.FromMinutes(2) : TimeSpan.FromDays(1)
        };
    }
}