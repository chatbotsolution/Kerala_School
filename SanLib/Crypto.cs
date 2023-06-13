using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SanLib
{
    public class Crypto
    {
        public static string DecryptText(string strText)
        {
            return Crypto.Decrypt(strText, "&%#@?,:*");
        }

        public static string EncryptText(string strText)
        {
            return Crypto.Encrypt(strText, "&%#@?,:*");
        }

        public static string Encrypt(string strText, string strEncrKey)
        {
            byte[] numArray = new byte[0];
            byte[] rgbIV = new byte[8]
      {
        (byte) 18,
        (byte) 52,
        (byte) 86,
        (byte) 120,
        (byte) 144,
        (byte) 171,
        (byte) 205,
        (byte) 239
      };
            try
            {
                byte[] bytes1 = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] bytes2 = Encoding.UTF8.GetBytes(strText);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateEncryptor(bytes1, rgbIV), CryptoStreamMode.Write);
                cryptoStream.Write(bytes2, 0, bytes2.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Decrypt(string strText, string sDecrKey)
        {
            byte[] numArray1 = new byte[0];
            byte[] rgbIV = new byte[8]
      {
        (byte) 18,
        (byte) 52,
        (byte) 86,
        (byte) 120,
        (byte) 144,
        (byte) 171,
        (byte) 205,
        (byte) 239
      };
            byte[] numArray2 = new byte[strText.Length + 1];
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] buffer = Convert.FromBase64String(strText);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
