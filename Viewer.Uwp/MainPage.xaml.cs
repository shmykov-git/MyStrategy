﻿using Windows.UI.Xaml.Controls;
using Viewer.Uwp.Viewer;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Viewer.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public Canvas Canvas => canvas;
    }
}