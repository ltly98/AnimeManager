using System;
using System.Collections.Generic;
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
    /// Statistics.xaml 的交互逻辑
    /// </summary>
    public partial class Statistics : Page
    {
        //创建数据库操作对象
        private SqliteOperation sql = new SqliteOperation();
        public Statistics()
        {
            InitializeComponent();

            if (sql.CheckDatabase())
            {
                StatisticsAnime();
            }
            else
            {
                MessageBox.Show("数据库不存在，已创建！");
            }
            
        }

        #region 基本操作

        /// <summary>
        /// 设定统计数值到总览界面
        /// </summary>
        private void StatisticsAnime()
        {
            BadAnime.Text = sql.GetAppointNum("SELECT count(*) FROM Anime WHERE BasicEvaluation='差评'").ToString();
            GeneralAnime.Text = sql.GetAppointNum("SELECT count(*) FROM Anime WHERE BasicEvaluation='一般'").ToString();
            GoodAnime.Text = sql.GetAppointNum("SELECT count(*) FROM Anime WHERE BasicEvaluation='好评'").ToString();
            TotalAnime.Text = sql.GetAppointNum("SELECT count(*) FROM Anime").ToString();
        }

        #endregion
    }
}
