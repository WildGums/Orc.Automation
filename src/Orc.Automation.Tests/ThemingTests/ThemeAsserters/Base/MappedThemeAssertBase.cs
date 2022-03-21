namespace Orc.Automation.Tests
{
    using Catel;

    public abstract class MappedThemeAssertBase<TAssert, TElement, TMap> : ThemeAssertBase<TAssert, TElement>
        where TAssert : ThemeAssertBase<TAssert, TElement>, new()
        where TElement : AutomationControl
        where TMap : AutomationBase
    {
        protected TMap _map;

        protected override void VerifyThemeColors(TElement element)
        {
            using (new DisposableToken(this, _ => _map = element.Map<TMap>(), _ => _map = null))
            {
                base.VerifyThemeColors(element);
            }
        }
    }
}
