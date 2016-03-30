using System;

namespace HandyComicNamer
{
    public static class Convert
    {
        static int HexaDecCharToDec(char ch)
        {
            if ('0'<=ch && ch<='9')
                return ch - '0';
            else if ('A'<=ch && ch <='F')
                return ch - 'A' + 10;
            else if ('a'<=ch && ch <='f')
                return ch - 'a' + 10;
            else
                return 0;
        }
        
        public static string URIToURL (string uri)
        {
            string url = null;
            if (uri != null) {
                url = uri.Substring (8);
            }
            return url;
        }
        public static string ToUTF8String (byte[] bytes)
        {
            byte pcnt = (byte)'%';
            byte[] b = new byte[bytes.Length+1];
            
            int i, j;
            for (i=0, j=0; i+j<bytes.Length; ++i) {
                if (pcnt==bytes[i+j])
                {
                    b[i] = (byte)
                        (16 * HexaDecCharToDec((char) bytes[i+j+1])
                         + HexaDecCharToDec((char) bytes[i+j+2]));
                    j += 2;
                }
                else
                {
                    b[i] = bytes[i+j];
                }
            }
            b[i] = 0;
            
            return System.Text.Encoding.UTF8.GetString (b);
        }
        public static string[] ToConvertedPathArray (byte[] bytes)
        {
            string str = Convert.ToUTF8String (bytes);
            string[] temp = {"\r\n"};
            string[] paths = str.Split (temp, StringSplitOptions.None);

            paths = Convert.NullDelete (paths);
            for (int i=0; i<paths.Length; ++i)
                paths[i] = Convert.URIToURL (paths[i]);

            return paths;
        }
        static string[] NullDelete(string[] tpaths)
        {
            string[] paths = new string [tpaths.Length-1];
            
            for (int i=0; i<tpaths.Length-1; ++i)
                paths[i] = tpaths[i];
            
            return paths;
        }
    }
}