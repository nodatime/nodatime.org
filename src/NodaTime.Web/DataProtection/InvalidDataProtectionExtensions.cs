// Copyright 2025 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace NodaTime.Web.DataProtection;

public static class InvalidDataProtectionExtensions
{
    /// <summary>
    /// Disables data protection as far as possible: a keyring is provided with no keys,
    /// key generation is disabled, any attempt to store a key will fail, and any attempt to encrypt
    /// data will fail.
    /// </summary>
    public static IDataProtectionBuilder DisableDataProtection(this IDataProtectionBuilder builder)
    {
        builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
            new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new InvalidXmlRepository();
                options.XmlEncryptor = new InvalidXmlEncryptor();
                options.AutoGenerateKeys = false;
            }));
        return builder;
    }

    private class InvalidXmlRepository : IXmlRepository
    {
        public IReadOnlyCollection<XElement> GetAllElements() => [];

        public void StoreElement(XElement element, string friendlyName) =>
            throw new NotSupportedException();
    }

    private class InvalidXmlEncryptor : IXmlEncryptor
    {
        public EncryptedXmlInfo Encrypt(XElement plaintextElement) =>
            throw new NotSupportedException();
    }
}
