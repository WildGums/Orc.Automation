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

    public abstract class RunMethodAutomationPeerBase : FrameworkElementAutomationPeer, IValueProvider, IInvokeProvider
    {
        #region Fields
        private readonly FrameworkElement _owner;

        private string _currentCommand;
        private readonly AutomationResultContainer _result = new ();

        private IList<IAutomationMethodRun> _automationMethods;
        #endregion

        #region Constructors
        public RunMethodAutomationPeerBase(FrameworkElement owner)
            : base(owner)
        {
            Argument.IsNotNull(() => owner);

            _owner = owner;

            _automationMethods = Array.Empty<IAutomationMethodRun>();

            Initialize();
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
                .Select(x => (IAutomationMethodRun) new ReflectionAutomationMethodRun(this, x.Name))
                .ToList();

            calls.Add(new AttachBehaviorMethodRun());

            return calls;
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
                return false;
            }

            if (this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(automationMethodType) is not IAutomationMethodRun automationMethod)
            {
                return false;
            }

            _automationMethods.Add(automationMethod);
            return true;
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
            _currentCommand = value;
        }

        public virtual bool IsReadOnly => false;
        public virtual string Value => _result?.ToString();
        #endregion

        #region InvokeProvider
        public virtual void Invoke()
        {
            try
            {
                var commandStr = _currentCommand;
                if (string.IsNullOrWhiteSpace(commandStr))
                {
                    return;
                }

                _currentCommand = string.Empty;

                var method = AutomationMethod.FromStr(commandStr);
                if (method is null)
                {
                    _result.LastInvokedMethodResult = null;

                    return;
                }

                var handle = method.Handle;
                var currentTarget = string.IsNullOrWhiteSpace(handle) ? _owner : _owner?.FindVisualDescendantWithAutomationId(handle) as FrameworkElement;

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
            catch (Exception ex)
            {
                File.AppendAllText("C:\\Temps\\Exception.txt", ex.Message);
                File.AppendAllText("C:\\Temps\\Exception.txt", "\r\n");
                File.AppendAllText("C:\\Temps\\Exception.txt", ex.StackTrace);
            }
        }

        protected void RaiseEvent(string eventName, object args)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            _result.LastEventName = eventName;
            _result.LastEventArgs = AutomationValue.FromValue(args);

            RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
        }
        #endregion
    }
}
