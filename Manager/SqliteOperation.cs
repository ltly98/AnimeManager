using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Windows.Media.Animation;
using System.Windows;

namespace Manager
{
    /// <summary>
    /// 存放一个动漫信息的类，翻遍链表存储
    /// </summary>
    class AnimeInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Sets { get; set; }
        public string Author { get; set; }
        public string Animation { get; set; }
        public string Cast { get; set; }
        public string BasicEvaluation { get; set; }
        public string Evaluation { get; set; }
    }


    /// <summary>
    /// sqlite操作的一个类，集合所有的自定义操作
    /// </summary>
    class SqliteOperation
    {
        //数据库路径
        public string SqlitePath { get; set; }

        //存储链表
        public List<AnimeInfo> AnimeList = new List<AnimeInfo>();

        //链表长度，主要是显示信息使用
        public int ListLength { get; set; }

        //新建对象的时候直接设置路径
        public SqliteOperation(){
            //直接设定路径，方便操作，减少代码量
            this.SqlitePath = "Anime.db";
        }


        #region 数据库基本操作

        /// <summary>
        /// 用于检查数据库状态，如果不存在就创建。
        /// </summary>
        /// <returns>操作是否成功的布尔值</returns>
        public bool CheckDatabase()
        {
            if (!File.Exists(SqlitePath))
            {
                string cmd = "CREATE TABLE Anime(" +
                        "Id varchar(5) PRIMARY KEY NOT NULL," +
                        "Name varchar(50) NOT NULL," +
                        "Date varchar(10) NOT NULL," +
                        "Category varchar(30) NOT NULL," +
                        "Sets varchar(5) NOT NULL," +
                        "Author varchar(50) NOT NULL," +
                        "Animation varchar(50) NOT NULL," +
                        "Cast varchar(200) NOT NULL," +
                        "BasicEvaluation varchar(5) NULL," +
                        "Evaluation varchar(100) NULL)";
                ExecuteNoReturn(cmd);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 添加基本信息，并能够重新初始化链表，自动生成Id，无需手动指定
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Date"></param>
        /// <param name="Category"></param>
        /// <param name="Sets"></param>
        /// <param name="Author"></param>
        /// <param name="Animation"></param>
        /// <param name="Cast"></param>
        /// <returns>操作是否成功的布尔值</returns>
        public bool InsertBasicInformation(string Name,string Date,string Category,string Sets,string Author,string Animation, string Cast)
        {
            if (File.Exists(SqlitePath))
            {
                string s_num = "";
                int i_num = GetAppointNum("SELECT count(*) FROM Anime");

                if(i_num == -1)
                {
                    return false;
                }

                s_num = (i_num+1).ToString();

                string cmd2 = "INSERT INTO Anime(Id,Name,Date,Category,Sets,Author,Animation,Cast) VALUES('" + s_num+"','"+Name+"','"+Date+"','" + Category+"','"+Sets+"','"+ Author + "','"+ Animation + "','"+ Cast + "')";
                ExecuteNoReturn(cmd2);
                if (this.AnimeListOperation("SELECT * FROM Anime"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 修改目标列的值，修改完成更新链表
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns>操作是否成功的布尔值</returns>
        public bool UpdateAppointInformation(string Id,string target,string value)
        {
            if (File.Exists(SqlitePath))
            {
                string cmd = "UPDATE Anime SET "+target+"='"+value+ "' WHERE Id='"+Id+"'";
                ExecuteNoReturn(cmd);
                if (this.AnimeListOperation("SELECT * FROM Anime"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 删除指定Id的信息，删除后会重新调整Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>操作是否成功的布尔值</returns>
        public bool DeleteAppointInformation(string Id)
        {
            if (File.Exists(SqlitePath))
            {
                string cmd = "DELETE FROM Anime WHERE Id = '"+Id+"'";
                ExecuteNoReturn(cmd);
                if (this.AnimeIdAdjustment())
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 通用操作

        /// <summary>
        /// 用于执行sql命令，不需要返回值的方法
        /// </summary>
        /// <param name="command"></param>
        private void ExecuteNoReturn(string command)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + this.SqlitePath);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = con;
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        /// <summary>
        /// 统计指定数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns>指定的数量</returns>
        public int GetAppointNum(string command)
        {

            string s_num = "";
            int i_num = 0;

            //获取行数
            SQLiteConnection con = new SQLiteConnection("Data Source=" + SqlitePath);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                SQLiteCommand cmd1 = new SQLiteCommand();
                cmd1.Connection = con;
                cmd1.CommandText = command;
                SQLiteDataReader sql_num = cmd1.ExecuteReader();
                while (sql_num.Read())
                {
                    s_num = sql_num[0].ToString();
                }
            }
            con.Close();

            try
            {
                i_num = int.Parse(s_num);
            }
            catch
            {
                return -1;
            }

            return i_num;
        }

        /// <summary>
        /// 调整序号的方法，主要用于删除后的操作
        /// </summary>
        /// <returns>操作是否成功的布尔值</returns>
        public bool AnimeIdAdjustment()
        {
            if (File.Exists(SqlitePath))
            {
                int num = 1;
                int i_num;
                SQLiteConnection con = new SQLiteConnection("Data Source=" + this.SqlitePath);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    SQLiteCommand cmd1 = new SQLiteCommand();
                    cmd1.Connection = con;
                    cmd1.CommandText = "SELECT * FROM Anime";
                    SQLiteDataReader Info = cmd1.ExecuteReader();
                    while (Info.Read())
                    {
                        try
                        {
                            i_num = int.Parse(Info[0].ToString());
                        }
                        catch
                        {
                            return false;
                        }

                        if (num != i_num)
                        {
                            if (!this.UpdateAppointInformation(Info[0].ToString(), "Id", num.ToString()))
                            {
                                return false;
                            }
                        }
                        num++;
                    }
                }
                con.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化存储番剧信息的链表，并记录链表长度
        /// </summary>
        /// <returns>操作是否成功的布尔值</returns>
        public bool AnimeListOperation(string command)
        {
            if (File.Exists(SqlitePath))
            {
                //防止多次添加
                AnimeList.Clear();

                SQLiteConnection con = new SQLiteConnection("Data Source=" + this.SqlitePath);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    SQLiteCommand cmd1 = new SQLiteCommand();
                    cmd1.Connection = con;
                    cmd1.CommandText = command;
                    SQLiteDataReader Info = cmd1.ExecuteReader();
                    while (Info.Read())
                    {
                        AnimeInfo i = new AnimeInfo();
                        i.Id = Info[0].ToString();
                        i.Name = Info[1].ToString();
                        i.Date = Info[2].ToString();
                        i.Category = Info[3].ToString();
                        i.Sets = Info[4].ToString();
                        i.Author = Info[5].ToString();
                        i.Animation = Info[6].ToString();
                        i.Cast = Info[7].ToString();
                        i.BasicEvaluation = Info[8].ToString();
                        i.Evaluation = Info[9].ToString();
                        AnimeList.Add(i);
                    }
                }
                con.Close();
                this.ListLength = this.AnimeList.ToArray().Length;
                return true;
            }
            return false;
        }

        #endregion
    }
}
