using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.SqlClient;

namespace back_side_system_25shiun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<int> serchIDs = new List<int>(); //進階搜尋的結果
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";
        }

        private void btn登入_Click(object sender, EventArgs e)
        {
            if (txt員工工號.Text != "" && txt密碼.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select 員工工號,員工姓名,職等,密碼 from employee where 員工工號=@NewNum;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@NewNum", txt員工工號.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                string Code = "";
                int i = 0;

                while (reader.Read())
                {
                    Code = reader["密碼"].ToString();                   
                    Global員工資訊.員工姓名 = reader["員工姓名"].ToString();
                    Global員工資訊.員工職等= reader["職等"].ToString();
                    Global員工資訊.員工工號 = reader["員工工號"].ToString();
                    Global員工資訊.員工密碼 = reader["密碼"].ToString();
                    i++;
                }
                reader.Close();
                con.Close();

                if ((txt員工工號.Text == Global員工資訊.員工工號) && Code == txt密碼.Text)
                {
                    MessageBox.Show("親愛的Sunny團隊夥伴，您已成功登入後台管理系統!");
                    if (Global員工資訊.員工職等 == "EM")
                    {
                        高層主管 news = new 高層主管();
                        this.Hide();
                        news.ShowDialog();
                    }
                    else if (Global員工資訊.員工職等 == "SN")
                    {
                        人力資源 news = new 人力資源();
                         this.Hide(); news.ShowDialog();
                       
                    }
                    else if (Global員工資訊.員工職等 == "GA")
                    {
                        一般員工 news = new 一般員工();
                        this.Hide(); news.ShowDialog();                        
                    }
                    else {
                        MessageBox.Show("不好意思，您無權登入後台管理系統！");
                    }
                } else if (txt員工工號.Text != Global員工資訊.員工工號)
                {
                    MessageBox.Show("查無此員工工號，您無權瀏覽!");
                    txt密碼.Text = "";
                    txt員工工號.Text = "";
                }
                else
                {
                    MessageBox.Show("您輸入的密碼有誤!");
                    txt密碼.Text = "";                   
                }
            }
            else
            {
                MessageBox.Show("請輸入完整員工登入資訊！");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("from closed");
        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("from closeing");
        }
    }
}
