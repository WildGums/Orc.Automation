namespace Orc.Automation.Tests;

using System;
using System.Threading;
using NUnit.Framework;
using Orc.Automation;

public static class EventAssert
{
    public static void Raised(object target, string eventName, Action code, int timeout = 200)
    {
        Assert.That(IsEventRaised(target, eventName, code, timeout), Is.True);
    }

    public static void NotRaised(object target, string eventName, Action code, int timeout = 200)
    {
        Assert.That(IsEventRaised(target, eventName, code), Is.False);
    }

    private static bool IsEventRaised(object target, string eventName, Action code, int timeout = 200)
    {
        using var eventRaised = new ManualResetEvent(false);

        // ReSharper disable once AccessToDisposedClosure
        EventHelper.TrySubscribeToEvent(target, eventName, () => { eventRaised.Set(); });

        code?.Invoke();

        return eventRaised.WaitOne(timeout);
    }
}
