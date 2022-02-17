namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Catel;

    public static class EventHelper
    {
        public static bool TrySubscribeToEvent(object target, string eventName, Action handler)
        {
            var @event = target.GetType().GetEvent(eventName);
            if (@event is null)
            {
                return false;
            }

            if (handler is null)
            {
                return false;
            }
            
            var @delegate = Create(@event, handler);
            @event.AddEventHandler(target, @delegate);

            return true;
        }

        public static void RaiseEvent(object target, string eventName, EventArgs eventArgs)
        {
            Argument.IsNotNull(() => target);
            Argument.IsNotNullOrWhitespace(() => eventName);

            var eventDelegate = (MulticastDelegate)target.GetType()
                .GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(target);

            if (eventDelegate is null)
            {
                return;
            }

            foreach (var handler in eventDelegate.GetInvocationList())
            {
                handler.Method.Invoke(handler.Target, new[] { target, eventArgs });
            }
        }

        private static Delegate Create(EventInfo evt, Action action)
        {
            var handlerType = evt.EventHandlerType;
            var eventParams = handlerType.GetMethod("Invoke").GetParameters();

            //lambda: (object x0, EventArgs x1) => d()
            var parameters = eventParams.Select(p => Expression.Parameter(p.ParameterType, "x"));
            var body = Expression.Call(Expression.Constant(action), action.GetType().GetMethod("Invoke"));
            var lambda = Expression.Lambda(body, parameters.ToArray());

            return Delegate.CreateDelegate(handlerType, lambda.Compile(), "Invoke", false);
        }
    }
}
