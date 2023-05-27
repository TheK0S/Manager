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

        public static void RemoveCategory(Category category)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"DELETE FROM Categories WHERE Categories.Id = {category.Id}";
                    MessageBox.Show($"Категория {category.CategoriesName} удалена", "Выполнено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при удалении категории", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RemoveWord(Word word)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"DELETE FROM {word.CategoryName} WHERE {word.CategoryName}.Id = {word.Id}";
                    MessageBox.Show($"Слово {word.Words} удалено", "Выполнено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при удалении слова", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
