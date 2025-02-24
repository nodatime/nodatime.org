﻿// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Google;

namespace NodaTime.Web.Services
{
    /// <summary>
    /// Implementation of IStorageRepository that uses the local file system, for offline development.
    /// </summary>
    public class LocalStorageRepository : IStorageRepository
    {
        private readonly string absoluteRoot;
        private List<string> allFiles;

        public LocalStorageRepository(IWebHostEnvironment environment, string relativeRoot)
        {
            absoluteRoot = Path.Combine(environment.ContentRootPath, relativeRoot);
            if (!Directory.Exists(absoluteRoot))
            {
                throw new ArgumentException($"No such directory: {absoluteRoot}");
            }

            var files = Directory.GetFiles(absoluteRoot, "*", SearchOption.AllDirectories);
            allFiles = files
                .Select(name => Path.GetRelativePath(absoluteRoot, name))
                .Select(name => name.Replace('\\', '/'))
                .ToList();
        }

        public void DownloadObject(string name, Stream stream)
        {
            var path = Path.Combine(absoluteRoot, name);
            using (var input = File.OpenRead(path))
            {
                input.CopyTo(stream);
            }
        }

        public async Task DownloadObjectAsync(string name, Stream stream, CancellationToken cancellationToken)
        {
            var path = Path.Combine(absoluteRoot, name);
            using (var input = File.OpenRead(path))
            {
                await input.CopyToAsync(stream, cancellationToken);
            }
        }

        public string GetDownloadUrl(string name) => $"fakestorage://{name}";

        public StorageFile GetObject(string name)
        {
            var path = Path.Combine(absoluteRoot, name);
            var info = new FileInfo(path);
            if (!info.Exists)
            {
                throw new GoogleApiException(nameof(LocalStorageRepository), "File doesn't exist");
            }
            // The CRC32 is used for detecting changes, which we don't support anyway, so it's fine to
            // give the same value all the time.
            return new StorageFile(name, null, info.LastWriteTimeUtc, "fake-crc32");
        }

        public Task<StorageFile> GetObjectAsync(string name, CancellationToken cancellationToken) =>
            Task.FromResult(GetObject(name));

        public IAsyncEnumerable<StorageFile> ListFilesAsync(string prefix) =>
            allFiles.Where(name => name.StartsWith(prefix)).Select(GetObject).ToAsyncEnumerable();
    }
}
