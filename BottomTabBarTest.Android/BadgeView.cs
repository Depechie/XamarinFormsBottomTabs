using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BottomTabBarTest.Droid
{
    public class BadgeView : TextView
    {
        public Color BadgeColor
        {
            get => ((ShapeDrawable)Background).Paint.Color;
            set
            {
                ((ShapeDrawable)Background).Paint.Color = value;
                Background.InvalidateSelf();
            }
        }

        public string BadgeCaption
        {
            get => Text;
            set
            {
                Text = value;
                if (Visibility == ViewStates.Visible && string.IsNullOrEmpty(value))
                    SetVisible(false);
                else if (Visibility == ViewStates.Gone && !string.IsNullOrEmpty(value))
                    SetVisible(true);
            }
        }

        public BadgeView(Context context) : base(context)
        {
            Init();
        }

        private void Init()
        {
            SetTextSize(ComplexUnitType.Dip, 11);
            SetTextColor(Color.White);
            Gravity = GravityFlags.Center;
            Background = CreateBackgroundShape();
            SetWidth(DipToPixels(18));
            SetHeight(DipToPixels(18));

            var layoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Top;
            layoutParameters.SetMargins(0, DipToPixels(5), DipToPixels(15), 0);

            LayoutParameters = layoutParameters;
        }

        private ShapeDrawable CreateBackgroundShape()
        {
            ShapeDrawable background = new ShapeDrawable(new OvalShape());
            background.Paint.Color = Color.Red;
            background.Paint.StrokeWidth = 0;

            return background;
        }

        private int DipToPixels(int dip)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
        }

        private void SetVisible(bool visible)
        {
            Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
            Invalidate();
        }
    }
}