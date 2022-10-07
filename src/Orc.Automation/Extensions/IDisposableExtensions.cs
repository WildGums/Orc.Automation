namespace Orc.Automation
{
    using System;
    using Catel;

    public static class IDisposableExtensions
    {
        public static TResult Execute<TResult>(this IDisposable disposable, Func<TResult> func)
        {
            ArgumentNullException.ThrowIfNull(disposable);
            ArgumentNullException.ThrowIfNull(func);

#pragma warning disable IDISP007 // Don't dispose injected.
            using (disposable)
#pragma warning restore IDISP007 // Don't dispose injected.
            {
                return func.Invoke();
            }
        }

        public static void Execute(this IDisposable disposable, Action action)
        {
            ArgumentNullException.ThrowIfNull(disposable);
            ArgumentNullException.ThrowIfNull(action);

            disposable.Execute(action.MakeDefault<bool>());
        }
    }
}
