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

        //метод который загружает список запрещенных слов
        public bool LoadForbiddenWords(string path="")
        {
            //TODO: Добавить проверку файла
            _forbiddenWords = File.ReadLines(path).ToList();
            OnPropertyChanged(nameof(ForbiddenWords));
            return true;
        }
        //метод сканирования директории
        public bool ScanDirectory()
        {
            List<string> files = 
            Directory.GetFiles(_selectedDirectory, "*.txt", SearchOption.AllDirectories).ToList();
            return true;
        }
    }
}
