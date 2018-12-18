using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using BottomTabBarTest;
using BottomTabBarTest.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(BottomTabbedPage), typeof(BottomTabbedPageRenderer))]
namespace BottomTabBarTest.Droid
{
    public class BottomTabbedPageRenderer : TabbedPageRenderer
    {
        private Dictionary<TabData, BottomNavigationItemView> _tabViews = new Dictionary<TabData, BottomNavigationItemView>();
        private int _badgeId;
        private bool _bottomBarInit = false;

        public BottomTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            BottomTabbedPage formsPage = (BottomTabbedPage)Element;

            if (!_bottomBarInit && !formsPage.Labels)
            {
                var childViews = ViewGroup.GetViewsByType(typeof(BottomNavigationItemView));
                int dpAsPixels = 0;

                foreach (var childView in childViews)
                {
                    if (dpAsPixels == 0)
                    {
                        var imageIcon = childView.FindViewById(Resource.Id.icon);

                        var parentHeightHalf = ((ViewGroup)childView.Parent).MeasuredHeight / 2;
                        var iconHeightHalf = imageIcon.MeasuredHeight / 2;
                        var iconTop = imageIcon.Top;

                        dpAsPixels = parentHeightHalf - iconHeightHalf - iconTop;
                    }

                    childView.SetPadding(childView.PaddingLeft, dpAsPixels, childView.PaddingRight, childView.PaddingBottom);
                }

                _bottomBarInit = true;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            BottomTabbedPage formsPage = (BottomTabbedPage)Element;

            if (!formsPage.Labels)
            {
                var childViews = ViewGroup.GetViewsByType(typeof(BottomNavigationItemView));

                foreach (var childView in childViews)
                {
                    childView.FindAndRemoveById(Resource.Id.largeLabel);
                    childView.FindAndRemoveById(Resource.Id.smallLabel);
                }
            }

            if (e.NewElement != null)
            {
                var relativeLayout = this.GetChildAt(0) as Android.Widget.RelativeLayout;
                if (relativeLayout != null)
                {
                    var bottomNavigationView = relativeLayout.GetChildAt(1) as BottomNavigationView;
                    bottomNavigationView.SetShiftMode(false, false);

                    BottomNavigationMenuView bottomNavigationMenuView = (BottomNavigationMenuView)bottomNavigationView.GetChildAt(0);

                    int tabCount = formsPage.Tabs.Count;

                    for (int i = 0; i < tabCount; i++)
                    {
                        var tabData = formsPage.Tabs[0];
                        BottomNavigationItemView tabItemView = (BottomNavigationItemView)bottomNavigationMenuView.GetChildAt(i);

                        if (tabData.BadgeCaption > 0)
                        {
                            if (_badgeId == 0)
                                _badgeId = Android.Views.View.GenerateViewId();

                            TextView badgeTextView = new BadgeView(Context) { Id = _badgeId, BadgeCaption = tabData.BadgeCaption.ToString(), BadgeColor = tabData.BadgeColor.ToAndroid() };

                            tabData.PropertyChanged += (sender, args) =>
                            {
                                TabData currentTabData = (TabData)sender;
                                BottomNavigationItemView currentTabItemView = _tabViews[currentTabData];

                                BadgeView currentBadgeTextView = currentTabItemView.FindViewById(_badgeId) as BadgeView;

                                if (currentBadgeTextView != null)
                                {
                                    currentBadgeTextView.BadgeColor = currentTabData.BadgeColor.ToAndroid();
                                    currentBadgeTextView.BadgeCaption = currentTabData.BadgeCaption > 0 ? currentTabData.BadgeCaption.ToString() : string.Empty;
                                }
                            };

                            tabItemView.AddView(badgeTextView);
                            _tabViews.Add(tabData, tabItemView);
                        }
                    }
                }
            }
        }
    }

    public static class AndroidViewUtils
    {
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