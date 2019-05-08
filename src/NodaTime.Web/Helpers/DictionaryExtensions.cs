// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NodaTime.Web.Helpers
{
    public static class DictionaryExtensions
    {
        public static TValue? GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : class =>
            dictionary.TryGetValue(key, out var value) ? value : null;
    }
}
