namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;

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

        protected virtual T Part<T>(IPartFinder finder)
            where T : AutomationControl
        {
            var control = this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<T>(Element);
            if (control is null)
            {
                return default;
            }

            control.Access = Access.Part(finder);

            return control;
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
