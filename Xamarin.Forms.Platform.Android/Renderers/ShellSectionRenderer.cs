using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Util;
using Android.Widget;
using Xamarin.Forms.Platform.Android.AppCompat;
using AView = Android.Views.View;
using Fragment = Android.Support.V4.App.Fragment;
using LP = Android.Views.ViewGroup.LayoutParams;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Xamarin.Forms.Platform.Android
{
	public class Tabsi : TabLayout
	{
		protected Tabsi(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public Tabsi(Context context) : base(context)
		{
		}

		public Tabsi(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public Tabsi(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

		public override void AddTab(Tab tab)
		{
			base.AddTab(tab);
		}

		public override void AddTab(Tab tab, bool setSelected)
		{
			base.AddTab(tab, setSelected);
		}

		public override void AddTab(Tab tab, int position)
		{
			base.AddTab(tab, position);
		}

		public override void AddTab(Tab tab, int position, bool setSelected)
		{
			//FrameLayout frameLayout = new FrameLayout(this.Context);
			//LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LP.WrapContent, LP.WrapContent);
			//frameLayout.LayoutParameters = layoutParams;

			//LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

			//TextView tabText = (TextView)inflater.Inflate(Resource.Layout.design_layout_tab_text, frameLayout);
			//tabText.Text = "1";
			//tabText.Id = Context.Resources.GetIdentifier("text1", "id", Context.PackageName);
			//frameLayout.AddView(tabText);

			//AView tabIcon = inflater.Inflate(Resource.Layout.design_layout_tab_icon, frameLayout, true);
			//tabIcon.Id = Context.Resources.GetIdentifier("icon", "id", Context.PackageName);

			//frameLayout.AddView(tabIcon);

			//var textView = new TextView(this.Context) { Id = Context.Resources.GetIdentifier("text1", "id", Context.PackageName), Text = "Test" };
			//textVIew

			//textView.LayoutParameters = layoutParams;

			//linearLayout.AddView(textView);
			tab.View.SetClipChildren(false);
			tab.View.SetClipToPadding(false);
			tab.View.ApplyBadge(Color.Red, "11", Color.Default);

			//AView findViewById = tab.View.FindViewById(global::Android.Resource.Id.Text1);

			//TextView textView = tab.View.GetChildrenOfType<TextView>().Single();
			//var indexOfTextView = tab.View.IndexOfChild(textView);

			//tab.View.RemoveView(textView);
			//tab.View.AddView(BadgeHelper.Merge(textView, Color.Red, "Text", Color.Default), indexOfTextView);

			//tab.SetCustomView(Resource.Layout.CustomTabView);

			//var textView = tab.CustomView.FindViewById<TextView>(global::Android.Resource.Id.Text1);
			//textView.SetTextColor(TabTextColors);

			//((ViewGroup)tab.CustomView).ApplyBadge(Color.Red, "1", Color.Default);

			//BadgeHelper.Merge((global::Android.Widget.RelativeLayout)tab.CustomView, textView, Color.Red, "1",
			//	Color.Default);

			//((ViewGroup)tab.CustomView).ApplyBadge2(Color.Red, "1", Color.Default);

			base.AddTab(tab, position, setSelected);
		}

		[Obsolete("deprecated")]
		public override void SetTabsFromPagerAdapter(PagerAdapter adapter)
		{
			base.SetTabsFromPagerAdapter(adapter);

			for (int i = 0; i < this.TabCount; i++)
			{
				Tab tabAt = this.GetTabAt(i);
				//((LinearLayout)tabAt.View.GetChildAt(0)).ApplyBadge(Color.Green, "1", Color.Default);
			}
		}

		public override void SetupWithViewPager(ViewPager viewPager)
		{
			base.SetupWithViewPager(viewPager);
		}

		public override void SetupWithViewPager(ViewPager viewPager, bool autoRefresh)
		{
			base.SetupWithViewPager(viewPager, autoRefresh);
		}
	}

	public class ShellSectionRenderer : Fragment, IShellSectionRenderer, ViewPager.IOnPageChangeListener, AView.IOnClickListener, IShellObservableFragment, IAppearanceObserver
	{
		#region IOnPageChangeListener

		void ViewPager.IOnPageChangeListener.OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
		{
		}

		void ViewPager.IOnPageChangeListener.OnPageScrollStateChanged(int state)
		{
		}

		void ViewPager.IOnPageChangeListener.OnPageSelected(int position)
		{
			if (_selecting)
				return;

			// TODO : Find a way to make this cancellable
			var shellSection = ShellSection;
			var shellContent = shellSection.Items[position];

			if (shellContent == shellSection.CurrentItem)
				return;

			var stack = shellSection.Stack.ToList();
			bool result = ((IShellController)_shellContext.Shell).ProposeNavigation(ShellNavigationSource.ShellContentChanged,
				(ShellItem)shellSection.Parent, shellSection, shellContent, stack, true);

			if (result)
			{
				var page = ((IShellContentController)shellContent).Page;
				if (page == null)
					throw new ArgumentNullException(nameof(page), "Shell Content Page is Null");

				ShellSection.SetValueFromRenderer(ShellSection.CurrentItemProperty, shellContent);

				_toolbarTracker.Page = page;
			}
			else
			{
				_selecting = true;
				var index = ShellSection.Items.IndexOf(ShellSection.CurrentItem);

				// Android doesn't really appreciate you calling SetCurrentItem inside a OnPageSelected callback.
				// It wont crash but the way its programmed doesn't really anticipate re-entrancy around that method
				// and it ends up going to the wrong location. Thus we must invoke.

				Device.BeginInvokeOnMainThread(() =>
				{
					_viewPager.SetCurrentItem(index, false);
					_selecting = false;
				});
			}
		}

		#endregion IOnPageChangeListener

		#region IAppearanceObserver

		void IAppearanceObserver.OnAppearanceChanged(ShellAppearance appearance)
		{
			if (appearance == null)
				ResetAppearance();
			else
				SetAppearance(appearance);
		}

		#endregion IAppearanceObserver

		#region IOnClickListener

		void AView.IOnClickListener.OnClick(AView v)
		{
		}

		#endregion IOnClickListener

		readonly IShellContext _shellContext;
		AView _rootView;
		bool _selecting;
		Tabsi _tablayout;

		IShellTabLayoutAppearanceTracker _tabLayoutAppearanceTracker;
		Toolbar _toolbar;
		IShellToolbarAppearanceTracker _toolbarAppearanceTracker;
		IShellToolbarTracker _toolbarTracker;
		ViewPager _viewPager;

		public ShellSectionRenderer(IShellContext shellContext)
		{
			_shellContext = shellContext;
		}

		protected ShellSectionRenderer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public event EventHandler AnimationFinished;

		Fragment IShellObservableFragment.Fragment => this;
		public ShellSection ShellSection { get; set; }

		public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var shellSection = ShellSection;
			if (shellSection == null)
				return null;

			var root = inflater.Inflate(Resource.Layout.RootLayout, null).JavaCast<CoordinatorLayout>();

			_toolbar = root.FindViewById<Toolbar>(Resource.Id.main_toolbar);
			var scrollview = root.FindViewById<NestedScrollView>(Resource.Id.main_scrollview);
			_tablayout = root.FindViewById<Tabsi>(Resource.Id.main_tablayout);

			_tablayout.SetClipChildren(false);
			_tablayout.SetClipToPadding(false);

			ViewGroup realTabs = ((ViewGroup)_tablayout.GetChildAt(0));
			realTabs.SetClipToPadding(false);
			realTabs.SetClipChildren(false);

			if (realTabs.GetChildAt(0) is ViewGroup) { // <-- This is the intermediate parent ViewGroup
				((ViewGroup)realTabs.GetChildAt(0)).SetClipToPadding(false);
				((ViewGroup)realTabs.GetChildAt(0)).SetClipChildren(false);
			}

			_viewPager = new FormsViewPager(Context)
			{
				LayoutParameters = new LP(LP.MatchParent, LP.MatchParent),
			};

			_viewPager.AddOnPageChangeListener(this);
			_viewPager.Id = Platform.GenerateViewId();

			_viewPager.Adapter = new ShellFragmentPagerAdapter(shellSection, ChildFragmentManager);
			_viewPager.OverScrollMode = OverScrollMode.Never;

			_tablayout.SetupWithViewPager(_viewPager);

			var currentPage = ((IShellContentController)shellSection.CurrentItem).GetOrCreateContent();
			var currentIndex = ShellSection.Items.IndexOf(ShellSection.CurrentItem);

			_toolbarTracker = _shellContext.CreateTrackerForToolbar(_toolbar);
			_toolbarTracker.Page = currentPage;

			_viewPager.CurrentItem = currentIndex;
			scrollview.AddView(_viewPager);

			if (shellSection.Items.Count == 1)
			{
				_tablayout.Visibility = ViewStates.Gone;
			}

			_tabLayoutAppearanceTracker = _shellContext.CreateTabLayoutAppearanceTracker(ShellSection);
			_toolbarAppearanceTracker = _shellContext.CreateToolbarAppearanceTracker();

			HookEvents();

			scrollview.Dispose();

			return _rootView = root;
		}

		// Use OnDestroy instead of OnDestroyView because OnDestroyView will be
		// called before the animation completes. This causes tons of tiny issues.
		public override void OnDestroy()
		{
			base.OnDestroy();

			if (_rootView != null)
			{
				UnhookEvents();

				var adapter = _viewPager.Adapter;
				_viewPager.Adapter = null;
				adapter.Dispose();

				_toolbarAppearanceTracker.Dispose();
				_tabLayoutAppearanceTracker.Dispose();
				_viewPager.RemoveOnPageChangeListener(this);
				_rootView.Dispose();
				_toolbarTracker.Dispose();

				_tablayout.Dispose();
				_toolbar.Dispose();
				_viewPager.Dispose();
				_rootView.Dispose();
			}

			_toolbarAppearanceTracker = null;
			_tabLayoutAppearanceTracker = null;
			_toolbarTracker = null;
			_toolbar = null;
			_tablayout = null;
			_rootView = null;
			_viewPager = null;
		}

		protected virtual void OnAnimationFinished(EventArgs e)
		{
			AnimationFinished?.Invoke(this, e);
		}

		protected virtual void OnItemsCollectionChagned(object sender, NotifyCollectionChangedEventArgs e) =>
			_tablayout.Visibility = (ShellSection.Items.Count > 1) ? ViewStates.Visible : ViewStates.Gone;

		protected virtual void OnShellItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_rootView == null)
				return;

			if (e.PropertyName == ShellSection.CurrentItemProperty.PropertyName)
			{
				var newIndex = ShellSection.Items.IndexOf(ShellSection.CurrentItem);

				if (newIndex >= 0)
				{
					_viewPager.CurrentItem = newIndex;
				}
			}
		}

		protected virtual void ResetAppearance()
		{
			_toolbarAppearanceTracker.ResetAppearance(_toolbar, _toolbarTracker);
			_tabLayoutAppearanceTracker.ResetAppearance(_tablayout);
		}

		protected virtual void SetAppearance(ShellAppearance appearance)
		{
			_toolbarAppearanceTracker.SetAppearance(_toolbar, _toolbarTracker, appearance);
			_tabLayoutAppearanceTracker.SetAppearance(_tablayout, appearance);
		}

		void HookEvents()
		{
			((INotifyCollectionChanged)ShellSection.Items).CollectionChanged += OnItemsCollectionChagned;
			((IShellController)_shellContext.Shell).AddAppearanceObserver(this, ShellSection);
			ShellSection.PropertyChanged += OnShellItemPropertyChanged;
		}

		void UnhookEvents()
		{
			((INotifyCollectionChanged)ShellSection.Items).CollectionChanged -= OnItemsCollectionChagned;
			((IShellController)_shellContext.Shell).RemoveAppearanceObserver(this);
			ShellSection.PropertyChanged -= OnShellItemPropertyChanged;
		}
	}
}