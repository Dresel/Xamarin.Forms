using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xamarin.Forms.Core.UnitTests
{
	[TestFixture]
	public class ShellBadgeTests
	{
		[Test]
		public void CheckBadgeColorInheritanceWithDefaultColor() =>
			CheckColorInheritanceWithDefaultColor(Shell.SetBadgeColor, (shell, element) => shell.GetInheritedBadgeColor(element));

		[Test]
		public void CheckBadgeColorInheritanceWithCustomColor() =>
			CheckColorInheritanceWithCustomColor(Shell.SetBadgeColor, (shell, element) => shell.GetInheritedBadgeColor(element));

		[Test]
		public void CheckBadgeTextColorInheritanceWithDefaultColor() =>
			CheckColorInheritanceWithDefaultColor(Shell.SetBadgeTextColor, (shell, element) => shell.GetInheritedBadgeTextColor(element));

		[Test]
		public void CheckBadgeTextColorInheritanceWithCustomColor() =>
			CheckColorInheritanceWithCustomColor(Shell.SetBadgeTextColor, (shell, element) => shell.GetInheritedBadgeTextColor(element));

		[Test]
		public void CheckBadgeUnselectedColorInheritanceWithDefaultColor() =>
			CheckColorInheritanceWithDefaultColor(Shell.SetBadgeUnselectedColor, (shell, element) => shell.GetInheritedBadgeUnselectedColor(element));

		[Test]
		public void CheckBadgeUnselectedColorInheritanceWithCustomColor() =>
			CheckColorInheritanceWithCustomColor(Shell.SetBadgeUnselectedColor, (shell, element) => shell.GetInheritedBadgeUnselectedColor(element));

		[Test]
		public void CheckBadgeUnselectedTextColorInheritanceWithDefaultColor() =>
			CheckColorInheritanceWithDefaultColor(Shell.SetBadgeUnselectedTextColor, (shell, element) => shell.GetInheritedBadgeUnselectedTextColor(element));

		[Test]
		public void CheckBadgeUnselectedTextColorInheritanceWithCustomColor() =>
			CheckColorInheritanceWithCustomColor(Shell.SetBadgeUnselectedTextColor, (shell, element) => shell.GetInheritedBadgeUnselectedTextColor(element));

		[Test]
		public void CheckPropertyChangedPropagatedForBadgeColor()
		{
			CheckPropertyChangedPropagatedForShell("EffectiveBadgeColor", Shell.SetBadgeColor, Color.Red);
			CheckPropertyChangedPropagatedForShellItem("EffectiveBadgeColor", Shell.SetBadgeColor, Color.Red);
			CheckPropertyChangedPropagatedForForShellSection("EffectiveBadgeColor", Shell.SetBadgeColor, Color.Red);
		}

		[Test]
		public void CheckPropertyChangedPropagatedForBadgeTextColor()
		{
			CheckPropertyChangedPropagatedForShell("EffectiveBadgeTextColor", Shell.SetBadgeTextColor, Color.Red);
			CheckPropertyChangedPropagatedForShellItem("EffectiveBadgeTextColor", Shell.SetBadgeTextColor, Color.Red);
			CheckPropertyChangedPropagatedForForShellSection("EffectiveBadgeTextColor", Shell.SetBadgeTextColor, Color.Red);
		}

		[Test]
		public void CheckPropertyChangedPropagatedForBadgeUnselectedColor()
		{
			CheckPropertyChangedPropagatedForShell("EffectiveBadgeColor", Shell.SetBadgeUnselectedColor, Color.Red);
			CheckPropertyChangedPropagatedForShellItem("EffectiveBadgeColor", Shell.SetBadgeUnselectedColor, Color.Red);
			CheckPropertyChangedPropagatedForForShellSection("EffectiveBadgeColor", Shell.SetBadgeUnselectedColor, Color.Red);
		}

		[Test]
		public void CheckPropertyChangedPropagatedForBadgeUnselectedTextColor()
		{
			CheckPropertyChangedPropagatedForShell("EffectiveBadgeTextColor", Shell.SetBadgeUnselectedTextColor, Color.Red);
			CheckPropertyChangedPropagatedForShellItem("EffectiveBadgeTextColor", Shell.SetBadgeUnselectedTextColor, Color.Red);
			CheckPropertyChangedPropagatedForForShellSection("EffectiveBadgeTextColor", Shell.SetBadgeUnselectedTextColor, Color.Red);
		}

		[Test]
		public void CheckPropertyChangedPropagatedForBadgeMoreText()
		{
			CheckPropertyChangedPropagatedForShell("EffectiveBadgeMoreText", Shell.SetBadgeMoreText, "!");
			CheckPropertyChangedPropagatedForShellItem("EffectiveBadgeMoreText", Shell.SetBadgeMoreText, "!");
			CheckPropertyChangedPropagatedForForShellSection("EffectiveBadgeMoreText", Shell.SetBadgeMoreText, "!");
		}

		[Test]
		public void CheckPropertyChangedPropagatedWhenCurrentItemChanged()
		{
			var shell = CreateShell();

			ShellContent currentShellContent = shell.CurrentItem.CurrentItem.CurrentItem;
			ShellContent nextShellContent = shell.Items.Last().Items.First().Items.First();

			IList<INotifyPropertyChanged> elements = new List<INotifyPropertyChanged>()
			{
				currentShellContent,
				currentShellContent.Parent,
				currentShellContent.Parent.Parent,
				nextShellContent,
				nextShellContent.Parent,
				nextShellContent.Parent.Parent,
			};

			var notifyPropertyChangedCalledForEffectiveBadgeTextColor = CreatePropertyChangedCalledArray("EffectiveBadgeTextColor", elements);
			var notifyPropertyChangedCalledForEffectiveBadgeColor = CreatePropertyChangedCalledArray("EffectiveBadgeTextColor", elements);

			shell.CurrentItem = shell.Items.Last();

			Assert.True(notifyPropertyChangedCalledForEffectiveBadgeTextColor.All(x => x));
			Assert.True(notifyPropertyChangedCalledForEffectiveBadgeColor.All(x => x));
		}

		static Shell CreateShell()
		{
			var shell = new Shell();

			var shellItem0 = new ShellItem();
			var shellItem1 = new ShellItem();

			var shellSection00 = new ShellSection();
			var shellSection01 = new ShellSection();
			var shellSection10 = new ShellSection();

			var shellContent000 = new ShellContent();
			var shellContent001 = new ShellContent();
			var shellContent010 = new ShellContent();
			var shellContent100 = new ShellContent();

			shellSection00.Items.Add(shellContent000);
			shellSection00.Items.Add(shellContent001);
			shellSection01.Items.Add(shellContent010);
			shellSection10.Items.Add(shellContent100);

			shellItem0.Items.Add(shellSection00);
			shellItem0.Items.Add(shellSection01);
			shellItem1.Items.Add(shellSection10);

			shell.Items.Add(shellItem0);
			shell.Items.Add(shellItem1);

			return shell;
		}

		static void CheckPropertyChangedPropagatedForShell<T>(string propertyName, Action<BindableObject, T> propertySetter, T propertyValue)
		{
			var shell = CreateShell();

			IList<INotifyPropertyChanged> elements = shell.Items.Cast<INotifyPropertyChanged>()
				.Concat(shell.Items.SelectMany(x => x.Items))
				.Concat(shell.Items.SelectMany(x => x.Items).SelectMany(x => x.Items)).ToList();

			var notifyPropertyChangedCalled = CreatePropertyChangedCalledArray(propertyName, elements);

			propertySetter(shell, propertyValue);

			Assert.True(notifyPropertyChangedCalled.All(x => x));
		}

		static void CheckPropertyChangedPropagatedForShellItem<T>(string propertyName, Action<BindableObject, T> propertySetter, T propertyValue)
		{
			var shell = CreateShell();
			var shellItem = shell.CurrentItem;

			IList<INotifyPropertyChanged> elements = shellItem.Items.Cast<INotifyPropertyChanged>()
				.Concat(shellItem.Items.SelectMany(x => x.Items)).ToList();

			var notifyPropertyChangedCalled = CreatePropertyChangedCalledArray(propertyName, elements);

			propertySetter(shellItem, propertyValue);

			Assert.True(notifyPropertyChangedCalled.All(x => x));
		}

		static void CheckPropertyChangedPropagatedForForShellSection<T>(string propertyName, Action<BindableObject, T> propertySetter, T propertyValue)
		{
			var shell = CreateShell();
			var shellSection = shell.CurrentItem.CurrentItem;

			IList<INotifyPropertyChanged> elements = shellSection.Items.Cast<INotifyPropertyChanged>().ToList();

			var notifyPropertyChangedCalled = CreatePropertyChangedCalledArray(propertyName, elements);

			propertySetter(shellSection, propertyValue);

			Assert.True(notifyPropertyChangedCalled.All(x => x));
		}

		static bool[] CreatePropertyChangedCalledArray(string propertyName, IList<INotifyPropertyChanged> elements)
		{
			var notifyPropertyChangedCalled = new bool[elements.Count];

			for (var i = 0; i < elements.Count; i++)
			{
				var index = i;

				elements[index].PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == propertyName)
					{
						notifyPropertyChangedCalled[index] = true;
					}
				};
			}

			return notifyPropertyChangedCalled;
		}

		static void CheckColorInheritanceWithDefaultColor(Action<BindableObject, Color> colorSetter, Func<Shell, Element, Color> colorGetter)
		{
			var shell = CreateShell();

			colorSetter(shell.Items[0], Color.Red);
			colorSetter(shell.Items[0].Items[0], Color.Green);
			colorSetter(shell.Items[0].Items[0].Items[0], Color.Blue);

			Assert.AreEqual(colorGetter(shell, shell), Color.Default);
			Assert.AreEqual(colorGetter(shell, shell.Items[1]), Color.Default);
			Assert.AreEqual(colorGetter(shell, shell.Items[1].Items[0]), Color.Default);
			Assert.AreEqual(colorGetter(shell, shell.Items[1].Items[0].Items[0]), Color.Default);

			Assert.AreEqual(colorGetter(shell, shell.Items[0]), Color.Red);
			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[1]), Color.Red);
			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[1].Items[0]), Color.Red);

			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[0]), Color.Green);
			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[0].Items[1]), Color.Green);

			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[0].Items[0]), Color.Blue);
		}

		static void CheckColorInheritanceWithCustomColor(Action<BindableObject, Color> colorSetter, Func<Shell, Element, Color> colorGetter)
		{
			var shell = CreateShell();

			colorSetter(shell, Color.Cyan);
			colorSetter(shell.Items[0], Color.Red);
			colorSetter(shell.Items[0].Items[0], Color.Green);
			colorSetter(shell.Items[0].Items[0].Items[0], Color.Blue);

			Assert.AreEqual(colorGetter(shell, shell), Color.Cyan);
			Assert.AreEqual(colorGetter(shell, shell.Items[1]), Color.Cyan);
			Assert.AreEqual(colorGetter(shell, shell.Items[1].Items[0]), Color.Cyan);
			Assert.AreEqual(colorGetter(shell, shell.Items[1].Items[0].Items[0]), Color.Cyan);

			Assert.AreEqual(colorGetter(shell, shell.Items[0]), Color.Red);
			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[1]), Color.Red);
			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[1].Items[0]), Color.Red);

			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[0]), Color.Green);
			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[0].Items[1]), Color.Green);

			Assert.AreEqual(colorGetter(shell, shell.Items[0].Items[0].Items[0]), Color.Blue);
		}
	}
}