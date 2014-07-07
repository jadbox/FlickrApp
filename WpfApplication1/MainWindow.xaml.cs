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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Newtonsoft.Json;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private string flickerRU(String args)
        {
            return "https://api.flickr.com/services/rest/?format=json" +
                    "&nojsoncallback=1&api_key=256663858aa10e52a838a58b7866d858" + args;
        }

        private Flickr flickr = new Flickr();

        private void CommonCommandBinding_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            //addt(flickr.getImageUrlForTag(searchFor.Text));
            flickr.getImageUrlForTagRx(searchFor.Text, addt);
        }

        public void addt(String url) {
            Image image = new Image();
            image.BeginInit();
            image.Source = new BitmapImage(new Uri(url));
            image.EndInit();
            image.Width = 100;
            image.Height = 100;
            image.Stretch = Stretch.Uniform;
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            image.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            myg.Children.Add(image);
        }
    }
}
