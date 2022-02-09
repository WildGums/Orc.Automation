namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Catel;
    using Catel.Data;
    using Catel.Reflection;
    using Catel.Windows.Interactivity;

    public class ControlModel : ModelBase
    {
        protected readonly AutomationElementAccessor _accessor;

        private readonly HashSet<string> _ignoredProperties = new()
        {
            nameof(ModelBase.IsReadOnly),
            nameof(ModelBase.IsDirty)
        };

        public ControlModel(AutomationElementAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override T GetValueFromPropertyBag<T>(string propertyName)
        {
            if (_accessor is null || _ignoredProperties.Contains(propertyName))
            {
                return base.GetValueFromPropertyBag<T>(propertyName);
            }

            var apiPropertyName = GetApiPropertyName(propertyName);
            if (!string.IsNullOrWhiteSpace(apiPropertyName))
            {
                return _accessor.GetValue<T>(apiPropertyName);
            }

            return base.GetValueFromPropertyBag<T>(propertyName);
        }

        protected override void SetValueToPropertyBag<TValue>(string propertyName, TValue value)
        {
            if (_accessor is null || _ignoredProperties.Contains(propertyName))
            {
                base.SetValueToPropertyBag(propertyName, value);

                return;
            }

            var apiPropertyName = GetApiPropertyName(propertyName);
            if (!string.IsNullOrWhiteSpace(apiPropertyName))
            {
                _accessor.SetValue(apiPropertyName, value);

                return;
            }

            base.SetValueToPropertyBag(propertyName, value);
        }

        private string GetApiPropertyName(string propertyName)
        {
            var property = PropertyHelper.GetPropertyInfo(this, propertyName);
            var apiAttribute = property.GetAttribute<ApiPropertyAttribute>();
            if (apiAttribute is null)
            {
                return GetType().IsDecoratedWithAttribute<AutomationAccessType>() ? propertyName : null;
            }

            return string.IsNullOrWhiteSpace(apiAttribute.OriginalName) ? propertyName : apiAttribute.OriginalName;
        }
    }

    public class AutomationControl : AutomationBase
    {
        public AutomationControl(AutomationElement element, ControlType controlType)
            : this(element)
        {
            Argument.IsNotNull(() => controlType);

            if (!Equals(element.Current.ControlType, controlType))
            {
                throw new AutomationException($"Can't create instance of type {GetType().Name}, because input Automation Element is not a {controlType}");
            }
        }

        public AutomationControl(AutomationElement element)
            : this(element, new AutomationElementAccessor(element))
        {
        }

        protected AutomationControl(AutomationElement element, AutomationElementAccessor accessor)
            : base(element)
        {
            Access = accessor ?? new AutomationElementAccessor(element);
            Access.AutomationEvent += OnEvent;
        }

        #region Automation Properties
        public AutomationElement.AutomationElementInformation AutomationProperties => Element.Current;

        protected AutomationElementAccessor Access { get; private set; }
        #endregion

        
        public void AttachBehavior<TBehavior>()
            where TBehavior : IBehavior
        {
            Access.AttachBehavior(typeof(TBehavior));
        }

        public bool IsEnabled => AutomationProperties.IsEnabled;
        public void SetFocus()
        {
            Element?.SetFocus();
        }

        public T Part<T>(IPartFinder finder)
            where T : AutomationControl
        {
            var control = Factory.Create<T>(Element);
            if (control is null)
            {
                return default;
            }

            control.Access = Access.Part(finder);

            Wait.UntilResponsive();

            return control;
        }

        public object TryFindResource(string resourceKey)
        {
            return Access.Execute(nameof(TestHostAutomationPeer.GetResource), resourceKey);
        }

        protected virtual void OnEvent(object sender, AutomationEventArgs args)
        {
            RaiseEvent(args.EventName, args);
        }
        
        public object Execute<TMethodType>(params object[] parameters)
            where TMethodType : IAutomationMethodRun
        {
            var result = (bool)Access.Execute(nameof(RunMethodAutomationPeerBase.AddAutomationMethod), typeof(TMethodType));
            if (!result)
            {
                return AutomationValue.NotSetValue;
            }

            return Execute(typeof(TMethodType).Name, parameters);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            return Access.Execute(methodName, parameters);
        }

        protected void RaiseEvent(string eventName, EventArgs eventArgs)
        {
            var eventDelegate = (MulticastDelegate)GetType()
                .GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(this);

            if (eventDelegate is null)
            {
                return;
            }

            foreach (var handler in eventDelegate.GetInvocationList())
            {
                handler.Method.Invoke(handler.Target, new object[] { this, eventArgs });
            }
        }
    }
}
