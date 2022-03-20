﻿namespace Orc.Automation
{
    using Catel;
    using Controls;

    public static class ListExtensions
    {
        /// <summary>
        /// Get virtualized item...
        /// </summary>
        /// <param name="list"></param>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public static ListItem TryGetVirtualizedItem(this List list, int itemIndex)
        {
            Argument.IsNotNull(() => list);

            var id = list.Execute<GotoVirtualizedItemAutomationMethodRun>(itemIndex);
            return string.IsNullOrWhiteSpace(id?.ToString()) ? null : list.Find<ListItem>(id: id.ToString());
        }

        public static ListItem SelectVirtualizedItem(this List list, int itemIndex)
        {
            var item = list.TryGetVirtualizedItem(itemIndex);
            if (item is not null)
            {
                item.Select();
                return item;
            }

            return null;
        }
    }
}
