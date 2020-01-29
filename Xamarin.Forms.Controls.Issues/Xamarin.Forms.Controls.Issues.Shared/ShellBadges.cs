using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

		const string SetContentBadgeColor = "CBC";

		const string SetSectionBadgeText = "SBT";

		const string SetSectionBadgeTextColor = "SBTC";

		const string SetSectionBadgeColor = "SBC";

		const string SetItemBadgeText = "IBT";

		const string SetItemBadgeTextColor = "IBTC";

		const string SetItemBadgeColor = "IBC";

#if __ANDROID__
		protected int TextColorDefault = -1;

		// Color.DarkBlue.ToAndroid().ToArgb();
		protected int TextColorSet = -16777077;

		// Color.FromRgb(255, 59, 48).ToAndroid().ToArgb();
		protected int BadgeColorDefault = -50384;

		// Color.DarkOrange.ToAndroid().ToArgb()
		protected int BadgeColorSet = -29696;
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
									CreateBadgeButton(SetContentBadgeText, (contentViewModel, sectionViewModel, itemViewModel) => { contentViewModel.Text = "2"; }),
									CreateBadgeButton(SetContentBadgeTextColor, (contentViewModel, sectionViewModel, itemViewModel) => { contentViewModel.TextColor = Color.DarkBlue; }),
									CreateBadgeButton(SetContentBadgeColor, (contentViewModel, sectionViewModel, itemViewModel) => { contentViewModel.Color = Color.DarkOrange; }),
								}
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetSectionBadgeText, (contentViewModel, sectionViewModel, itemViewModel) => { sectionViewModel.Text = "2"; }),
									CreateBadgeButton(SetSectionBadgeTextColor, (contentViewModel, sectionViewModel, itemViewModel) => { sectionViewModel.TextColor = Color.DarkBlue; }),
									CreateBadgeButton(SetSectionBadgeColor, (contentViewModel, sectionViewModel, itemViewModel) => { sectionViewModel.Color = Color.DarkOrange; }),
								},
							},
							new StackLayout
							{
								Orientation = StackOrientation.Horizontal,
								Children =
								{
									CreateBadgeButton(SetItemBadgeText, (contentViewModel, sectionViewModel, itemViewModel) => { itemViewModel.Text = "2"; }),
									CreateBadgeButton(SetItemBadgeTextColor, (contentViewModel, sectionViewModel, itemViewModel) => { itemViewModel.TextColor = Color.DarkBlue; }),
									CreateBadgeButton(SetItemBadgeColor, (contentViewModel, sectionViewModel, itemViewModel) => { itemViewModel.Color = Color.DarkOrange; }),
								},
							}
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

		static Button CreateBadgeButton(string text, Action<BadgeViewModel, BadgeViewModel, BadgeViewModel> action)
		{
			return CreateButton(text, (sender, args) =>
			{
				Element element = ((Element)sender);

				do
				{
					element = element.Parent;
				} while (!(element is ShellContent));

				ShellContent shellContent = (ShellContent)element;

				action((BadgeViewModel)shellContent.BindingContext,
					(BadgeViewModel)((ShellSection)shellContent.Parent).BindingContext,
					(BadgeViewModel)((ShellItem)shellContent.Parent.Parent).BindingContext);
			});
		}

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

		public void Test(string buttonIdentifier, Action preCondition, Action postCondition)
		{
			RunningApp.WaitForElement(buttonIdentifier);

			preCondition();
			RunningApp.Tap(buttonIdentifier);
			postCondition();
		}
#endif
	}

#if UITEST && __ANDROID__
	public static class AppQueryExtension
	{
		// AKA Top Badge
		public static AppQuery ContentBadge(this AppQuery query)
		{
			return query.Class("AppBarLayout").Descendant().Marked("Content 111");
		}

		// AKA Bottom Badge
		public static AppQuery SectionBadge(this AppQuery query)
		{
			return query.Class("BottomNavigationView").Descendant().Marked("Section 11");
		}

		// AKA Flyout Badge
		public static AppQuery ItemBadge(this AppQuery query)
		{
			return query.Class("RecyclerView").Class("FrameRenderer").Index(0);
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