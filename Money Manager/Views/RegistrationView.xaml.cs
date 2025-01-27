using Money_Manager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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

namespace Money_Manager
{
    /// <summary>
    /// Логика взаимодействия для RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : Page
    {
        Database database = new Database();

        public RegistrationView()
        {
            InitializeComponent();
        }

        private void Button_Registration_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationSuccsecs())
            {
                NavigationService.GoBack();
            }
            else { MessageBox.Show("Ошибка"); }
        }


        private bool RegistrationSuccsecs()
        {
            var firstname = firstnameRegistrarion.Text;
            var lastname = lastnameRegistrarion.Text;
            var login = loginRegistrarion.Text;
            var password = passwordRegistration.Password;
            var confirmPassword = confirmPasswordRegistration.Password;

            string hashedPassword = HashPassword(password);

            if (inserUser(firstname, lastname, login, hashedPassword) && password.Equals(confirmPassword) && password.Length >= 8)
            {
                return true;
            }
            return false;
        }

        private bool inserUser(string firstname, string lastname, string login, string hashedPassword)
        {
            string query = "INSERT INTO Users (FirstName, LastName, Login, Password_hash) VALUES (@FirstName, @LastName, @Login, @PasswordHash)";
            try
            {
                using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@FirstName", firstname);
                    command.Parameters.AddWithValue("@LastName", lastname);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    database.OpenConnection();

                    // Выполнение запроса
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected == 1;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
