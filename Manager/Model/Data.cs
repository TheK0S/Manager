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
using static Dapper.SqlMapper;
using System.Windows.Input;

namespace Manager.Model
{
    class Data
    {
        static string connectionString = @"Data Source = DESKTOP-HHO6PH0; Initial Catalog = WordsDB; Trusted_Connection=True; Encrypt = False";

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
                MessageBox.Show(ex.Message, "Ошибка при загрузке списка слов", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Word>();
            }
        }

        public static bool RemoveCategory(Category category, bool isShowSuccessful)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"DELETE FROM Categories WHERE Categories.Id = {category.Id}";
                    db.Query<Category>(sqlCommand);

                    if(isShowSuccessful)
                        MessageBox.Show($"Категория {category.CategoriesName} удалена", "Выполнено");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при удалении категории", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool RemoveWord(Word word, bool isShowSuccessful)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"DELETE FROM {word.CategoryName} WHERE {word.CategoryName}.Id = {word.Id}";
                    db.Query<Word>(sqlCommand);

                    if(isShowSuccessful)
                        MessageBox.Show($"Слово {word.Words} удалено", "Выполнено");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при удалении слова", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static void CreateCategory(Category category, bool isShowSuccessful)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"INSERT INTO Categories VALUES({category.LevelsId},'{category.CategoriesName}')";
                    db.Query(sqlCommand);

                    sqlCommand = $"CREATE TABLE {category.CategoriesName} (" +
                        $"Id INT IDENTITY PRIMARY KEY," +
                        $" CategoryName NVARCHAR(50) REFERENCES Categories(CategoriesName) ON DELETE CASCADE," +
                        $" Words NVARCHAR(20) NOT NULL," +
                        $" Transcriptions NVARCHAR(50) NOT NULL," +
                        $" Sentence NVARCHAR(120) NOT NULL," +
                        $" TranslateWords NVARCHAR(20) NOT NULL," +
                        $" TransSentence NVARCHAR(120) NOT NULL )";

                    db.Query(sqlCommand);

                    if(isShowSuccessful)
                        MessageBox.Show($"Категория {category.CategoriesName} создана", "Выполнено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при создании категории", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateCategory(Category newCategory, Category oldCategory, bool isShowSuccessful)
        {
            try
            {               
                using (IDbConnection db = new SqlConnection(connectionString))
                {                    
                    CreateCategory(newCategory, false);

                    string sqlCommand = $"INSERT INTO {newCategory.CategoriesName} " +
                        $"SELECT '{newCategory.CategoriesName}', Words, Transcriptions, Sentence, TranslateWords, TransSentence" +
                        $" from {oldCategory.CategoriesName}";
                    db.Query<Category>(sqlCommand);

                    if(!RemoveCategory(oldCategory, false))
                        MessageBox.Show($"Не удалена категория с именем {oldCategory.CategoriesName}", "Ошибка при удалении категории",
                            MessageBoxButton.OK, MessageBoxImage.Error);

                    if (isShowSuccessful)
                        MessageBox.Show($"Категория изменена", "Выполнено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при изменении категории", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool AddWord(Word word, bool isShowSuccessful)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"INSERT INTO {word.CategoryName} VALUES (" +
                        $"'{word.CategoryName}', " +
                        $"'{word.Words}', " +
                        $"N'{word.Transcriptions}', " +
                        $"'{word.Sentence}', " +
                        $"'{word.TranslateWords}', " +
                        $"'{word.TransSentence}')";

                    db.Query<Word>(sqlCommand);

                    if (isShowSuccessful)
                        MessageBox.Show($"Слово {word.Words} добавлено", "Выполнено");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при добавлении слова", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool UpdateWord(Word newWord, Word oldWord, bool isShowSuccessful)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlCommand = $"UPDATE {oldWord.CategoryName} SET " +
                        $"CategoryName = '{oldWord.CategoryName}', " +
                        $"Words = '{newWord.Words}', " +
                        $"Transcriptions = N'{newWord.Transcriptions}', " +
                        $"Sentence = '{newWord.Sentence}', " +
                        $"TranslateWords = '{newWord.TranslateWords}', " +
                        $"TransSentence = '{newWord.TransSentence}'";

                    db.Query<Word>(sqlCommand);

                    if (isShowSuccessful)
                        MessageBox.Show($"Слово {oldWord.Words} изменено", "Выполнено");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при изменении слова", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
