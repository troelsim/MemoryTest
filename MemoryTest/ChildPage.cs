using System;
using System.Linq;
using Xamarin.Forms;

namespace MemoryTest
{
	class ImageVM
	{
		public string Source { get; set; }
	}

	public class ChildPage : ContentPage
	{
		public ChildPage()
		{
			var template = new DataTemplate(typeof(ImageCell));
			template.SetBinding(ImageCell.ImageSourceProperty, nameof(ImageVM.Source));
			var listView = new ListView
			{
				ItemTemplate = template,
				ItemsSource = Enumerable.Repeat(new ImageVM { Source = "icon.png" }, 20).ToList()
			};
			Content = listView;
		}
	}
}

