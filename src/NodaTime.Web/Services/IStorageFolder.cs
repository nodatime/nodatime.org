// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NodaTime.Web.Services
{
    /// <summary>
    /// Abstraction over Google Cloud Storage and the file system,
    /// as far as we need it.
    /// </summary>
    public interface IStorageRepository
    {
        IEnumerable<StorageFile> ListFiles(string prefix);
        StorageFile GetObject(string name);
        void DownloadObject(string name, Stream stream);
        string GetDownloadUrl(string name);
    }
}
