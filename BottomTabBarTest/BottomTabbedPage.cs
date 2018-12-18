using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BottomTabBarTest
{
    public class BottomTabbedPage : TabbedPage
    {
        public bool Labels { get; set; }

        public Dictionary<int, TabData> Tabs = new Dictionary<int, TabData>();

        public BottomTabbedPage()
        {
            Tabs.Add(0, new TabData() { BadgeColor = Color.RoyalBlue , BadgeCaption = 5 });
        }
    }
}