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
    /// Appraise.xaml 的交互逻辑
    /// </summary>
    public partial class Appraise : Page
    {
        private SqliteOperation sql = new SqliteOperation();
        private int ShowIndex { get; set; }
        private string RadioButtonValue { get; set; }
        public Appraise()
        {
            InitializeComponent();
            ShowIndex = 0;
            RadioButtonValue = "";

            if (sql.CheckDatabase())
            {
                //初始化链表
                if(sql.AnimeListOperation("SELECT * FROM Anime"))
                {
                    if(sql.ListLength != 0)
                    {
                        AppraiseAnime(ShowIndex);
                    } 
                }
                else
                {
                    MessageBox.Show("初始化链表失败！");
                } 
            }
            else
            {
                MessageBox.Show("数据库不存在，已创建！");
            }

            
        }

        #region 通用操作
        /// <summary>
        /// 显示动漫基本信息
        /// </summary>
        /// <param name="index"></param>
        private void AppraiseAnime(int index)
        {
            //设置序号
            AnimeId.Text = "序号："+sql.AnimeList[index].Id;
            //设置名称
            AnimeName.Text = "动漫名称：" + sql.AnimeList[index].Name;
            //设置发行时间
            AnimeDate.Text = "发行时间：" + sql.AnimeList[index].Date;
            //设置分类
            AnimeCategory.Text = "分类：" + sql.AnimeList[index].Category;
            //设置总话数
            AnimeSets.Text = "总话数：" + sql.AnimeList[index].Sets;
            //设置作者
            AnimeAuthor.Text = "原作：" + sql.AnimeList[index].Author;
            //设置动画制作
            AnimeAnimation.Text = "动画制作：" + sql.AnimeList[index].Animation;
            //设置CAST
            string cast = sql.AnimeList[index].Cast;
            //替换换行再显示
            AnimeCast.Text = "CAST：\n"+ cast.Replace(",", "\n");
            //设置基本评价
            switch (sql.AnimeList[index].BasicEvaluation)
            {
                case "差评":
                    BadRadioButton.IsChecked = true;
                    break;
                case "一般":
                    GeneralRadioButton.IsChecked = true;
                    break;
                case "好评":
                    GoodRadioButton.IsChecked = true;
                    break;
                default:
                    BadRadioButton.IsChecked = false;
                    GeneralRadioButton.IsChecked = false;
                    GoodRadioButton.IsChecked = false;
                    break;
            }
            //设置简单评价
            if (!sql.AnimeList[index].Evaluation.Equals("")){
                InputEvaluation.Text = sql.AnimeList[index].Evaluation;
            }
            else
            {
                InputEvaluation.Text = "";
            } 
        }

        #endregion

        #region 控件操作

        /// <summary>
        /// 单选框通用操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            RadioButtonValue = radioButton.Content.ToString();
        }

        /// <summary>
        /// 按钮通用操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sql.ListLength == 0)
            {
                MessageBox.Show("数据库为空");
                return;
            }
            Button button = (Button)sender;
            switch (button.Content)
            {
                case "上一部":
                    if (ShowIndex == 0)
                    {
                        MessageBox.Show("已经是第一部了！");
                    }
                    else
                    {
                        ShowIndex--;
                        AppraiseAnime(ShowIndex);
                    }
                    break;
                case "下一部":
                    if (ShowIndex == sql.ListLength - 1)
                    {
                        MessageBox.Show("已经是最后一部了！");
                    }
                    else
                    {
                        ShowIndex++;
                        AppraiseAnime(ShowIndex);
                    }
                    break;
                case "跳转":
                    int input = 0;
                    try
                    {
                        input = int.Parse(InputIndex.Text);
                        input--;
                    }
                    catch
                    {
                        MessageBox.Show("输入有误，请重新输入！");
                        break;
                    }
                    if (input >= 0 && input <= ShowIndex)
                    {
                        MessageBox.Show("目标存在，即刻跳转！");
                        ShowIndex = input;
                        AppraiseAnime(ShowIndex);
                    }
                    else
                    {
                        MessageBox.Show("目标不存在！");
                    }
                    break;
                case "基本评价提交":
                    if (RadioButtonValue.Equals(""))
                    {
                        MessageBox.Show("没有评价！");
                        break;
                    }
                    sql.UpdateAppointInformation(sql.AnimeList[ShowIndex].Id, "BasicEvaluation", RadioButtonValue);
                    MessageBox.Show("基本评价提交成功！");
                    break;
                case "简单评价提交":
                    if (InputEvaluation.Text.Equals(""))
                    {
                        MessageBox.Show("没有评价！");
                        break;
                    }
                    sql.UpdateAppointInformation(sql.AnimeList[ShowIndex].Id, "Evaluation", InputEvaluation.Text);
                    MessageBox.Show("简单评价提交成功！");
                    break;
                default:
                    break;
            }
            
        }


        #endregion

        
    }
}
