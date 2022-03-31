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
    public partial class 訂單資訊 : Form
    {
        public 訂單資訊()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<string> serchIDs = new List<string>(); //進階搜尋的結果
        List<string> 明細id = new List<string>(); //進階搜尋的結果
        private void 訂單資訊_Load(object sender, EventArgs e)
        {
            if (Global員工資訊.員工職等 == "EM" || Global員工資訊.員工職等 == "SN")
            {
                groupBox權限.Visible = true;
            }
            else
            {
                groupBox權限.Visible = false;
            }

            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";

            cbox搜尋欄位.Items.Add("訂單編號");
            cbox搜尋欄位.Items.Add("姓名");
            cbox搜尋欄位.Items.Add("訂購日期");
            cbox搜尋欄位.SelectedIndex = 0;
        }

        private void btn搜尋_Click(object sender, EventArgs e)
        {
            listBox訂單內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from master where( " + strFieldName + " like  @SerchString );";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox訂單內容.Items.Add("訂單編號：" + reader["訂單編號"] + "  " + "姓名：" + reader["姓名"] + "  " + "訂購日期：" + reader["訂購日期"]);
                    serchIDs.Add(reader["訂單編號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無相關訂單資訊!");                  
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入搜關鍵字!");
            }
        }

        private void btn進階搜尋_Click(object sender, EventArgs e)
        {
            listBox訂單內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from master where( " + strFieldName + " like  @SerchString )and (訂購日期 between @Startdate and @Enddate);";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");
                cmd.Parameters.AddWithValue("@Startdate", startDate.Value);
                cmd.Parameters.AddWithValue("@Enddate", EndDate.Value);
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox訂單內容.Items.Add("訂單編號：" + reader["訂單編號"] + "  " + "姓名：" + reader["姓名"] + "  " + "訂購日期：" + reader["訂購日期"]);
                    serchIDs.Add(reader["訂單編號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無相關訂單資訊!");
                }
                reader.Close();
                con.Close();
            }
            else if (txt進階搜尋內容.Text == "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from master where 訂購日期 between @Startdate and @Enddate;";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@Startdate", startDate.Value);
                cmd.Parameters.AddWithValue("@Enddate", EndDate.Value);
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox訂單內容.Items.Add("訂單編號：" + reader["訂單編號"] + "  " + "姓名：" + reader["姓名"] + "  " + "訂購日期：" + reader["訂購日期"]);
                    serchIDs.Add(reader["訂單編號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無相關訂單資訊!");
                }
                reader.Close();
                con.Close();
            }
            else { 
            
            }
        }

        private void btn產品列表_Click(object sender, EventArgs e)
        {
            listBox訂單內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            SqlConnection con = new SqlConnection(mySunnyConnectionString);
            string str = "SELECT  * FROM master  ;";
            //查詢是變動的，SQL指令會不同
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;

            while (reader.Read())   //把東西讀出來
            {
                listBox訂單內容.Items.Add("訂單編號：" + reader["訂單編號"] + "  " + "姓名：" + reader["姓名"] + "  " + "訂購日期：" + reader["訂購日期"]);
                serchIDs.Add(reader["訂單編號"].ToString());
                i++;
            }
            if (i <= 0)
            {
                MessageBox.Show("查無相關資訊!");              
            }
            reader.Close();
            con.Close();
        }

        private void btn清空欄位_Click(object sender, EventArgs e)
        {
            txt單價.Text = "";
            txt訂單明細id.Text = "";
            txt訂單編號.Text = "";
            txt訂購人.Text = "";
            txt訂購人連絡電話.Text = "";           
            txt訂購名稱.Text = "";
            txt訂購數量.Text = "";
            txt訂購的人資訊.Text = "";
            txt進階搜尋內容.Text = "";
            listBox明細.Items.Clear();
            listBox訂單內容.Items.Clear();
        }

        private void btn修改_Click(object sender, EventArgs e)
        {
            txt訂單編號.ReadOnly = true;
            txt訂單明細id.ReadOnly = true;
            if (txt訂單明細id.Text != "" && txt訂單編號.Text != "" && txt訂購人.Text != "")
            {               
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "Update detail set 產品名稱 = @NewName,數量 = @NewConut,價格 = @NewPrice,日期 = @NewDate,姓名 = @NewUser,訂單編號 = @NewNum where 訂單id = @Serchid;";
                //不要用字串合成!會被入侵，避免SQL inJection!用字串插入
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@Serchid", txt訂單明細id.Text);               
                cmd.Parameters.AddWithValue("@NewName", txt訂購名稱.Text);
                int a = 0;
                a=Convert.ToInt32(txt訂購數量.Text);
                cmd.Parameters.AddWithValue("@NewConut",a);
                int b = 0;
                b = Convert.ToInt32(txt單價.Text);
                cmd.Parameters.AddWithValue("@NewPrice", b);
                cmd.Parameters.AddWithValue("@NewDate", dpt訂購日期.Value.ToString());
                cmd.Parameters.AddWithValue("@NewUser", txt訂購人.Text);
                cmd.Parameters.AddWithValue("@NewNum", txt訂單編號.Text);

                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();

                MessageBox.Show($"{rows}筆資料更新成功!");

                txt單價.Text = "";
                txt訂單明細id.Text = "";
                txt訂單編號.Text = "";
                txt訂購人.Text = "";
                txt訂購人連絡電話.Text = "";
                txt訂購名稱.Text = "";
                txt訂購數量.Text = "";
                txt訂購的人資訊.Text = "";
                txt進階搜尋內容.Text = "";

            }
            else
            {
                MessageBox.Show("欲修改明細資料需有明細id及訂單編號!");
            }

        }

        private void btn新增_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("確定要新增一筆訂單明細嗎?", "新增明信提醒", MessageBoxButtons.OKCancel);

            if (Result == DialogResult.OK)
            {
                Random myrand = new Random();  
                string 亂數 = DateTime.Now.ToString("yyyyMMdd") + myrand.Next(1000, 9999).ToString();
                txt訂單編號.Text = 亂數;
                txt訂單明細id.Text = "系統將自動產生，誤填!";
                MessageBox.Show("請新增明細資訊");
            }
            else if (Result == DialogResult.Cancel)
            {
                
            }
        }

        private void btn刪除_Click(object sender, EventArgs e)
        {
            string intid = "";
            intid = txt訂單明細id.Text;

            if (intid != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "delete from detail where 訂單id =@SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intid);
                int rows = cmd.ExecuteNonQuery();
                con.Close();


                txt單價.Text = "";
                txt訂單明細id.Text = "";
                txt訂單編號.Text = "";
                txt訂購人.Text = "";
                txt訂購人連絡電話.Text = "";
                txt訂購名稱.Text = "";
                txt訂購數量.Text = "";
                txt訂購的人資訊.Text = "";
                txt進階搜尋內容.Text = "";
                listBox訂單內容.Items.Clear();
                listBox明細.Items.Clear();

                MessageBox.Show("資料刪除成功!");
            }
        }
        private void listBox訂單內容_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox訂單內容.SelectedIndex > -1)
            {
                string intId = serchIDs[listBox訂單內容.SelectedIndex];
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "SELECT *FROM master as m INNER JOIN detail as d ON d.訂單編號=m.訂單編號 where d.訂單編號=@SerchID;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchID", intId);
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                listBox明細.Items.Clear();
                明細id.Clear();
                while (reader.Read())   //把東西讀出來
                {

                    listBox明細.Items.Add("明細id：" + reader["訂單id"] + "  " + reader["產品名稱"]);
                    txt訂單編號.Text = $"{reader["訂單編號"] }";
                    明細id.Add(reader["訂單id"].ToString());
                    serchIDs.Add(reader["訂單編號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無相關資訊!");
                }
                reader.Close();
                try
                {
                    string str02 = "SELECT c.手機,c.姓名 from client as c INNER JOIN master as m  ON m.會員id=c.會員id where m.訂單編號=@SerchID;";
                    SqlCommand cmd02 = new SqlCommand(str02, con);
                    cmd02.Parameters.AddWithValue("@SerchID", intId);
                    SqlDataReader reader02 = cmd02.ExecuteReader();
                    if (reader02.Read())
                    {
                        txt訂購的人資訊.Text = $"{reader02["姓名"]}";
                        txt訂購人連絡電話.Text = $"{reader02["手機"]}";
                    }
                    else
                    {
                        txt訂購的人資訊.Text = $"{reader02["姓名"]}";
                    }
                    reader02.Close();
                } catch (Exception)
                { 
                
                }
                
            }
            else
            {
                MessageBox.Show("您尚未點選欲察看詳情之訂單項目!");
              
            }
        }

        private void listBox明細_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox明細.SelectedIndex > -1)
            {
                
                string intId = 明細id[listBox明細.SelectedIndex];
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "SELECT  * FROM detail where 訂單id=@SerchID;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchID", intId);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    txt訂單編號.Text = $"{reader["訂單編號"] }";
                    txt訂單明細id.Text = $"{reader["訂單id"] }";
                    txt單價.Text = $"{reader["價格"]}";
                    txt訂購人.Text = $"{reader["姓名"]}";
                    txt訂購名稱.Text = $"{reader["產品名稱"]}";
                    txt訂購數量.Text = $"{reader["數量"]}";                   
                    dpt訂購日期.Value = Convert.ToDateTime($"{reader["日期"]}");
                }
                else
                {
                    MessageBox.Show("查無相關資訊!");
                    txt單價.Text = "";
                    txt訂單明細id.Text = "";
                    txt訂單編號.Text = "";
                    txt訂購人.Text = "";
                    txt訂購人連絡電話.Text = "";
                    txt訂購名稱.Text = "";
                    txt訂購數量.Text = "";
                    txt訂購的人資訊.Text = "";
                    txt進階搜尋內容.Text="";
                    dpt訂購日期.Value= Convert.ToDateTime("1990-01-01");
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("您尚未點選欲察看詳情之訂單!");
                txt單價.Text = "";
                txt訂單明細id.Text = "";
                txt訂單編號.Text = "";
                txt訂購人.Text = "";
                txt訂購人連絡電話.Text = "";
                txt訂購名稱.Text = "";
                txt訂購數量.Text = "";
                txt訂購的人資訊.Text = "";
                txt進階搜尋內容.Text = "";
                dpt訂購日期.Value = Convert.ToDateTime("1990-01-01");
            }
        }

        private void btn刪除訂單_Click(object sender, EventArgs e)
        {
            string intid = "";
            intid = txt訂單編號.Text;

            if (intid != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "delete from master where 訂單編號 =@SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intid);
                int rows = cmd.ExecuteNonQuery();

                string str02 = "delete from detail where 訂單編號 =@SerchId;";
                SqlCommand cmd02 = new SqlCommand(str02, con);
                cmd02.Parameters.AddWithValue("@SerchId", intid);
                int rows02 = cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("資料刪除成功!");
                txt單價.Text = "";
                txt訂單明細id.Text = "";
                txt訂單編號.Text = "";
                txt訂購人.Text = "";
                txt訂購人連絡電話.Text = "";
                txt訂購名稱.Text = "";
                txt訂購數量.Text = "";
                txt訂購的人資訊.Text = "";
                txt進階搜尋內容.Text = "";
                listBox明細.Items.Clear();
                listBox訂單內容.Items.Clear();

                
            }
        }

        private void btn提交新增資料_Click(object sender, EventArgs e)
        {
            if (txt訂單編號.Text != "" && txt訂購人.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "insert into detail values (@NewName,@NewConut,@NewPrice,@NewDate,@NewUser,@NewNum);";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@NewName", txt訂購名稱.Text);
                int a = 0;
                a = Convert.ToInt32(txt訂購數量.Text);
                cmd.Parameters.AddWithValue("@NewConut", a);
                int b = 0;
                b = Convert.ToInt32(txt單價.Text);
                cmd.Parameters.AddWithValue("@NewPrice", b);
                cmd.Parameters.AddWithValue("@NewDate", dpt訂購日期.Value.ToString());
                cmd.Parameters.AddWithValue("@NewUser", txt訂購人.Text);
                cmd.Parameters.AddWithValue("@NewNum", txt訂單編號.Text);
                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數

                string str02 = "insert into master values (@NewNum01,@NewUser01,@NewDate01,@NewCode01);";
                SqlCommand cmd02 = new SqlCommand(str02, con);
                
                cmd02.Parameters.AddWithValue("@NewNum01", txt訂單編號.Text);
                cmd02.Parameters.AddWithValue("@NewUser01", txt訂購人.Text);
                cmd02.Parameters.AddWithValue("@NewDate01",dpt訂購日期.Value);
                string 通用 = "";
                cmd02.Parameters.AddWithValue("@NewCode01", 通用);


                int rows02 = cmd02.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數

                con.Close();
                DialogResult Result = MessageBox.Show("已新增"+rows+"筆訂單明細!\n請問還需要在同一筆訂單新增明係嗎?", "提醒", MessageBoxButtons.OKCancel);

                if (Result == DialogResult.OK)
                {
                    txt單價.Text = "";                    
                    txt訂購人連絡電話.Text = "";
                    txt訂購名稱.Text = "";
                    txt訂購數量.Text = "";
                    MessageBox.Show("請新增明細資訊");
                }
                else if (Result == DialogResult.Cancel)
                {
                    txt單價.Text = "";
                    txt訂單明細id.Text = "";
                    txt訂單編號.Text = "";
                    txt訂購人.Text = "";
                    txt訂購人連絡電話.Text = "";
                    txt訂購名稱.Text = "";
                    txt訂購數量.Text = "";
                }
            }
            else
            {
                MessageBox.Show("欲新增明細資料需有明細id及訂單編號!");
            }
        }

        private void btn繼續新增_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(mySunnyConnectionString);
            con.Open();
            string str = "insert into detail values (@NewName,@NewConut,@NewPrice,@NewDate,@NewUser,@NewNum);";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@NewName", txt訂購名稱.Text);
            int a = 0;
            a = Convert.ToInt32(txt訂購數量.Text);
            cmd.Parameters.AddWithValue("@NewConut", a);
            int b = 0;
            b = Convert.ToInt32(txt單價.Text);
            cmd.Parameters.AddWithValue("@NewPrice", b);
            cmd.Parameters.AddWithValue("@NewDate", dpt訂購日期.Value.ToString());
            cmd.Parameters.AddWithValue("@NewUser", txt訂購人.Text);
            cmd.Parameters.AddWithValue("@NewNum", txt訂單編號.Text);
            int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
            con.Close();
            DialogResult Result = MessageBox.Show("已新增" + rows + "筆訂單明細!\n請問還需要在同一筆訂單新增明細嗎?", "提醒", MessageBoxButtons.OKCancel);

            if (Result == DialogResult.OK)
            {
                txt單價.Text = "";               
                txt訂購人連絡電話.Text = "";
                txt訂購名稱.Text = "";
                txt訂購數量.Text = "";
                MessageBox.Show("請新增明細資訊");
            }
            else if (Result == DialogResult.Cancel)
            {
                txt單價.Text = "";
                txt訂單明細id.Text = "";
                txt訂單編號.Text = "";
                txt訂購人.Text = "";
                txt訂購人連絡電話.Text = "";
                txt訂購名稱.Text = "";
                txt訂購數量.Text = "";
            }
        }
    }
}
