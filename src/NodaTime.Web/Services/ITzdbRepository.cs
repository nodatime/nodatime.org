// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using NodaTime.Web.Models;
using System.Collections.Generic;

namespace NodaTime.Web.Services
{
    public interface ITzdbRepository
    {
        /// <summary>
        /// Returns the list of releases, most recent first.
        /// </summary>
        IList<TzdbDownload> GetReleases();

        /// <summary>
        /// Gets the given release, if it exists (or null otherwise).
        /// </summary>
        TzdbDownload? GetRelease(string name);
    }
}
