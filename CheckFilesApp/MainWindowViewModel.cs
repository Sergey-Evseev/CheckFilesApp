using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;


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
        ForbiddenWords = new ObservableCollection<string>(File.ReadLines(FORB_FILE).ToList());
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
            List<string> files = Directory.GetFiles(_selectedDirectory, "*.txt", SearchOption.AllDirectories).ToList();
            
            //чтение каждого файла из списка
            foreach (var file in files)
            {
                using (StreamReader stream = new StreamReader(file))
                {
                    string line;
                    while ( (line = await stream.ReadLineAsync()) != null)
                    {
                        bool find = false;
                        foreach (var word in ForbiddenWords) //ObservableCollection
                          if (line.ToLower().Contains(word.ToLower()))
                          {
                                find = true;
                                var fi = new FileInfo(file); //получаем информацию о файле для имени файла
                                File.Copy(file, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                    + $"\\{COPY_DIRECT_NAME}\\{fi.Name}_forbidden");
                                break; 
                          }
                    }
                }
            }
        }
    }
}
