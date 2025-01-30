using Money_Manager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Money_Manager
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        RegistrationView registrationView = new RegistrationView();
        Database database = new Database();
        public LoginView()
        {
            InitializeComponent();
        }

        private void Registration_Lable_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationView());
            ((MainWindow)Application.Current.MainWindow).TitleBarText = "Registration";
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            var login = loginLog.Text.Trim();
            var password = PasswordLog.Password;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Не введён логин или пароль");
            }
            if (AuthorizationUser(login, password))
            {
                ((MainWindow)Application.Current.MainWindow).TitleBarText = "Home";
                NavigationService.Navigate(new HomePage());
            }
        }

        private bool AuthorizationUser(string login, string password)
        {
            string query = "SELECT Id, Firstname FROM Users WHERE Login = @Login AND Password_hash = @PasswordHash";

            try
            {
                using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@PasswordHash", registrationView.HashPassword(password)); 

                    database.OpenConnection();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstname = reader["Firstname"].ToString();
                            MessageBox.Show($"Добро пожаловать, {firstname}!");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }

            return false;
        }
    }
}