using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class LoggerModel
    {
        public string Name { get; set; } = String.Empty;
        public string FileSource { get; set; } = String.Empty;
        public string FileTarget { get; set; } = String.Empty;
        public string FileSize { get; set; } = String.Empty;
        public string FileTransferTime { get; set; } = String.Empty;
        public DateTime Time { get; set; } = DateTime.Now;

    }
}
