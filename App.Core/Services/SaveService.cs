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
        {
            return "IN : "+ saveModel.InPath + ", OUT : " + saveModel.OutPath + ", Type : " + saveModel.Type + ", Name : " + saveModel.SaveName;
        }
    }
}
