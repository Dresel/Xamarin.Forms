﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Internals;
using System.ComponentModel;

namespace Xamarin.Forms
{
	[DebuggerDisplay("Title = {Title}, Route = {Route}")]
	public class BaseShellItem : NavigableElement, IPropertyPropagationController, IVisualController, IFlowDirectionController, ITabStopElement
	{
		public event EventHandler Appearing;
		public event EventHandler Disappearing;

		bool _hasAppearing;

		#region PropertyKeys

		internal static readonly BindablePropertyKey IsCheckedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsChecked), typeof(bool), typeof(BaseShellItem), false);

		#endregion PropertyKeys

		public static readonly BindableProperty FlyoutIconProperty =
			BindableProperty.Create(nameof(FlyoutIcon), typeof(ImageSource), typeof(BaseShellItem), null, BindingMode.OneTime);

		public static readonly BindableProperty IconProperty =
			BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(BaseShellItem), null, BindingMode.OneWay,
				propertyChanged: OnIconChanged);

		public static readonly BindableProperty IsCheckedProperty = IsCheckedPropertyKey.BindableProperty;

		public static readonly BindableProperty IsEnabledProperty =
			BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(BaseShellItem), true, BindingMode.OneWay);

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(BaseShellItem), null, BindingMode.OneTime);

		public static readonly BindableProperty TabIndexProperty =
			BindableProperty.Create(nameof(TabIndex),
							typeof(int),
							typeof(BaseShellItem),
							defaultValue: 0,
							propertyChanged: OnTabIndexPropertyChanged,
							defaultValueCreator: TabIndexDefaultValueCreator);

		public static readonly BindableProperty IsTabStopProperty =
			BindableProperty.Create(nameof(IsTabStop),
									typeof(bool),
									typeof(BaseShellItem),
									defaultValue: true,
									propertyChanged: OnTabStopPropertyChanged,
									defaultValueCreator: TabStopDefaultValueCreator);

		public static readonly BindableProperty BadgeTextProperty =
			BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(BaseShellItem), null, BindingMode.OneWay);

		public static readonly BindableProperty BadgeMoreTextProperty =
			BindableProperty.Create(nameof(BadgeMoreText), typeof(string), typeof(BaseShellItem), null, BindingMode.OneWay);

		public static readonly BindableProperty BadgeTextColorProperty =
			BindableProperty.Create(nameof(BadgeTextColor), typeof(Color), typeof(BaseShellItem), Color.Default, BindingMode.OneWay);

		public static readonly BindableProperty BadgeUnselectedTextColorProperty =
			BindableProperty.Create(nameof(BadgeUnselectedTextColor), typeof(Color), typeof(BaseShellItem), Color.Default, BindingMode.OneWay);

		public static readonly BindableProperty BadgeEffectiveTextColorProperty =
			BindableProperty.CreateReadOnly(nameof(BadgeEffectiveTextColor), typeof(Color), typeof(BaseShellItem), Color.Default, BindingMode.OneWay).BindableProperty;

		public static readonly BindableProperty BadgeColorProperty =
			BindableProperty.Create(nameof(BadgeColor), typeof(Color), typeof(BaseShellItem), Color.Default, BindingMode.OneWay);

		public static readonly BindableProperty BadgeUnselectedColorProperty =
			BindableProperty.Create(nameof(BadgeUnselectedColor), typeof(Color), typeof(BaseShellItem), Color.Default, BindingMode.OneWay);

		public static readonly BindableProperty BadgeEffectiveColorProperty =
			BindableProperty.CreateReadOnly(nameof(BadgeEffectiveColor), typeof(Color), typeof(BaseShellItem), Color.Default, BindingMode.OneWay).BindableProperty;

		static void OnTabIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
			((BaseShellItem)bindable).OnTabIndexPropertyChanged((int)oldValue, (int)newValue);

		static object TabIndexDefaultValueCreator(BindableObject bindable) =>
			((BaseShellItem)bindable).TabIndexDefaultValueCreator();

		static void OnTabStopPropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
			((BaseShellItem)bindable).OnTabStopPropertyChanged((bool)oldValue, (bool)newValue);

		static object TabStopDefaultValueCreator(BindableObject bindable) =>
			((BaseShellItem)bindable).TabStopDefaultValueCreator();

		public ImageSource FlyoutIcon
		{
			get { return (ImageSource)GetValue(FlyoutIconProperty); }
			set { SetValue(FlyoutIconProperty, value); }
		}

		public ImageSource Icon
		{
			get { return (ImageSource)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public bool IsChecked => (bool)GetValue(IsCheckedProperty);

		public bool IsEnabled
		{
			get { return (bool)GetValue(IsEnabledProperty); }
			set { SetValue(IsEnabledProperty, value); }
		}

		public string Route
		{
			get { return Routing.GetRoute(this); }
			set { Routing.SetRoute(this, value); }
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public int TabIndex
		{
			get => (int)GetValue(TabIndexProperty);
			set => SetValue(TabIndexProperty, value);
		}

		protected virtual void OnTabIndexPropertyChanged(int oldValue, int newValue) { }

		protected virtual int TabIndexDefaultValueCreator() => 0;

		public bool IsTabStop
		{
			get => (bool)GetValue(IsTabStopProperty);
			set => SetValue(IsTabStopProperty, value);
		}

		public string BadgeText
		{
			get { return (string)GetValue(BadgeTextProperty); }
			set { SetValue(BadgeTextProperty, value); }
		}

		public string BadgeMoreText
		{
			get { return (string)GetValue(BadgeMoreTextProperty); }
			set { SetValue(BadgeMoreTextProperty, value); }
		}

		public Color BadgeTextColor
		{
			get { return (Color)GetValue(BadgeTextColorProperty); }
			set { SetValue(BadgeTextColorProperty, value); }
		}

		public Color BadgeUnselectedTextColor
		{
			get { return (Color)GetValue(BadgeUnselectedTextColorProperty); }
			set { SetValue(BadgeUnselectedTextColorProperty, value); }
		}

		public Color BadgeColor
		{
			get { return (Color)GetValue(BadgeColorProperty); }
			set { SetValue(BadgeColorProperty, value); }
		}

		public Color BadgeUnselectedColor
		{
			get { return (Color)GetValue(BadgeUnselectedColorProperty); }
			set { SetValue(BadgeUnselectedColorProperty, value); }
		}

		public Color BadgeEffectiveTextColor => GetBadgeEffectiveTextColor(IsChecked);

		public Color GetBadgeEffectiveTextColor(bool isSelected)
		{
			if (isSelected)
			{
				return BadgeTextColor;
			}

			return !BadgeUnselectedTextColor.IsDefault ? BadgeUnselectedTextColor : BadgeTextColor;
		}

		public Color BadgeEffectiveColor => GetBadgeEffectiveColor(IsChecked);

		public Color GetBadgeEffectiveColor(bool isSelected)
		{
			if (isSelected)
			{
				return BadgeColor;
			}

			return !BadgeUnselectedColor.IsDefault ? BadgeUnselectedColor : BadgeColor;
		}

		internal virtual void SendAppearing()
		{
			if (_hasAppearing)
				return;

			_hasAppearing = true;
			OnAppearing();
			Appearing?.Invoke(this, EventArgs.Empty);
		}

		internal virtual void SendDisappearing()
		{
			if (!_hasAppearing)
				return;

			_hasAppearing = false;
			OnDisappearing();
			Disappearing?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnAppearing()
		{
		}

		protected virtual void OnDisappearing()
		{
		}

		internal void OnAppearing(Action action)
		{
			if (_hasAppearing)
				action();
			else
			{
				EventHandler eventHandler = null;
				eventHandler = (_, __) =>
				{
					this.Appearing -= eventHandler;
					action();
				};

				this.Appearing += eventHandler;
			}
		}

		protected virtual void OnTabStopPropertyChanged(bool oldValue, bool newValue) { }

		protected virtual bool TabStopDefaultValueCreator() => true;

		IVisual _effectiveVisual = Xamarin.Forms.VisualMarker.Default;
		IVisual IVisualController.EffectiveVisual
		{
			get { return _effectiveVisual; }
			set
			{
				if (value == _effectiveVisual)
					return;

				_effectiveVisual = value;
				OnPropertyChanged(VisualElement.VisualProperty.PropertyName);
			}
		}
		IVisual IVisualController.Visual => Xamarin.Forms.VisualMarker.MatchParent;

		static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (newValue == null || bindable.IsSet(FlyoutIconProperty))
				return;

			var shellItem = (BaseShellItem)bindable;
			shellItem.FlyoutIcon = (ImageSource)newValue;
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == BadgeTextColorProperty.PropertyName || propertyName == BadgeUnselectedTextColorProperty.PropertyName || propertyName == IsCheckedProperty.PropertyName)
			{
				OnPropertyChanged(BadgeEffectiveTextColorProperty.PropertyName);
			}

			if (propertyName == BadgeColorProperty.PropertyName || propertyName == BadgeUnselectedColorProperty.PropertyName || propertyName == IsCheckedProperty.PropertyName)
			{
				OnPropertyChanged(BadgeEffectiveColorProperty.PropertyName);
			}

			if (Parent != null)
			{
				if (propertyName == Shell.ItemTemplateProperty.PropertyName || propertyName == nameof(Parent))
					Propagate(Shell.ItemTemplateProperty, this, Parent, true);
			}
		}

		internal static void PropagateFromParent(BindableProperty property, Element me)
		{
			if (me == null || me.Parent == null)
				return;

			Propagate(property, me.Parent, me, false);
		}

		internal static void Propagate(BindableProperty property, BindableObject from, BindableObject to, bool onlyToImplicit)
		{
			if (from == null || to == null)
				return;

			if (onlyToImplicit && Routing.IsImplicit(from))
				return;

			if (to is Shell)
				return;

			if (from.IsSet(property) && !to.IsSet(property))
				to.SetValue(property, from.GetValue(property));
		}

		void IPropertyPropagationController.PropagatePropertyChanged(string propertyName)
		{
			PropertyPropagationExtensions.PropagatePropertyChanged(propertyName, this, LogicalChildren);
		}

		EffectiveFlowDirection _effectiveFlowDirection = default(EffectiveFlowDirection);
		EffectiveFlowDirection IFlowDirectionController.EffectiveFlowDirection
		{
			get { return _effectiveFlowDirection; }
			set
			{
				if (value == _effectiveFlowDirection)
					return;

				_effectiveFlowDirection = value;

				var ve = (Parent as VisualElement);
				ve?.InvalidateMeasureInternal(InvalidationTrigger.Undefined);
				OnPropertyChanged(VisualElement.FlowDirectionProperty.PropertyName);
			}
		}
		bool IFlowDirectionController.ApplyEffectiveFlowDirectionToChildContainer => true;
		double IFlowDirectionController.Width => (Parent as VisualElement)?.Width ?? 0;


		internal virtual void ApplyQueryAttributes(IDictionary<string, string> query)
		{
		}
	}

	public interface IQueryAttributable
	{
		void ApplyQueryAttributes(IDictionary<string, string> query);
	}
}