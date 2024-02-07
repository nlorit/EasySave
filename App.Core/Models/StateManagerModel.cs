using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class StateManagerModel
    {
        public String SaveName { get; set; } = "";
        public String SourceFilePath { get; set; } = "";
        public String TargetFilePath { get; set; } = "";
        public String State { get; set; } = ""; 
        public int TotalFilesToCopy { get; set; } = 0;
        public int TotalFilesSize { get; set; } = 0;
        public int NbFilesLeftToDo { get; set; } = 0;
        public float Progression { get; set; } = 0;

    }
}
