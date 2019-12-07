using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfFiftens
{
    public partial class WindowForBoard : Window
    {
        Window mainwindow;
        List<Button> buttons = new List<Button>();
        int row, columns, hardmod;
        Label[,] label;
        Facade facade;

        public WindowForBoard(int row1, int colum, int lvl, int hardcore, Window window)
        {
            mainwindow = window;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            InitializeComponent();
            uniformGrid.Rows = row1;
            this.row = row1;
            uniformGrid.Columns = colum;
            this.columns = colum;
            int multiplier = 10;
            lvl *= multiplier;
            facade = new Facade(row, columns, lvl , (int)(lvl / multiplier));
            if (hardcore == 1)
            {
                HardModeON(this, new RoutedEventArgs());
                chekbox1.IsChecked = true;
            }

            int z = 0;
            label = new Label[row, columns];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < columns; j++)
                {
                    label[i, j] = new LabelForGame(facade.board[i, j]);
                    label[i, j].MouseDown += LFG_MouseDown;
                    label[i, j].Drop += Label_Drop;
                    uniformGrid.Children.Add(label[i, j]);
                    z++;
                }
            AllowDropBoard();
            facade.Saver();
        }

        private void LFG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop((Label)sender, ((Label)sender).Content.ToString(), DragDropEffects.Copy);
        }

        private void Label_Drop(object sender, DragEventArgs e)
        {
            bool endgame;
            Label lbldrop = e.Data as Label;
            Label lblsender = sender as Label;
            int contentdrop = Convert.ToInt32(e.Data.GetData(DataFormats.Text));
            int contentsender = Convert.ToInt32(((Label)sender).Content.ToString());
            if (contentdrop != 0 && contentsender != 0) return;
            if (contentdrop != 0 && contentsender == 0) endgame = facade.Rouler(contentdrop, contentsender,false);
            else endgame = facade.Rouler(contentsender, contentdrop, false);
            DoRandom();
            LoadBoard();
            AllowDropBoard();
        }

        private void Winers(bool a)
        {
            if (a != true)
            {
                MessageBox.Show("Winner!");
                RoutedEventArgs forform = new RoutedEventArgs();
                MenuMainMenu(this, forform);
            }
        }

        int ifrandomon = 1;
        private void DoRandom()
        {
            ifrandomon += hardmod;
            if (ifrandomon % 3 == 0)
            {
                facade.Random();
                ifrandomon = 1;
                LoadBoard();
                AllowDropBoard();
            }
        }

        private void AllowDropBoard()
        {
            int height_zero=0, width_zero=0;
            for (int i = 0; i < row; ++i)
                for (int j = 0; j < columns; ++j)
                {
                    label[i, j].AllowDrop = false;
                    if (Convert.ToInt32(label[i, j].Content) == 0)
                    {
                        width_zero  = j;
                        height_zero =  i;
                    }
                }
            try
            {
                label[height_zero , width_zero].AllowDrop = true;
                if (height_zero - 1 >= 0) label[height_zero  - 1, width_zero].AllowDrop = true;
                if (height_zero + 1 < row ) label[height_zero + 1, width_zero].AllowDrop = true;
                if (width_zero - 1 >= 0) label[height_zero, width_zero - 1].AllowDrop = true;
                if (width_zero + 1 < columns) label[height_zero, width_zero + 1].AllowDrop = true;
            }
            catch (Exception e) { MessageBox.Show("error" + e); }
            Winers(facade.Rouler(0, 0, true));
        }

        private void LoadBoard()
        {
            for (int i = 0; i < row; i++)
                for (int j = 0; j < columns; j++)
                {
                    label[i, j].Content = facade.board[i, j];
                }
            AllowDropBoard();
        }

        private void MenuMainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void MenuExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void MenuSurrender(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Loser!");
            MenuItem menuItem = (MenuItem)sender;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void MoveBack(object sender, RoutedEventArgs e)
        {
            facade.Cancel();
            LoadBoard();
            AllowDropBoard();
        }

        private void HardModeON(object sender, RoutedEventArgs e)
        {
            hardmod = 1;
        }

        private void HardModeOFF(object sender, RoutedEventArgs e)
        {
            hardmod = 0;
        }
    }

    class LabelForGame : Label
    {
        public LabelForGame(int content)
        {
            this.Width = 100;
            this.Height = 100;
            this.FontFamily = new FontFamily("Consolas");
            this.FontSize = 33;
            this.Content = content;
            this.BorderBrush = Brushes.Aqua;
            this.BorderThickness = new Thickness(5);
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
        }
    }
}
