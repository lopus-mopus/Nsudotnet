using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Enigma
{
    public enum CryptoType
    {
        AES,
        DES,
        RC2,
        Default
    }
    class Crypter
    {
        SymmetricAlgorithm myAlgorithm;
        FileInfo inputFile;
        FileInfo outputFile;
        public Crypter(CryptoType cryptoType, FileInfo inputFile, FileInfo outputFile)
        {
            switch(cryptoType)
            {
                case CryptoType.AES:
                    myAlgorithm = Aes.Create();
                    break;
                case CryptoType.DES:
                    myAlgorithm = DES.Create();
                    break;
                case CryptoType.RC2:
                    myAlgorithm = RC2.Create();
                    break;
            }
            this.inputFile = inputFile;
            this.outputFile = outputFile;
        }
        public void Crypt()
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(inputFile.Open(FileMode.Open)))
            {
                while(!sr.EndOfStream)
                    sb.Append(sr.ReadLine()).Append("\n");
            }
            using (BinaryWriter bw = new BinaryWriter(outputFile.Open(FileMode.Create)))
            {
                byte[] encrypted = EncryptStringToBytes_Aes(sb.ToString(), myAlgorithm.Key, myAlgorithm.IV);
                bw.Write(encrypted.Length);
                bw.Write(encrypted);
            }
            using (StreamWriter sw = new StreamWriter((new FileInfo("file.key.txt")).Open(FileMode.Create)))
            {
                sw.WriteLine(Convert.ToBase64String(myAlgorithm.IV));
                sw.WriteLine(Convert.ToBase64String(myAlgorithm.Key));
            }
        }
        byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            ICryptoTransform encryptor = myAlgorithm.CreateEncryptor(myAlgorithm.Key, myAlgorithm.IV);
            
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            return encrypted;

        }
    }
}
