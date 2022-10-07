namespace Orc.Automation
{
    using System;
    using System.Linq;
    using Controls;

    public static class TabExtensions
    {
        /// <summary>
        /// Find control in tab index. Use it Instead of TabScope.ExecuteAutomationMethod -> it should work faster
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="tab"></param>
        /// <param name="tabIndex"></param>
        /// <param name="searchFunc"></param>
        /// <param name="restorePreviousTab"></param>
        /// <returns></returns>
        public static TControl InTab<TControl>(this Tab tab, int tabIndex, Func<TControl> searchFunc, bool restorePreviousTab = false)
          //  where TControl : FrameworkElement
        {
            ArgumentNullException.ThrowIfNull(tab);
            ArgumentNullException.ThrowIfNull(searchFunc);

            var control = searchFunc.Invoke();
          //  if (control.IsVisible())
            if(control is not null)
            {
                return control;
            }

#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            return tab.TabScope(tabIndex).Execute(searchFunc);
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
        }

        public static void Select(this Tab tab, string name)
        {
            ArgumentNullException.ThrowIfNull(tab);

            tab.Items.FirstOrDefault(x => Equals(x.Header, name))?.TrySelect();
        }
    }
}
