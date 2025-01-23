// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.Web.Services
{
    /// <summary>
    /// Abstraction over Google Cloud Storage and the file system,
    /// as far as we need it.
    /// </summary>
    public interface IStorageRepository
    {
        IAsyncEnumerable<StorageFile> ListFilesAsync(string prefix);
        StorageFile GetObject(string name);
        Task<StorageFile> GetObjectAsync(string name, CancellationToken cancellationToken);
        void DownloadObject(string name, Stream stream);
        Task DownloadObjectAsync(string name, Stream stream, CancellationToken cancellationToken);
        string GetDownloadUrl(string name);
    }
}
