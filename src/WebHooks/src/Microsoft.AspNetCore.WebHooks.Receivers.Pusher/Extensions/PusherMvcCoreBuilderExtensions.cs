// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up Pusher WebHooks in an <see cref="IMvcCoreBuilder" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PusherMvcCoreBuilderExtensions
    {
        /// <summary>
        /// <para>
        /// Add Pusher WebHook configuration and services to the specified <paramref name="builder"/>. See
        /// <see href="https://pusher.com/docs/webhooks"/> for additional details about Pusher WebHook requests.
        /// </para>
        /// <para>
        /// '<c>WebHooks:Pusher:SecretKey:default:{application key}</c>' configuration values contain secret keys for
        /// Pusher WebHook URIs of the form '<c>https://{host}/api/webhooks/incoming/pusher</c>'.
        /// '<c>WebHooks:Pusher:SecretKey:{id}:{application key}</c>' configuration values contain secret keys for
        /// Pusher WebHook URIs of the form '<c>https://{host}/api/webhooks/incoming/pusher/{id}</c>'. Users optionally
        /// provide <c>{id}</c> values while Pusher defines the <c>{application key}</c> / secret key pairs.
        /// </para>
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder" /> to configure.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        public static IMvcCoreBuilder AddPusherWebHooks(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            PusherServiceCollectionSetup.AddPusherServices(builder.Services);

            return builder
                .AddJsonFormatters()
                .AddWebHooks();
        }
    }
}
