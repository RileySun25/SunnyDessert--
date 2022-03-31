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
    public partial class 更改密碼 : Form
    {
        public 更改密碼()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<int> serchIDs = new List<int>(); //進階搜尋的結果

        private void 更改密碼_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";
        }

        private void btn確認_Click(object sender, EventArgs e)
        {
            if (txt目前密碼.Text == Global員工資訊.員工密碼 )
            {
                if (txt確認新密碼.Text == txt新密碼.Text)
                {
                    SqlConnection con = new SqlConnection(mySunnyConnectionString);
                    con.Open();
                    string str = "Update employee set 密碼 = @NewCode where 員工工號 = @SerchId;";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.Parameters.AddWithValue("@SerchId", Global員工資訊.員工工號);
                    cmd.Parameters.AddWithValue("@NewCode", txt確認新密碼.Text);

                    int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                    con.Close();
                    MessageBox.Show("密碼已重新設定完成!");
                    this.Close();
                }
                else {
                    MessageBox.Show("確認密碼需與新密碼相同!");
                }
            }
            else {
                MessageBox.Show("目前密碼輸入錯誤!!");
            }
            
            
        }
    }
}
