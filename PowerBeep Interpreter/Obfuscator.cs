using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBeep
{
    public static class Obfuscator
    {
        public static string ToHexadecimal(string Target)
        {
            StringBuilder Builder = new StringBuilder();

            byte[] Bytes = Encoding.Unicode.GetBytes(Target);
            for (int i = 0; i < Bytes.Length; i++)
            {
                Builder.Append(Bytes[i].ToString("X2"));
            }

            return Builder.ToString();
        }

        public static string ToString(string Target)
        {
            byte[] Bytes = new byte[(Target.Length / 2)];
            for (int i = 0; i < Bytes.Length; i++) {
                Bytes[i] = Convert.ToByte(Target.Substring((i * 2), 2), 16);
            }

            return Encoding.Unicode.GetString(Bytes);
        }
    }
}
