using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BEST.CheckFiles.ViewModels
{
    public class FileCheckerViewModel : PropertyChangedBase
    {
        public FileCheckerViewModel()
        {
            Pcaps = new List<string>();
            ModifiedFiles = new ObservableCollection<string>();
            CalculatedHashes = new List<string>();
            FolderPath = String.Empty;
        }
        private List<string> pcaps;
        public List<string> Pcaps
        {
            get
            {
                return pcaps;
            }
            set
            {
                pcaps = value;
                NotifyOfPropertyChange(() => Pcaps);
            }
        }
        private List<String> desiredHashes = new List<string>()
        {
        "0ad788ad4e16fd9742f75b1b4b85ddac",
        "51f54b89be8edbee118e30250bc38d33",
        "1ac9826e88213009f1875217ac5a192b",
        "40d97917ddeac6b222270a78714c6c39",
        "481caec15e4db661014e7392470b0448",
        "d48a9970c01d8236130d52c129cb093b",
        "c7f5c7c15714e661af19911ae0e4adbe"
        };
        
        private bool showAnalyze;
        public bool ShowAnalyze
        {
            get
            {
                return showAnalyze;
            }
            set
            {
                showAnalyze = value;
                NotifyOfPropertyChange(() => ShowAnalyze);
            }
        }

        private ObservableCollection<string> modifiedFiles;
        public ObservableCollection<string> ModifiedFiles
        {
            get
            {
                return modifiedFiles;
            }
            set
            {
                modifiedFiles = value;
                NotifyOfPropertyChange(() => ModifiedFiles);
            }
        }

        private int pcapsCount;
        public int PcapsCount
        {
            get
            {
                return pcapsCount;
            }
            set
            {
                pcapsCount = value;
                NotifyOfPropertyChange(() => PcapsCount);
            }
        }

        private string folderPath;
        public string FolderPath
        {
            get
            {
                return folderPath;
            }
            set
            {
                folderPath = value;
                NotifyOfPropertyChange(() => FolderPath);
            }
        }

        public List<string> CalculatedHashes { get; set; }

        private bool isAnalizeComplete;
        public bool IsAnalizeComplete
        {
            get
            {
                return isAnalizeComplete;
            }
            set
            {
                isAnalizeComplete = value;
                NotifyOfPropertyChange(() => IsAnalizeComplete);
            }
        }

        public void SelectFolder()
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result;

            using (dialog)
            {
                result = dialog.ShowDialog();
                FolderPath = dialog.SelectedPath;
            }
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Pcaps = Directory.GetFiles(FolderPath).Where(f => f.EndsWith(".pcap") || f.EndsWith(".pcapng")).ToList();
                PcapsCount = Pcaps.Count;
            }
            ShowAnalyze = PcapsCount > 0;
        }

        public void Analyze()
        {
            CalculatedHashes.Clear();
            ModifiedFiles.Clear();
            foreach (var file in Pcaps)
            {
                string hash = CalculateMD5(file);
                Console.WriteLine("\"" +hash+"\",");
                if (desiredHashes.Contains(hash))
                {
                    var fileName = file.Split('\\').LastOrDefault();
                    ModifiedFiles.Add(fileName);
                }
            }
            ModifiedFiles.OrderBy(x => x);
            IsAnalizeComplete = true;
        }

        public void OpenFile(string path)
        {
            System.Diagnostics.Process.Start(FolderPath + '\\' + path);
        }

        public string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
