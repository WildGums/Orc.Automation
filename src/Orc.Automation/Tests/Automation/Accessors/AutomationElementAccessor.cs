namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Automation;
    using Catel;

    public class AutomationElementAccessor
    {
        private readonly IPartFinder _finder;

        private AutomationElement _accessElement;
        private AutomationElement _element;
        private InvokePattern _invokePattern;
        private ValuePattern _valuePattern;

        private bool _isInitialized;

        public static AutomationElementAccessor PartAccessor(AutomationElement element, IPartFinder partFinder)
        {
            return new AutomationElementAccessor(element, partFinder);
        }

        private AutomationElementAccessor(AutomationElement element, IPartFinder partFinder)
            : this(element)
        {
            _finder = partFinder;
        }

        public AutomationElementAccessor(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            _element = element;
        }

        public AutomationElementAccessor Part(IPartFinder partFinder)
        {
            Argument.IsNotNull(() => partFinder);

            return new AutomationElementAccessor(_element, partFinder);
        }

        private void InitializeAccessElement(AutomationElement element)
        {
            if (_isInitialized)
            {
                return;
            }

            _element = element;
            _accessElement = element;

            _valuePattern = _accessElement.TryGetPattern<ValuePattern>();
            if (_valuePattern is null)
            {
                _accessElement = _accessElement.Find(className: typeof(Controls.AutomationInformer).FullName, scope:TreeScope.Parent);
                _valuePattern = _accessElement?.TryGetPattern<ValuePattern>();
            }

            _invokePattern = _accessElement?.TryGetPattern<InvokePattern>();
            if (_invokePattern is not null && _valuePattern is not null)
            {
                System.Windows.Automation.Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, _accessElement, TreeScope.Element, OnEventInvoke);
            }

            _isInitialized = true;
        }

        private void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                InitializeAccessElement(_element);
            }

            if (_accessElement is null)
            {
                throw new AutomationException("Can't access element API...this element doesn't implement Run method and there is no AutomationInformer present");
            }
        }

        private void OnEventInvoke(object sender, System.Windows.Automation.AutomationEventArgs e)
        {
            var result = _valuePattern.Current.Value;

            var automationResult = AutomationResultContainer.FromStr(result);

            var eventName = automationResult.LastEventName;
            var eventData = automationResult.LastEventArgs?.ExtractValue();

            _automationEvent?.Invoke(this, new AutomationEventArgs
            {
                EventName = eventName,
                Data = eventData
            });
        }
        
        public void SetValue(string propertyName, object value, Type ownerType = null)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var result = ownerType is null
                ? Execute(nameof(AutomationControlPeerBase.SetPropertyValue), propertyName, value)
                : Execute(nameof(AutomationControlPeerBase.SetAttachedPropertyValue), ownerType, propertyName, value);
        }

        public object GetValue(string propertyName, Type ownerType = null)
        {
            Argument.IsNotNull(() => propertyName);

            var result = ownerType is null 
                ? Execute(nameof(AutomationControlPeerBase.GetPropertyValue), propertyName)
                : Execute(nameof(AutomationControlPeerBase.GetAttachedPropertyValue), ownerType, propertyName);

            return result;
        }

        public object TryFindResource(string resourceKey)
        {
            return Execute(nameof(TryFindResource), resourceKey);
        }

        public void AttachBehavior(Type behaviorType)
        {
            Execute(AttachBehaviorMethodRun.AttachBehaviorMethodPrefix, behaviorType);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            var automationValues = AutomationValueList.Create(parameters);
            return Execute(methodName, automationValues, 20);
        }

        private object Execute(string methodName, AutomationValueList parameters, int delay = 200)
        {
            var method = new AutomationMethod
            {
                Name = methodName,
                Parameters = parameters
            };

            var result = Execute(method, delay);
            var resultValue = result?.ExtractValue();

            return resultValue;
        }

        private AutomationValue Execute(AutomationMethod method, int delay)
        {
            EnsureInitialized();

            if (method is null)
            {
                return null;
            }

            if (!Equals(_accessElement, _element))
            {
                var current = _element.Current;

                method.Handle = current.AutomationId;
                method.SearchRectangle = current.BoundingRectangle;
                method.SearchTypeName = current.ClassName;
            }

            method.Finder = _finder;

            var methodStr = method.ToString();
            
            //AutomationMethodsList.Instance.Methods.Add(method);

            if (string.IsNullOrWhiteSpace(methodStr))
            {
                return null;
            }

            _valuePattern.SetValue(methodStr);

            Thread.Sleep(delay);

            try
            {
                _invokePattern?.Invoke();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            Thread.Sleep(delay);

            var result = _valuePattern.Current.Value;
            var automationResultContainer = AutomationResultContainer.FromStr(result);

            return automationResultContainer.LastInvokedMethodResult;
        }
        
        private event EventHandler<AutomationEventArgs> _automationEvent; 
        public event EventHandler<AutomationEventArgs> AutomationEvent
        {
            add
            {
                EnsureInitialized();
                _automationEvent += value;
            }

            remove
            {
                _automationEvent -= value;
            }
        }
    }

    [KnownType(typeof(SearchContextFinder))]
    public class AML
    {
        public List<AutomationMethod> Methods { get; set; } = new();
    }

    public static class AutomationMethodsList
    {
        public static AML Instance { get; private set; } = new();

        public static void Save()
        {
            var automationMethodsXml = XmlSerializerHelper.SerializeValue(Instance);
            File.WriteAllText("C:\\Temps\\AMs.xml", automationMethodsXml);
        }

        public static void Load()
        {
            var automationMethodsXml = File.ReadAllText("C:\\Temps\\AMs.xml");

            Instance = XmlSerializerHelper.DeserializeValue(automationMethodsXml, typeof(AML)) as AML;
        }
    }
}
