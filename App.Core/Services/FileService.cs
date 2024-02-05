using App.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class FileService
    {

        public void GetFileInfo(FileModel model)
        {
            model.File = new FileInfo(model.FilePath);
            if (model.File.Exists)
            {
                // Get the file size in bytes
                long fileSizeInBytes = model.File.Length;

                // Convert bytes to kilobytes, megabytes, etc. for better readability
                double fileSizeInKB = fileSizeInBytes / 1024.0;

                Console.WriteLine($"File Size: {fileSizeInKB} KB");
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
    }
}
