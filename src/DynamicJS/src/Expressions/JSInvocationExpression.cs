// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.DynamicJS
{
    internal class JSInvocationExpression : IJSExpression
    {
        public JSExpressionType Type => JSExpressionType.Invocation;

        public long TargetObjectId { get; set; }

        public object?[]? Args { get; set; }
    }
}
