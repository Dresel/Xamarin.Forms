using System;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using LP = Android.Views.ViewGroup.LayoutParams;
using RL = Android.Widget.RelativeLayout;

namespace Xamarin.Forms.Platform.Android
{
	internal static class BadgeHelper
	{
		public static void ApplyBadge(this TabLayout.TabView tabView, Color color, string text,
			Color textColor)
		{
			if (!tabView.GetChildrenOfType<BadgeFrameLayout>().Any())
			{
				//FrameLayout frameLayout = new FrameLayout(tabView.Context);
				//var frameLayoutParameters = new FrameLayout.LayoutParams(LP.MatchParent, LP.MatchParent);
				//frameLayout.LayoutParameters = frameLayoutParameters;

				TextView tabTextView = tabView.GetChildrenOfType<TextView>().Single();
				tabTextView.Id = global::Android.Views.View.GenerateViewId();

				var tabTextViewLayoutParameters =  new RL.LayoutParams(LP.WrapContent, LP.WrapContent);
				//tabTextViewParameters.Gravity = GravityFlags.Center;
				
				tabTextViewLayoutParameters.AddRule(LayoutRules.CenterInParent);
				tabTextView.LayoutParameters = tabTextViewLayoutParameters;

				var indexOfTextView = tabView.IndexOfChild(tabTextView);
				tabView.RemoveView(tabTextView);

				//frameLayout.AddView(tabTextView);

				RL relativeLayout = new RL(tabView.Context);
				//relativeLayout.SetPadding(
				//	(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 12, tabView.Context.Resources.DisplayMetrics),
				//	(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 0, tabView.Context.Resources.DisplayMetrics),
				//	(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 12, tabView.Context.Resources.DisplayMetrics),
				//	(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 0, tabView.Context.Resources.DisplayMetrics));

				relativeLayout.SetClipChildren(false);
				relativeLayout.SetClipToPadding(false);

				var relativeLayoutParameters =
					new LinearLayout.LayoutParams(LP.MatchParent, LP.MatchParent);
				//relativeLayoutParameters.Gravity = GravityFlags.Right;
				relativeLayout.LayoutParameters = relativeLayoutParameters;

				//relativeLayout.Visibility = !string.IsNullOrEmpty(text) ? ViewStates.Visible : ViewStates.Invisible;

				relativeLayout.AddView(tabTextView);

				var space = new Space(tabView.Context);

				space.Id = global::Android.Views.View.GenerateViewId();

				var spaceLayoutParameters =
					new RL.LayoutParams((int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6,
					tabView.Context.Resources.DisplayMetrics), 0);
				spaceLayoutParameters.AddRule(LayoutRules.CenterVertical);
				//spaceLayoutParameters.AddRule(LayoutRules.RightOf, tabTextView.Id);
				space.LayoutParameters = spaceLayoutParameters;

				space.Visibility = ViewStates.Invisible;

				relativeLayout.AddView(space);

				FrameLayout frameLayout = new FrameLayout(tabView.Context);
				frameLayout.SetClipChildren(false);
				frameLayout.SetClipToPadding(false);

				var frameLayoutParameters =
					new RL.LayoutParams(LP.WrapContent, LP.WrapContent);
				frameLayoutParameters.AddRule(LayoutRules.Above, space.Id);
				frameLayoutParameters.AddRule(LayoutRules.RightOf, tabTextView.Id);

				frameLayout.LayoutParameters = frameLayoutParameters;

				var badgeFrameLayout = new BadgeFrameLayout(tabView.Context);
				badgeFrameLayout.SetClipChildren(false);
				badgeFrameLayout.SetClipToPadding(false);

				var badgeLayoutParameters =
					new RL.LayoutParams(LP.WrapContent, LP.WrapContent);
				//badgeLayoutParameters.AddRule(LayoutRules.Above, space.Id);
				//badgeLayoutParameters.AddRule(LayoutRules.RightOf, tabTextView.Id);
				//badgeLayoutParameters.LeftMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, -4,
				//	tabView.Context.Resources.DisplayMetrics);

				//badgeLayoutParameters.TopMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4,
				//	tabView.Context.Resources.DisplayMetrics);

				badgeFrameLayout.LayoutParameters = badgeLayoutParameters;

				badgeFrameLayout.SetPadding(
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, tabView.Context.Resources.DisplayMetrics),
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 2, tabView.Context.Resources.DisplayMetrics),
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, tabView.Context.Resources.DisplayMetrics),
					(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 2, tabView.Context.Resources.DisplayMetrics));

				var badgeFrameLayoutBackground = new PaintDrawable();

				badgeFrameLayoutBackground.Paint.Color = color.IsDefault ? Color.FromRgb(255, 59, 48).ToAndroid() : color.ToAndroid();

				badgeFrameLayoutBackground.Shape = new RectShape();
				badgeFrameLayoutBackground.SetCornerRadius(TypedValue.ApplyDimension(ComplexUnitType.Dip, 8,
					tabView.Context.Resources.DisplayMetrics));

				badgeFrameLayout.Background = badgeFrameLayoutBackground;

				var textView = new TextView(tabView.Context);

				textView.Text = text;
				textView.SetTextColor(textColor.IsDefault ? Color.White.ToAndroid() : textColor.ToAndroid());
				textView.SetTextSize(ComplexUnitType.Sp, 10);

				var textViewLayoutParameters =
					new FrameLayout.LayoutParams(LP.WrapContent, LP.WrapContent);
				textViewLayoutParameters.Gravity = GravityFlags.Center;

				textView.LayoutParameters = textViewLayoutParameters;

				badgeFrameLayout.AddView(textView);
				frameLayout.AddView(badgeFrameLayout);

				relativeLayout.AddView(frameLayout);

				//frameLayout.AddView(relativeLayout);
				tabView.AddView(relativeLayout);
			}
			else
			{
				//RL relativeLayout = tabView.GetChildrenOfType<RL>().Single();
				//relativeLayout.Visibility = !string.IsNullOrEmpty(text) ? ViewStates.Visible : ViewStates.Invisible;

				//BadgeFrameLayout badgeFrameLayout = (BadgeFrameLayout)relativeLayout.GetChildAt(1);
				//((PaintDrawable)badgeFrameLayout.Background).Paint.Color = color.IsDefault ? Color.FromRgb(255, 59, 48).ToAndroid() : color.ToAndroid();

				//TextView textView = (TextView)badgeFrameLayout.GetChildAt(0);
				//textView.Text = text;
				//textView.SetTextColor(textColor.IsDefault ? Color.White.ToAndroid() : textColor.ToAndroid());
			}
		}

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


		public static RL Merge(this TextView view, Color color, string text, Color textColor)
		{
			RL relativeLayout = new RL(view.Context);

			var relativeLayoutParameters =
				new RL.LayoutParams(LP.MatchParent, LP.MatchParent);
			relativeLayout.LayoutParameters = relativeLayoutParameters;

			//relativeLayout.Visibility = !string.IsNullOrEmpty(text) ? ViewStates.Visible : ViewStates.Invisible;

			//var space = new Space(view.Context);

			//space.Id = global::Android.Views.View.GenerateViewId();

			//var spaceLayoutParameters =
			//	new RL.LayoutParams(0, 0);
			//spaceLayoutParameters.AddRule(LayoutRules.CenterHorizontal);
			//spaceLayoutParameters.AddRule(LayoutRules.CenterVertical);
			//space.LayoutParameters = spaceLayoutParameters;

			//space.Visibility = ViewStates.Invisible;

			//relativeLayout.AddView(space);
			relativeLayout.AddView(view);

			var badgeFrameLayout = new BadgeFrameLayout(view.Context);

			var badgeLayoutParameters =
				new RL.LayoutParams(LP.WrapContent, LP.WrapContent);
			badgeLayoutParameters.AddRule(LayoutRules.RightOf, view.Id);
			badgeLayoutParameters.AddRule(LayoutRules.Above, view.Id);

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

			return relativeLayout;
		}

		public static void ApplyBadge2(this ViewGroup view, Color color, string text,
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
				spaceLayoutParameters.AddRule(LayoutRules.CenterVertical);
				space.LayoutParameters = spaceLayoutParameters;

				space.Visibility = ViewStates.Invisible;

				relativeLayout.AddView(space);

				var badgeFrameLayout = new BadgeFrameLayout(view.Context);

				var badgeLayoutParameters =
					new RL.LayoutParams(LP.WrapContent, LP.WrapContent);
				badgeLayoutParameters.AddRule(LayoutRules.RightOf, space.Id);
				badgeLayoutParameters.AddRule(LayoutRules.Above, space.Id);

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