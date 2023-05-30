using Manager.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Manager
{
    /// <summary>
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        bool isShowSuccessfulOperations = true;
        string ImgLoc = "";
        List<Category> categories = new List<Category>();
        List<Level> levels = new List<Level>();
        Dictionary<string, List<Word>> wordTables = new Dictionary<string, List<Word>>();

        public DataPage()
        {
            InitializeComponent();

            categoryGrid.CanUserAddRows = false;
            wordGrid.CanUserAddRows = false;
            

            levels = Data.GetLevels();
            categories = Data.GetCategories();        

            if(categories != null )
                foreach (var category in categories)
                    wordTables.Add(category.CategoriesName, Data.GetWords(category.CategoriesName));

            categoryGrid.ItemsSource = Data.GetCategories();
            wordCategory.ItemsSource = Data.GetCategories();
            langLevel.ItemsSource = levels;
            categoryNameOfWord.ItemsSource = Data.GetCategories();

        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            if(langLevel.SelectedItem != null)
            {
                Level level = (Level)langLevel.SelectedItem;
                if(categoryNameField.Text?.Length > 0)
                {
                    Data.CreateCategory(new Category { Id = 0, LevelsId = level.Id, CategoriesName = categoryNameField.Text }, isShowSuccessfulOperations);
                    List<Category> tempCtegory = Data.GetCategories();
                    categoryGrid.ItemsSource = tempCtegory.ToList();
                    wordCategory.ItemsSource = tempCtegory.ToList();
                    categoryNameOfWord.ItemsSource = tempCtegory.ToList();
                    categories = tempCtegory.ToList();
                }
                else
                {
                    MessageBox.Show("Введите имя категории", "Пустое поле имени категории",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }            
            else
            {
                MessageBox.Show("Виберите уровень языка в списке и повторите попытку", "Не выбран уровень языка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addWord_Click(object sender, RoutedEventArgs e)
        {
            if(categoryNameOfWord.SelectedItem != null)
            {
                if(wordsField.Text?.Length > 0)
                {
                    if(translateWordsField.Text?.Length > 0)
                    {
                        if(transcriptionsField.Text?.Length > 0)
                        {
                            if (sentenceField.Text?.Length > 0)
                            {
                                if (transSentenceField.Text?.Length > 0)
                                {
                                    string transcription = "";
                                    transcription = transcriptionsField.Text;
                                    if(!(ImgLoc?.Length > 0))
                                    {
                                        ImgLoc = "\\image\\default_picture.png";
                                    }

                                    byte[] image = null;
                                    FileStream file = new FileStream(ImgLoc, FileMode.Open, FileAccess.Read);
                                    BinaryReader binaryReader = new BinaryReader(file);
                                    image = binaryReader.ReadBytes((int)file.Length);

                                    Word word = new Word
                                    {
                                        Id = 0,
                                        CategoryName = categoryNameOfWord.Text,
                                        Words = wordsField.Text,
                                        Transcriptions = transcription,
                                        Sentence = sentenceField.Text,
                                        TranslateWords = translateWordsField.Text,
                                        TransSentence = transSentenceField.Text,
                                        Picture = image
                                    };

                                    Data.AddWord(word, isShowSuccessfulOperations);

                                    if (wordTables[word.CategoryName] != null)
                                    {
                                        wordTables[word.CategoryName] = Data.GetWords(word.CategoryName);

                                        wordCategory.SelectedItem = categories.FirstOrDefault(c => c.CategoriesName == word.CategoryName);
                                        wordGrid.ItemsSource = wordTables[word.CategoryName];
                                    }
                                                                                
                                }
                                else
                                {
                                    MessageBox.Show("Введите перевод предложения", "Пустое поле \"Перевод предложения\"",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Введите предложение на иностранном", "Пустое поле \"Предложение в оригинале\"",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Введите транскрипцию слова", "Пустое поле \"Транскрипция\"",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите перевод слова", "Пустое поле \"Перевод слова\"",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите оригинал слова", "Пустое поле \"Слово в оригинале\"",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Виберите категорию слов и повторите попытку", "Не выбрана категория",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateCategory_Click(object sender, RoutedEventArgs e)
        {
            if(categoryGrid.SelectedItem != null)
            {
                Category category = (Category)categoryGrid.SelectedItem;

                foreach (var item in categories)
                {
                    if(category.Id == item.Id)
                    {
                        if(category.LevelsId == item.LevelsId && category.CategoriesName == item.CategoriesName)
                        {
                            MessageBox.Show("Не требуется изменение категории, так как данные категории в таблице не изменились");
                        }
                        else
                        {
                            Data.UpdateCategory(category, item, isShowSuccessfulOperations);

                            List<Category> tempCtegory = Data.GetCategories();
                            categoryGrid.ItemsSource = tempCtegory.ToList();
                            wordCategory.ItemsSource = tempCtegory.ToList();
                            categoryNameOfWord.ItemsSource = tempCtegory.ToList();
                            categories = tempCtegory.ToList();
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Виберите категорию в таблице и повторите попытку", "Не выбрана категория",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            if(categoryGrid.SelectedItem != null)
            {
                Data.RemoveCategory((Category)categoryGrid.SelectedItem, isShowSuccessfulOperations);

                List<Category> tempCtegory = Data.GetCategories();
                categoryGrid.ItemsSource = tempCtegory.ToList();
                wordCategory.ItemsSource = tempCtegory.ToList();
                categoryNameOfWord.ItemsSource = tempCtegory.ToList();
                categories = tempCtegory.ToList();
            }
            else
            {
                MessageBox.Show("Виберите категорию в таблице и повторите попытку", "Не выбрана категория для удаления",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeWord_Click(object sender, RoutedEventArgs e)
        {
            if (wordGrid.SelectedItem != null)
            {
                Word word = (Word)wordGrid.SelectedItem;
                Data.RemoveWord(word, isShowSuccessfulOperations);

                if (word.CategoryName != null)
                    wordTables[word.CategoryName] = Data.GetWords(word.CategoryName);
            }
            else
            {
                MessageBox.Show("Виберите слово в таблице и повторите попытку", "Не выбрано слово для удаления", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void wordCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wordCategory.SelectedItem != null)
            {
                try
                {
                    Category category = (Category)wordCategory.SelectedItem;
                    if (category.CategoriesName != null)
                        wordGrid.ItemsSource = wordTables[category.CategoriesName];
                }
                catch (Exception)
                {
                    MessageBox.Show("После редактирования категории в таблице, не изменяйте выбор строки до нажатия кнопки \"Редактировать категорию\"",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void isShowSuccOperations_Unchecked(object sender, RoutedEventArgs e)
        {
            isShowSuccessfulOperations = false;
        }

        private void isShowSuccOperations_Checked(object sender, RoutedEventArgs e)
        {
            isShowSuccessfulOperations = true;
        }

        private void wordSearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            Category category = (Category)wordCategory.SelectedItem;
            if (category != null)
            {
                List<Word> words = Data.GetWords(category.CategoriesName);

                if (wordSearchField.Text?.Length > 0)
                {
                    wordGrid.ItemsSource = words.FindAll(w => w.Words.ToLower().Contains(wordSearchField.Text.ToLower())
                    || w.TranslateWords.ToLower().Contains(wordSearchField.Text.ToLower()));
                }
                else
                {
                    wordGrid.ItemsSource = words;
                }
            }
        }

        private void categorySearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Category> categoriesTemp = Data.GetCategories();

            if(categorySearchField.Text?.Length > 0)
            {
                categoryGrid.ItemsSource = categoriesTemp.FindAll(c => c.CategoriesName.ToLower().Contains(categorySearchField.Text.ToLower()));
            }
            else
            {
                categoryGrid.ItemsSource = categoriesTemp;
            }
        }

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //OpenFileDialog ofdPicture = new OpenFileDialog();
                //ofdPicture.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
                //ofdPicture.FilterIndex = 1;

                //if (ofdPicture.ShowDialog() == true)
                //    imageWord.Source = new BitmapImage(new Uri(ofdPicture.FileName));

                //ImgLoc = ofdPicture.FileName.ToString();

                OpenFileDialog ofdPicture = new OpenFileDialog();
                ofdPicture.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
                ofdPicture.FilterIndex = 1;

                if (ofdPicture.ShowDialog() == true)
                {
                    ImgLoc = ofdPicture.FileName.ToString();

                    byte[] img = null;

                    FileStream file = new FileStream(ImgLoc, FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(file);
                    img = binaryReader.ReadBytes((int)file.Length);

                    using (MemoryStream stream = new MemoryStream(img))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        image.Freeze();
                    }
                }

                                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при загрузке картинки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
