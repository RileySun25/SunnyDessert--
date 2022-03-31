using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace back_side_system_25shiun
{
    public partial class 一般員工 : Form
    {
        public 一般員工()
        {
            InitializeComponent();
        }

        private void 一般員工_Load(object sender, EventArgs e)
        {
            lbl登入員工姓名.Text = Global員工資訊.員工姓名;
            lbl職等.Text = Global員工資訊.員工職等;
            lbl最底下的版本資訊.Text = "此為Sunny Dessert後台管理系統，版本為1.0。";
        }

        private void btn個人資訊查詢_Click(object sender, EventArgs e)
        {
            個人資訊 news = new 個人資訊();
            news.ShowDialog();
        }

        private void btn登出_Click(object sender, EventArgs e)
        {
            Global員工資訊.員工姓名 = "";
            Global員工資訊.員工工號 = "";
            Global員工資訊.員工職等 = "";
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void btn產品資訊查詢_Click(object sender, EventArgs e)
        {
            產品資訊 news = new 產品資訊();
            news.ShowDialog();
        }

        private void btn會員資訊查詢_Click(object sender, EventArgs e)
        {
            會員資訊 news = new 會員資訊();
            news.ShowDialog();
        }

        private void btn品牌消息查詢_Click(object sender, EventArgs e)
        {
            品牌資訊 news = new 品牌資訊();
            news.ShowDialog();
        }

        private void btn訂單資訊查詢_Click(object sender, EventArgs e)
        {
            訂單資訊 news = new 訂單資訊();
            news.ShowDialog();
        }

        private void btn更改密碼_Click(object sender, EventArgs e)
        {
            更改密碼 news = new 更改密碼();
            news.ShowDialog();
        }
    }
}
