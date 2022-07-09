using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoginForms
{
    public class Encrypt
    {

        private static readonly string passPhrase = "PasswordPromoEsp";
        private static byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private static int BlockSize = 128;

        //public static string GetAES256(string str) {
        //    if (str == "") return "String vacio";


        //    byte[] bytes = Encoding.Unicode.GetBytes(str);
        //    SymmetricAlgorithm crypt = Aes.Create();
        //    HashAlgorithm hash = MD5.Create();
        //    var key = hash.ComputeHash(Encoding.Unicode.GetBytes(pass));

        //    crypt.BlockSize = BlockSize;
        //    crypt.Key = key;
        //    crypt.IV = IV;

            

        //    using (MemoryStream memoryStream = new MemoryStream()) {
        //        using (CryptoStream cryptoStream =
        //            new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write)) {
        //            cryptoStream.Write(bytes, 0, bytes.Length);
        //        }

        //        string encrypted = Convert.ToBase64String(memoryStream.ToArray());
        //        Console.WriteLine("\n***************\nEste es el login encriptado: "+encrypted);
        //        return encrypted;
        //    }

        //}

        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return "";
            }
            // generate salt
            byte[] key, iv;
            var salt = new byte[8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);
            DeriveKeyAndIv(passPhrase, salt, out key, out iv);
            // encrypt bytes
            var encryptedBytes = EncryptStringToBytesAes(plainText, key, iv);
            // add salt as first 8 bytes
            var encryptedBytesWithSalt = new byte[salt.Length + encryptedBytes.Length + 8];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes("Salted__"), 0, encryptedBytesWithSalt, 0, 8);
            Buffer.BlockCopy(salt, 0, encryptedBytesWithSalt, 8, salt.Length);
            Buffer.BlockCopy(encryptedBytes, 0, encryptedBytesWithSalt, salt.Length + 8, encryptedBytes.Length);
            // base64 encode

            var temp = Convert.ToBase64String(encryptedBytesWithSalt);
            //Console.WriteLine("\n*********************\nLa cadena encriptada se ve asi:" +temp);
            return temp;
        }

        private static void DeriveKeyAndIv(string passPhrase, byte[] salt, out byte[] key, out byte[] iv)
        {
            // generate key and iv
            var concatenatedHashes = new List<byte>(48);
            var password = Encoding.UTF8.GetBytes(passPhrase);
            var currentHash = new byte[0];
            var md5 = MD5.Create();
            bool enoughBytesForKey = false;
            // See http://www.openssl.org/docs/crypto/EVP_BytesToKey.html#KEY_DERIVATION_ALGORITHM
            while (!enoughBytesForKey)
            {
                var preHashLength = currentHash.Length + password.Length + salt.Length;
                var preHash = new byte[preHashLength];
                Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
                Buffer.BlockCopy(password, 0, preHash, currentHash.Length, password.Length);
                Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + password.Length, salt.Length);
                currentHash = md5.ComputeHash(preHash);
                concatenatedHashes.AddRange(currentHash);
                if (concatenatedHashes.Count >= 48)
                    enoughBytesForKey = true;
            }
            key = new byte[32];
            iv = new byte[16];
            concatenatedHashes.CopyTo(0, key, 0, 32);
            concatenatedHashes.CopyTo(32, iv, 0, 16);
            md5.Clear();
        }

        static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt;
            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;
            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush();
                        swEncrypt.Close();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                aesAlg?.Clear();
            }
            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }


    }
}
