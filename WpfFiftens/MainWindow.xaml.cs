using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

namespace WpfFiftens
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int hardcore = 0;
            if ((bool)CheckBox1.IsChecked) hardcore=1;
            if (ComboBox1.SelectedItem == null) return;
            if (ComboBox2.SelectedItem == null) return;
            if (ComboBox3.SelectedItem == null) return;
            int form = Convert.ToInt32(((TextBlock)ComboBox1.SelectedItem).Text);
            int row = Convert.ToInt32(((TextBlock)ComboBox2.SelectedItem).Text);
            int columns = Convert.ToInt32(((TextBlock)ComboBox3.SelectedItem).Text);
            WindowForBoard window1 = new WindowForBoard(row, columns, form, hardcore, this);
            window1.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            MessageBox.Show(menuItem.Header.ToString());
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
