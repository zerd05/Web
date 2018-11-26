using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CefSharp;
using CefSharp.Wpf;

namespace WebBrowser
{
    public class BrowserTab:TabItem
    {
        public string HistoryPath = @"C:\Users\zerd\Desktop\WebBrowser\WebBrowser\History\History.xml";

        public Button ForwardButton;
        public Button BackButton;
        public TextBox UrlTextBox;
        public Button NavigateButton;
        public Button NewTabButton;
        public Button CloseTabButton;

        public ChromiumWebBrowser Browser;

        public Grid MainGrid;
        public Grid BrowserGrid;
        public bool Pined;  //Закреплена ли вкладка

        public ContextMenu TabContextMenu;
        public MenuItem DublicateMenuItem;
        public MenuItem CloseTabMenuItem;
        public MenuItem PinTabMenuItem;

        public StackPanel HeaderStackPanel;
        public Button CloseTabHeaderButton;
        public Label TabTitle;

        public BrowserTab()
        {
            HeaderStackPanel = new StackPanel{Orientation = Orientation.Horizontal};
            CloseTabHeaderButton = new Button{Content = " X ",HorizontalAlignment = HorizontalAlignment.Right,Height = 19};
            TabTitle = new Label{HorizontalAlignment = HorizontalAlignment.Left};
            
            CloseTabHeaderButton.Click += CloseTab;
            HeaderStackPanel.Children.Add(TabTitle);
            HeaderStackPanel.Children.Add(CloseTabHeaderButton);
           
            Header = HeaderStackPanel;



            BrowserGrid = new Grid();
            MainGrid = new Grid();

            ForwardButton = new Button
            {
                Content = "Вперед",
                Margin = new Thickness(60, 1, 0, -1),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 55,
                Height = 25
            };
            ForwardButton.Click += GoForward;

            BackButton = new Button
            {
                Content = "Назад",
                Margin = new Thickness(1, 1, 0, -1),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 55,
                Height = 25
            };
            BackButton.Click += GoBack;

            UrlTextBox = new TextBox
            {
                Height = 25,
                TextWrapping = TextWrapping.NoWrap,
                Text = "",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(120, 1, 283, -1),

            };
            UrlTextBox.KeyUp += URLboxEnter;
            

            NavigateButton = new Button
            {
                Content = "Перейти",
                Margin = new Thickness(0, 1, 208, -1),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 70,
                Height = 25,

            };
            NavigateButton.Click += Navigate;

            NewTabButton = new Button
            {
                Content = "Новая вкладка",
                Margin = new Thickness(0, 1, 114, -1),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 89,
                Height = 25,

            };
            NewTabButton.Click += NewTab;

            CloseTabButton = new Button
            {
                Content = "Закрыть вкладку",
                Margin = new Thickness(0, 1, 5, -1),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 100,
                Height = 25
            };
            CloseTabButton.Click += CloseTab;

            
            
           
            

            Browser = new ChromiumWebBrowser();
            Browser.DownloadHandler = new DownloadHandler();
            Browser.FrameLoadStart += UpdateTitle;
            Browser.LoadingStateChanged += UpdateTitle;
            Browser.FrameLoadEnd += UpdateTitle;
            Browser.LoadingStateChanged += UpdateURL;
            Browser.FrameLoadStart += UpdateURL;
            Browser.FrameLoadEnd += UpdateURL;
            
            TabContextMenu = new ContextMenu();
            Content = MainGrid;
            Background = new SolidColorBrush(Colors.White);

            MainGrid.Margin = new Thickness(1, 1, 1, 1);
            MainGrid.Background = new SolidColorBrush(Colors.White);

            BrowserGrid.Margin = new Thickness(0, 33, 0, 0);


            MainGrid.Children.Add(ForwardButton);
            MainGrid.Children.Add(BackButton);
            MainGrid.Children.Add(NavigateButton);
            MainGrid.Children.Add(UrlTextBox);
            MainGrid.Children.Add(NewTabButton);
            MainGrid.Children.Add(CloseTabButton);
            MainGrid.Children.Add(BrowserGrid);




            BrowserGrid.Children.Add(Browser);
            TabTitle.Content = "Новая вкладка";
            Browser.Address = "google.com";
            IsSelected = true;

            InitContextMenu();

        }

        private void InitContextMenu()
        {

            DublicateMenuItem = new MenuItem { Header = "Дублировать вкладку" };
            DublicateMenuItem.Click += CloneTab;

            CloseTabMenuItem = new MenuItem { Header = "Закрыть вкладку" };
            CloseTabMenuItem.Click += CloseTab;

            PinTabMenuItem = new MenuItem { Header = "Закрепить/Открепить вкладку" };
            PinTabMenuItem.Click += PinTab;

            TabContextMenu.Items.Add(DublicateMenuItem);
            TabContextMenu.Items.Add(CloseTabMenuItem);
            TabContextMenu.Items.Add(PinTabMenuItem);
            ContextMenu = TabContextMenu;
        }

        private void PinTab(object sender, RoutedEventArgs e)
        {
            Pined = !Pined;

            UpdateTitle(null, null);

        }

     

        private void UpdateTitle(object sender, object e)
        {
            //Dispatcher.Invoke(delegate ()
            //{
            //    if (Browser.Title != null)
            //    {
            //        if (Pined)
            //        {
            //            Header = "🔒 " + Browser.Title;
            //        }
            //        else
            //        {
            //            Header = Browser.Title;
            //        }

            //    }

            //});

            Dispatcher.Invoke(delegate ()
            {
                if (Browser.Title != null)
                {
                    if (Pined)
                    {
                        TabTitle.Content = "🔒 " + Browser.Title;
                    }
                    else
                    {
                        TabTitle.Content = Browser.Title;
                    }

                }

            });

        }

        public void NewTab(object sender, object e)
        {
            BrowserTab addTab = new BrowserTab();
            ((TabControl)Parent).Items.Add(addTab);
           
        }

        public void CloneTab(object sender, object e)
        {
            BrowserTab cloneTab = new BrowserTab();
            cloneTab.Browser.Address = Browser.Address;
            ((TabControl)Parent).Items.Add(cloneTab);
        }

        public void UpdateURL(object sender, object e)
        {
            try
            {
                Dispatcher.Invoke(() => UrlTextBox.Text = Browser.Address);
            }
            catch
            {

            }

            Dispatcher.Invoke(() => History.AddToHistory(Browser.Address,HistoryPath));
          
       
          
            
        }

        public void Navigate(object sender, RoutedEventArgs e)
        {
            Browser.Address = "";
            Browser.Load(UrlTextBox.Text);

        }

        public void GoBack(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack)
                Browser.WebBrowser.Back();
        }
        public void GoForward(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoForward)
                Browser.WebBrowser.Forward();
        }

        public void URLboxEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Browser.Address = "";
                Browser.Load(UrlTextBox.Text);
            }
        }

        public void CloseTab(object sender, object e)
        {
            if(!Pined)
                ((TabControl)Parent).Items.Remove(this);
            
        }

    }
}
