﻿// Copyright (C) 2015 Smart&Soft SAS (http://www.smartnsoft.com/) and contributors
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// Contributors:
//   Smart&Soft - initial API and implementation

using System;
using System.Windows;

namespace ModernApp4Me.WP8.Sample
{

    /// <author>Ludovic ROLAND</author>
    /// <since>2015.03.05</since>
    public partial class MainPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnExceptionHandler_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ExceptionHandler/ExceptionHandlerPage.xaml", UriKind.Relative));
        }

        private void BtnMemoryProfiler_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MemoryProfiler/MemoryProfilerPage.xaml", UriKind.Relative));
        }

        private void BtnBitmapDownloader_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/BitmapDownloader/BitmapDownloaderPage.xaml", UriKind.Relative));
        }

        private void BtnPhoneApplicationPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PhoneApplicationPage/PhoneApplicationPagePage.xaml", UriKind.Relative));
        }
    }
}