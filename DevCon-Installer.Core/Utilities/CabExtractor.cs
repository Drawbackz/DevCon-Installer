using System;
using Microsoft.Deployment.Compression.Cab;

namespace DevConInstaller.Core.Utilities
{
    public class CabExtractor
    {
        public event Action ExtractionStarted;
        public event Action ExtractionCompleted;

        public void ExtractFile(string cabPath, string packedFile, string outputFilePath)
        {
            ExtractionStarted?.Invoke();
            var cabInfo = new CabInfo(cabPath);
            cabInfo.UnpackFile(packedFile, outputFilePath);
            ExtractionCompleted?.Invoke();
        }
    }
}