using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BottomTabBarTest
{
    public class TabData : INotifyPropertyChanged
    {
        private int _badgeCaption;
        public int BadgeCaption
        {
            get => _badgeCaption;
            set
            {
                _badgeCaption = value;
                OnPropertyChanged();
            }
        }

        private Color _badgeColor;
        public Color BadgeColor
        {
            get => _badgeColor;
            set
            {
                _badgeColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}