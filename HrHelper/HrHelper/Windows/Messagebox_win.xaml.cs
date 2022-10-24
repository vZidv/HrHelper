﻿using HrHelper.Classes;
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

namespace HrHelper.Windows
{
    /// <summary>
    /// Interaction logic for Messagebox_win.xaml
    /// </summary>
    public partial class Messagebox_win : Window
    {
        public string title {
            get
            {
                return titile_tb.Text;
            }
            set {
                titile_tb.Text = value;
            } 
        }
        public string message
        {
            get
            {
                return message_tb.Text;
            }
            set
            {
                message_tb.Text = value;
            }
        }
        public Messagebox_win(MyMessageBoxOptions messageBoxoptions)
        {
            InitializeComponent();

            if (messageBoxoptions == MyMessageBoxOptions.Ok)
                ok_sp.Visibility = Visibility.Visible;
            if (messageBoxoptions == MyMessageBoxOptions.YesNo)
                yesNo_sp.Visibility = Visibility.Visible;
        }

        private void ok_but_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void toolBar_grid_MouseDown(object sender, MouseButtonEventArgs e)=> DragMove();
    }
}
