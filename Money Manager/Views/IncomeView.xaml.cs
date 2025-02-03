using Money_Manager.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Money_Manager
{
    /// <summary>
    /// Логика взаимодействия для IncomeView.xaml
    /// </summary>
    public partial class IncomeView : Page
    {
        Database database = new Database();
        public IncomeView()
        {
            InitializeComponent();
        }

        private void Add_New_Income(object sender, RoutedEventArgs e)
        {
            if(CheckIncome())
            {

            }
        }

        private bool CheckIncome()
        {
            var amount = amountBox.Text;
            var description = descriptionBox.Text;

            if (amount.Equals(""))
            {
                MessageBox.Show("Введите сумму");
            }
            if (description.Equals(""))
            {
                MessageBox.Show("Введите описание");
            }
            return (AddIncome(amount, description));
        }

        private bool AddIncome(string amount, string description)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount))
            {
                MessageBox.Show("Введите корректную сумму.");
                return false;
            }

            string query = "INSERT INTO Transactions (Amount, Type, Description, User_id, Category_id) VALUES (@Amount, 'Income', @Description, @UserId, @CategoryId)";
            try
            {
                using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                {
                    // Передаем параметры
                    command.Parameters.AddWithValue("@Amount", parsedAmount);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@UserId", currentUserId); // Текущий ID пользователя
                    command.Parameters.AddWithValue("@CategoryId", "Доход"); // ID категории для дохода (например, "Доход")

                    database.OpenConnection();

                    // Выполняем запрос
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected == 1; // Возвращаем true, если добавлено 1 строка
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return false;
            }
        }
    }
}