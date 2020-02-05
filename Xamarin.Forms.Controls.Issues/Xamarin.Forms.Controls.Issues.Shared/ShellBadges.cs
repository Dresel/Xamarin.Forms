﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
#if UITEST
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest.Queries;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.None, 0, "Shell Badges Test",
		PlatformAffected.All)]
#if UITEST
	[NUnit.Framework.Category(UITestCategories.Shell)]
#endif
	public class ShellBadges : TestShell
	{
		const string ToggleFlyout = "Flyout";

		const string SetContentBadgeText = "CBT";

		const string SetContentBadgeTextColor = "CBTC";

		const string SetContentBadgeUnselectedTextColor = "CBUTC";

		const string SetContentBadgeColor = "CBC";

		const string SetContentBadgeUnselectedColor = "CBUC";

		const string SetSectionBadgeText = "SBT";

		const string SetSectionBadgeTextColor = "SBTC";

		const string SetSectionBadgeUnselectedTextColor = "SBUTC";

		const string SetSectionBadgeColor = "SBC";

		const string SetSectionBadgeUnselectedColor = "SBUC";

		const string SetItemBadgeText = "IBT";

		const string SetItemBadgeTextColor = "IBTC";

		const string SetItemBadgeUnselectedTextColor = "IBUTC";

		const string SetItemBadgeColor = "IBC";

		const string SetItemBadgeUnselectedColor = "IBUC";

#if __ANDROID__
		// Color.White.ToAndroid().ToArgb()
		protected int TextColorDefault = -1;

		// Color.DarkBlue.ToAndroid().ToArgb();
		protected int TextColorSet = -16777077;

		// Color.DarkGreen.ToAndroid().ToArgb();
		protected int UnselectedTextColorSet = -16751616;

		// Color.FromRgb(255, 59, 48).ToAndroid().ToArgb();
		protected int BadgeColorDefault = -50384;

		// Color.DarkOrange.ToAndroid().ToArgb()
		protected int BadgeColorSet = -29696;

		// Color.DarkMagenta.ToAndroid().ToArgb();
		protected int UnselectedColorSet = -7667573;
#endif

		// Test implicit ShellItem, ShellSection
		// Test explicit ShellItem, ShellSection, ShellContent
		// Test updates
		// Test updates with default values (color)
		// Test updates with default values and conditional things (unselected)
		// Styles https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/styles/xaml/dynamic

		protected override void Init()
		{
			Items.Add(CreateShellItem("Item 1", new[]
			{
				CreateShellSection("Section 11", new[]
				{
					CreateShellContent("Content 111"),
					CreateShellContent("Content 112")
				}),
				CreateShellSection("Section 12", new[]
				{
					CreateShellContent("Content 121"),
					CreateShellContent("Content 122")
				}),
			}));

			Items.Add(CreateShellItem("Item 2", new[]
			{
				CreateShellSection("Section 21", new[]
				{
					CreateShellContent("Content 211"),
					CreateShellContent("Content 212")
				}),
				CreateShellSection("Section 22", new[]
				{
					CreateShellContent("Content 221"),
					CreateShellContent("Content 222")
				}),
			}));
		}

		static ShellContent CreateShellContent(string title)
		{
			var shellContent = new ShellContent
			{
				Title = title,
				Content = new ContentPage
				{
					Content = new StackLayout
					{
						Children =
						{
							CreateButton(ToggleFlyout, (sender, args) => { Shell.Current.FlyoutIsPresented = true; }),
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetContentBadgeText, () => { Shell.Current.ShellContentBadgeViewModel("Content 111").Text = "2"; }),
									CreateBadgeButton(SetContentBadgeTextColor, () => { Shell.Current.ShellContentBadgeViewModel("Content 111").TextColor = Color.DarkBlue; }),
									CreateBadgeButton(SetContentBadgeColor, () => { Shell.Current.ShellContentBadgeViewModel("Content 111").Color = Color.DarkOrange; }),
								}
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetContentBadgeUnselectedTextColor, () => { Shell.Current.ShellContentBadgeViewModel("Content 112").UnselectedTextColor = Color.DarkGreen; }),
									CreateBadgeButton(SetContentBadgeUnselectedColor, () => { Shell.Current.ShellContentBadgeViewModel("Content 112").UnselectedColor = Color.DarkMagenta; }),
								}
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetSectionBadgeText, () => { Shell.Current.ShellSectionBadgeViewModel("Section 11").Text = "2"; }),
									CreateBadgeButton(SetSectionBadgeTextColor, () => { Shell.Current.ShellSectionBadgeViewModel("Section 11").TextColor = Color.DarkBlue; }),
									CreateBadgeButton(SetSectionBadgeColor, () => { Shell.Current.ShellSectionBadgeViewModel("Section 11").Color = Color.DarkOrange; }),
								},
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetSectionBadgeUnselectedTextColor, () => { Shell.Current.ShellSectionBadgeViewModel("Section 12").UnselectedTextColor = Color.DarkGreen; }),
									CreateBadgeButton(SetSectionBadgeUnselectedColor, () => { Shell.Current.ShellSectionBadgeViewModel("Section 12").UnselectedColor = Color.DarkMagenta; }),
								},
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetItemBadgeText, () => { Shell.Current.ShellItemBadgeViewModel("Item 1").Text = "2"; }),
									CreateBadgeButton(SetItemBadgeTextColor, () => { Shell.Current.ShellItemBadgeViewModel("Item 1").TextColor = Color.DarkBlue; }),
									CreateBadgeButton(SetItemBadgeColor, () => { Shell.Current.ShellItemBadgeViewModel("Item 1").Color = Color.DarkOrange; }),
								},
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetItemBadgeUnselectedTextColor, () => { Shell.Current.ShellItemBadgeViewModel("Item 2").UnselectedTextColor = Color.DarkGreen; }),
									CreateBadgeButton(SetItemBadgeUnselectedColor, () => { Shell.Current.ShellItemBadgeViewModel("Item 2").UnselectedColor = Color.DarkMagenta; }),
								},
							},
						}
					}
				},
				Icon = "coffee.png",
				BindingContext = new BadgeViewModel()
				{
					Text = "1",
				}
			};

			ApplyBadgeViewModel(shellContent);

			return shellContent;
		}

		static void ApplyBadgeViewModel(BindableObject bindableObject)
		{
			bindableObject.SetBinding(BaseShellItem.BadgeTextProperty, nameof(BadgeViewModel.Text));
			bindableObject.SetBinding(BaseShellItem.BadgeMoreTextProperty, nameof(BadgeViewModel.MoreText));
			bindableObject.SetBinding(BaseShellItem.BadgeTextColorProperty, nameof(BadgeViewModel.TextColor));
			bindableObject.SetBinding(BaseShellItem.BadgeUnselectedTextColorProperty, nameof(BadgeViewModel.UnselectedTextColor));
			bindableObject.SetBinding(BaseShellItem.BadgeColorProperty, nameof(BadgeViewModel.Color));
			bindableObject.SetBinding(BaseShellItem.BadgeUnselectedColorProperty, nameof(BadgeViewModel.UnselectedColor));
		}

		static ShellItem CreateShellItem(string title, params ShellSection[] shellSections)
		{
			var shellItem = new ShellItem()
			{
				Title = title,
				BindingContext = new BadgeViewModel()
				{
					Text = "1",
				}
			};

			shellItem.AutomationId = title;

			foreach (ShellSection shellSection in shellSections)
			{
				shellItem.Items.Add(shellSection);
			}

			ApplyBadgeViewModel(shellItem);

			return shellItem;
		}

		static ShellSection CreateShellSection(string title, params ShellContent[] shellContents)
		{
			var shellSection = new ShellSection()
			{
				Title = title,
				BindingContext = new BadgeViewModel()
				{
					Text = "1",
				}
			};

			foreach (ShellContent shellContent in shellContents)
			{
				shellSection.Items.Add(shellContent);
			}

			ApplyBadgeViewModel(shellSection);

			return shellSection;
		}

		static Button CreateButton(string text, Action<object, EventArgs> action)
		{
			Button button = new Button() { Text = text };
			button.Clicked += action.Invoke;

			return button;
		}

		static Button CreateBadgeButton(string text, Action action) => CreateButton(text, (sender, args) => { action(); });

#if UITEST && __ANDROID__
		[Test]
		public void SetContentBadgeTextTest()
		{
			Test(SetContentBadgeText, () =>
			{
				RunningApp.WaitForElement(x => x.ContentBadge().ContentBadgeText("1"));
			}, () =>
			{
				RunningApp.WaitForElement(x => x.ContentBadge().ContentBadgeText("2"));
			});
		}

		[Test]
		public void SetSectionBadgeTextTest()
		{
			Test(SetSectionBadgeText, () =>
			{
				RunningApp.WaitForElement(x => x.SectionBadge().ContentBadgeText("1"));
			}, () =>
			{
				RunningApp.WaitForElement(x => x.SectionBadge().ContentBadgeText("2"));
			});
		}

		[Test]
		public void SetItemBadgeTextTest()
		{
			Test(SetItemBadgeText, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement(x => x.ItemBadge().ItemBadgeText("1"));
				RunningApp.Tap("Item 1");
			}, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement(x => x.ItemBadge().ItemBadgeText("2"));
			});
		}

		[Test]
		public void SetContentBadgeTextColorTest()
		{
			Test(SetContentBadgeTextColor, () =>
			{
				Assert.AreEqual(TextColorDefault, RunningApp.Query(x => x.ContentBadge().ContentBadgeTextColor()).Single());
			}, () =>
			{
				Assert.AreEqual(TextColorSet, RunningApp.Query(x => x.ContentBadge().ContentBadgeTextColor()).Single());
			});
		}

		[Test]
		public void SetSectionBadgeTextColorTest()
		{
			Test(SetSectionBadgeTextColor, () =>
			{
				Assert.AreEqual(TextColorDefault, RunningApp.Query(x => x.SectionBadge().SectionBadgeTextColor()).Single());
			}, () =>
			{
				Assert.AreEqual(TextColorSet, RunningApp.Query(x => x.SectionBadge().SectionBadgeTextColor()).Single());
			});
		}

		[Test]
		public void SetItemBadgeTextColorTest()
		{
			Test(SetItemBadgeTextColor, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(TextColorDefault, RunningApp.Query(x => x.ItemBadge().ItemBadgeTextColor()).Single());
				RunningApp.Tap("Item 1");
			}, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(TextColorSet, RunningApp.Query(x => x.ItemBadge().ItemBadgeTextColor()).Single());
			});
		}

		[Test]
		public void SetContentBadgeUnselectedTextColorTest()
		{
			Test(SetContentBadgeUnselectedTextColor, () =>
			{
				Assert.AreEqual(TextColorDefault, RunningApp.Query(x => x.ContentBadge("Content 112").ContentBadgeTextColor()).Single());
			}, () =>
			{
				Assert.AreEqual(UnselectedTextColorSet, RunningApp.Query(x => x.ContentBadge("Content 112").ContentBadgeTextColor()).Single());
			});
		}

		[Test]
		public void SetSectionBadgeUnselectedTextColorTest()
		{
			Test(SetSectionBadgeUnselectedTextColor, () =>
			{
				Assert.AreEqual(TextColorDefault, RunningApp.Query(x => x.SectionBadge("Section 12").SectionBadgeTextColor()).Single());
			}, () =>
			{
				Assert.AreEqual(UnselectedTextColorSet, RunningApp.Query(x => x.SectionBadge("Section 12").SectionBadgeTextColor()).Single());
			});
		}

		[Test]
		public void SetItemBadgeUnselectedTextColorTest()
		{
			Test(SetItemBadgeUnselectedTextColor, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(TextColorDefault, RunningApp.Query(x => x.ItemBadge("Item 2").ItemBadgeTextColor()).Single());
				RunningApp.Tap("Item 1");
			}, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(UnselectedTextColorSet, RunningApp.Query(x => x.ItemBadge("Item 2").ItemBadgeTextColor()).Single());
			});
		}

		[Test]
		public void SetContentBadgeColorTest()
		{
			Test(SetContentBadgeColor, () =>
			{
				Assert.AreEqual(BadgeColorDefault, RunningApp.Query(x => x.ContentBadge().ContentBadgeColor()).Single());
			}, () =>
			{
				Assert.AreEqual(BadgeColorSet, RunningApp.Query(x => x.ContentBadge().ContentBadgeColor()).Single());
			});
		}

		[Test]
		public void SetSectionBadgeColorTest()
		{
			Test(SetSectionBadgeColor, () =>
			{
				Assert.AreEqual(BadgeColorDefault, RunningApp.Query(x => x.SectionBadge().SectionBadgeColor()).Single());

			}, () =>
			{
				Assert.AreEqual(BadgeColorSet, RunningApp.Query(x => x.SectionBadge().SectionBadgeColor()).Single());
			});
		}

		[Test]
		public void SetItemBadgeColorTest()
		{
			Test(SetItemBadgeColor, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(BadgeColorDefault, RunningApp.Query(x => x.ItemBadge().ItemBadgeColor()).Single());
				RunningApp.Tap("Item 1");
			}, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(BadgeColorSet, RunningApp.Query(x => x.ItemBadge().ItemBadgeColor()).Single());
			});
		}

		[Test]
		public void SetContentBadgeUnselectedColorTest()
		{
			Test(SetContentBadgeUnselectedColor, () =>
			{
				Assert.AreEqual(BadgeColorDefault, RunningApp.Query(x => x.ContentBadge("Content 112").ContentBadgeColor()).Single());
			}, () =>
			{
				Assert.AreEqual(UnselectedColorSet, RunningApp.Query(x => x.ContentBadge("Content 112").ContentBadgeColor()).Single());
			});
		}

		[Test]
		public void SetSectionBadgeUnselectedColorTest()
		{
			Test(SetSectionBadgeUnselectedColor, () =>
			{
				Assert.AreEqual(BadgeColorDefault, RunningApp.Query(x => x.SectionBadge("Section 12").SectionBadgeColor()).Single());
			}, () =>
			{
				Assert.AreEqual(UnselectedColorSet, RunningApp.Query(x => x.SectionBadge("Section 12").SectionBadgeColor()).Single());
			});
		}

		[Test]
		public void SetItemBadgeUnselectedColorTest()
		{
			Test(SetItemBadgeUnselectedColor, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(BadgeColorDefault, RunningApp.Query(x => x.ItemBadge("Item 2").ItemBadgeColor()).Single());
				RunningApp.Tap("Item 1");
			}, () =>
			{
				RunningApp.Tap(ToggleFlyout);
				RunningApp.WaitForElement("Item 1");
				Assert.AreEqual(UnselectedColorSet, RunningApp.Query(x => x.ItemBadge("Item 2").ItemBadgeColor()).Single());
			});
		}

		public void Test(string buttonIdentifier, Action preCondition, Action postCondition)
		{
			RunningApp.WaitForElement(buttonIdentifier);

			preCondition();
			RunningApp.Tap(buttonIdentifier);
			postCondition();
		}
#endif
	}

	public static class ShellExtension
	{
		public static BadgeViewModel ShellContentBadgeViewModel(this Shell shell, string title) =>
			(BadgeViewModel)shell.Items
			.SelectMany(x => x.Items)
			.SelectMany(x => x.Items)
			.Single(x => x.Title == title)
			.BindingContext;

		public static BadgeViewModel ShellSectionBadgeViewModel(this Shell shell, string title) => 
			(BadgeViewModel)shell.Items
			.SelectMany(x => x.Items)
			.Single(x => x.Title == title)
			.BindingContext;

		public static BadgeViewModel ShellItemBadgeViewModel(this Shell shell, string title) =>
			(BadgeViewModel)shell.Items
			.Single(x => x.Title == title)
			.BindingContext;
	}

#if UITEST && __ANDROID__

	public static class AppQueryExtension
	{
		// AKA Top Badge
		public static AppQuery ContentBadge(this AppQuery query, string content = "Content 111")
		{
			return query.Class("AppBarLayout").Descendant().Marked(content);
		}

		// AKA Bottom Badge
		public static AppQuery SectionBadge(this AppQuery query, string section = "Section 11")
		{
			return query.Class("BottomNavigationView").Descendant().Marked(section);
		}

		// AKA Flyout Badge
		public static AppQuery ItemBadge(this AppQuery query, string item = "Item 1")
		{
			return query.Raw($"* marked:'{item}' parent * index:1 descendant FrameRenderer");
		}

		public static AppQuery ContentBadgeText(this AppQuery query, string text)
		{
			return query.Class("TextView").Text(text);
		}

		public static AppQuery SectionBadgeText(this AppQuery query, string text) => query.ContentBadgeText(text);

		public static AppQuery ItemBadgeText(this AppQuery query, string text)
		{
			return query.Class("FormsTextView").Text(text);
		}

		public static AppTypedSelector<int> ContentBadgeTextColor(this AppQuery query)
		{
			return query.Class("TextView").Invoke("getCurrentTextColor").Value<int>();
		}

		public static AppTypedSelector<int> SectionBadgeTextColor(this AppQuery query) => query.ContentBadgeTextColor();

		public static AppTypedSelector<int> ItemBadgeTextColor(this AppQuery query)
		{
			return query.Class("FormsTextView").Invoke("getCurrentTextColor").Value<int>();
		}

		public static AppTypedSelector<int> ContentBadgeColor(this AppQuery query)
		{
			return query.Class("BadgeHelper_BadgeFrameLayout").Invoke("getBackground").Invoke("getPaint").Invoke("getColor").Value<int>();
		}

		public static AppTypedSelector<int> SectionBadgeColor(this AppQuery query) => query.ContentBadgeColor();

		public static AppTypedSelector<int> ItemBadgeColor(this AppQuery query)
		{
			return query.Invoke("getBackground").Invoke("getColor").Invoke("getDefaultColor").Value<int>();
		}
	}
#endif

	public class BadgeViewModel : INotifyPropertyChanged
	{
		string _text;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged();
			}
		}

		string _moreText;

		public string MoreText
		{
			get => _moreText;
			set
			{
				_moreText = value;
				OnPropertyChanged();
			}
		}

		Color _textColor;

		public Color TextColor
		{
			get => _textColor;
			set
			{
				_textColor = value;
				OnPropertyChanged();
			}
		}

		Color _unselectedTextColor;

		public Color UnselectedTextColor
		{
			get => _unselectedTextColor;
			set
			{
				_unselectedTextColor = value;
				OnPropertyChanged();
			}
		}

		Color _color;

		public Color Color
		{
			get => _color;
			set
			{
				_color = value;
				OnPropertyChanged();
			}
		}

		Color _unselectedColor;

		public Color UnselectedColor
		{
			get => _unselectedColor;
			set
			{
				_unselectedColor = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}