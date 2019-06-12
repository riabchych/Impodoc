﻿using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace ImpoDoc.Entities
{
    public class Attachment : BaseEntity<Attachment>
    {
        public string Filename
        {
            get { return GetValue(() => Filename); }
            set { SetValue(() => Filename, value); }
        }

        public string Path
        {
            get { return GetValue(() => Path); }
            set { SetValue(() => Path, value); }
        }

        public byte[] Content
        {
            get { return GetValue(() => Content); }
            set { SetValue(() => Content, value); }
        }

        public void Create()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            bool? fileOpenResult = openFileDialog.ShowDialog();

            if (fileOpenResult != true)
            {
                return;
            }

            Path = openFileDialog.FileName;
            Filename = openFileDialog.SafeFileName;
            Content = File.ReadAllBytes(Path);
        }

        public string CreateTempFile()
        {
            string path = System.IO.Path.GetTempFileName() + System.IO.Path.GetExtension(Path);
            @File.WriteAllBytes(path, Content);
            return path;
        }

        public void CreateAndOpenFile()
        {
            string path = CreateTempFile();
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Exited += (s, e) => @File.Delete(path);
            process.Start();
        }

        public void CreateAndPrintFile()
        {
            PrintDialog printDialog = new PrintDialog();
            string path = CreateTempFile();

            if (printDialog.ShowDialog() == true)
            {
                ProcessStartInfo info = new ProcessStartInfo(path)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    Verb = "PrintTo"

                };
                Process process = new Process
                {
                    StartInfo = info
                };
                process.Exited += (s, e) => @File.Delete(path);
                process.Start();
            }
        }

    }
}