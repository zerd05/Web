using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CefSharp.Wpf;
using CefSharp;

namespace WebBrowser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //private int TabsCount = 1;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewTab(object sender, RoutedEventArgs e)
        {
          
          
            BrowserTab NewBrowserTab = new BrowserTab();


            NewBrowserTab.DublicateMenuItem.Click += DublicateTab;
            NewBrowserTab.Browser.LoadingStateChanged += UpdateURL;
            NewBrowserTab.CloseTabMenuItem.Click += CloseTab;

            Tabs.Items.Add(NewBrowserTab);
            Dispatcher.InvokeAsync(() => Go(null, null));
        }

        private void DublicateTab(object sender, RoutedEventArgs e)
        {
            BrowserTab NewBrowserTab = new BrowserTab();
            NewBrowserTab.Browser.Address = ((BrowserTab) Tabs.Items[Tabs.SelectedIndex]).Browser.Address;
            NewBrowserTab.IsSelected = true;


            NewBrowserTab.DublicateMenuItem.Click += DublicateTab;
            NewBrowserTab.Browser.LoadingStateChanged += UpdateURL;
            NewBrowserTab.CloseTabMenuItem.Click += CloseTab;

            Tabs.Items.Add(NewBrowserTab);
        }

        private void Go(object sender, RoutedEventArgs e)
        {
            ((BrowserTab) Tabs.Items[Tabs.SelectedIndex]).Browser.Address = URLbox.Text;


        }

        public class BrowserTab:TabItem
        {
            public ChromiumWebBrowser Browser;
            public Grid grid;
            public ContextMenu TabContextMenu;
            public MenuItem DublicateMenuItem;
            public MenuItem CloseTabMenuItem;

            public BrowserTab()
            {
                Browser = new ChromiumWebBrowser();
                grid = new Grid();
                TabContextMenu = new ContextMenu();
               

                Content = grid;
                grid.Children.Add(Browser);
                Header = "Новая вкладка";
                Browser.Address = "google.com";
                IsSelected = true;
                Browser.FrameLoadStart += UpdateTitle;
                Browser.LoadingStateChanged += UpdateTitle;
                Browser.FrameLoadEnd += UpdateTitle;
                InitContextMenu();

            }

            private void InitContextMenu()
            {

                DublicateMenuItem = new MenuItem {Header = "Дублировать вкладку"};
                DublicateMenuItem.Click += SelectTab;

                CloseTabMenuItem = new MenuItem{Header = "Закрыть вкладку"};
                CloseTabMenuItem.Click += SelectTab;

                TabContextMenu.Items.Add(DublicateMenuItem);
                TabContextMenu.Items.Add(CloseTabMenuItem);
                ContextMenu = TabContextMenu;
            }

            private void SelectTab(object sender, RoutedEventArgs e)
            {
                IsSelected = true;
            }

            private void UpdateTitle(object sender, object e)
            {
                Dispatcher.Invoke(new Action(delegate()
                {
                    if (Browser.Title != null)
                        Header = Browser.Title;
                }));
              
            }
 


        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if(((BrowserTab)Tabs.Items[Tabs.SelectedIndex]).Browser.CanGoBack)
                ((BrowserTab)Tabs.Items[Tabs.SelectedIndex]).Browser.WebBrowser.Back();
            

        }

        private void GoForward(object sender, RoutedEventArgs e)
        {
           
            if (((BrowserTab)Tabs.Items[Tabs.SelectedIndex]).Browser.CanGoForward)
                ((BrowserTab)Tabs.Items[Tabs.SelectedIndex]).Browser.WebBrowser.Forward();
        }


        private void UpdateURL(object sender, object e)
        {
            
                Dispatcher.Invoke(delegate()
                {
                    try
                    {
                        URLbox.Text = ((BrowserTab) Tabs.Items[Tabs.SelectedIndex]).Browser.Address;
                    }
                    catch
                    {

                    }
                    
                });
           


        }

        private void OnFormLoad(object sender, RoutedEventArgs e)
        {
            NewTab(null,null);
            URLbox.TextWrapping = TextWrapping.NoWrap;
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            if(Tabs.SelectedIndex!=-1)
            Tabs.Items.Remove(Tabs.Items[Tabs.SelectedIndex]);
            else
            {
                URLbox.Text = "";
            }
        }

        private void OnEnterURLBox(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Go(null,null);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Topmost = true;
                
            }

            if (e.Key == Key.F10)
            {
                WindowStyle = WindowStyle.ThreeDBorderWindow;
            }
                

        }
    }


}
