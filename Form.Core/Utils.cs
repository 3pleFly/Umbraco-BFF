namespace Form.Core;

public static class TryHelper
{
    public static async Task<(T?, Exception?)> TryAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return (await action(), null);
        }
        catch (Exception e)
        {
            return (default, e);
        }
    }

    public static async Task<Exception?> TryAsync(Func<Task> action)
    {
        try
        {
            await action();
            return null;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static (T?, Exception?) Try<T>(Func<T> action)
    {
        try
        {
            return (action(), null);
        }
        catch (Exception e)
        {
            return (default, e);
        }
    }

    public static Exception? Try(Action action)
    {
        try
        {
            action();
            return null;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}

