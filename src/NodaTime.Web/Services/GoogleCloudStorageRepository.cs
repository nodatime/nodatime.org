// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Google.Api.Gax;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NodaTime.Web.Services
{
    /// <summary>
    /// Adapter for Google Cloud Storage to implement IStorageRepository.
    /// </summary>
    public sealed class GoogleCloudStorageRepository : IStorageRepository
    {
        private readonly StorageClient client;
        private readonly string bucket;

        public GoogleCloudStorageRepository(string bucket)
        {
            client = StorageClient.Create();
            this.bucket = GaxPreconditions.CheckNotNull(bucket, nameof(bucket));
        }

        public void DownloadObject(string name, Stream stream) =>
            client.DownloadObject(bucket, name, stream);

        public StorageFile GetObject(string name) =>
            ConvertObject(client.GetObject(bucket, name));

        public IEnumerable<StorageFile> ListFiles(string prefix) =>
            client.ListObjects(bucket, prefix)
                .Select(ConvertObject);

        public string GetDownloadUrl(string name) => $"https://storage.googleapis.com/{bucket}/{name}";

        private static StorageFile ConvertObject(Google.Apis.Storage.v1.Data.Object obj) =>
            new StorageFile(obj.Name, obj.Metadata, obj.UpdatedDateTimeOffset ?? DateTimeOffset.MinValue, obj.Crc32c);
    }
}
