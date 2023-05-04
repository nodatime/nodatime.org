// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.DocAsCode.Common;
using Microsoft.DocAsCode.DataContracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TocCombiner
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = new List<TocItemViewModel>();
            foreach (var file in args)
            {
                var toc = YamlUtility.Deserialize<TocViewModel>(file);
                items.AddRange(toc);
            }
            var combined = new TocViewModel();
            combined.AddRange(items.OrderBy(item => item.Name));
            YamlUtility.Serialize(Console.Out, combined, "YamlMime:TableOfContent");
        }

        /* Later version of docfx...
        static void Main(string[] args)
        {
            var items = new List<TocItemViewModel>();
            TocRootViewModel combined = null;
            foreach (var file in args)
            {
                Console.WriteLine("Loading " + file);
                var toc = YamlUtility.Deserialize<TocRootViewModel>(file);
                items.AddRange(toc.Items);
                combined ??= toc;
            }
            combined.Items = new TocViewModel(items.OrderBy(item => item.Name));
            YamlUtility.Serialize(Console.Out, combined, "YamlMime:TableOfContent");
        }*/
    }
}
