using System;
using System.IO;
using System.Security.Cryptography;

namespace DevConInstaller.Core.Utilities
{
    public static class ChecksumTool
    {
        public static string GetHashFromFile(string fileName, HashAlgorithm algorithm)
        {
            using (var stream = File.OpenRead(fileName))
            {
                return BitConverter.ToString(algorithm.ComputeHash(stream)).Replace("-", string.Empty).ToUpper();
            }
        }
    }
}