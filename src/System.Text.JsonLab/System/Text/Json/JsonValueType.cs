﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    // Do not change the order of the enum values, since IsSimpleValue relies on it.
    public enum JsonValueType : byte
    {
        Object = 0,
        Array = 1,
        String = 2,
        Number = 3,
        True = 4,
        False = 5,
        Null = 6,
        Unknown = 7
    }
}
