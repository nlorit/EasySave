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

        public void Run(SaveModel saveModel) 

        {
            CopyService copyService = new CopyService();
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel);

            //Console.WriteLine("Save "+ model.SaveName +" running ");
        }

        public String ShowInfo(SaveModel saveModel)
        
            return "IN : "+ model.InPath + ", OUT : " + model.OutPath + ", Type : " + model.Type + ", Name : " + model.SaveName + "DateTime : " + model.Date;
        }
    }
}
