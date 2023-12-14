﻿namespace Orc.Automation;

using System;

public static class ActionExtensions
{
    public static Func<TResult?> MakeDefault<TResult>(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return () =>
        {
            action.Invoke(); 
            return default;
        };
    }
}