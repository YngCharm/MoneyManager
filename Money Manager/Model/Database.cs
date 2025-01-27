using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Manager.Model
{
    public class Database
    {
        private SqlConnection sqlConnection = new SqlConnection(
            @"Data Source=CHARMY\SQLEXPRESS01;Initial Catalog=Money_manager_db;Integrated Security=True;");

        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
/*-- Создание таблицы Users
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Firstname NVARCHAR(255) NOT NULL,
    Lastname NVARCHAR(255) NOT NULL,
    Login NVARCHAR(255) UNIQUE CHECK (LEN(Login) >= 6) NOT NULL,
    Password_hash NVARCHAR(255) NOT NULL,
    Registration_date DATETIME DEFAULT GETDATE() NOT NULL
);

-- Создание таблицы Category
CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Создание таблицы Transaction
CREATE TABLE Transaction (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Amount DECIMAL(18, 2) NOT NULL,
    Date DATETIME DEFAULT GETDATE() NOT NULL,
    CategoryId INT NOT NULL,
    AccountId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id),
    FOREIGN KEY (AccountId) REFERENCES Account(Id)
);

-- Создание таблицы Account
CREATE TABLE Account (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL,
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
); */