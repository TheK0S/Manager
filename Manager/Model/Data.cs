using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using System.Data.SqlClient;

namespace Manager.Model
{
    class Data
    {
        static string connectionString = @"Data Source = DESKTOP-K60TA32\SQLEXPRESS; Initial Catalog = WordsDB; Trusted_Connection=True; Encrypt = False";

        public static List<Level> GetLevels()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = @"SELECT * FROM Levels";
                    return db.Query<Level>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при загрузке уровней", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Level>();
            }
        }

        public static List<Category> GetCategories()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = @"SELECT * FROM Categories";
                    return db.Query<Category>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при загрузке категорий", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Category>();
            }
        }

        public static List<Word> GetWords(string wordCategory)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = @"SELECT * FROM " + wordCategory;
                    return db.Query<Word>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при загрузке категорий", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Word>();
            }
        }
    }
}
