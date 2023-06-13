using System;
using System.Security.Cryptography;
using System.Text;

namespace Classes.BL
{
    public sealed class GeneratePassword
    {
        private static char[] hexDigits = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };

        public static string GetPassword(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for (int index = 0; index < length; ++index)
            {
                if (random.Next(6) > 3)
                    stringBuilder.Append(Convert.ToChar(random.Next(97, 122)));
                else if (random.Next(5) > 2)
                    stringBuilder.Append(Convert.ToChar(random.Next(48, 57)));
                else
                    stringBuilder.Append(Convert.ToChar(random.Next(65, 90)));
            }
            return stringBuilder.ToString();
        }

        public static string ToHexString(byte[] bytes)
        {
            char[] chArray = new char[bytes.Length * 2];
            for (int index = 0; index < bytes.Length; ++index)
            {
                int num = (int)bytes[index];
                chArray[index * 2] = GeneratePassword.hexDigits[num >> 4];
                chArray[index * 2 + 1] = GeneratePassword.hexDigits[num & 15];
            }
            return new string(chArray);
        }

        public static string EncryptPassword(string strPlainText)
        {
            return GeneratePassword.ToHexString(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(strPlainText)));
        }
    }
}