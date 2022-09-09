using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saker_Winform.CommonBaseModule
{
     public static class CByteOPerate
    {
        public static byte[] RightShift(this byte[] ba, int n)
        {
            if (n < 0)
            {
                return ba.LeftShift(Math.Abs(n));
            }
            byte[] ba2 = null;
            ba2 = ba.Clone() as byte[];
            int loop = (int)Math.Ceiling(n / 8.0);
            byte tempByte = 0;
            byte tempByte2 = 0;
            byte Header = 0;

            for (int i = 0; i < loop; i++)
            {
                var tempN = i + 1 == loop ? n % 8 : 8;
                if (tempN == 0 && n != 0)
                {
                    tempN = 8;
                }
                for (int j = 0; j < ba.Length; j++)
                {
                    if (j == 0)
                    {
                        Header = (byte)((ba2.First() & ((byte)(Math.Pow(2, tempN) - 1))) << (8 - tempN));
                        tempByte = (byte)((ba2[ba.Length - 1 - j] & ((byte)(Math.Pow(2, tempN) - 1))) << (8 - tempN));
                        ba2[ba.Length - 1 - j] >>= tempN;
                    }
                    else
                    {
                        tempByte2 = (byte)((ba2[ba.Length - 1 - j] & ((byte)(Math.Pow(2, tempN) - 1))) << (8 - tempN));
                        ba2[ba.Length - 1 - j] >>= tempN;
                        ba2[ba.Length - 1 - j] |= tempByte;
                        tempByte = tempByte2;
                        if (j + 1 == ba.Length)
                        {
                            ba2[j] |= Header;
                        }
                    }
                }
            }
            return ba2;
        }
        public static byte[] LeftShift(this byte[] ba, int n)
        {
            if (n < 0)
            {
                return ba.RightShift(Math.Abs(n));
            }
            byte[] ba2 = null;
            ba2 = ba.Clone() as byte[];
            int loop = (int)Math.Ceiling(n / 8.0);
            byte tempByte = 0;
            byte tempByte2 = 0;
            byte Header = 0;

            for (int i = 0; i < loop; i++)
            {
                var tempN = i + 1 == loop ? n % 8 : 8;
                if (tempN == 0 && n != 0)
                {
                    tempN = 8;
                }
                for (int j = 0; j < ba.Length; j++)
                {
                    if (j == 0)
                    {
                        Header = (byte)(ba2.Last() & ((byte)(Math.Pow(2, tempN) - 1) << (8 - tempN)));
                        tempByte = (byte)(ba2[j] & ((byte)(Math.Pow(2, tempN) - 1) << (8 - tempN)));
                        ba2[j] <<= tempN;
                    }
                    else
                    {
                        tempByte2 = (byte)(ba2[j] & ((byte)(Math.Pow(2, tempN) - 1) << (8 - tempN)));
                        ba2[j] <<= tempN;
                        ba2[j] |= (byte)(tempByte >> (8 - tempN));
                        tempByte = tempByte2;
                        if (j + 1 == ba.Length)
                        {
                            ba2[0] |= (byte)(Header >> (8 - tempN));
                        }
                    }
                }
            }
            return ba2;
        }
        public static byte[] BitAnd(this byte[] ba1, byte[] ba2)
        {
            if (ba1.Length != ba2.Length)
            {
                return new byte[0];
            }
            var ba3 = new byte[ba1.Length];
            for (int i = 0; i < ba3.Length; i++)
            {
                ba3[i] = (byte)((byte)ba1[i] & (byte)ba2[i]);
            }
            return ba3;

        }
        public static byte[] BitOR(this byte[] ba1, byte[] ba2)
        {
            if (ba1.Length != ba2.Length)
            {
                return new byte[0];
            }
            var ba3 = new byte[ba1.Length];
            for (int i = 0; i < ba3.Length; i++)
            {
                ba3[i] = (byte)((byte)ba1[i] | (byte)ba2[i]);
            }
            return ba3;

        }
        public static string getBinaryStrFromByte(byte b)
        {
            string result = "";
            byte a = b;
            for (int i = 0; i < 8; i++)
            {
                result = (a % 2) + result;
                a = (byte)(a >> 1);
            }
            return result;
        }
        public static string[] getBinaryStrFromByteBuffer(byte[] buffer,int offset, int count)
        {
            string[] result = new string[count/4+1];
            int k = 0;
            for (int i = offset; i < offset+ count; i++)
            {
                k = (i - offset) / 4;
                result[k] += getBinaryStrFromByte(buffer[i]);                        
            }
            return result;
        }
    }
}
