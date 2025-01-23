// Copyright 2025 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.Web.Middleware;

public sealed record ConfiguredRefreshableCache(IRefreshableCache Cache, Duration RefreshInterval);

