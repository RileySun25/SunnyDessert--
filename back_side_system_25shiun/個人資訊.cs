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
    public partial class 個人資訊 : Form
    {
        public 個人資訊()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<int> serchIDs = new List<int>(); //進階搜尋的結果

        private void 個人資訊_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";

            SqlConnection con = new SqlConnection(mySunnyConnectionString);
            con.Open();
            string str = "select* from employee where 員工工號=@NewNum;";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@NewNum", Global員工資訊.員工工號);
            SqlDataReader reader = cmd.ExecuteReader();            
            int i = 0;

            while (reader.Read())
            {                
                lbl姓名.Text = reader["員工姓名"].ToString();
                lbl職等.Text = reader["職等"].ToString();
                lbl工號.Text = reader["員工工號"].ToString();
                lbl信箱.Text = reader["員工信箱"].ToString();
                lbl到職日.Text= reader["到職日"].ToString();
                lbl電話.Text= reader["員工電話"].ToString();
                lbl住址.Text= reader["員工住址"].ToString();
                dtp生日.Value = Convert.ToDateTime(reader["員工生日"]);
                Boolean sex = Convert.ToBoolean(reader["員工性別"]);
                ckbox男生.Checked = Convert.ToBoolean(reader["員工性別"]);
                if (Convert.ToBoolean(reader["員工性別"]))
                {
                    ckbox男生.Checked = Convert.ToBoolean(reader["員工性別"]);
                }
                else {
                    ckbox女生.Checked = !sex;
                }
                i++;
            }
            reader.Close();
            con.Close();
        }
    }
}
