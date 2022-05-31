using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace diplom
{
    
    public partial class Form1 : Form
    {
        private int borderSize = 2;
        private Form currentChildForm;
        private Button currentBtn;

            //Переменная соединения

            MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;");
            //string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;";

            DataTable dt = new DataTable();
            BindingSource bs = new BindingSource();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            private MySqlDataAdapter MyDA = new MySqlDataAdapter();
            //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
            private BindingSource bSource = new BindingSource();
            private DataSet ds = new DataSet();
            //Представляет одну таблицу данных в памяти.
            private DataTable table = new DataTable();

            public void GetList(BindingSource bs1, DataGridView dg1)
            {

                //Запрос для вывода строк в БД
                string commandStr = "SELECT name AS 'Категория', information AS 'Информация' FROM uchet ";
                //Открываем соединение
                conn.Open();
                //Объявляем команду, которая выполнить запрос в соединении conn
                MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
                //Заполняем таблицу записями из БД
                MyDA.Fill(table);
                //Указываем, что источником данных в bindingsource является заполненная выше таблица
                bs1.DataSource = table;
                //Указываем, что источником данных ДатаГрида является bindingsource
                dg1.DataSource = bs1;
                //Закрываем соединение
                conn.Close();
            }

            public void reload_list(BindingSource bs1, DataGridView dg1)
            {
                //Чистим виртуальную таблицу
                table.Clear();
                //Вызываем метод получения записей, который вновь заполнит таблицу
                GetList(bs1, dg1);
            }

            public void stol()
            {
                conn.Open();
                string commandStr = $"SELECT COUNT(name) FROM uchet WHERE name = 'Рабочее место (стол + кресло)'";
                MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
                MySqlDataReader reader_list = MyDA.SelectCommand.ExecuteReader();
                while (reader_list.Read())
                {
                    label1.Text = ("Количество столов и кресел: " + reader_list[0].ToString());
                }
                // закрываем подключение к БД
                conn.Close();

            }
            public void pc()
            {
            conn.Open();
            string commandStr = $"SELECT COUNT(name) FROM uchet WHERE name = 'Компьютер (полный комплект)'";
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MySqlDataReader reader_list = MyDA.SelectCommand.ExecuteReader();
            while (reader_list.Read())
            {
                label3.Text = ("Количество компьютеров: " + reader_list[0].ToString());
            }
            // закрываем подключение к БД
            conn.Close();

            }
            public void printer()
            {
            conn.Open();
            string commandStr = $"SELECT COUNT(name) FROM uchet WHERE name = 'Принтер'";
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MySqlDataReader reader_list = MyDA.SelectCommand.ExecuteReader();
            while (reader_list.Read())
            {
                label4.Text = ("Количество принтеров: " + reader_list[0].ToString());
            }
            // закрываем подключение к БД
            conn.Close();

            }
        public void vsego()
        {
            conn.Open();
            string commandStr = $"SELECT COUNT(name) FROM uchet ";
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MySqlDataReader reader_list = MyDA.SelectCommand.ExecuteReader();
            while (reader_list.Read())
            {
                label5.Text = ("Количество всей инвент.принадлежности: " + reader_list[0].ToString());
            }
            // закрываем подключение к БД
            conn.Close();

        }

        public void Finfo(string msg, info.enmType type)
        {
            info frm = new info();
            frm.showAlert(msg, type);
        }


        public void ManagerRole(int role)
        {
            switch (role)
            {
                //И в зависимости от того, какая роль (цифра) хранится в поле класса и передана в метод, показываются те или иные кнопки.
                //Вы можете скрыть их и не отображать вообще, здесь они просто выключены
                case 1:
                   // label4.Text = "Директор";
                   // label4.ForeColor = Color.Green;
                    btnMenu.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    break;
                case 2:
                   // label4.Text = "Умеренный";
                   // label4.ForeColor = Color.YellowGreen;
                    btnMenu.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = false;
                    break;
                case 3:
                  //  label4.Text = "Минимальный";
                   // label4.ForeColor = Color.Yellow;
                    btnMenu.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = true;
                    break;
                //Если по какой то причине в классе ничего не содержится, то всё отключается вообще
                default:
                 //   label4.Text = "Неопределённый";
                  //  label4.ForeColor = Color.Red;
                    btnMenu.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    break;
            }
        }
        public Form1()
        {
            InitializeComponent();
            CollapseMenu();
            this.Padding = new Padding(borderSize);
            this.BackColor = Color.FromArgb(98, 102, 244);

        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                //open onlyform
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDasktop.Controls.Add(childForm);
            panelDasktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            //Сокрытие текущей формы
            this.Hide();
            //Инициализируем и вызываем форму диалога авторизации
            Form2 form17_Auth2 = new Form2();
            //Вызов формы в режиме диалога
            form17_Auth2.ShowDialog();
            //Если авторизации была успешна и в поле класса хранится истина, то делаем движуху:
            if (diplom.Auth.auth)
            {
                //Отображаем рабочую форму
                this.Show();
                //Вытаскиваем из класса поля в label'ы
               // label2.Text = diplom.Auth.auth_id;
                //label3.Text = diplom.Auth.auth_fio;
                //label1.Text = "Вы авторизованы!";
                //Красим текст в label в зелёный цвет
                //label1.ForeColor = Color.Green;
                //label3.ForeColor = Color.Black;
                //Вызываем метод управления ролями
                ManagerRole(diplom.Auth.auth_role);
            }
            //иначе
            else
            {
                //Закрываем форму
                this.Close();
            }
            GetList(bindingSource1, dataGridView1);
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;

            dataGridView1.Columns[0].FillWeight = 5;
            dataGridView1.Columns[1].FillWeight = 10;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;
            if(m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }
            base.WndProc(ref m);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustForm();
        }

        private void AdjustForm()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(0,8,8,0);
                    break;
                case FormWindowState.Normal:
                    if (this.Padding.Top != borderSize)
                    this.Padding = new Padding(borderSize);
                    break;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if(this.panelMenu.Width > 200)
            {
                panelMenu.Width = 100;
                btnMenu.Dock = DockStyle.Top;
                foreach(Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }
            else
            {
                panelMenu.Width = 230;
                btnMenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = " " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10,0,0,0);
                }
            }
        }
        private void Reset()
        {
            DisableButton();
            panelMenu.Visible = true;
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(31, 30, 68);
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Form3());
            this.Finfo("Вы перешли на вкладку 'Заявки'", info.enmType.Info);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
            try
            {
                if (currentChildForm != null)
                {
                    currentChildForm.Hide();
                }
            }
            catch (Exception ex)
            {

            }
            this.Finfo("Вы перешли на вкладку 'Главная'", info.enmType.Info);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Finfo("Вы вышли из программы", info.enmType.Info);
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Form4());
            this.Finfo("Вы перешли на вкладку 'Сотрудники'", info.enmType.Info);
        }

        private void panelDasktop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new Form5());
            this.Finfo("Вы перешли на вкладку 'Учет'", info.enmType.Info);
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("T");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) 
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            stol();
            pc();
            printer();
            vsego();
        }
    }
}
