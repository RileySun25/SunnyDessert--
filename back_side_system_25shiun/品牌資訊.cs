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
using System.IO;

namespace back_side_system_25shiun
{
    public partial class 品牌資訊 : Form
    {
        public 品牌資訊()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<string> serchIDs = new List<string>(); //進階搜尋的結果
        string image_dir = @"image\";  //將圖檔路徑寫成欄位
        string image_name = "";

        private void 品牌資訊_Load(object sender, EventArgs e)
        {
            if (Global員工資訊.員工職等 == "EM" || Global員工資訊.員工職等 == "SN")
            {
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
            }

            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";

            cbox搜尋欄位.Items.Add("最新消息標題");
            cbox搜尋欄位.Items.Add("更新日期");
            cbox搜尋欄位.Items.Add("更新者");
            cbox搜尋欄位.SelectedIndex = 0;
        }

        private void btn搜尋_Click(object sender, EventArgs e)
        {
            listBox搜尋.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from news where( " + strFieldName + " like  @SerchString );";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox搜尋.Items.Add("標題：" + reader["最新消息標題"] );
                    serchIDs.Add(reader["最新消息id"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無此訊息刊登!!");
                    txt內容.Text = "";
                    txt更新者.Text = "";
                    txt標題.Text = "";
                    dtp上架日期.Value = Convert.ToDateTime("1990-01-01");

                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入搜關鍵字!");
            }
        }

        private void btn產品列表_Click(object sender, EventArgs e)
        {
            listBox搜尋.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            SqlConnection con = new SqlConnection(mySunnyConnectionString);
            string str = "select*from news ;";
            //查詢是變動的，SQL指令會不同
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;

            while (reader.Read())   //把東西讀出來
            {
                listBox搜尋.Items.Add("標題：" + reader["最新消息標題"] );
                serchIDs.Add(reader["最新消息id"].ToString());
                i++;
            }
            if (i <= 0)
            {
                MessageBox.Show("查無此訊息刊登!!");
                txt內容.Text = "";
                txt更新者.Text = "";
                txt標題.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                lbl最新消息id.Text = "";
            }
            reader.Close();
            con.Close();
        }

        private void btn清空欄位_Click(object sender, EventArgs e)
        {
            txt內容.Text = "";
            txt更新者.Text = "";
            txt標題.Text = "";
            dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
            listBox搜尋.Items.Clear();
            txt進階搜尋內容.Text = "";
        }

        private void btn修改_Click(object sender, EventArgs e)
        {
            if (txt標題.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "Update news set 最新消息標題 = @NewTitle,最新消息內容 = @NewCintent,更新者 = @NewPerson,更新日期 = @NewDate where 最新消息id = @Serchid;";
                //不要用字串合成!會被入侵，避免SQL inJection!用字串插入
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@NewTitle", txt標題.Text);
                cmd.Parameters.AddWithValue("@NewCintent", txt內容.Text);
                cmd.Parameters.AddWithValue("@NewDate",dtp上架日期.Value);
                cmd.Parameters.AddWithValue("@NewPerson", txt更新者.Text);                
                cmd.Parameters.AddWithValue("@Serchid", lbl最新消息id.Text);
                

                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料更新成功!");
                txt內容.Text = "";
                txt更新者.Text = "";
                txt標題.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                lbl最新消息id.Text = "";
            }
            else
            {
                MessageBox.Show("欲修改最新消息資料需填標題及上架時間!");
            }
        }

        private void btn新增_Click(object sender, EventArgs e)
        {
            if ((txt標題.Text != ""))
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "insert into news values (@NewTitle,@NewContent,@NewDate,@NewPerson);";
                SqlCommand smd = new SqlCommand(str, con);
                smd.Parameters.AddWithValue("@NewTitle", txt標題.Text);
                smd.Parameters.AddWithValue("@NewContent", txt內容.Text);
                smd.Parameters.AddWithValue("@NewDate", dtp上架日期.Value);
                smd.Parameters.AddWithValue("@NewPerson", txt更新者.Text);


                int rows = smd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料新增成功!");
                txt內容.Text = "";
                txt更新者.Text = "";
                txt標題.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                lbl最新消息id.Text = "";
            }
            else
            {
                MessageBox.Show("慾新增最新消息，\n需輸入最新消息標題!!");
            }
        }

        private void btn刪除_Click(object sender, EventArgs e)
        {
            string intid = "";
            intid = lbl最新消息id.Text;

            if (intid != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "delete from news where 最新消息id =@SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intid);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                txt內容.Text = "";
                txt更新者.Text = "";
                txt標題.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                lbl最新消息id.Text = "";

                MessageBox.Show("資料刪除成功!");
            }
        }
        private void listBox搜尋_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox搜尋.SelectedIndex > -1)
            {
                
                string intId = serchIDs[listBox搜尋.SelectedIndex];
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select*from news where 最新消息id = @SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    txt內容.Text = (reader["最新消息內容"].ToString());
                    txt標題.Text = (reader["最新消息標題"].ToString());
                    txt更新者.Text = (reader["更新者"].ToString());
                    dtp上架日期.Value = Convert.ToDateTime(reader["更新日期"]);
                    lbl最新消息id.Text = (reader["最新消息id"].ToString());

                }
                else
                {
                    MessageBox.Show("查無此訊息!!");
                    txt內容.Text = "";
                    txt更新者.Text = "";
                    txt標題.Text = "";
                    dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                    lbl最新消息id.Text = "";
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("您尚未點選最新消息!!");
                txt內容.Text = "";
                txt更新者.Text = "";
                txt標題.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                lbl最新消息id.Text = "";
            }
        }
    }
}
