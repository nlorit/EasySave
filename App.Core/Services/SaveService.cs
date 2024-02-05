using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class SaveService
    {
        public void Create(SaveModel model) 
        { 
            Console.WriteLine("Save created");
        }

        public void Run(SaveModel model) 

        {
            CopyService copyService = new CopyService();
            copyService.RunCopy(new CopyModel { SourcePath = model.InPath, TargetPath = model.OutPath });

            Console.WriteLine("Save "+ model.SaveName +" running ");
        }

        public String ShowInfo(SaveModel model)
        {
            return "IN : "+ model.InPath + ", OUT : " + model.OutPath + ", Type : " + model.Type + ", Name : " + model.SaveName;
        }
    }
}
