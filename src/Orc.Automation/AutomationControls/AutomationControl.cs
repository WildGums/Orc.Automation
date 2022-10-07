namespace Orc.Automation
{
    using System;
    using System.Windows;
    using System.Windows.Automation;
    using Catel.Caching;
    using Catel.IoC;
    using Catel.Reflection;
    using Microsoft.Xaml.Behaviors;

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

        protected TValue? GetMapValue<TValue>(object? source, string propertyName)
        {
            if (source is null)
            {
                return default;
            }

            return PropertyHelper.GetPropertyValue<TValue>(source, propertyName);
        }

        protected void SetMapValue<TValue>(object? source, string propertyName, TValue value)
        {
            if (source is null)
            {
                return;
            }

            PropertyHelper.SetPropertyValue(source, propertyName, value);
        }
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
            ArgumentNullException.ThrowIfNull(controlType);

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
        }

        //TODO:Vladimir: Temporary solution / should be OK if we move this subscribition to ctor; 
        //But construction would be a little slower (but clearer)...so for now i prefer to explisitly activate
        public void Activate()
        {
            Access.AutomationEvent += OnEvent;
        }

        #region Automation Properties
        public AutomationElement.AutomationElementInformation AutomationProperties => Element.Current;

        public bool IsPart { get; protected set; }

        protected AutomationElementAccessor Access { get; private set; }
        public bool IsEnabled => AutomationProperties.IsEnabled;
        public virtual Rect BoundingRectangle => Element.Current.BoundingRectangle;
        #endregion

        public void AttachBehavior<TBehavior>()
            where TBehavior : Behavior
        {
            Access.AttachBehavior(typeof(TBehavior));
        }

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
            control.IsPart = true;

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
            return Access.ExecuteAutomationMethod<TMethodType>(parameters);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            return Access.Execute(methodName, parameters);
        }

        public virtual TControlModel Model<TControlModel>()
            where TControlModel : AutomationControlModel
        {
            return (TControlModel)_models.GetFromCacheOrFetch(typeof(TControlModel), CreateModel<TControlModel>);
        }

        protected virtual TControlModel CreateModel<TControlModel>()
        {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
            return this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TControlModel>(Access);
#pragma warning restore IDISP004 // Don't ignore created IDisposable
        }

#pragma warning disable CS0067
        public event EventHandler<EventArgs> AutomationMessageSent;
#pragma warning restore CS0067
    }
}
