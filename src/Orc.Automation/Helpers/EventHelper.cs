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
            ArgumentNullException.ThrowIfNull(target);
            Argument.IsNotNullOrWhitespace(() => eventName);
            ArgumentNullException.ThrowIfNull(handler);

            var @event = target.GetType().GetEvent(eventName);
            if (@event is null)
            {
                return false;
            }

            if (handler is null)
            {
                return false;
            }

            //TODO:Vladimir: Should be more generic/base
            if (target is AutomationControl control)
            {
                control.Activate();
            }

            var @delegate = Create(@event, handler);
            @event.AddEventHandler(target, @delegate);

            return true;
        }

        public static void RaiseEvent(object target, string eventName, EventArgs eventArgs)
        {
            ArgumentNullException.ThrowIfNull(target);
            Argument.IsNotNullOrWhitespace(() => eventName);

            MulticastDelegate? eventDelegate = null;
            var type = target.GetType();

            while (eventDelegate is null && type is not null)
            {
                eventDelegate = (MulticastDelegate?)type.GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(target);

                type = type.BaseType;
            }

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
            ArgumentNullException.ThrowIfNull(evt);
            ArgumentNullException.ThrowIfNull(action);

            var handlerType = evt.EventHandlerType;
            if (handlerType is null)
            {
                throw new InvalidOperationException($"Event handler cannot be null");
            }

            var handlerInvokeMethod = handlerType.GetMethod("Invoke");
            if (handlerInvokeMethod is null)
            {
                throw new InvalidOperationException($"Event Invoke method cannot be null");
            }

            var actionInvokeMethod = action.GetType().GetMethod("Invoke");
            if (actionInvokeMethod is null)
            {
                throw new InvalidOperationException($"Action Invoke method cannot be null");
            }

            var eventParams = handlerInvokeMethod.GetParameters();

            //lambda: (object x0, EventArgs x1) => d()
            var parameters = eventParams.Select(p => Expression.Parameter(p.ParameterType, "x"));
            var body = Expression.Call(Expression.Constant(action), actionInvokeMethod);
            var lambda = Expression.Lambda(body, parameters.ToArray());

            return Delegate.CreateDelegate(handlerType, lambda.Compile(), "Invoke", false);
        }
    }
}
