using Manager.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //创建字典，方便导航
        private Dictionary<string, Uri> allViews = new Dictionary<string, Uri>();

        //新建sql操作对象
        private SqliteOperation sql = new SqliteOperation();

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            allViews.Add("番剧总览", new Uri("pack://application:,,,/Pages/Statistics.xaml", UriKind.RelativeOrAbsolute));
            allViews.Add("番剧评价", new Uri("pack://application:,,,/Pages/Appraise.xaml", UriKind.RelativeOrAbsolute));
            allViews.Add("番剧管理", new Uri("pack://application:,,,/Pages/Management.xaml", UriKind.RelativeOrAbsolute));
            allViews.Add("番剧数据库", new Uri("pack://application:,,,/Pages/Database.xaml", UriKind.RelativeOrAbsolute));
        }

        #region 控件操作
        /// <summary>
        /// 设置导航功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;
            if (myFrame != null)
            {
                myFrame.Navigate(allViews[listBoxItem.Content.ToString()]);
            }
        }

        #endregion
    }
}
