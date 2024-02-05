using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class CopyService
    {
        public void Copy(CopyModel model)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = $"/c {model.RobocopyCommand}"
                }
            };

            process.Start();

            String output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);

        }

        public void CopyFile()
        {

        }

        public void CopyFolder()
        {

        }
    }
}
