using App.Core.Models;
using System.IO;

namespace App.Core.Services
{
    public class CopyService
    {
        public void RunCopy(CopyModel model)
        {
            try 
            {
                File.Copy(model.SourcePath, model.TargetPath, true);
                
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

        }


    }
}
