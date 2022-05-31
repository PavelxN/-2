using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace diplom
{

    public partial class Form5 : Form
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
            public DataTable table = new DataTable();


            public void GetListStaff(BindingSource bs1, DataGridView dg1)
            {

                //Запрос для вывода строк в БД
                string commandStr = "SELECT uchet.id AS Код, uchet.name AS Наименование,uchet.number AS Инв_номер,uchet.price AS Цена, room.number AS Кабинет, employee.fio AS Сотрудник, information AS Информация FROM(uchet INNER JOIN employee ON uchet.id_employee = employee.id) INNER JOIN room ON uchet.id_room = room.id";
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

            public void reload(BindingSource bs1, DataGridView dg1)
            {
                //Чистим виртуальную таблицу
                table.Clear();
                //Вызываем метод получения записей, который вновь заполнит таблицу
                Database DBO = new Database();
                DBO.GetListStaff(bs1, dg1);
            }
            public void GetCurrentID(DataGridView dataGridView1)
            {
                index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
                // MessageBox.Show("Индекс выбранной строки" + index_selected_rows);
                id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
                // MessageBox.Show("Содержимое поля Код, в выбранной строке" + id_selected_rows);
                class_edit_user.id = id_selected_rows;
            }
            public void DeleteEmpl(int id)
            {
                string del = "DELETE FROM uchet WHERE id = " + id;
                MySqlCommand del_empl = new MySqlCommand(del, conn);

                try
                {
                    conn.Open();
                    del_empl.ExecuteNonQuery();
                    MessageBox.Show("Удалено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            public void Insert(TextBox textBox1, TextBox textBox2, TextBox textBox3, ComboBox comboBox2, ComboBox comboBox3, RichTextBox richTextBox1)
            {
                string connStr = "server=chuc.caseum.ru;port=33333;user=st_1_18_19;database=is_1_18_st19_VKR;password=123123123;";

                MySqlConnection conn = new MySqlConnection(connStr);
                //Получение новых параметров пользователя
                string new_name = textBox1.Text;
                string new_number = textBox2.Text;
                string new_price = textBox3.Text;
                string new_room = comboBox2.SelectedValue.ToString();
                string new_employee = comboBox3.SelectedValue.ToString();
                string new_information = richTextBox1.Text;

                if (textBox1.Text.Length > 0)
                {
                    //Формируем строку запроса на добавление строк
                    string sql_insert_clothes = " INSERT INTO  `uchet` (name, number,price,id_room,id_employee,information) " +
                        "VALUES ('" + new_name + "','" + new_number + "','" + new_price + "','" + new_room + "', '" + new_employee + "','"+ new_information +"')";


                    //Посылаем запрос на добавление данных
                    MySqlCommand insert_clothes = new MySqlCommand(sql_insert_clothes, conn);
                    try
                    {
                        conn.Open();
                        insert_clothes.ExecuteNonQuery();
                        MessageBox.Show("Успешно добавлено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            public void Update(TextBox textBox1, TextBox textBox2, TextBox textBox3, ComboBox comboBox2, ComboBox comboBox3, RichTextBox richTextBox1)
            {
                //Получаем ID пользователя
                string id = class_edit_user.id;
                string SQL_izm = "UPDATE uchet SET name=N'" + textBox1.Text + "', number=N'" + textBox2.Text + "', price=N'" + textBox3.Text + "' , id_room=N'" + comboBox2.Text + "' , id_employee=N'" + comboBox3.Text + "', information=N'" + richTextBox1.Text + "' where id=" + id;
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
                comboBox2.Text = "";
                comboBox3.Text = "";
                richTextBox1.Text = "";
            }
            public void GetComboBox2(ComboBox comboBox2)
            {
                //Формирование списка статусов
                DataTable list_room_table = new DataTable();
                MySqlCommand list_room_command = new MySqlCommand();
                //Открываем соединение
                conn.Open();
                //Формируем столбцы для комбобокса 
                list_room_table.Columns.Add(new DataColumn("id", System.Type.GetType("System.Int32")));
                list_room_table.Columns.Add(new DataColumn("number", System.Type.GetType("System.String")));
                //Настройка видимости полей комбобокса
                comboBox2.DataSource = list_room_table;
                comboBox2.DisplayMember = "number";
                comboBox2.ValueMember = "id";
                //Формируем строку запроса на отображение списка статусов прав пользователя
                string sql_list_users = "SELECT id, number FROM room";
                list_room_command.CommandText = sql_list_users;
                list_room_command.Connection = conn;
                //Формирование списка для combobox'a
                MySqlDataReader list_room_reader;
                try
                {
                    //Инициализируем ридер
                    list_room_reader = list_room_command.ExecuteReader();
                    while (list_room_reader.Read())
                    {
                        DataRow rowToAdd = list_room_table.NewRow();
                        rowToAdd["id"] = Convert.ToInt32(list_room_reader[0]);
                        rowToAdd["number"] = list_room_reader[1].ToString();
                        list_room_table.Rows.Add(rowToAdd);
                    }
                    list_room_reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения списка  \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    conn.Close();
                }
            }
            public void GetComboBox3(ComboBox comboBox3)
            {
                //Формирование списка статусов
                DataTable list_pers_table = new DataTable();
                MySqlCommand list_pers_command = new MySqlCommand();
                //Открываем соединение
                conn.Open();
                //Формируем столбцы для комбобокса 
                list_pers_table.Columns.Add(new DataColumn("id", System.Type.GetType("System.Int32")));
                list_pers_table.Columns.Add(new DataColumn("fio", System.Type.GetType("System.String")));
                //Настройка видимости полей комбобокса
                comboBox3.DataSource = list_pers_table;
                comboBox3.DisplayMember = "fio";
                comboBox3.ValueMember = "id";
                //Формируем строку запроса на отображение списка статусов прав пользователя
                string sql_list_users = "SELECT id, fio FROM employee";
                list_pers_command.CommandText = sql_list_users;
                list_pers_command.Connection = conn;
                //Формирование списка для combobox'a
                MySqlDataReader list_pers_reader;
                try
                {
                    //Инициализируем ридер
                    list_pers_reader = list_pers_command.ExecuteReader();
                    while (list_pers_reader.Read())
                    {
                        DataRow rowToAdd = list_pers_table.NewRow();
                        rowToAdd["id"] = Convert.ToInt32(list_pers_reader[0]);
                        rowToAdd["fio"] = list_pers_reader[1].ToString();
                        list_pers_table.Rows.Add(rowToAdd);
                    }
                    list_pers_reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения списка \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetListStaff(bindingSource1, dataGridView1);
            DBO.GetComboBox2(comboBox2);
            DBO.GetComboBox3(comboBox3);

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[4].Visible = true;
            dataGridView1.Columns[5].Visible = true;
            dataGridView1.Columns[6].Visible = false;

            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 5;
            dataGridView1.Columns[1].FillWeight = 15;
            dataGridView1.Columns[2].FillWeight = 8;
            dataGridView1.Columns[3].FillWeight = 5;
            dataGridView1.Columns[4].FillWeight = 6;
            dataGridView1.Columns[5].FillWeight = 10;
            dataGridView1.Columns[6].FillWeight = 10;


            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.Insert(textBox1, textBox2, textBox3, comboBox2,comboBox3, richTextBox1);
            DBO.reload(bindingSource1, dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetCurrentID(dataGridView1);
            DBO.Update(textBox1, textBox2, textBox3,comboBox2, comboBox3, richTextBox1);
            DBO.reload(bindingSource1, dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Database DBO = new Database();
            DBO.GetCurrentID(dataGridView1);
            DBO.DeleteEmpl(Convert.ToInt32(id_selected_rows));
            DBO.reload(bindingSource1, dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string fromDGtoTB = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox1.Text =
                dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text =
                dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox3.Text =
               dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
            comboBox2.Text =
               dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
            comboBox3.Text =
               dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
            richTextBox1.Text =
               dataGridView1[6, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindingSource1.Filter = "[Сотрудник] LIKE'" + comboBox1.Text + "%'";
            //Database DBO = new Database();

            //DBO.table.DefaultView.RowFilter = String.Format("Кабинет LIKE '" + comboBox1.Text +" '");
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void exprt_Click(object sender, EventArgs e)
        {
            Excel.Application exapp = new Excel.Application();
            exapp.Workbooks.Add();
            Excel.Worksheet wsh = (Excel.Worksheet)exapp.ActiveSheet;
            wsh.Columns[3].ColumnWidth = 30;
            wsh.Columns[7].ColumnWidth = 30;
            wsh.Columns[8].ColumnWidth = 170;

            int i, j;
            for(i=0;i<=dataGridView1.RowCount-2;i++)
            {
                for(j=0;j<=dataGridView1.ColumnCount-1;j++)
                {
                    wsh.Cells[1, j + 2] = dataGridView1.Columns[j].HeaderText.ToString();
                    wsh.Cells[i + 2, j + 2] = dataGridView1[j, i].Value.ToString();
                }
            }
            exapp.Visible = true;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            Database orders = new Database();
            orders.GetListStaff(bindingSource1, dataGridView1);
            bindingSource1.Filter = "[Сотрудник] LIKE'" + comboBox1.Text + "%'";
        }
    }
}
