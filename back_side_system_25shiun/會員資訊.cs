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
    public partial class 會員資訊 : Form
    {
        public 會員資訊()
        {
            InitializeComponent();
        }
        SqlConnectionStringBuilder scsb; //資料庫物件連線字串產生器，不用精靈，自己來。簡寫一個名字
        string mySunnyConnectionString = ""; //資料庫產生的字串存在這裡
        List<string> serchIDs = new List<string>(); //進階搜尋的結果
        Boolean 是否有文字 = false;
        Boolean 進階是否有文字 = false;
        Boolean 是否修改過圖檔 = false;
        string image_dir = @"image\";  //將圖檔路徑寫成欄位
        string image_name = "";
        private void 會員資訊_Load(object sender, EventArgs e)
        {
           
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
                scsb.DataSource = @".";
                scsb.InitialCatalog = "Sunny";
                scsb.IntegratedSecurity = true;
                mySunnyConnectionString = scsb.ToString();
                lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";

                cbox搜尋欄位.Items.Add("會員id");
                cbox搜尋欄位.Items.Add("姓名");
                cbox搜尋欄位.Items.Add("性別");
                cbox搜尋欄位.SelectedIndex = 0;
            
        }


        private void btn搜尋_Click(object sender, EventArgs e)
        {
            listBox搜尋內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from client where( " + strFieldName + " like  @SerchString );";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox搜尋內容.Items.Add("會員編號：" + reader["會員id"] + "  " + "姓名：" + reader["姓名"] + "  " + "電話：" + reader["手機"] );
                    serchIDs.Add(reader["會員id"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無相關資訊!");
                    checkBox男生.Checked = false;
                    checkBox女生.Checked = false;
                    lbl會員id.Text = "";
                    txt信箱.Text = "";
                    txt地址.Text = "";
                    txt姓名.Text = "";                  
                    txt手機.Text = "";
                    txt進階搜尋內容.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");                  
                    pbox會員照片.Image = null;
                    txt進階搜尋內容.Text = "";
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
            listBox搜尋內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from client where( " + strFieldName + " like  @SerchString )and (生日 between @Startdate and @Enddate);";
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
                    listBox搜尋內容.Items.Add("會員編號：" + reader["會員id"] + "  " + "姓名：" + reader["姓名"] + "  " + "電話：" + reader["手機"]);
                    serchIDs.Add(reader["會員id"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("查無相關資訊!");
                    lbl會員id.Text = "";
                    checkBox男生.Checked = false;
                    checkBox女生.Checked = false;
                    txt信箱.Text = "";
                    txt地址.Text = "";
                    txt姓名.Text = "";                    
                    txt手機.Text = "";
                    txt進階搜尋內容.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");
                    pbox會員照片.Image = null;
                    txt進階搜尋內容.Text = "";
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入搜關鍵字!");
            }
        }

        private void btn會員列表_Click(object sender, EventArgs e)
        {
            listBox搜尋內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            SqlConnection con = new SqlConnection(mySunnyConnectionString);
            string str = "select*from client ;";
            //查詢是變動的，SQL指令會不同
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;

            while (reader.Read())   //把東西讀出來
            {
                listBox搜尋內容.Items.Add("會員編號：" + reader["會員id"] + "  " + "姓名：" + reader["姓名"] + "  " + "電話：" + reader["手機"]);
                serchIDs.Add(reader["會員id"].ToString());
                i++;
            }
            if (i <= 0)
            {
                MessageBox.Show("查無相關資訊!");
                checkBox男生.Checked = false;
                checkBox女生.Checked = false;
                lbl會員id.Text = "";
                txt信箱.Text = "";
                txt地址.Text = "";
                txt姓名.Text = "";
                txt手機.Text = "";
                txt進階搜尋內容.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                pbox會員照片.Image = null;
                txt進階搜尋內容.Text = "";
            }
            reader.Close();
            con.Close();
        }

        private void btn更改密碼_Click(object sender, EventArgs e)
        {

        }

        private void btn清空欄位_Click(object sender, EventArgs e)
        {
            lbl會員id.Text = "";
            txt信箱.Text = "";
            txt地址.Text = "";
            txt姓名.Text = "";            
            txt手機.Text = "";
            checkBox男生.Checked = false;
            checkBox女生.Checked = false;
            txt進階搜尋內容.Text = "";
            dtp生日.Value = Convert.ToDateTime("1990-01-01");
            startDate.Value = Convert.ToDateTime("1990-01-01");
            EndDate.Value = Convert.ToDateTime("2022-01-01");
            pbox會員照片.Image = null;
            txt進階搜尋內容.Text = "";
            listBox搜尋內容.Items.Clear();
        }

        private void btn選產品圖片_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();  //選取新的圖片跳出對話框
            f.Filter = "圖檔類型(*.jpg,*.JPG,*.png)|*.jpeg;*.jpg;*.png";
            //限制可以選取圖片的附檔名

            DialogResult R = f.ShowDialog();

            if (R == DialogResult.OK)
            {
               pbox會員照片.Image = Image.FromFile(f.FileName);
                string fileExt = Path.GetExtension(f.SafeFileName); //用檔案名稱來取得附檔名
                Random myrand = new Random();  //隨機物件
                image_name = DateTime.Now.ToString("yyyyMMddHHmmss") + myrand.Next(1000, 9999).ToString() + fileExt;
                //設定隨機檔名的格式
                是否修改過圖檔 = true;
                Console.WriteLine(image_name);
            }
        }

        private void btn修改_Click(object sender, EventArgs e)
        {
            if (txt姓名.Text != "" && (lbl會員id.Text != ""))
            {
                if (是否修改過圖檔 == true)
                {
                    //將照片存檔
                    pbox會員照片.Image.Save(image_dir + image_name);
                    是否修改過圖檔 = false;
                }
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "Update client set 性別 = @NewSex,生日 = @NewBirth,電子信箱 = @NewEmail,手機 = @NewPhone,地址 = @NewAdress,會員大頭照路徑 = @NewPath,姓名 = @NewName where 會員id = @Serchid;";
                //不要用字串合成!會被入侵，避免SQL inJection!用字串插入
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@Serchid", lbl會員id.Text);
                int a = 0;
                if (checkBox男生.Checked)
                {
                    a = 1;
                }
                else {
                    a = 0;
                }
                cmd.Parameters.AddWithValue("@NewSex", a);
                cmd.Parameters.AddWithValue("@NewBirth", dtp生日.Value);
                cmd.Parameters.AddWithValue("@NewEmail", txt信箱.Text);               
                cmd.Parameters.AddWithValue("@NewPhone", txt手機.Text);
                cmd.Parameters.AddWithValue("@NewAdress", txt地址.Text);
                cmd.Parameters.AddWithValue("@NewPath", image_name);
                cmd.Parameters.AddWithValue("@NewName", txt姓名.Text);                                

                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();

                MessageBox.Show($"{rows}筆資料更新成功!");
                lbl會員id.Text = "";
                txt信箱.Text = "";
                txt地址.Text = "";
                txt姓名.Text = "";                
                txt手機.Text = "";
                checkBox男生.Checked = false;
                checkBox女生.Checked = false;
                txt進階搜尋內容.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");               
                pbox會員照片.Image = null;
            }
            else
            {
                MessageBox.Show("欲修改會員資料需有會員編號及會員姓名!");
            }
        }

        private void btn新增_Click(object sender, EventArgs e)
        {
            if (txt姓名.Text != "" && (txt手機.Text != ""))
            {
                if (是否修改過圖檔 == true)
                {
                    //將照片存檔
                    pbox會員照片.Image.Save(image_dir + image_name);
                    是否修改過圖檔 = false;
                }
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "insert into client values (@NewSex,@NewBirth,@NewEmail,@NewCode,@NewTel,@NewAdress,@NewPath,@NewName);";
                SqlCommand cmd = new SqlCommand(str, con);
                int a = 0;
                if (checkBox男生.Checked)
                {
                    a = 1;
                }
                else
                {
                    a = 0;
                }
                cmd.Parameters.AddWithValue("@NewSex", a);
                cmd.Parameters.AddWithValue("@NewBirth", dtp生日.Value);
                cmd.Parameters.AddWithValue("@NewEmail", txt信箱.Text);               
                cmd.Parameters.AddWithValue("@NewCode", txt手機.Text);               
                cmd.Parameters.AddWithValue("@NewTel", txt手機.Text);
                cmd.Parameters.AddWithValue("@NewAdress", txt地址.Text);
                cmd.Parameters.AddWithValue("@NewPath", image_name);
                cmd.Parameters.AddWithValue("@NewName", txt姓名.Text);
                
                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料新增成功!");

                lbl會員id.Text = "";
                txt信箱.Text = "";
                txt地址.Text = "";
                txt姓名.Text = "";
                txt手機.Text = "";
                checkBox男生.Checked = false;
                checkBox女生.Checked = false;
                txt進階搜尋內容.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                pbox會員照片.Image = null;
            }
            else
            {
                MessageBox.Show("慾新增會員資料，\n必填寫姓名及手機!!");
            }
        }

        private void btn刪除_Click(object sender, EventArgs e)
        {
            string intid = "";
            intid = lbl會員id.Text;

            if (intid != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "delete from client where 會員id =@SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intid);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                lbl會員id.Text = "";
                txt信箱.Text = "";
                txt地址.Text = "";
                txt姓名.Text = "";                
                txt手機.Text = "";
                checkBox男生.Checked = false;
                checkBox女生.Checked = false;
                txt進階搜尋內容.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                pbox會員照片.Image = null;

                MessageBox.Show("資料刪除成功!");

            }
        }

        private void listBox搜尋內容_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox搜尋內容.SelectedIndex > -1)
            {
                string intId = serchIDs[listBox搜尋內容.SelectedIndex];
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select*from client where 會員id = @SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    txt信箱.Text = $"{reader["電子信箱"] }";
                    txt地址.Text = $"{reader["地址"] }";
                    txt姓名.Text = $"{reader["姓名"]}";
                    txt手機.Text = $"{reader["手機"]}";
                    lbl會員id.Text = $"{reader["會員id"]}";
                    image_name = reader["會員大頭照路徑"].ToString();
                    try
                    {
                        pbox會員照片.Image = Image.FromFile(image_dir + image_name);
                    }
                    catch (Exception)
                    {
                        pbox會員照片.Image = null;
                    }
                    Boolean sex = Convert.ToBoolean(reader["性別"]);
                    checkBox男生.Checked = Convert.ToBoolean(reader["性別"]);
                    if (Convert.ToBoolean(reader["性別"]))
                    {
                        checkBox男生.Checked = Convert.ToBoolean(reader["性別"]);
                    }
                    else
                    {
                        checkBox女生.Checked = !sex;
                    }
                    dtp生日.Value= Convert.ToDateTime($"{reader["生日"]}");
                }
                else
                {
                    MessageBox.Show("查無相關資訊!");
                    lbl會員id.Text = "";
                    txt信箱.Text = "";
                    txt地址.Text = "";
                    txt姓名.Text = "";
                    txt手機.Text = "";
                    checkBox男生.Checked = false;
                    checkBox女生.Checked = false;
                    txt進階搜尋內容.Text = "";
                    dtp生日.Value = Convert.ToDateTime("1990-01-01");
                    pbox會員照片.Image = null;
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("您尚未點選欲察看詳情之會員!");
                lbl會員id.Text = "";
                txt信箱.Text = "";
                txt地址.Text = "";
                txt姓名.Text = "";
                txt手機.Text = "";
                checkBox男生.Checked = false;
                checkBox女生.Checked = false;
                txt進階搜尋內容.Text = "";
                dtp生日.Value = Convert.ToDateTime("1990-01-01");
                pbox會員照片.Image = null;
                txt進階搜尋內容.Text = "";
            }
        }
    }
}
