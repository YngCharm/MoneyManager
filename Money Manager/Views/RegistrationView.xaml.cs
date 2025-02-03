using Money_Manager.Model;
using System;
using System.Collections;
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
            if (RegistrationSuccess())
            {
                NavigationService.GoBack();
            }
            else { MessageBox.Show("Ошибка"); }
        }


        private bool RegistrationSuccess()
        {
            var firstname = firstnameRegistrarion.Text;
            var lastname = lastnameRegistrarion.Text;
            var login = loginRegistrarion.Text;
            var password = passwordRegistration.Password;
            var confirmPassword = confirmPasswordRegistration.Password;

            if (password.Length < 8)
            {
                MessageBox.Show("Пароль должен быть не менее 8 символов.");
                return false;
            }

            if (!password.Equals(confirmPassword))
            {
                MessageBox.Show("Пароли не совпадают.");
                return false;
            }

            string hashedPassword = HashPassword(password);

            return (insertUser(firstname, lastname, login, hashedPassword));
            
        }

        private bool insertUser(string firstname, string lastname, string login, string hashedPassword)
        {
            string query = "INSERT INTO Users (Firstname, Lastname, Login, Password_hash) VALUES (@Firstname, @Lastname, @Login, @PasswordHash)"
                + "SELECT SCOPE_IDENTITY()";
            try
            {
                using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Firstname", firstname);
                    command.Parameters.AddWithValue("@Lastname", lastname);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Passwordhash", hashedPassword);

                    database.OpenConnection();

                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value && result != null) // Проверяем, что результат не null
                    {
                        int newUserId = Convert.ToInt32(result); // Преобразуем результат в int
                        return CreateDefaultAccount(newUserId); // Передаем ID пользователя в метод создания счета
                    }
                    else
                    {
                        MessageBox.Show("Не удалось получить ID пользователя.");
                    }

                }
                
            }       
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;

        }

        private bool CreateDefaultAccount(int userId)
        {
            string accountQuery = "INSERT INTO Account (Title, Balance, User_id) VALUES (@Title, @Balance, @UserId)";
            try
            {
                using (SqlCommand command = new SqlCommand(accountQuery, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Title", "Основной счет");
                    command.Parameters.AddWithValue("@Balance", 0);
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected == 1;

                }
            }
            catch(Exception ex)
            {
             MessageBox.Show(ex.Message );
                return false;
            }
        }

        public string HashPassword(string password)
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
