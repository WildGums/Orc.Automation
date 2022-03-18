namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;

    public abstract class AutomationControlPeerBase<TControl> : AutomationControlPeerBase
        where TControl : FrameworkElement
    {
        protected AutomationControlPeerBase(TControl owner)
            : base(owner)
        {
            Control = owner;
        }

        protected TControl Control { get; }

        protected override string GetClassNameCore()
        {
            return typeof(TControl).FullName;
        }

        [AutomationMethod]
        public object TryFindResource(string key)
        {
            return Control.TryFindResource(key);
        }
    }

    public abstract class AutomationControlPeerBase : FrameworkElementAutomationPeer, IValueProvider, IInvokeProvider
    {
        #region Fields
        private readonly FrameworkElement _owner;

        private AutomationResultContainer _result = new();

        private AutomationMethod _pendingMethod;

        private IList<IAutomationMethodRun> _automationMethods;
        #endregion

        #region Constructors
        public AutomationControlPeerBase(FrameworkElement owner)
            : base(owner)
        {
            Argument.IsNotNull(() => owner);

            _owner = owner;

            _automationMethods = Array.Empty<IAutomationMethodRun>();

            Initialize();

            AutomationRoutedEvents.AddAutomationMessageSentHandler(owner, (sender, args) =>
            {
                RaiseEvent("AutomationMessageSent", args.Message);
            });
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            _automationMethods = GetAvailableAutomationMethods();
        }

        protected virtual IList<IAutomationMethodRun> GetAvailableAutomationMethods()
        {
            var calls = GetType().GetMethods().Where(x => x.IsDecoratedWithAttribute(typeof(AutomationMethodAttribute)))
                .Select(x => (IAutomationMethodRun)new ReflectionAutomationMethodRun(this, x.Name))
                .ToList();

            calls.Add(new AttachBehaviorMethodRun());

            return calls;
        }

        [AutomationMethod]
        public object GetAttachedPropertyValue([Target] FrameworkElement target, Type ownerType, string propertyName)
        {
            return DependencyPropertyAutomationHelper.TryGetDependencyPropertyValue(target, ownerType, propertyName, out var propertyValue)
                ? propertyValue
                : AutomationValue.NotSetValue;
        }

        [AutomationMethod]
        public void SetAttachedPropertyValue([Target] FrameworkElement target, Type ownerType, string propertyName, object value)
        {
            DependencyPropertyAutomationHelper.SetDependencyPropertyValue(target, ownerType, propertyName, value);
        }

        [AutomationMethod]
        public object GetPropertyValue([Target] FrameworkElement target, string propertyName)
        {
            return DependencyPropertyAutomationHelper.TryGetDependencyPropertyValue(target, propertyName, out var propertyValue)
                ? propertyValue
                : AutomationValue.NotSetValue;
        }

        [AutomationMethod]
        public void SetPropertyValue([Target] FrameworkElement target, string propertyName, object value)
        {
            DependencyPropertyAutomationHelper.SetDependencyPropertyValue(target, propertyName, value);
        }

        [AutomationMethod]
        public bool AddAutomationMethod(Type automationMethodType)
        {
            if (_automationMethods.Any(x => x.GetType() == automationMethodType))
            {
                return true;
            }

            if (this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(automationMethodType) is not IAutomationMethodRun automationMethod)
            {
                return false;
            }

            _automationMethods.Add(automationMethod);
            return true;
        }

        [AutomationMethod]
        public object GetResource(string name)
        {
            return _owner.TryFindResource(name);
        }

        protected override string GetClassNameCore()
        {
            return _owner.GetType().FullName;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        protected override bool IsContentElementCore()
        {
            return true;
        }

        protected override bool IsControlElementCore()
        {
            return true;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Value)
            {
                return this;
            }

            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }
        #endregion

        #region IValueProvider
        public virtual void SetValue(string value)
        {
            _pendingMethod = AutomationMethod.FromStr(value);

            if (Equals(_pendingMethod, AutomationMethod.Empty))
            {
                _result = null;

                SetValuePatternInvoke(value);

                return;
            }
        }

        public virtual bool IsReadOnly => false;

        public virtual string Value
        {
            get
            {
                if (_result is null)
                {
                    return GetValueFromPattern();
                }

                var result = _result;
                _result = null;

                return result.ToString();
            }
        }

        protected virtual void SetValuePatternInvoke(string value)
        {

        }

        protected virtual string GetValueFromPattern()
        {
            return null;
        }
        #endregion

        #region InvokeProvider
        public virtual void Invoke()
        {
            try
            {
                var method = _pendingMethod;
                if (method is null)
                {
                    InvokePatternInvoke();

                    return;
                }

                _pendingMethod = null;

                _result ??= new AutomationResultContainer();

                var handle = method.Handle;
                var currentTarget = _owner;

                if (!string.IsNullOrWhiteSpace(handle))
                {
                    currentTarget = _owner?.FindVisualDescendantWithAutomationId(handle) as FrameworkElement;
                }
                else
                {
                    var searchRectangle = method.SearchRectangle;
                    if (searchRectangle is not null)
                    {
                        var ownerWindow = Window.GetWindow(_owner);
                        var hitTestPoint = ownerWindow.PointFromScreen(searchRectangle.Value.GetClickablePoint());

                        currentTarget = SearchHelper.GetDirectlyOver(ownerWindow, hitTestPoint, method.SearchTypeName) as FrameworkElement;
                    }
                }

                var finder = method.Finder;
                if (finder is not null)
                {
                    currentTarget = finder.Find(currentTarget);
                }

                var methodRun = _automationMethods.FirstOrDefault(x => x.IsMatch(currentTarget, method));
                if (methodRun is null)
                {
                    _result.LastInvokedMethodResult = null;

                    return;
                }

                if (!methodRun.TryInvoke(currentTarget, method, out var methodResult))
                {
                    _result.LastInvokedMethodResult = null;

                    return;
                }

                _result.LastInvokedMethodResult = methodResult;
            }
            catch
            {
                //TODO:Vladimir:Log
            }
        }

        protected virtual void InvokePatternInvoke()
        {

        }

        protected void RaiseEvent(string eventName, object args)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            _result ??= new AutomationResultContainer();

            _result.LastEventName = eventName;
            _result.LastEventArgs = AutomationValue.FromValue(args);

            //MessageBox.Show($"EventName = {eventName}; Args = {_result.LastEventArgs}");

            RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
        }
        #endregion
    }
}
