﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private bool FulScreen = false;

        public MainWindow()
        {
            InitializeComponent();
        }

       



        private void OnFormLoad(object sender, RoutedEventArgs e)
        {
          BrowserTab NewTab = new BrowserTab();
            Tabs.Items.Add(NewTab);
        }

       

  

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                if (!FulScreen)
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    ResizeMode = ResizeMode.NoResize;
                    FulScreen = !FulScreen;
                }
                else
                {
                    
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    ResizeMode = ResizeMode.CanResize;
                    FulScreen = !FulScreen;
                }
                
            }

         
                

        }
    }


}
