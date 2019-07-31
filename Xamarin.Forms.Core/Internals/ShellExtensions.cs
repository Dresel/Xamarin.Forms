using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Internals
{
	internal static class ShellExtensions
	{
		public static IEnumerable<BaseShellItem> GetShellDescendants(this BindableObject element)
		{
			IEnumerable<BaseShellItem> baseShellItems = new List<BaseShellItem>();

			switch (element)
			{
				case Shell shell:
					baseShellItems = shell.GetShellDescendants();
					break;

				case ShellItem shellItem:
					baseShellItems = shellItem.GetShellDescendants();
					break;

				case ShellSection shellSection:
					baseShellItems = shellSection.GetShellDescendants();
					break;
			}

			return baseShellItems;
		}

		public static IEnumerable<BaseShellItem> GetShellDescendants(this Shell shell)
		{
			foreach (ShellItem shellItem in shell.Items)
			{
				yield return shellItem;

				foreach (var baseShellItem in shellItem.GetShellDescendants())
				{
					yield return baseShellItem;
				}
			}
		}

		public static IEnumerable<BaseShellItem> GetShellDescendants(this ShellItem shellItem)
		{
			foreach (ShellSection shellSection in shellItem.Items)
			{
				yield return shellSection;

				foreach (var baseShellItem in shellSection.GetShellDescendants())
				{
					yield return baseShellItem;
				}
			}
		}

		public static IEnumerable<BaseShellItem> GetShellDescendants(this ShellSection shellSection)
		{
			foreach (ShellContent baseShellItem in shellSection.Items)
			{
				yield return baseShellItem;
			}
		}
	}
}
