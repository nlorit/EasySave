using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class SaveModel
    {
        public String IN_PATH { get; set; } = "";
        public String OUT_PATH { get; set; } = "";
        public bool TYPE { get; set; } = false; // false = Complete, true = Sequentiel
        public String SAVE_NAME { get; set; } = "";

    }
}
