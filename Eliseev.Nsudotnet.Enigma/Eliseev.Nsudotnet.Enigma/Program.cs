using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CryptoType cryptoType;
                switch(args[2])
                {
                    case "AES":
                        cryptoType = CryptoType.AES;
                        break;
                    case "DES":
                        cryptoType = CryptoType.DES;
                        break;
                    case "RC2":
                        cryptoType = CryptoType.RC2;
                        break;
                    default:
                        cryptoType = CryptoType.Default;
                        break;
                }
                FileInfo inputFIle=new FileInfo(args[1]);
                FileInfo outputFile = new FileInfo(args[args.Length-1]); ;
                switch (args[0])
                {
                    case "encrypt":
                        {
                            Crypter cr = new Crypter(cryptoType, inputFIle, outputFile);
                            cr.Crypt();
                        }
                        break;
                    case "decrypt":
                        {
                            FileInfo keyFile = new FileInfo(args[3]);
                            Decryptor dcr = new Decryptor(cryptoType, inputFIle, keyFile, outputFile);
                            dcr.Decrypt();
                        }
                        break;

                    default:
                        {
                            Console.WriteLine("ERROR");
                        }
                        break;
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Передано недостаточно параметров");
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("Один из файлов не найден");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Неизвестная ошибка");
                Console.WriteLine(ex.Message);
            }
        }        
    }
}
