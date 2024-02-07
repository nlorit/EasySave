using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class OpenerModel
    {
        public String LogPath { get; set; } = "logs.json";
        public String StatePath { get; set; } = "state.json";
    }
}
