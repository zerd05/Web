using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using CefSharp.Wpf;
using CefSharp;

namespace WebBrowser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int TabsCount = 1;
        

        public MainWindow()
        {
            InitializeComponent();
        }



        private void NewTab(object sender, RoutedEventArgs e)
        {

            BrowserTab Tab = new BrowserTab();
           
           
            Tab.Browser.Address = "google.com";
            
            Tab.Children.Add(Tab.Browser);
            

            Tabs.Items.Add(new TabItem
            {
                Header = "Вкладка " + Convert.ToString(TabsCount),
                Content = Tab,
                IsSelected = true

        });
      


            TabsCount++;
            Dispatcher.InvokeAsync(() => Go(null, null));
           

        }

        private void Go(object sender, RoutedEventArgs e)
        {
            TabItem CurrentTab = (TabItem) Tabs.Items[Tabs.SelectedIndex];
            BrowserTab CurrentBrowser = (BrowserTab)CurrentTab.Content;
            CurrentBrowser.Browser.Address = URLbox.Text;
            CurrentBrowser.Browser.WebBrowser.FrameLoadEnd += UpdateURL;
            CurrentBrowser.Browser.WebBrowser.LoadingStateChanged += UpdateURL;
            Dispatcher.Invoke(() => CanGo());
 
        }

        public class BrowserTab:Grid
        {
            public ChromiumWebBrowser Browser;


            public BrowserTab()
            {
                Browser = new ChromiumWebBrowser();
                
            }
            private void GetURL(object sender, FrameLoadEndEventArgs e)
            {
                MessageBox.Show("Страница загруженна");
               
            }


        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            TabItem CurrentTab = (TabItem)Tabs.Items[Tabs.SelectedIndex];
            BrowserTab CurrentBrowser = (BrowserTab)CurrentTab.Content;
            if(CurrentBrowser.Browser.CanGoBack)
            CurrentBrowser.Browser.WebBrowser.Back();
            Dispatcher.Invoke(() => CanGo());

        }

        private void GoForward(object sender, RoutedEventArgs e)
        {
            TabItem CurrentTab = (TabItem)Tabs.Items[Tabs.SelectedIndex];
            BrowserTab CurrentBrowser = (BrowserTab)CurrentTab.Content;
            if (CurrentBrowser.Browser.CanGoForward)
                CurrentBrowser.Browser.WebBrowser.Forward();
            Dispatcher.Invoke(() => CanGo());


        }

        private void UpdateURL(object sender, object e)
        {
            Dispatcher.Invoke(()=>URLbox.Text = ((ChromiumWebBrowser)sender).Address);
            Dispatcher.Invoke(() => CanGo());



        }

        private void ChangeTab(object sender, object e)
        {
            try
            {
                TabItem CurrentTab = (TabItem) Tabs.Items[Tabs.SelectedIndex];
                BrowserTab CurrentBrowser = (BrowserTab) CurrentTab.Content;
                URLbox.Text = CurrentBrowser.Browser.Address;
                
                CanGo();
            }
            catch
            {

            }
            
        }

        private void CanGo()
        {


            TabItem CurrentTab = (TabItem)Tabs.Items[Tabs.SelectedIndex];
            BrowserTab CurrentBrowser = (BrowserTab)CurrentTab.Content;
            if (CurrentBrowser.Browser.CanGoForward)
                ForwardButton.IsEnabled = true;
            else
                ForwardButton.IsEnabled = false;

            if (CurrentBrowser.Browser.CanGoBack)
                BackButton.IsEnabled = true;
            else
                BackButton.IsEnabled = false;
        }

        private void OnFormLoad(object sender, RoutedEventArgs e)
        {
            NewTab(null,null);
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {

            //Tabs.Items[Tabs.SelectedIndex] = null;
            
            Tabs.Items.Remove(Tabs.Items[Tabs.SelectedIndex]);
            
            //Tabs.SelectedIndex -= 1;

        }

        private void OnEnterURLBox(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Go(null,null);
        }
    }


}
