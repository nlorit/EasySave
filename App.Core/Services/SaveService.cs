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
            copyService.Copy(new CopyModel { sourcePath = model.IN_PATH, targetPath = model.OUT_PATH });

            Console.WriteLine("Save "+ model.SAVE_NAME +" running ");
        }

        public String ShowInfo(SaveModel model)
        {
            return "IN : "+ model.IN_PATH + ", OUT : " + model.OUT_PATH + ", Type : " + model.TYPE + ", Name : " + model.SAVE_NAME;
        }
    }
}
