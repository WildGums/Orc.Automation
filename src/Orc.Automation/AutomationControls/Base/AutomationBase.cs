namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel.Caching;

    /// <summary>
    /// This is a base class for Automation proxies, such as Maps and Controls
    /// 
    /// It wraps <see cref="AutomationElement"/> and provides additional access functions
    /// such as:
    /// Search (<see cref="Orc.Automation.By"/>),
    /// Construct (<see cref="AutomationFactory"/>),
    /// Map(<see cref="Map{T}"/>)
    /// </summary>
    public abstract class AutomationBase
    {
        private readonly CacheStorage<Type, AutomationBase> _maps = new();

        public AutomationBase(AutomationElement element)
        {
            ArgumentNullException.ThrowIfNull(element);

            Element = element;
        }

        public AutomationElement Element { get; }
        public virtual By By => new (Element);
        protected AutomationFactory Factory { get; } = new();

        public T Map<T>()
            where T : AutomationBase
        {
            return (T)_maps.GetFromCacheOrFetch(typeof(T), () => Factory.Create<T>(this));
        }
    }
}
