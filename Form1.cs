using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Button = System.Windows.Forms.Button;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public List<Button> choose_btn = new List<Button>();
        public static int[] lotto_num = new int[6];
        public static List<int> lotto_list = new List<int>();
        public static int totalMoney;

        void create_btn()
        {
            int select_num = 0;
            for (int i  = 0; i < 7; i++)
            {
                for(int j =  0; j < 6; j++)
                {
                    if(select_num < 38)
                    {
                        select_num += 1;
                        Button num_but = new Button();
                        num_but.BackColor = Color.Gold;
                        num_but.ForeColor = Color.Black;
                        num_but.Font = new Font("微軟正黑體", 18);
                        num_but.Location = new Point(440 + 60 * j, 20 + 50 * i);
                        num_but.Size = new Size(55, 40);
                        num_but.Text = select_num.ToString();
                        num_but.Click += new EventHandler(num_but_Click);

                        Controls.Add(num_but);
                        choose_btn.Add(num_but);
                    }

                }
            }
        }


        private void num_but_Click(object sender, EventArgs e)
        {
            Button this_btn = (Button) sender;
            //foreach (Button button in this_btn.Controls)
            //button.Enabled = false;

            if (lotto_list.Count < 6)
            {
                if (this_btn.BackColor == Color.Gold)
                {
                    this_btn.BackColor = Color.Black;
                    this_btn.ForeColor = Color.Gold;
                    int btn_num = int.Parse(this_btn.Text);
                    lotto_list.Add(btn_num);
                    Console.WriteLine(lotto_list.Count);
                }
                else
                {
                    this_btn.BackColor = Color.Gold;
                    this_btn.ForeColor = Color.Black;
                }
            }
            return;
            
        }


        public int[] produce_lotto()//樂透產生器
        {
            //int[] lotto_num = new int[6];
            for (int i = 0; i < lotto_num.Length; i++)
            {
                int num;
                do
                {
                    Random random = new Random();
                    num = random.Next(1, 38);
                } while (Array.IndexOf(lotto_num, num) != -1);//比較不重複
                lotto_num[i] = num;
                //Console.WriteLine($"{lotto_num[i]}");
            }
            Array.Sort(lotto_num);//由小至大排序
            return lotto_num;
        }


        public Form1()
        {
            InitializeComponent();
        }

        

        private void btn包牌自動選號_Click(object sender, EventArgs e)
        {
            if (tBx包牌自動選號.Text == "請輸入數字" || tBx包牌自動選號.Text == "")
            {
                MessageBox.Show("請輸入組數");
            }
            else
            {
                string lottoStr = tBx包牌自動選號.Text;
                int lotto_quantity = int.Parse(lottoStr);
                for (int i = 0;i < lotto_quantity; i++)
                {
                    //int[] lotto_num;
                    string strnum = " ";
                    lotto_num = produce_lotto();


                    for (int j = 0; j < lotto_num.Length; j++)
                    {
                        if (j == lotto_num.Length - 1)
                        {
                            strnum += ($" {lotto_num[j]}");
                        }
                        else
                        {
                            strnum += ($" {lotto_num[j]} ,");
                        }
                    }
                    listBox選號紀錄.Items.Add(strnum);
                }
            }
            //lbl顯示金額.Text += lotto_quantity * 500;

        }

        private void btn自動選號_Click(object sender, EventArgs e)
        {
            string MsMag = "";
            string strnum = " ";
            int[] lotto_num;
            lotto_num = produce_lotto();
            MsMag = "~~~~~您的幸運號碼~~~~~\n \n";

            foreach (int i in lotto_num){
                MsMag += ($"{i} ");
            }

            MsMag += "\n\n確認後請付款";

            for (int i = 0; i < lotto_num.Length; i++){
                strnum += ($" {lotto_num[i]} ,");
            }

            listBox選號紀錄.Items.Add(strnum);
            MessageBox.Show(MsMag);

        }       
        private void Form1_Load_1(object sender, EventArgs e)
        {
            create_btn();
        }

        private void btn刪除號碼_Click(object sender, EventArgs e)
        {
            for (int i = listBox選號紀錄.SelectedIndices.Count - 1; i >= 0; i--)
                listBox選號紀錄.Items.RemoveAt(listBox選號紀錄.SelectedIndices[i]);
        }


        private void tBx包牌自動選號_TextChanged(object sender, EventArgs e)
        {
            int number;
            if (!int.TryParse(tBx包牌自動選號.Text, out number))
            {
                tBx包牌自動選號.Text = "";                
            }
        }
        private void tBx包牌自動選號_Enter(object sender, EventArgs e)
        {
            if (tBx包牌自動選號.Text == "請輸入數字")
            {
                tBx包牌自動選號.Text = "";
            }
        }

        private void tBx包牌自動選號_Leave(object sender, EventArgs e)
        {
            if (tBx包牌自動選號.Text == "")
            {
                tBx包牌自動選號.Text = "請輸入數字";
            }
        }

        private void lbl顯示金額_Click(object sender, EventArgs e)
        {
            

        }

        private void btn結帳_Click_1(object sender, EventArgs e)
        {
            string MsMag = "";
            string strnum = " ";
            string[] items = listBox選號紀錄.Items.Cast<string>().ToArray();                       
            string message = string.Join(Environment.NewLine, items);

            MsMag = "~~~~~您的幸運號碼~~~~~\n \n";
            MsMag += $"{message}{Environment.NewLine}";
            MsMag += "\n\n謝謝光臨\n祝您中獎!";
          
            MessageBox.Show(MsMag);

            //取得選號紀錄組數添加至開獎金額
            int count = listBox選號紀錄.Items.Count;
            int totalMoney = int.Parse(lbl顯示金額.Text);
            Console.WriteLine(lbl顯示金額.Text);

            totalMoney += count * 100;
            string moneystr = totalMoney.ToString();
            lbl顯示金額.Text = moneystr;
            listBox選號紀錄.Items.Clear();
            tBx包牌自動選號.Text = "請輸入數字";
            Form1.totalMoney = totalMoney;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string strnum = " ";
            lotto_list.Sort();//由小至大排序
            for (int i = 0; i < lotto_list.Count; i++)
            {
                strnum += ($" {lotto_list[i]} ,");
            }
            listBox選號紀錄.Items.Add(strnum);
            lotto_list.Clear();
            foreach (Button button in choose_btn)
            {
                if (button.BackColor == Color.Black)
                {
                    button.BackColor = Color.Gold;
                    button.ForeColor = Color.Black;
                }
            }

        }
    }
}

123
12345