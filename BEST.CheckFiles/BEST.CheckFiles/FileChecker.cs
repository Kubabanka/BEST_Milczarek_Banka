using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BEST.CheckFiles
{
    public class FileChecker
    {
        public FileChecker()
        {
            CalculatedHashes = new List<string>();
            FolderPath = "Select folder by clicking on the button";
        }

        public int PcapsCount { get { return CalculatedHashes.Count; } }

        public string FolderPath { get; set; }

        public List<string> CalculatedHashes { get; set; }

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
                var pcaps = Directory.GetFiles(FolderPath).Where(f => f.EndsWith(".pcap")).ToList();
                foreach (var item in pcaps)
                {
                    CalculateMD5(item);
                }
            }
        }

        public void CalculateMD5(string filePath)
        {
            CalculatedHashes.Clear();
            string hashStr;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    hashStr = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            CalculatedHashes.Add(hashStr);
        }
    }
}
