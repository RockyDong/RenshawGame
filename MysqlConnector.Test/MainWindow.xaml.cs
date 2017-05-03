using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MysqlConnector.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
//        private MySqlDataAdapter daCountry;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            string connStr = "server=localhost;user=root;database=world;port=3306;password=123456;";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
//                label2.Text = "Connecting to MySQL...";
                conn.Open();

                string sql = "SELECT Code, Name, HeadOfState FROM Country WHERE Continent='North America'";
                var countryList = new List<Country>();
                var command = conn.CreateCommand();
                command.CommandText = sql;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var code = dataReader.GetString(0);
                    var name = dataReader.GetString(1);
                    countryList.Add(new Country(code,name));
                }
                /*daCountry = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(daCountry);

                dsCountry = new DataSet();
                daCountry.Fill(dsCountry, "Country");
                dataGridView1.DataSource = dsCountry;
                dataGridView1.DataMember = "Country";*/
            }
            catch (Exception ex)
            {
//                label2.Text = ex.ToString();
            }
        }
    }

    public class Country
    {
        public Country(string code,string name)
        {
            Code = code;
            Name = name;
        }
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
