// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.AspNetCore.Html;

namespace NodaTime.Web.Configuration
{
    /// <summary>
    /// Options for Try .NET integration
    /// </summary>
    public class TryDotNetOptions
    {
        public string? Agent { get; set; }
        public string? Origin { get; set; }
        public string Path { get; set; } = "v2/editor";
        public IHtmlContent IFrameSrc => new HtmlString($"{Agent}/{Path}?hostOrigin={Origin}&waitForConfiguration=true");
    }
}
