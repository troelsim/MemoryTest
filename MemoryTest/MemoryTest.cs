using System;

using Xamarin.Forms;

namespace MemoryTest
{
	public class App : Application
	{
		public App()
		{
			// The root page of your application
			var content = new ContentPage
			{
				Title = "MemoryTest",
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};

			var childPage = new ChildPage();
			var NavigationPage = new NavigationPage(content);
			MainPage = NavigationPage;

			var childVisible = false;
			Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
			{
				if (childVisible)
				{
					NavigationPage.PopAsync();
				}
				else
				{
					NavigationPage.PushAsync(childPage);
				}
				childVisible = !childVisible;
				return true;
			});
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
