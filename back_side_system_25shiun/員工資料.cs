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
    public partial class 員工資料 : Form
    {
        public 員工資料()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<string> serchIDs = new List<string>(); //進階搜尋的結果
        Boolean 是否有文字 = false;
        Boolean 進階是否有文字 = false;
        private void 員工資料_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";

            cbox搜尋欄位.Items.Add("員工姓名");
            cbox搜尋欄位.Items.Add("員工工號");
            cbox搜尋欄位.Items.Add("職等");
            cbox搜尋欄位.SelectedIndex = 0;
         
        }

        private void btn搜尋_Click(object sender, EventArgs e)
        {
            ckbox女生.Checked = false;
            ckbox男生.Checked = false;
            listBox員工資訊.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from employee where( " + strFieldName + " like  @SerchString );";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");             
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox員工資訊.Items.Add("員工工號：" + reader["員工工號"] + "  " + "姓名："+reader["員工姓名"]+" \n " + "電話：" + reader["員工電話"]);
                    serchIDs.Add(reader["員工工號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無此人!");
                    txt住址.Text = "";
                    txt信箱.Text = "";
                    txt姓名.Text = "";
                    txt工號.Text = "";
                    txt職等.Text = "";
                    txt電話.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");
                    ckbox男生.Checked = false;
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入搜關鍵字!");
            }
        }

        private void btn檢視資料_Click(object sender, EventArgs e)
        {

        }

        private void btn修改_Click(object sender, EventArgs e)
        {           
            if ( txt工號.Text != "" && (txt姓名.Text != "") && (txt工號.Text != ""))
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "Update employee set 員工姓名 = @NewName,員工電話 = @NewPhone,員工住址 = @NewAdress,員工信箱 = @NewEmail,員工生日 = @NewBirth,員工性別 = @NewSex,到職日 = @NewDate,職等 = @Newlevel where 員工工號 = @SerchSum;";
                //不要用字串合成!會被入侵，避免SQL inJection!用字串插入
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchSum", txt工號.Text);
                cmd.Parameters.AddWithValue("@NewName", txt姓名.Text);
                cmd.Parameters.AddWithValue("@NewPhone",txt電話.Text);
                cmd.Parameters.AddWithValue("@NewAdress", txt住址.Text);
                cmd.Parameters.AddWithValue("@NewEmail", txt信箱.Text);
                cmd.Parameters.AddWithValue("@NewBirth", dtp生日.Value);
                cmd.Parameters.AddWithValue("@NewDate", dtp到職日.Value);
                cmd.Parameters.AddWithValue("@NewSex", ckbox男生.Checked);               
                cmd.Parameters.AddWithValue("@Newlevel", txt職等.Text);

                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料更新成功!");
                txt住址.Text = "";
                txt信箱.Text = "";
                txt姓名.Text = "";
                txt工號.Text = "";
                txt職等.Text = "";
                txt電話.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                ckbox男生.Checked = false;
                ckbox女生.Checked = false;
                dtp到職日.Value = Convert.ToDateTime("2020-01-01");
            }
            else
            {
                MessageBox.Show("資料有誤!\n欲修改員工資料需填員工工號及員工姓名!");
            }
        }

        private void btn新增_Click(object sender, EventArgs e)
        {
            if ((txt工號.Text != "") && (txt姓名.Text != ""))
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "insert into employee values (@NewNum,@Newname,@NewDate,@NewLevel,@NewSex,@NewBirth,@NewPhone,@NewAdress,@NewEmail,@NewCode);";
                SqlCommand smd = new SqlCommand(str, con);
                smd.Parameters.AddWithValue("@NewNum", txt工號.Text);
                smd.Parameters.AddWithValue("@Newname",txt姓名.Text);
                smd.Parameters.AddWithValue("@NewDate", dtp到職日.Value.ToString());
                smd.Parameters.AddWithValue("@NewLevel",txt職等.Text);
                smd.Parameters.AddWithValue("@NewAdress", txt住址.Text);
                smd.Parameters.AddWithValue("@NewEmail", txt信箱.Text);
                smd.Parameters.AddWithValue("@NewBirth", dtp生日.Value);
                smd.Parameters.AddWithValue("@NewSex", ckbox男生.Checked);
                smd.Parameters.AddWithValue("@NewPhone", txt電話.Text);
                smd.Parameters.AddWithValue("@NewCode",txt工號.Text);

                int rows = smd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料新增成功!");
                txt住址.Text = "";
                txt信箱.Text = "";
                txt姓名.Text = "";
                txt工號.Text = "";
                txt職等.Text = "";
                txt電話.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                ckbox男生.Checked = false;
                ckbox女生.Checked = false;
                dtp到職日.Value = Convert.ToDateTime("2020-01-01");
            }
            else
            {
                MessageBox.Show("慾新增員工資料，\n需輸入員工工號及姓名!");
            }
        }

        private void btn刪除_Click(object sender, EventArgs e)
        {
            string intid ="";
            intid = txt工號.Text;

            if (intid != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "delete from employee where 員工工號 =@SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intid);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                txt住址.Text = "";
                txt信箱.Text = "";
                txt姓名.Text = "";
                txt工號.Text = "";
                txt職等.Text = "";
                txt電話.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                ckbox男生.Checked = false;
                ckbox女生.Checked = false;
                dtp到職日.Value = Convert.ToDateTime("2020-01-01");

                MessageBox.Show("資料刪除成功!");


            }
        }

        private void btn產生員工列表_Click(object sender, EventArgs e)
        {
            listBox員工資訊.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from employee ;";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);               
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                listBox員工資訊.Items.Add("員工工號：" + reader["員工工號"] + "  " + "姓名：" + reader["員工姓名"]+ "  " + "職等：" + reader["職等"]);             
                serchIDs.Add(reader["員工工號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無此人!");
                    txt住址.Text = "";
                    txt信箱.Text = "";
                    txt姓名.Text = "";
                    txt工號.Text = "";
                    txt職等.Text = "";
                    txt電話.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");
                    ckbox男生.Checked = false;
                }
                reader.Close();
                con.Close();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txt輸入員工工號查詢.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select*from employee where 員工工號=@SerchNum;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchNum", txt輸入員工工號查詢.Text);
                //參數是會檢查格式Parameters.AddWithValue-->欄位檢查
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txt住址.Text = $"{reader["員工住址"] }";
                    txt信箱.Text = $"{reader["員工信箱"]}";
                    txt姓名.Text = $"{reader["員工姓名"]}";
                    txt工號.Text = $"{reader["員工工號"]}";
                    txt職等.Text = $"{reader["職等"]}";
                    txt電話.Text = $"{reader["員工電話"]}";
                    dtp到職日.Value = Convert.ToDateTime($"{reader["到職日"]}");
                    dtp生日.Value = Convert.ToDateTime($"{reader["員工生日"]}");
                    Boolean sex = Convert.ToBoolean(reader["員工性別"]);
                    ckbox男生.Checked = Convert.ToBoolean(reader["員工性別"]);
                    if (Convert.ToBoolean(reader["員工性別"]))
                    {
                        ckbox男生.Checked = Convert.ToBoolean(reader["員工性別"]);
                    }
                    else
                    {
                        ckbox女生.Checked = !sex;
                    }
                    txt輸入員工工號查詢.Text = "";

                }
                else
                {
                    MessageBox.Show("查無此員工工號!");
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入要查詢的員工工號!");
            }
        }

        private void txt輸入員工工號查詢_Leave(object sender, EventArgs e)
        {
            if (txt輸入員工工號查詢.Text == "")
            {
                txt輸入員工工號查詢.Text = "輸入員工工號查詢員工資料";
                txt輸入員工工號查詢.ForeColor = Color.Black;
                是否有文字 = false;
            }
            else {
                是否有文字 = true;
            }
        }

        private void txt輸入員工工號查詢_Enter(object sender, EventArgs e)
        {
            if (是否有文字 == false)
            {
                txt輸入員工工號查詢.Text = "";
                txt輸入員工工號查詢.ForeColor = Color.Black;
            }
        }

        private void txt進階搜尋內容_Leave(object sender, EventArgs e)
        {

            if (txt進階搜尋內容.Text == "")
            {
                txt進階搜尋內容.Text = "輸入進階搜尋關鍵字";
                txt進階搜尋內容.ForeColor = Color.Black;
                進階是否有文字 = false;
            }
            else
            {
                進階是否有文字 = true;
            }
        }

        private void txt進階搜尋內容_Enter(object sender, EventArgs e)
        {
            if (進階是否有文字 == false)
            {
                txt進階搜尋內容.Text = "";
                txt進階搜尋內容.ForeColor = Color.Black;
            }
        }

        private void btn清空欄位_Click(object sender, EventArgs e)
        {
            進階是否有文字 = false;
            txt住址.Text = "";
            txt信箱.Text = "";
            txt姓名.Text = "";
            txt工號.Text = "";
            txt職等.Text = "";
            txt電話.Text = "";
            dtp生日.Value = Convert.ToDateTime("1990-01-01");
            ckbox男生.Checked = false;
            ckbox女生.Checked = false;
            dtp到職日.Value = Convert.ToDateTime("2020-01-01");
            listBox員工資訊.Items.Clear();
        }

        private void listBox員工進階搜尋結果_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox員工資訊.SelectedIndex > -1)
            {
                ckbox男生.Checked = false;
                ckbox女生.Checked = false;
                string intId = serchIDs[listBox員工資訊.SelectedIndex];
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select*from employee where 員工工號 = @SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txt住址.Text = $"{reader["員工住址"] }";
                    txt信箱.Text = $"{reader["員工信箱"]}";
                    txt姓名.Text = $"{reader["員工姓名"]}";
                    txt工號.Text = $"{reader["員工工號"]}";
                    txt職等.Text = $"{reader["職等"]}";
                    txt電話.Text = $"{reader["員工電話"]}";
                    dtp到職日.Value = Convert.ToDateTime($"{reader["到職日"]}");
                    dtp生日.Value = Convert.ToDateTime($"{reader["員工生日"]}");
                    Boolean sex = Convert.ToBoolean(reader["員工性別"]);
                    ckbox男生.Checked = Convert.ToBoolean(reader["員工性別"]);
                    if (Convert.ToBoolean(reader["員工性別"]))
                    {
                        ckbox男生.Checked = Convert.ToBoolean(reader["員工性別"]);
                    }
                    else
                    {
                        ckbox女生.Checked = !sex;
                    }
                }
                else
                {
                    MessageBox.Show("查無此人資訊!");
                    txt住址.Text = "";
                    txt信箱.Text = "";
                    txt姓名.Text = "";
                    txt工號.Text = "";
                    txt職等.Text = "";
                    txt電話.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");
                    ckbox男生.Checked = false;
                    ckbox女生.Checked = false;
                    dtp到職日.Value = Convert.ToDateTime("2020-01-01");
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("查無此人!");
                txt住址.Text = "";
                txt信箱.Text = "";
                txt姓名.Text = "";
                txt工號.Text = "";
                txt職等.Text = "";
                txt電話.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                ckbox男生.Checked = false;
                ckbox女生.Checked = false;
                dtp到職日.Value = Convert.ToDateTime("2020-01-01");
            }
        }

        private void btn進階搜尋_Click(object sender, EventArgs e)
        {
            ckbox女生.Checked = false;
            ckbox男生.Checked = false;
            listBox員工資訊.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from employee where( " + strFieldName + " like  @SerchString )and (到職日 between @Startdate and @Enddate);";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");
                cmd.Parameters.AddWithValue("@Startdate", startDate.Value.ToString());
                cmd.Parameters.AddWithValue("@Enddate", EndDate.Value.ToString());
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox員工資訊.Items.Add("員工工號：" + reader["員工工號"] + "  " + "姓名：" + reader["員工姓名"] + " \n " + "電話：" + reader["員工電話"]);
                    serchIDs.Add(reader["員工工號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無此人!");
                    txt住址.Text = "";
                    txt信箱.Text = "";
                    txt姓名.Text = "";
                    txt工號.Text = "";
                    txt職等.Text = "";
                    txt電話.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");
                    ckbox男生.Checked = false;
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入搜關鍵字!");
            }
        
    }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.None;
        }
    }
}
