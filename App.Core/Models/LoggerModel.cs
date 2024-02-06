using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class LoggerModel
    {
        public String Name { get; set; } = "";
        public String FileSource { get; set; } = "";
        public String FileTarget { get; set; } = "";
        public String FileSize { get; set; } = "";
        public String FileTransferTime { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;

    }
}
