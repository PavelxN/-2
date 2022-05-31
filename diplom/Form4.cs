using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace diplom
{
    public partial class Form4 : Form
    {

        static string index_selected_rows;
        static string id_selected_rows;
        public class Database
        {
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
                string commandStr = "SELECT id AS 'ID', fio AS 'ФИО', age AS 'Возраст', position AS 'Должность' FROM employee";
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
                Database DBO = new Database();
                DBO.GetList(bs1, dg1);
            }
            public void GetCurrentID(DataGridView dataGridView1)
            {
                index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
                // MessageBox.Show("Индекс выбранной строки" + index_selected_rows);
                id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
                // MessageBox.Show("Содержимое поля Код, в выбранной строке" + id_selected_rows);
                class_edit_user.id = id_selected_rows;
            }
            public void Insert(TextBox textBox1, TextBox textBox2, TextBox textBox3)
            {
                string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;";

                MySqlConnection conn = new MySqlConnection(connStr);
                //Получение новых параметров пользователя
                string new_fio = textBox1.Text;
                string new_birth = textBox2.Text;
                string new_position = textBox3.Text;


                if (textBox1.Text.Length > 0)
                {
                    //Формируем строку запроса на добавление строк
                    string sql_insert_clothes = " INSERT INTO `employee` (fio, age,position) " +
                        "VALUES ('" + new_fio + "', '" + new_birth + "', '" + new_position + "')";


                    //Посылаем запрос на добавление данных
                    MySqlCommand insert_clothes = new MySqlCommand(sql_insert_clothes, conn);
                    try
                    {
                        conn.Open();
                        insert_clothes.ExecuteNonQuery();
                        MessageBox.Show("Добавление сотрудника прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления сотрудника \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка!", "Информация");
                }
            }
            public void Delete(int id)
            {
                string del = "DELETE FROM employee WHERE id = " + id;
                MySqlCommand del_empl = new MySqlCommand(del, conn);

                try
                {
                    conn.Open();
                    del_empl.ExecuteNonQuery();
                    //this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления пользователя \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
            public void Update(TextBox textBox1, TextBox textBox2, TextBox textBox3)
            {
                //Получаем ID пользователя
                string id = class_edit_user.id;
                string SQL_izm = "UPDATE employee SET fio=N'" + textBox1.Text + "', age=N'" + textBox2.Text + "', position=N'" + textBox3.Text + "' where id=" + id;
                MessageBox.Show(SQL_izm);
                MySqlConnection conn = new MySqlConnection("server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;");
                conn.Open();
                MySqlCommand command1 = new MySqlCommand(SQL_izm, conn);
                MySqlDataReader dr = command1.ExecuteReader();
                dr.Close();
                conn.Close();
                MessageBox.Show("Данные изменены");
                //this.Activate();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetList(bindingSource1, dataGridView1);

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;

            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 5;
            dataGridView1.Columns[1].FillWeight = 10;
            dataGridView1.Columns[2].FillWeight = 5;
            dataGridView1.Columns[3].FillWeight = 5;


            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string fromDGtoTB = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox1.Text =
                dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text =
                dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox3.Text =
               dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
           
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.Insert(textBox1, textBox2, textBox3);
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetCurrentID(dataGridView1);
            DBO.Update(textBox1, textBox2, textBox3);
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetCurrentID(dataGridView1);
            DBO.Delete(Convert.ToInt32(id_selected_rows));
            DBO.reload_list(bindingSource1, dataGridView1);
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string fromDGtoTB = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox1.Text =
                dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text =
                dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox3.Text =
               dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
        }
    }
}

