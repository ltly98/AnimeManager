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
    /// Management.xaml 的交互逻辑
    /// </summary>
    public partial class Management : Page
    {
        //创建数据库操作对象
        private SqliteOperation sql = new SqliteOperation();
        //存储链表索引
        private int AnimeIndex { get; set; }
        public Management()
        {
            InitializeComponent();

            if (sql.CheckDatabase())
            {
                //初始化链表
                if(!sql.AnimeListOperation("SELECT * FROM Anime"))
                {
                    MessageBox.Show("初始化链表失败！");
                }
                AnimeIndex = -1;
            }
            else
            {
                MessageBox.Show("数据库不存在，已创建！");
            }
        }

        #region 四个按钮功能的实现

        /// <summary>
        /// 查找信息按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimeId.Text.Equals(""))
            {
                MessageBox.Show("请输入序号进行查找！");
                return;
            }
            try
            {
                AnimeIndex = int.Parse(AnimeId.Text) -1;
            }
            catch
            {
                MessageBox.Show("输入的序号格式出错！");
                return;
            }
            
            if (AnimeIndex < 0 || AnimeIndex > sql.ListLength - 1)
            {
                MessageBox.Show("目标不存在！");
                return;
            }


            //设置名称
            AnimeName.Text = sql.AnimeList[AnimeIndex].Name;
            //设置发行时间
            AnimeDate.Text = sql.AnimeList[AnimeIndex].Date;
            //设置分类
            AnimeCategory.Text = sql.AnimeList[AnimeIndex].Category;
            //设置总话数
            AnimeSets.Text = sql.AnimeList[AnimeIndex].Sets;
            //设置作者
            AnimeAuthor.Text = sql.AnimeList[AnimeIndex].Author;
            //设置动画制作
            AnimeAnimation.Text = sql.AnimeList[AnimeIndex].Animation;
            //设置CAST
            AnimeCast.Text = sql.AnimeList[AnimeIndex].Cast;
        }

        /// <summary>
        /// 添加信息按钮功能的实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            //必要内容必须不为空
            if(AnimeName.Text.Equals("") || AnimeDate.Text.Equals("") ||
                AnimeCategory.Text.Equals("") || AnimeSets.Text.Equals("") || 
                AnimeAuthor.Text.Equals("") || AnimeAnimation.Text.Equals("") ||
                AnimeCast.Text.Equals(""))
            {
                MessageBox.Show("输入的内容不全！");
                return;
            }

            //命令太长了，为了美观，所以多写了点
            bool Result = sql.InsertBasicInformation(AnimeName.Text, AnimeDate.Text, AnimeCategory.Text, AnimeSets.Text, AnimeAuthor.Text, AnimeAnimation.Text, AnimeCast.Text);

            if (Result)
            {
                MessageBox.Show("成功添加！");
            }
            else
            {
                MessageBox.Show("添加失败！");
            }
        }

        /// <summary>
        /// 修改按钮功能的实现，支持部分修改，单独报错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if(AnimeIndex == -1)
            {
                MessageBox.Show("请先查找目标再进行修改！");
                return;
            }

            //命令太长了，为了美观，所以多写了点
            bool IsSuccess = false;
            bool IsUpdate = false;

            //修改名称
            if (!AnimeName.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex+1).ToString(),"Name", AnimeName.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改名称失败！");
                }
            }

            //修改发行日期
            if (!AnimeDate.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex + 1).ToString(), "Date", AnimeDate.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改发行日期失败！");
                }
            }

            //修改分类
            if (!AnimeCategory.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex + 1).ToString(), "Category", AnimeCategory.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改分类失败！");
                }
            }

            //修改总话数
            if (!AnimeSets.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex + 1).ToString(), "Sets", AnimeSets.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改总话数失败！");
                }
            }

            //修改作者
            if (!AnimeAuthor.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex + 1).ToString(), "Author", AnimeAuthor.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改作者失败！");
                }
            }

            //修改动画制作
            if (!AnimeAnimation.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex + 1).ToString(), "Animation", AnimeAnimation.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改动画制作失败！");
                }
            }

            //修改Cast
            if (!AnimeCast.Text.Equals(""))
            {
                IsSuccess = sql.UpdateAppointInformation((AnimeIndex + 1).ToString(), "Cast", AnimeCast.Text);
                if (IsSuccess)
                {
                    IsUpdate = true;
                }
                else
                {
                    MessageBox.Show("修改Cast失败！");
                }
            }

            if (IsUpdate)
            {
                MessageBox.Show("修改成功！");
            }
            else
            {
                MessageBox.Show("未做任何修改！");
            }
        }

        /// <summary>
        /// 删除按钮功能的实现，支持确认后删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimeId.Text.Equals(""))
            {
                MessageBox.Show("请输入序号进行删除！");
                return;
            }

            try
            {
                AnimeIndex = int.Parse(AnimeId.Text)-1;
            }
            catch
            {
                MessageBox.Show("输入的序号格式出错！");
                return;
            }

            if (AnimeIndex < 0 || AnimeIndex > sql.ListLength - 1)
            {
                MessageBox.Show("目标不存在！");
                return;
            }

            if ((int)MessageBox.Show("确认删除？", "提示",MessageBoxButton.OKCancel) == 1)
            {
                sql.DeleteAppointInformation((AnimeIndex + 1).ToString());
                MessageBox.Show("删除成功！");
            }
        }

        #endregion
    }
}
