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
    public partial class 產品資訊 : Form
    {
        public 產品資訊()
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
        private void 產品資訊_Load(object sender, EventArgs e)
        {
            if (Global員工資訊.員工職等 == "EM" || Global員工資訊.員工職等 == "SN")
            {
                groupBox1.Visible = true;
            }
            else {
                groupBox1.Visible = false;
            }
            
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "Sunny";
            scsb.IntegratedSecurity = true;
            mySunnyConnectionString = scsb.ToString();
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";

            cbox搜尋欄位.Items.Add("產品編號");
            cbox搜尋欄位.Items.Add("口味");           
            cbox搜尋欄位.SelectedIndex = 0;
        }

        private void btn搜尋_Click(object sender, EventArgs e)
        {           
            listBox產品內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from product where( " + strFieldName + " like  @SerchString );";
                //查詢是變動的，SQL指令會不同
                con.Open();
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchString", "%" + txt進階搜尋內容.Text + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())   //把東西讀出來
                {
                    listBox產品內容.Items.Add("產品編號：" + reader["產品編號"] + "  " + "名項：" + reader["產品名稱"] + " \n " + "單價：" + reader["價格"]+"元");
                    serchIDs.Add(reader["產品編號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("無此商品!");
                    txt口味.Text = "";
                    txt單價.Text = "";
                    txt數量.Text = "";
                    txt產品名稱.Text = "";
                    txt產品系列.Text = "";
                    txt產品編號.Text = "";
                    dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                    txt註記.Text = "";
                    pbox產品照片.Image = null;
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
            
            listBox產品內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            if (txt進階搜尋內容.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                string str = "select*from product where( " + strFieldName + " like  @SerchString )and (上架日期 between @Startdate and @Enddate);";
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
                    listBox產品內容.Items.Add("產品編號：" + reader["產品編號"] + "  " + "名項：" + reader["產品名稱"] + " \n " + "單價：" + reader["價格"] + "元");
                    serchIDs.Add(reader["產品編號"].ToString());
                    i++;
                }
                if (i <= 0)
                {
                    MessageBox.Show("無此商品!");
                    txt口味.Text = "";
                    txt單價.Text = "";
                    txt數量.Text = "";
                    txt產品名稱.Text = "";
                    txt產品系列.Text = "";
                    txt產品編號.Text = "";
                    dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                    txt註記.Text = "";
                    txt進階搜尋內容.Text = "";
                    pbox產品照片.Image = null;
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
            listBox產品內容.Items.Clear();  //先清空
            serchIDs.Clear(); //集合也內容清掉，可能之前有東西
            string strFieldName = cbox搜尋欄位.SelectedItem.ToString(); //取欄位名稱

            SqlConnection con = new SqlConnection(mySunnyConnectionString);
            string str = "select*from product ;";
            //查詢是變動的，SQL指令會不同
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;

            while (reader.Read())   //把東西讀出來
            {
                listBox產品內容.Items.Add("產品編號：" + reader["產品編號"] + "  " + "名項：" + reader["產品名稱"] + " \n " + "單價：" + reader["價格"] + "元");
                serchIDs.Add(reader["產品編號"].ToString());
                i++;
            }
            if (i <= 0)
            {
                MessageBox.Show("無此商品!");
                txt口味.Text = "";
                txt單價.Text = "";
                txt數量.Text = "";
                txt產品名稱.Text = "";
                txt產品系列.Text = "";
                txt產品編號.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                txt註記.Text = "";
                txt進階搜尋內容.Text = "";
                pbox產品照片.Image = null;
            }
            reader.Close();
            con.Close();
        }

        private void btn清空欄位_Click(object sender, EventArgs e)
        {
            進階是否有文字 = false;
            txt口味.Text = "";
            txt單價.Text = "";
            txt數量.Text = "";
            txt產品名稱.Text = "";
            txt產品系列.Text = "";
            txt產品編號.Text = "";
            dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
            txt註記.Text = "";
            txt進階搜尋內容.Text = "";
            pbox產品照片.Image = null;
            listBox產品內容.Items.Clear();
        }

        private void btn修改_Click(object sender, EventArgs e)
        {
            if (txt產品編號.Text != "" && (txt單價.Text != "") )
            {
                if (是否修改過圖檔 == true)
                {
                    //將照片存檔
                    pbox產品照片.Image.Save(image_dir + image_name);
                    是否修改過圖檔 = false;
                }
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "Update product set 產品名稱 = @NewName,口味 = @NewTaste,數量 = @NewCount,價格 = @NewPrice,註記 = @NewNote,level = @NewLevel,上架日期 = @NewDate,圖片路徑 = @NewImage where 產品編號 = @SerchSum;";
                //不要用字串合成!會被入侵，避免SQL inJection!用字串插入
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchSum", txt產品編號.Text);
                cmd.Parameters.AddWithValue("@NewName", txt產品名稱.Text);
                cmd.Parameters.AddWithValue("@NewTaste", txt口味.Text);
                int intPrice;
                Int32.TryParse(txt單價.Text, out intPrice);
                int intCount;
                Int32.TryParse(txt數量.Text, out intCount);
                cmd.Parameters.AddWithValue("@NewCount", intCount);
                cmd.Parameters.AddWithValue("@NewPrice", intPrice);
                cmd.Parameters.AddWithValue("@NewNote", txt註記.Text);
                cmd.Parameters.AddWithValue("@NewImage", image_name);
                string a = "";
                a = txt產品系列.Text;
                int b = 0;
                if (a == "單片蛋糕系列")
                {
                    b = 1;
                }
                else if (a == "重乳酪蛋糕系列")
                {
                    b = 2;
                }
                else if (a == "奶酪系列")
                {
                    b = 3;
                }
                else if (a == "單品系列")
                {
                    b = 4;
                }
                else {
                    b = 5;
                }
                cmd.Parameters.AddWithValue("@NewLevel", b);
                cmd.Parameters.AddWithValue("@NewDate", dtp上架日期.Value.ToString());              

                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料更新成功!");
                txt口味.Text = "";
                txt單價.Text = "";
                txt數量.Text = "";
                txt產品名稱.Text = "";
                txt產品系列.Text = "";
                txt產品編號.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                txt註記.Text = "";
                pbox產品照片.Image = null;
            }
            else
            {
                MessageBox.Show("欲修改產品資料需填產品編號及單價!");
            }
        }

        private void btn新增_Click(object sender, EventArgs e)
        {
            if ((txt產品編號.Text != "") && (txt單價.Text != ""))
            {

                pbox產品照片.Image.Save(image_dir + image_name);
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "insert into product values (@NewNum,@Newname,@NewTaste,@NewCount,@NewPrice,@NewNote,@NeWiD,@NewLevel,@NewImage,@NewDate);";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@NewNum", txt產品編號.Text);
                cmd.Parameters.AddWithValue("@NewName", txt產品名稱.Text);
                cmd.Parameters.AddWithValue("@NewTaste", txt口味.Text);
                int intPrice;
                Int32.TryParse(txt單價.Text, out intPrice);
                int intCount;
                Int32.TryParse(txt數量.Text, out intCount);
                cmd.Parameters.AddWithValue("@NewCount", intCount);
                int id = 0;
                cmd.Parameters.AddWithValue("@NeWiD", id);
                cmd.Parameters.AddWithValue("@NewPrice", intPrice);
                cmd.Parameters.AddWithValue("@NewNote", txt註記.Text);
                cmd.Parameters.AddWithValue("@NewImage", image_name);
                string a = "";
                a = txt產品系列.Text;
                int b = 0;
                if (a == "單片蛋糕系列")
                {
                    b = 1;
                }
                else if (a == "重乳酪蛋糕系列")
                {
                    b = 2;
                }
                else if (a == "奶酪系列")
                {
                    b = 3;
                }
                else if (a == "單品系列")
                {
                    b = 4;
                }
                else
                {
                    b = 5;
                }
                cmd.Parameters.AddWithValue("@NewLevel", b);
                cmd.Parameters.AddWithValue("@NewDate", dtp上架日期.Value);

                int rows = cmd.ExecuteNonQuery();  //只執行部查詢，顯示影響的資料筆數
                con.Close();
                MessageBox.Show($"{rows}筆資料新增成功!");
                txt口味.Text = "";
                txt單價.Text = "";
                txt數量.Text = "";
                txt產品名稱.Text = "";
                txt產品系列.Text = "";
                txt產品編號.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                txt註記.Text = "";
                pbox產品照片.Image = null;
            }
            else
            {
                MessageBox.Show("慾新增產品資料，\n必填寫禪品編號及單價!!");
            }
        }

        private void btn刪除_Click(object sender, EventArgs e)
        {
            string intid = "";
            intid = txt產品編號.Text;

            if (intid != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "delete from product where 產品編號 =@SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intid);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                txt口味.Text = "";
                txt單價.Text = "";
                txt數量.Text = "";
                txt產品名稱.Text = "";
                txt產品系列.Text = "";
                txt產品編號.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                txt註記.Text = "";
                pbox產品照片.Image = null;

                MessageBox.Show("資料刪除成功!");

            }
        }
            private void listBox產品內容_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox產品內容.SelectedIndex > -1)
            {               
                string intId = serchIDs[listBox產品內容.SelectedIndex];
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select*from product where 產品編號 = @SerchId;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchId", intId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                                      
                    txt產品編號.Text = $"{reader["產品編號"] }";
                    txt口味.Text = $"{reader["口味"] }";
                    txt產品名稱.Text = $"{reader["產品名稱"]}";
                    txt數量.Text = $"{reader["數量"]}";
                    txt單價.Text = $"{reader["價格"]}";
                    txt註記.Text = $"{reader["註記"]}";
                    image_name = reader["圖片路徑"].ToString();
                    try {
                        pbox產品照片.Image = Image.FromFile(image_dir + image_name);
                    } catch (Exception)
                    {
                        pbox產品照片.Image = null;
                    }
                    
                    string a = $"{reader["level"]}";
                    if (a == "1")
                    {
                        txt產品系列.Text = "單片蛋糕系列";
                    }
                    else if (a == "2")
                    {
                        txt產品系列.Text = "重乳酪蛋糕系列";
                    }
                    else if (a == "3")
                    {
                        txt產品系列.Text = "奶酪系列";
                    }
                    else if (a == "4")
                    {
                        txt產品系列.Text = "人氣單品系列";
                    }
                    else {
                        txt產品系列.Text = "此為單品，不屬任何系列";
                    }

                    dtp上架日期.Value = Convert.ToDateTime(reader["上架日期"]);
                }
                else
                {
                    MessageBox.Show("查無此產品資訊!");
                    txt口味.Text = "";
                    txt單價.Text = "";
                    txt數量.Text = "";
                    txt產品名稱.Text = "";
                    txt產品系列.Text = "";
                    txt產品編號.Text = "";
                    dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                    txt註記.Text = "";
                    txt進階搜尋內容.Text = "";
                    pbox產品照片.Image = null;
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("您尚未點選欲察看詳情之產品!");
                txt口味.Text = "";
                txt單價.Text = "";
                txt數量.Text = "";
                txt產品名稱.Text = "";
                pbox產品照片.Image = null;
                txt產品系列.Text = "";
                txt產品編號.Text = "";
                dtp上架日期.Value = Convert.ToDateTime("1990-01-01");
                txt註記.Text = "";
                txt進階搜尋內容.Text = "";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txt輸入產品編號.Text != "")
            {
                SqlConnection con = new SqlConnection(mySunnyConnectionString);
                con.Open();
                string str = "select*from product where 產品編號=@SerchNum;";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.Parameters.AddWithValue("@SerchNum", txt輸入產品編號.Text);
                //參數是會檢查格式Parameters.AddWithValue-->欄位檢查
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txt產品編號.Text = $"{reader["產品編號"] }";
                    txt口味.Text = $"{reader["口味"] }";
                    txt產品名稱.Text = $"{reader["產品名稱"]}";
                    txt數量.Text = $"{reader["數量"]}";
                    txt單價.Text = $"{reader["價格"]}";
                    txt註記.Text = $"{reader["註記"]}";
                    image_name = reader["圖片路徑"].ToString();
                    pbox產品照片.Image = Image.FromFile(image_dir + image_name);
                    string a = $"{reader["level"]}";
                    if (a == "1")
                    {
                        txt產品系列.Text = "單片蛋糕系列";
                    }
                    else if (a == "2")
                    {
                        txt產品系列.Text = "重乳酪蛋糕系列";
                    }
                    else if (a == "3")
                    {
                        txt產品系列.Text = "奶酪系列";
                    }
                    else if (a == "4")
                    {
                        txt產品系列.Text = "人氣單品系列";
                    }
                    else
                    {
                        txt產品系列.Text = "此為單品，不屬任何系列";
                    }

                    dtp上架日期.Value = Convert.ToDateTime($"{reader["上架日期"]}");

                }
                else
                {
                    MessageBox.Show("查無此產品資訊!");
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("請輸入要查詢的產品編號!");
            }
        }

        private void txt輸入產品編號_Enter(object sender, EventArgs e)
        {
            if (是否有文字 == false)
            {
                txt輸入產品編號.Text = "";
                txt輸入產品編號.ForeColor = Color.Black;
            }
        }

        private void txt輸入產品編號_Leave(object sender, EventArgs e)
        {
            if (txt輸入產品編號.Text == "")
            {
                txt輸入產品編號.Text = "輸入欲查詢的產品編號";
                txt輸入產品編號.ForeColor = Color.Black;
                是否有文字 = false;
            }
            else
            {
                是否有文字 = true;
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

        private void btn選產品圖片_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();  //選取新的圖片跳出對話框
            f.Filter = "圖檔類型(*.jpg,*.JPG,*.png)|*.jpeg;*.jpg;*.png";
            //限制可以選取圖片的附檔名

            DialogResult R = f.ShowDialog();

            if (R == DialogResult.OK)
            {
                pbox產品照片.Image = Image.FromFile(f.FileName);
                string fileExt = Path.GetExtension(f.SafeFileName); //用檔案名稱來取得附檔名
                Random myrand = new Random();  //隨機物件
                image_name = DateTime.Now.ToString("yyyyMMddHHmmss") + myrand.Next(1000, 9999).ToString() + fileExt;
                //設定隨機檔名的格式
                是否修改過圖檔 = true;
                Console.WriteLine(image_name);
            }
        }
    }
}
