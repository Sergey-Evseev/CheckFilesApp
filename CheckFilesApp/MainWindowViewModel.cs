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
        const string FORB_FILE = "ForbiddenWords.txt";
        
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
        List <string> _forbiddenWords;

        public List <string> ForbiddenWords
        {
            get { return _forbiddenWords;}
        }

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
            _forbiddenWords = File.ReadLines(FORB_FILE).ToList();
            OnPropertyChanged(nameof(ForbiddenWords));
            return true;
        }
        //сохранение запрещенных слов в файл (перезапись в файл)
        public bool SaveForbiddenWords()
        {
            try
            {                
                File.WriteAllLines(FORB_FILE, _forbiddenWords);
                return true;
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error saving forbidden words: {ex.Message}");
                return false;
            }
        }


        //метод сканирования директории - найти все текстовые файлы
        public bool ScanDirectory()
        {
            List<string> files = //просканировать директорию, привести список файлов к строке и присвоить списку
            Directory.GetFiles(_selectedDirectory, "*.txt", SearchOption.AllDirectories).ToList();
            return true;
        }
    }
}
