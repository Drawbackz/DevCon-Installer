using System;
using System.IO;
using System.Security.Cryptography;

namespace devcon_installer
{
    public static class Algorithms
    {
        public static readonly HashAlgorithm MD5 = new MD5CryptoServiceProvider();
        public static readonly HashAlgorithm SHA1 = new SHA1Managed();
        public static readonly HashAlgorithm SHA256 = new SHA256Managed();
        public static readonly HashAlgorithm SHA384 = new SHA384Managed();
        public static readonly HashAlgorithm SHA512 = new SHA512Managed();
        public static readonly HashAlgorithm RIPEMD160 = new RIPEMD160Managed();
    }
    public static class ChecksumTool
    {
        public static string GetHashFromFile(string fileName, HashAlgorithm algorithm)
        {
            using (var stream = File.OpenRead(fileName))
            {
                return BitConverter.ToString(algorithm.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }        
    }
}
