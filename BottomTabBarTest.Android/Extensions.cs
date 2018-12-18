using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
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

        public static void SetShiftMode(this BottomNavigationView bottomNavigationView, bool enableShiftMode, bool enableItemShiftMode)
        {
            try
            {
                using (var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView)
                {
                    if (menuView == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Unable to find BottomNavigationMenuView");
                        return;
                    }

                    var shiftMode = menuView.Class.GetDeclaredField("mShiftingMode");
                    shiftMode.Accessible = true;
                    shiftMode.SetBoolean(menuView, enableShiftMode);
                    shiftMode.Accessible = false;
                    shiftMode.Dispose();

                    for (int i = 0; i < menuView.ChildCount; i++)
                    {
                        var child = menuView.GetChildAt(i);
                        var item = child as BottomNavigationItemView;
                        if (item != null)
                        {
                            item.SetShiftingMode(enableItemShiftMode);
                            item.SetChecked(item.ItemData.IsChecked);
                        }

                        child.Dispose();
                    }

                    menuView.UpdateMenuView();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to set shift mode: {ex}");
            }
        }
    }
}