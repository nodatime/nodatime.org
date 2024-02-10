// Copyright 2024 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Docfx.DataContracts.Common;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TocCombiner;

public class TocModel
{
    [YamlMember(Alias = "items")]
    public List<TocItemViewModel> Items { get; set; } = new();

    [YamlMember(Alias = "memberLayout")]
    public string MemberLayout { get; set; }
}
