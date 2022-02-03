namespace Orc.Automation
{
    using System.Windows.Automation;
    using Catel;
    using Catel.Windows.Interactivity;

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

        protected AutomationElementAccessor Access { get; private set; }

        protected virtual void OnEvent(object sender, AutomationEventArgs args)
        {
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
    }
}
