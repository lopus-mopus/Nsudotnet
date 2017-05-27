using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Decryptor
    {
        SymmetricAlgorithm myAlgorithm;
        FileInfo inputFile;
        FileInfo keyFile;
        FileInfo outputFile;
        public Decryptor(CryptoType cryptoType, FileInfo inputFile, FileInfo keyFile, FileInfo outputFile)
        {
            switch (cryptoType)
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
            this.keyFile = keyFile;
            this.inputFile = inputFile;
            this.outputFile = outputFile;
        }
        public void Decrypt()
        {
            byte[] encrypted=null;
            using (BinaryReader dataReader = new BinaryReader(inputFile.Open(FileMode.Open)))
            {
                int len = dataReader.ReadInt32();
                encrypted = new byte[len];
                dataReader.Read(encrypted, 0, len);
            }
            using (StreamReader keyReader = new StreamReader(keyFile.Open(FileMode.Open)))
            {
                myAlgorithm.IV = Convert.FromBase64String(keyReader.ReadLine());
                myAlgorithm.Key = Convert.FromBase64String(keyReader.ReadLine());
            }
            using (StreamWriter sw = new StreamWriter(outputFile.Open(FileMode.Create)))
            {
                string decrypted = DecryptStringFromBytes_Aes(encrypted, myAlgorithm.Key, myAlgorithm.IV);
                sw.WriteLine(decrypted);
            }
        }

        string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            
            string plaintext = null;
            
                ICryptoTransform decryptor = myAlgorithm.CreateDecryptor(myAlgorithm.Key, myAlgorithm.IV);
            
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                
            return plaintext;

        }
    }
}
