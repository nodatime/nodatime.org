// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Docfx.Common;
using Docfx.DataContracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TocCombiner
{
    class Program
    {
        static void Main(string[] args)
        {
            TocModel combined = null;
            foreach (var file in args)
            {
                TocModel toc = null;
                // Handle both new and old TOC representations
                try
                {
                    toc = YamlUtility.Deserialize<TocModel>(file);
                }
                catch
                {
                    var items = YamlUtility.Deserialize<List<TocItemViewModel>>(file);
                    toc = new TocModel
                    {
                        Items = items,
                        MemberLayout = "SamePage"
                    };
                }
                if (combined is null)
                {
                    combined = toc;
                }
                else
                {
                    combined.Items.AddRange(toc.Items);
                }
            }
            combined.Items = [.. combined.Items.OrderBy(item => item.Name)];
            YamlUtility.Serialize(Console.Out, combined, "YamlMime:TableOfContent");
        }
    }
}
