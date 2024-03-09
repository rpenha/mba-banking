namespace Banking.Core;

public static class DateTimeProvider
{
    public static TimeProvider Current { get; private set; } = TimeProvider.System;

    public static void Set(TimeProvider provider)
    {
        Current = provider;
    }

    public static DateTimeOffset Now => Current.GetUtcNow();
}