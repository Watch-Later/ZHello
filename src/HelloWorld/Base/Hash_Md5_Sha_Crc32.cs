#pragma warning disable CS3003,CS3002,CS3001

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//using System.IO;
//using System.Security;

namespace ZHello.Base
{
    /// <summary>
    /// ����Hash md5 sha crc32ֵ����
    /// </summary>
    public static class Hash_Md5_Sha_Crc32
    {
        #region Expand Function

        /// <summary>
        /// ��ȡ�ַ�����n���ַ���ת��ΪASCII
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte CharAt(this string str, int index)
        {
            Contract.Requires(index >= 0 && str != null && index < str.Length);
            char c = '\0';
            try
            {
                if (index < str.ToCharArray().Length)
                {
                    c = str.ToCharArray()[index];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            byte b = c.ToByte_ASCII();
            return b;
        }

        public static byte ToByte_ASCII(this char c)
        {
            return Encoding.ASCII.GetBytes(c.ToString())[0];
        }

        public static byte ToByte_Unicode(this char c)
        {
            return Encoding.Unicode.GetBytes(c.ToString())[0];
        }

        public static byte ToByte_UTF8(this char c)
        {
            return Encoding.UTF8.GetBytes(c.ToString())[0];
        }

        /// <summary>
        /// ��ȡ�ַ�����MD5ֵ
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="salt">����ֵ</param>
        /// <returns></returns>
        public static string MD5(this string str, string salt = "")
        {
            //MD5������
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytValue, bytHash;
                //��Ҫ������ַ���ת��Ϊ�ֽ�����
                bytValue = Encoding.UTF8.GetBytes(salt + str);
                //������ͬ�����ֽ�����
                bytHash = md5.ComputeHash(bytValue);
                //���ֽ�����ת��Ϊ�ַ���
                string sTemp = "";
                for (int i = 0; i < bytHash.Length; i++)
                {
                    sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
                }
                return sTemp;
            }
        }

        #endregion Expand Function
    }

    /// <summary>
    /// ͨ��Hashֵ����
    /// </summary>
    public static class GeneralHashAlgorithm
    {
        /*RSHash*/

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long RSHash(string str)
        {
            int b = 378551;
            int a = 63689;
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = hash * a + str.CharAt(i);
                a = a * b;
            }
            return hash;
        }

        /*JSHash*/

        public static long JSHash(string str)
        {
            long hash = 1315423911;
            for (int i = 0; i < str.Length; i++)
                hash ^= ((hash << 5) + str.CharAt(i) + (hash >> 2));
            return hash;
        }

        /*PJWHash*/

        public static long PJWHash(string str)
        {
            int BitsInUnsignedInt = (int)(4 * 8);
            int ThreeQuarters = (int)Math.Ceiling((decimal)((BitsInUnsignedInt * 3) / 4)) + 1;
            int OneEighth = (int)Math.Ceiling((decimal)(BitsInUnsignedInt / 8)) + 1;
            var HighBits = (0xFFFFFFFF) << (BitsInUnsignedInt - OneEighth);
            long hash = 0;
            long test = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = (hash << OneEighth) + str.CharAt(i);
                if ((test = hash & HighBits) != 0)
                    hash = ((hash ^ (test >> ThreeQuarters)) & (~HighBits));
            }
            return hash;
        }

        /*ELFHash*/

        public static long ELFHash(string str)
        {
            long hash = 0;
            long x = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = (hash << 4) + str.CharAt(i);
                if ((x = hash & 0xF0000000L) != 0)
                    hash ^= (x >> 24);
                hash &= ~x;
            }
            return hash;
        }

        /*BKDRHash*/

        public static long BKDRHash(string str)
        {
            long seed = 131;//31131131313131131313etc..
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
                hash = (hash * seed) + str.CharAt(i);
            return hash;
        }

        /*SDBMHash*/

        public static long SDBMHash(string str)
        {
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
                hash = str.CharAt(i) + (hash << 6) + (hash << 16) - hash;
            return hash;
        }

        /*DJBHash*/

        public static long DJBHash(string str)
        {
            long hash = 5381;
            for (int i = 0; i < str.Length; i++)
                hash = ((hash << 5) + hash) + str.CharAt(i);
            return hash;
        }

        /*DEKHash*/

        public static long DEKHash(string str)
        {
            long hash = str.Length;
            for (int i = 0; i < str.Length; i++)
                hash = ((hash << 5) ^ (hash >> 27)) ^ str.CharAt(i);
            return hash;
        }

        /*BPHash*/

        public static long BPHash(string str)
        {
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
                hash = hash << 7 ^ str.CharAt(i);
            return hash;
        }

        /*FNVHash*/

        public static long FNVHash(string str)
        {
            long fnv_prime = 0x811C9DC5;
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash *= fnv_prime;
                hash ^= str.CharAt(i);
            }
            return hash;
        }

        /*APHash*/

        public static long APHash(string str)
        {
            long hash = 0xAAAAAAAA;
            for (int i = 0; i < str.Length; i++)
            {
                if ((i & 1) == 0)
                    hash ^= ((hash << 7) ^ str.CharAt(i) ^ (hash >> 3));
                else
                    hash ^= (~((hash << 11) ^ str.CharAt(i) ^ (hash >> 5)));
            }
            return hash;
        }
    }

    /// <summary>
    /// Md5����
    /// </summary>
    public static class MD5Lib
    {
        private static string Md5Encrypt(string input)
        {
            //��������MD5ֵ�Ķ���
            using (MD5 md5Hash = MD5.Create())
            {
                //��ȡ�ַ�����Ӧ��byte���飬����MD5ֵ
                byte[] md5Byts = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));
                //����һ���µ�Stringbuilder���ռ����ֽںʹ���һ���ַ���
                StringBuilder sb = new StringBuilder();
                //ѭ������ÿ���ֽڵ�ɢ�е����ݺ�ÿһ��ʮ�����Ƹ�ʽ�ַ���

                for (int i = 0; i < md5Byts.Length; i++)
                {
                    //"x"��ʾ16���ƣ�2��ʾ������λ������2����>02
                    sb.Append(md5Byts[i].ToString("x2"));
                }
                //����ʮ�������ַ�����
                return sb.ToString();
            }
        }

        /// <summary>
        /// ����32λMD5��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_MD5_32(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP
                     = new MD5CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                //���ݼ���õ���Hash�뷭��ΪMD5��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ����16λMD5��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_MD5_16(string word, bool toUpper = true)
        {
            try
            {
                string sHash = Hash_MD5_32(word).Substring(8, 16);
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ����32λ2��MD5��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_2_MD5_32(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP
                    = new MD5CryptoServiceProvider();

                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);

                //���ݼ���õ���Hash�뷭��ΪMD5��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                bytValue = Encoding.UTF8.GetBytes(sHash);
                bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                sHash = "";

                //���ݼ���õ���Hash�뷭��ΪMD5��
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ����16λ2��MD5��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_2_MD5_16(string word, bool toUpper = true)
        {
            try
            {
                MD5CryptoServiceProvider MD5CSP
                        = new MD5CryptoServiceProvider();

                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);

                //���ݼ���õ���Hash�뷭��ΪMD5��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                sHash = sHash.Substring(8, 16);

                bytValue = Encoding.UTF8.GetBytes(sHash);
                bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                sHash = "";

                //���ݼ���õ���Hash�뷭��ΪMD5��
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                sHash = sHash.Substring(8, 16);

                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    /// <summary>
    /// Sha����
    /// </summary>
    public static class SHALib
    {
        /// <summary>
        /// ����SHA-1��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_SHA_1(string word, bool toUpper = true)
        {
            try
            {
                SHA1CryptoServiceProvider SHA1CSP
                    = new SHA1CryptoServiceProvider();

                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA1CSP.ComputeHash(bytValue);
                SHA1CSP.Clear();

                //���ݼ���õ���Hash�뷭��ΪSHA-1��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ����SHA-256��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_SHA_256(string word, bool toUpper = true)
        {
            try
            {
                SHA256CryptoServiceProvider SHA256CSP
                    = new SHA256CryptoServiceProvider();

                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA256CSP.ComputeHash(bytValue);
                SHA256CSP.Clear();

                //���ݼ���õ���Hash�뷭��ΪSHA-1��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ����SHA-384��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_SHA_384(string word, bool toUpper = true)
        {
            try
            {
                SHA384CryptoServiceProvider SHA384CSP = new SHA384CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA384CSP.ComputeHash(bytValue);
                SHA384CSP.Clear();
                //���ݼ���õ���Hash�뷭��ΪSHA-1��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ����SHA-512��
        /// </summary>
        /// <param name="word">�ַ���</param>
        /// <param name="toUpper">���ع�ϣֵ��ʽ true��Ӣ�Ĵ�д��false��Ӣ��Сд</param>
        /// <returns></returns>
        public static string Hash_SHA_512(string word, bool toUpper = true)
        {
            try
            {
                SHA512CryptoServiceProvider SHA512CSP = new SHA512CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA512CSP.ComputeHash(bytValue);
                SHA512CSP.Clear();

                //���ݼ���õ���Hash�뷭��ΪSHA-1��
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Length; counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }

                //���ݴ�Сд����������ص��ַ���
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// �����ļ��� SHA256 ֵ
        /// </summary>
        /// <param name="fileStream">�ļ���</param>
        /// <returns>System.String.</returns>
        public static string SHA256File(FileStream fileStream)
        {
            SHA256 mySHA256 = SHA256Managed.Create();

            byte[] hashValue;

            // Create a fileStream for the file.
            //FileStream fileStream = fInfo.Open(FileMode.Open);
            // Be sure it's positioned to the beginning of the stream.
            fileStream.Position = 0;
            // Compute the hash of the fileStream.
            hashValue = mySHA256.ComputeHash(fileStream);

            // Close the file.
            fileStream.Close();
            // Write the hash value to the Console.
            return FileMd5ShaCrc32.PrintByteArray(hashValue);
        }
    }

    /// <summary>
    /// �����ļ���Md5��Sha��Crc32
    /// </summary>
    public static class FileMd5ShaCrc32
    {
        /// <summary>
        /// �����ļ��� MD5 ֵ
        /// </summary>
        /// <param name="fileName">Ҫ���� MD5 ֵ���ļ�����·��</param>
        /// <returns>MD5 ֵ16�����ַ���</returns>
        public static string MD5File(string fileName)
        {
            return HashFile(fileName, "md5");
        }

        /// <summary>
        /// �����ļ��� sha1 ֵ
        /// </summary>
        /// <param name="fileName">Ҫ���� sha1 ֵ���ļ�����·��</param>
        /// <returns>sha1 ֵ16�����ַ���</returns>
        public static string SHA1File(string fileName)
        {
            return HashFile(fileName, "sha1");
        }

        /// <summary>
        /// �����ļ��Ĺ�ϣֵ
        /// </summary>
        /// <param name="fileName">Ҫ�����ϣֵ���ļ�����·��</param>
        /// <param name="algName">�㷨:sha1,md5</param>
        /// <returns>��ϣֵ16�����ַ���</returns>
        private static string HashFile(string fileName, string algName)
        {
            if (!File.Exists(fileName))
                return string.Empty;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// �����ϣֵ
        /// </summary>
        /// <param name="stream">Ҫ�����ϣֵ�� Stream</param>
        /// <param name="algName">�㷨:sha1,md5</param>
        /// <returns>��ϣֵ�ֽ�����</returns>
        private static byte[] HashData(Stream stream, string algName)
        {
            HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName ����Ϊ null");
            }
            if (string.Compare(algName, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName ֻ��ʹ�� sha1 �� md5");
                }
                algorithm = MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary>
        /// �ֽ�����ת��Ϊ16���Ʊ�ʾ���ַ���
        /// </summary>
        public static string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }

        public static string PrintByteArray(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            for (i = 0; i < array.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", array[i]));
            }
            return sb.ToString();
        }

        /// <summary>
        ///  ����ָ���ļ���CRC32ֵ
        /// </summary>
        /// <param name="fileName">ָ���ļ�����ȫ�޶�����</param>
        /// <returns>����ֵ���ַ�����ʽ</returns>
        public static String Crc32File(String fileName)
        {
            String hashCRC32 = String.Empty;
            //����ļ��Ƿ���ڣ�����ļ���������м��㣬���򷵻ؿ�ֵ
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //�����ļ���CSC32ֵ
                    Crc32 calculator = new Crc32();
                    Byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //���ֽ�����ת����ʮ�����Ƶ��ַ�����ʽ
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X2"));
                    }
                    hashCRC32 = stringBuilder.ToString();
                }//�ر��ļ���
            }
            return hashCRC32;
        }
    }

    /// <summary>
    /// �ṩ CRC32 �㷨��ʵ��
    ///
    /// </summary>
    public class Crc32 : HashAlgorithm
    {
        public const uint DefaultPolynomial = 0xedb88320;
        public const uint DefaultSeed = 0xffffffff;
        private uint hash;
        private uint seed;
        private uint[] table;
        private static uint[] defaultTable;

        public Crc32()
        {
            table = InitializeTable(DefaultPolynomial);
            seed = DefaultSeed;
            Initialize();
        }

        public Crc32(uint polynomial, uint seed)
        {
            table = InitializeTable(polynomial);
            this.seed = seed;
            Initialize();
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            hash = CalculateHash(table, hash, buffer, start, length);
        }

        protected override byte[] HashFinal()
        {
            byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public static UInt32 Compute(byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        private static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
            {
                return defaultTable;
            }
            UInt32[] createTable = new UInt32[256];
            for (int i = 0; i < 256; i++)
            {
                UInt32 entry = (UInt32)i;
                for (int j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                }
                createTable[i] = entry;
            }
            if (polynomial == DefaultPolynomial)
            {
                defaultTable = createTable;
            }
            return createTable;
        }

        private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
        {
            UInt32 crc = seed;
            for (int i = start; i < size; i++)
            {
                unchecked
                {
                    crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                }
            }
            return crc;
        }

        private byte[] UInt32ToBigEndianBytes(UInt32 x)
        {
            return new byte[] { (byte)((x >> 24) & 0xff), (byte)((x >> 16) & 0xff), (byte)((x >> 8) & 0xff), (byte)(x & 0xff) };
        }
    }
}