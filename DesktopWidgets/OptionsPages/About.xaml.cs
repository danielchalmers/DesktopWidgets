﻿using System.Windows.Controls;
using DesktopWidgets.WindowViewModels;

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About(AboutViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}