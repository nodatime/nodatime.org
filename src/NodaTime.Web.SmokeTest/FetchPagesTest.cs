// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.using System;

using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace NodaTime.Web.SmokeTest
{
    public class FetchPagesTest
    {
        private readonly WebServerFixture fixture;

        public FetchPagesTest()
        {
            // TODO: Is there no better way of doing this?
            fixture = WebServerFixture.Instance;
            Assert.NotNull(fixture);
        }

        [Test]
        [TestCase("1.0.x/userguide/text", "There are two options for text handling")]
        [TestCase("1.0.x/api/NodaTime.DateTimeZone.html", "The mapping is unambiguous")]
        // Note: not the full URL as that depends on the server URL
        [TestCase("tzdb/index.txt", "/tzdb/tzdb2018h.nzd")]
        public async Task TextPage(string path, string expectedContent)
        {
            var client = new HttpClient();
            string page = await client.GetStringAsync($"{fixture.BaseUrl}/{path}");
            Assert.True(page.Contains(expectedContent), $"Couldn't find {expectedContent} in path {path}");
        }

        [Test]
        [TestCase("tzdb/tzdb2018h.nzd", 135698)]
        public async Task Binary(string path, int expectedSize)
        {
            var client = new HttpClient();
            byte[] data = await client.GetByteArrayAsync($"{fixture.BaseUrl}/{path}");
            Assert.AreEqual(expectedSize, data.Length);
        }
    }
}
