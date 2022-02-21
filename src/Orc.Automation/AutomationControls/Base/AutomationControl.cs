namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Catel.Caching;
    using Catel.IoC;
    using Catel.Windows.Interactivity;

    public class AutomationControl<TControlModel, TMap> : AutomationControl<TControlModel>
        where TControlModel : AutomationControlModel
        where TMap : AutomationBase
    {
        public AutomationControl(AutomationElement element, ControlType controlType)
            : base(element, controlType)
        {
        }

        public AutomationControl(AutomationElement element)
            : base(element)
        {
        }

        protected TMap Map => Map<TMap>();
    }

    public class AutomationControl<TControlModel> : AutomationControl
        where TControlModel : AutomationControlModel
    {
        public AutomationControl(AutomationElement element, ControlType controlType)
            : base(element, controlType)
        {
        }

        public AutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public TControlModel Current => Model<TControlModel>();
    }

    public class AutomationControl : AutomationBase
    {
        private readonly CacheStorage<Type, AutomationControlModel> _models = new();

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
            EventHelper.RaiseEvent(this, args.EventName, args);
        }
        
        public object Execute<TMethodType>(params object[] parameters)
            where TMethodType : IAutomationMethodRun
        {
            return Access.Execute<TMethodType>(parameters);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            return Access.Execute(methodName, parameters);
        }

        public TControlModel Model<TControlModel>()
            where TControlModel : AutomationControlModel
        {
            return (TControlModel)_models.GetFromCacheOrFetch(typeof(TControlModel), () => this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TControlModel>(Access));
        }
    }
}
