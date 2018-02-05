using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeSlice
{
    /// <summary>
    /// PopupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PopupWindow : Window
    {
        int width = 1080;
        int height = 1920;

        public PopupWindow(bool isSuccess)
        {
            InitializeComponent();
            SetPopupImage(isSuccess);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = (width - this.ActualWidth) / 2;
            this.Top = (height - this.ActualHeight) / 2;
        }

        public void SetPopupImage(bool isSuccess)
        {
            BitmapImage popupImage;
            if (isSuccess)
            {
                popupImage = new BitmapImage(new Uri("pack://application:,,,/Resources/send_success_UI.png", UriKind.Absolute));
            }
            else
            {
                popupImage = new BitmapImage(new Uri("pack://application:,,,/Resources/send_fail_UI.png", UriKind.Absolute));
            }

            imgPopup.Source = popupImage;
        }

        private void Popup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            
        }
    }
}
