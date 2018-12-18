using Xamarin.Forms;

namespace BottomTabBarTest
{
    public partial class Tab1Page : ContentPage
    {
        public Tab1Page()
        {
            InitializeComponent();
        }

        private void OnHideClicked(object sender, System.EventArgs e)
        {
            ((BottomTabbedPage)App.Current.MainPage).Tabs[0].BadgeCaption = 0;
        }

        private void OnIncrementClicked(object sender, System.EventArgs e)
        {
            ++((BottomTabbedPage)App.Current.MainPage).Tabs[0].BadgeCaption;
        }
    }
}