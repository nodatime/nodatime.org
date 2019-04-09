// Copyright 2015 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using System;

namespace NodaTime.TzValidate.NodaDump
{
    /// <summary>
    /// An exception caused by user error, e.g. invalid options.
    /// Note: this is a copy of the code in the NodaTime repo, as it's not published as a NuGet package but we use it in the web site.
    /// </summary>
    public sealed class UserErrorException : Exception
    {
        public UserErrorException(string message) : base(message)
        {
        }
    }
}
