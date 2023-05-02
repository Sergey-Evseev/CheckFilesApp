using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Xml.Linq;
using System.Windows.Shapes;
using Path = System.IO.Path; 

namespace CheckFilesApp
{
    internal class MainWindowViewModel : INPC
    {
        const string FORB_FILE = "ForbiddenWords.txt";//константа с именем файла с запрещ. словами
        const string COPY_DIRECT_NAME = "Forbidden Files";//дир.для сохр. найденных файлов с запрещ. словами
        
        //Выбранная директория
        private string _selectedDirectory;

        public string SelectedDirectory
        {
            get { return _selectedDirectory; }

            set {
                _selectedDirectory = value;
                OnPropertyChanged();
            }
        }
        //Список запрещенных слов
        public ObservableCollection <string> ForbiddenWords {get; set;}
        

        //прогресс чтения и перезаписи слов
        private int _progress = 0;
        public int Progress
        {
            get { return _progress; }
        }

        //метод который загружает запрещенные слова из файла
        public bool LoadForbiddenWords()
        {
            //TODO: Добавить проверку файла
            //Method uses the File.ReadLines method to read all the lines from a file located at the path
            //specified by the FORB_FILE constant, converts the result to a list and adds all the lines
            //to the ForbiddenWords collection.
            ForbiddenWords = new ObservableCollection<string>(File.ReadLines(FORB_FILE).ToList());
            //calling the OnPropertyChanged method and passing the name of a property as an argument
            //nameof is used with a property, it returns the name of the property as a string.
            //The string is then passed as an argument to the OnPropertyChanged method, which raises
            //the PropertyChanged event with the name of the property.
            OnPropertyChanged(nameof(ForbiddenWords));
            return true;
        }
        //сохранение запрещенных слов в файл (перезапись в файл)
        public bool SaveForbiddenWords()
        {
            try
            {                
                File.WriteAllLines(FORB_FILE, ForbiddenWords);
                return true;
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error saving forbidden words: {ex.Message}");
                return false;
            }
        }


        //метод сканирования директории - найти все текстовые файлы в директории 
        public async Task ScanDirectory()
        {
            ////просканировать директорию, привести список файлов к строке и сделать список файлов
            List<string> files = Directory.GetFiles(_selectedDirectory, "*.txt", 
                SearchOption.AllDirectories).ToList();
            
            //чтение каждого файла из списка
            foreach (var file in files)
            {
                using (StreamReader stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        string line = await stream.ReadLineAsync();
                        foreach (var word in ForbiddenWords) //ObservableCollection
                        {
                            if (line.ToLower().Contains(word.ToLower()))
                            {
                                var fi = new FileInfo(file);
                                //copyPath contains the path of a file going to be copied to a new location.
                                //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                //returns the path to the "My Documents" folder of the current user's profile
                                var copyPath = Path.Combine(Environment.GetFolderPath(Environment.
                                    SpecialFolder.MyDocuments), COPY_DIRECT_NAME, $"{fi.Name}_forbidden");
                                File.Copy(file, copyPath);
                                break;
                            }
                        }
                    }
                }
            }
        }//end of public async Task ScanDirectory()

        //заменить все запрещенные слова в скопированных файлах на звездочки
        public async Task ReplaceTextFiles()
        {
            var copyPath = Path.Combine(Environment.GetFolderPath(Environment. //получаем адрес директории
                SpecialFolder.MyDocuments), COPY_DIRECT_NAME); //где хранятся скопированные файлы
            
            List<string> files = Directory.GetFiles(copyPath).ToList(); //получаем список файлов

            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        foreach (var word in ForbiddenWords)
                            if (line.ToLower().Contains(word.ToLower()))
                            {
                                //string asterisks = new string('*', word.Length);
                                //line = line.Replace(word, asterisks);

                                //"new string('*', word.Length)"-creates a new string consisting of a sequence
                                //of asterisks (*) of the same length as the original word.
                                Regex.Replace(line, word, new string('*', word.Length), 
                                    RegexOptions.IgnoreCase);
                            }
                    }
                }
            }
        }

    }//end of internal class MainWindowViewModel : INPC
}//end of namespace CheckFilesApp
