using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manager.Pages
{
    /// <summary>
    /// Database.xaml 的交互逻辑
    /// </summary>
    public partial class Database : Page
    {
        //创建数据库操作对象
        private SqliteOperation sql = new SqliteOperation();
        //显示集合
        private ObservableCollection<AnimeInfo> DataGridList = new ObservableCollection<AnimeInfo>();
        //获取选项的值
        private string ComboBoxValue { get; set; }

        public Database()
        {
            InitializeComponent();
            if(sql.AnimeListOperation("SELECT* FROM Anime"))
            {
                ShowDataGrid();
            }
            else
            {
                MessageBox.Show("初始化链表失败！");
            }
            
        }

        #region 基本操作

        /// <summary>
        /// 显示表格
        /// </summary>
        private void ShowDataGrid()
        {
            DataGridList.Clear();
            foreach(AnimeInfo a in sql.AnimeList)
            {
                DataGridList.Add(a);
            }
            AnimeDataGrid.ItemsSource = DataGridList;
        }

        #endregion

        #region 控件操作

        /// <summary>
        /// 动态获取选项的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)sender;
            ComboBoxValue = item.Content.ToString();
        }
        /// <summary>
        /// 关键词查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputKeyWord.Text.Equals(""))
            {
                MessageBox.Show("输入内容不能为空！");
                return;
            }

            string cmd = "SELECT* FROM Anime WHERE "+ ComboBoxValue + " LIKE '%"+InputKeyWord.Text+"%'";
            
            if (sql.AnimeListOperation(cmd))
            {
                ShowDataGrid();
                MessageBox.Show("查找成功！");
            }
            else
            {
                MessageBox.Show("查找失败！");
            }
        }

        #endregion


    }
}
