
namespace App.Core.Models
{
    public class LoggerModel
    {
        public string Name { get; set; } = "";                            // Name of the save
        public string FileSource { get; set; } = "";                      // Path of the source file
        public string FileTarget { get; set; } = "";                      // Path of the target file
        public string FileSize { get; set; } = "";                        // Size of the file
        public string FileEncryptionTime { get; set; } = "";             // Time of the encryption    
        public string FileTransferTime { get; set; } = "";                // Time of the transfer
        public DateTime Time { get; set; } = DateTime.Now;                          // Date of the save

    }
}
