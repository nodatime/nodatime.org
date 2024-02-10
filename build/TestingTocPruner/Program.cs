// Copyright 2024 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Docfx.Common;
using TocCombiner;

if (args.Length != 2)
{
    Console.WriteLine("Argument: <input-toc-file> <output-toc-file>");
    return 1;
}

var toc = YamlUtility.Deserialize<TocModel>(args[0]);
toc.Items = [.. toc.Items.Where(item => item.Name.StartsWith("NodaTime.Testing"))];
YamlUtility.Serialize(args[1], toc, "YamlMime:TableOfContent");

return 0;