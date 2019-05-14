using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.XamStore
{
	[Preserve(AllMembers = true)]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StoreShell : TestShell
	{
		public StoreShell()
		{
			InitializeComponent();
			CurrentItem = _storeItem;

			var storeShellViewModel = new StoreShellViewModel() { BadgeText = "1" };
			BindingContext = storeShellViewModel;

			// BindingContext is not propagated to ShellContent when ShellSection is created implicitly via ShellSection.CreateFromShellContent
			this.Items[5].Items[0].Items[0].BindingContext = this.BindingContext;

			Task.Run(async () =>
			{
				for (int i = 0; i < 100; i++)
				{
					await Task.Delay(500);

					Device.BeginInvokeOnMainThread(() => {
						((StoreShellViewModel)this.BindingContext).BadgeText = i.ToString();
					});
				}
			});
		}

		protected override void Init()
		{
			var fontFamily = "";
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					fontFamily = "Ionicons";
					break;
				case Device.UWP:
					fontFamily = "Assets/Fonts/ionicons.ttf#ionicons";
					break;
				case Device.Android:
				default:
					fontFamily = "fonts/ionicons.ttf#";
					break;
			}
			FlyoutIcon = new FontImageSource
			{
				Glyph = "\uf2fb",
				FontFamily = fontFamily,
				Size = 20,
				AutomationId = "shellIcon"
			};

			FlyoutIcon.SetAutomationPropertiesHelpText("This as Shell FlyoutIcon");
			FlyoutIcon.SetAutomationPropertiesName("SHELLMAINFLYOUTICON");
			Routing.RegisterRoute("demo", typeof(DemoShellPage));
			Routing.RegisterRoute("demo/demo", typeof(DemoShellPage));
		}



		//bool allow = false;

		//protected override void OnNavigating(ShellNavigatingEventArgs args)
		//{
		//	if (allow)
		//		args.Cancel();

		//	allow = !allow;
		//	base.OnNavigating(args);
		//}
	}

	[Preserve(AllMembers = true)]
	public class StoreShellViewModel : INotifyPropertyChanged
	{
		string _badgeText;

		public string BadgeText
		{
			get => _badgeText;
			set
			{
				_badgeText = value;
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