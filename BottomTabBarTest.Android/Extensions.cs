using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;

namespace BottomTabBarTest.Droid
{
    public static class Extensions
    {
        public static List<View> GetViewsByType(this View view, Type viewType = null)
        {
            if (!(view is ViewGroup group))
                return new List<View>();

            var result = new List<View>();

            for (int i = 0; i < group.ChildCount; i++)
            {
                var child = group.GetChildAt(i);
                result.AddRange(child.GetViewsByType(viewType));

                if (viewType == null || child.GetType() == viewType)
                    result.Add(child);
            }

            return result.Distinct().ToList();
        }

        public static void FindAndRemoveById(this View view, int id)
        {
            var childView = view.FindViewById(id);
            ((ViewGroup)childView.Parent).RemoveView(childView);
        }
    }
}