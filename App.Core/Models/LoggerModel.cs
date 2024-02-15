namespace App.Core.Models
{
    public class LoggerModel
    {
        public string Name { get; set; } = string.Empty;                            // Name of the save
        public string FileSource { get; set; } = string.Empty;                      // Path of the source file
        public string FileTarget { get; set; } = string.Empty;                      // Path of the target file
        public string FileSize { get; set; } = string.Empty;                        // Size of the file
        public string FileTransferTime { get; set; } = string.Empty;                // Time of the transfer
        public string FileEncryptionTime { get; set; } = string.Empty;                // Time of the encryption
        public DateTime Time { get; set; } = DateTime.Now;                          // Date of the save

    }
}
