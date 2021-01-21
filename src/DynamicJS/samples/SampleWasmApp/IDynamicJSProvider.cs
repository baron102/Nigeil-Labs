// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DynamicJS;

namespace SampleWasmApp
{
    public interface IDynamicJSProvider
    {
        Task<object> Evaluate<TValue>(dynamic jsObject);
        Task<object> RunWithWindow(Func<JSObject, Task<object>> operation);
    }
}
