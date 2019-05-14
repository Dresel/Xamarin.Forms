using System;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LP = Android.Views.ViewGroup.LayoutParams;
using RL = Android.Widget.RelativeLayout;

namespace Xamarin.Forms.Platform.Android
{
	internal static class BadgeHelper
	{
		public static void ApplyBadge(this ViewGroup view, Color color, string text,
			Color textColor)
		{
			if (!view.GetChildrenOfType<RL>().Any())
			{
				RL relativeLayout = new RL(view.Context);

				var relativeLayoutParameters =
					new RL.LayoutParams(LP.MatchParent, LP.MatchParent);
				relativeLayout.LayoutParameters = relativeLayoutParameters;

				relativeLayout.Visibility = !string.IsNullOrEmpty(text) ? ViewStates.Visible : ViewStates.Invisible;

				var space = new Space(view.Context);

				space.Id = global::Android.Views.View.GenerateViewId();

				var spaceLayoutParameters =
					new RL.LayoutParams(0, 0);
				spaceLayoutParameters.AddRule(LayoutRules.CenterHorizontal);
				space.LayoutParameters = spaceLayoutParameters;

				space.Visibility = ViewStates.Invisible;

				relativeLayout.AddView(space);

				var badgeFrameLayout = new BadgeFrameLayout(view.Context);

				var badgeLayoutParameters =
					new RL.LayoutParams(LP.WrapContent, LP.WrapContent);
				badgeLayoutParameters.AddRule(LayoutRules.RightOf, space.Id);
				badgeLayoutParameters.AddRule(LayoutRules.AlignParentTop);

				badgeLayoutParameters.TopMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4,
					view.Context.Resources.DisplayMetrics);
				badgeLayoutParameters.MarginStart = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4,
					view.Context.Resources.DisplayMetrics);
				badgeLayoutParameters.LeftMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4,
					view.Context.Resources.DisplayMetrics);

				badgeFrameLayout.LayoutParameters = badgeLayoutParameters;

				badgeFrameLayout.SetPadding(
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, view.Context.Resources.DisplayMetrics),
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 2, view.Context.Resources.DisplayMetrics),
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, view.Context.Resources.DisplayMetrics),
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 2, view.Context.Resources.DisplayMetrics));

				var badgeFrameLayoutBackground = new PaintDrawable();

				badgeFrameLayoutBackground.Paint.Color = color.IsDefault ? Color.FromRgb(255, 59, 48).ToAndroid() : color.ToAndroid();

				badgeFrameLayoutBackground.Shape = new RectShape();
				badgeFrameLayoutBackground.SetCornerRadius(TypedValue.ApplyDimension(ComplexUnitType.Dip, 8,
					view.Context.Resources.DisplayMetrics));

				badgeFrameLayout.Background = badgeFrameLayoutBackground;

				var textView = new TextView(view.Context);

				textView.Text = text;
				textView.SetTextColor(textColor.IsDefault ? Color.White.ToAndroid() : textColor.ToAndroid());
				textView.SetTextSize(ComplexUnitType.Sp, 10);

				var textViewLayoutParameters =
					new FrameLayout.LayoutParams(LP.WrapContent, LP.WrapContent);
				textViewLayoutParameters.Gravity = GravityFlags.Center;

				textView.LayoutParameters = textViewLayoutParameters;

				badgeFrameLayout.AddView(textView);

				relativeLayout.AddView(badgeFrameLayout);

				view.AddView(relativeLayout);
			}
			else
			{
				RL relativeLayout = view.GetChildrenOfType<RL>().Single();
				relativeLayout.Visibility = !string.IsNullOrEmpty(text) ? ViewStates.Visible : ViewStates.Invisible;

				BadgeFrameLayout badgeFrameLayout = (BadgeFrameLayout)relativeLayout.GetChildAt(1);
				((PaintDrawable)badgeFrameLayout.Background).Paint.Color = color.IsDefault ? Color.FromRgb(255, 59, 48).ToAndroid() : color.ToAndroid();

				TextView textView = (TextView)badgeFrameLayout.GetChildAt(0);
				textView.Text = text;
				textView.SetTextColor(textColor.IsDefault ? Color.White.ToAndroid() : textColor.ToAndroid());
			}
		}

		internal class BadgeFrameLayout : FrameLayout
		{
			public BadgeFrameLayout(Context context) : base(context)
			{
			}

			public BadgeFrameLayout(Context context, IAttributeSet attrs) : base(context, attrs)
			{
			}

			public BadgeFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
				defStyleAttr)
			{
			}

			public BadgeFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context,
				attrs, defStyleAttr, defStyleRes)
			{
			}

			protected BadgeFrameLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
			{
			}

			protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

				var width = MeasuredWidth;
				var height = MeasuredHeight;

				base.OnMeasure(
					MeasureSpec.MakeMeasureSpec(width > height ? width : height, MeasureSpecMode.Exactly),
					MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly));
			}
		}
	}
}