using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class SaveModel
    {
        public String InPath { get; set; } = "";
        public String OutPath { get; set; } = "";
        public bool Type { get; set; } = false; // false = Complete, true = Sequentiel
        public String SaveName { get; set; } = "";
        public DateTime Date { get; set; }
        public StateManagerModel StateManager { get; set; } = new StateManagerModel();
    }
}
