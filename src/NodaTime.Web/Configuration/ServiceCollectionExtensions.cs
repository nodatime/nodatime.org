﻿// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.Web.Configuration;

/// <summary>
/// Additional extension methods on IServiceCollection
/// </summary>
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSingletonWithArguments<TService, TImplementation>(
        this IServiceCollection services, params object[] parameters)
        where TService : class
        where TImplementation : class, TService =>
        services.AddSingleton<TService, TImplementation>(
            provider => ActivatorUtilities.CreateInstance<TImplementation>(provider, parameters));

    internal static IServiceCollection AddSingletonWithArguments<TService>(
        this IServiceCollection services, params object[] parameters)
        where TService : class =>
        services.AddSingleton(provider => ActivatorUtilities.CreateInstance<TService>(provider, parameters));
}
