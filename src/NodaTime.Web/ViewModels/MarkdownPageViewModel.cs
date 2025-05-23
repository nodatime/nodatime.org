﻿// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using NodaTime.Web.Models;

namespace NodaTime.Web.ViewModels
{
    public class MarkdownPageViewModel
    {
        public string HostOrigin { get; }
        public MarkdownPage Page { get; }

        public MarkdownPageViewModel(string hostOrigin, MarkdownPage page) =>
            (HostOrigin, Page) = (hostOrigin, page);
    }
}
