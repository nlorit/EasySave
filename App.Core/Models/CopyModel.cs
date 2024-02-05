using System;

namespace App.Core.Models
{
    public class CopyModel
    {
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }

        public string RobocopyCommand
        {
            get { return $"robocopy \"{SourcePath}\" \"{TargetPath}\" /MIR"; }
        }
    }
}
