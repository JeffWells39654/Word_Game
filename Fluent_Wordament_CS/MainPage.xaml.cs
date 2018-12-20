using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Fluent_Wordament_CS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			((Button)sender).Content = "Z";
		}


		void myButton_DragEnter(object sender, DragEventArgs e)
		{
			((Button)sender).Content = "Z";
		}

		private void myButton_DragEnter(object sender, PointerRoutedEventArgs e)
		{
			((Button)sender).Content = "Y";
		}

		private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
		{

			tb_X_Mouse.Text = Windows.UI.Input.PointerPoint.GetCurrentPoint(e.GetCurrentPoint((UIElement)sender).PointerId).Position.X.ToString();
			tb_Y_Mouse.Text = Windows.UI.Input.PointerPoint.GetCurrentPoint(e.GetCurrentPoint((UIElement)sender).PointerId).Position.Y.ToString();
			((Button)sender).Content = "X";
			//Windows.UI.Input.PointerPoint
		}

		private void Button_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			tb_X_Mouse.Text = Windows.UI.Input.PointerPoint.GetCurrentPoint(e.GetCurrentPoint((UIElement)sender).PointerId).Position.X.ToString();
			tb_Y_Mouse.Text = Windows.UI.Input.PointerPoint.GetCurrentPoint(e.GetCurrentPoint((UIElement)sender).PointerId).Position.Y.ToString();
		}
	}
}
