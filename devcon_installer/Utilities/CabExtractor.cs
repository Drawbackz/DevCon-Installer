using System;
using Microsoft.Deployment.Compression.Cab;

namespace devcon_installer.Utilities
{
    public class CabExtractor
    {
        public event Action ExtractionStarted;
        public event Action ExtractionCompleted;

        public void ExtractFile(string cabPath, string packedFile, string outputFilePath)
        {
            SendExtractionStarted();
            var cabInfo = new CabInfo(cabPath);
            cabInfo.UnpackFile(packedFile, outputFilePath);
            SendExtractionCompleted();
        }

        private void SendExtractionStarted()
        {
            ExtractionStarted?.Invoke();
        }

        private void SendExtractionCompleted()
        {
            ExtractionCompleted?.Invoke();
        }
    }
}