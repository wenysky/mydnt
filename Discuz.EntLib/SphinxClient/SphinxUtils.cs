using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Discuz.EntLib.SphinxClient
{
    /// <summary>
    /// Sphinx工具类
    /// </summary>
    class SphinxUtils
    {
        #region 网络工具方法
        public static string ReadUtf8(BinaryReader sw)
        {
            int length = ReadInt32(sw);

            if (length > 0)
            {
                byte[] data = sw.ReadBytes(length);
                return Encoding.UTF8.GetString(data);
            }
            return "";
        }

        public static short ReadInt16(BinaryReader br)
        {
            byte[] idata = br.ReadBytes(2);

            return BitConverter.ToInt16(_Reverse(idata), 0);
        }
        public static int ReadInt32(BinaryReader br)
        {
            byte[] idata = br.ReadBytes(4);

            return BitConverter.ToInt32(_Reverse(idata), 0);
        }
        public static uint ReadUInt32(BinaryReader br)
        {
            byte[] idata = br.ReadBytes(4);

            return BitConverter.ToUInt32(_Reverse(idata), 0);
        }
        public static Int64 ReadInt64(BinaryReader br)
        {
            byte[] idata = br.ReadBytes(8);

            return BitConverter.ToInt64(_Reverse(idata), 0);
        }

        public static void WriteToStream(BinaryWriter sw, short data)
        {
            byte[] d = BitConverter.GetBytes(data);
            sw.Write(_Reverse(d));
        }
        public static void WriteToStream(BinaryWriter sw, int data)
        {
            byte[] d = BitConverter.GetBytes(data);
            sw.Write(_Reverse(d));
        }
        public static void WriteToStream(BinaryWriter sw, float data)
        {
            byte[] d = BitConverter.GetBytes(data);
            sw.Write(_Reverse(d));
        }
        public static void WriteToStream(BinaryWriter sw, byte[] data)
        {
            sw.Write(data);
        }
        public static void WriteToStream(BinaryWriter sw, string data)
        {
            byte[] d = Encoding.UTF8.GetBytes(data);
            WriteToStream(sw, d.Length);
            sw.Write(d);
        }

        public static byte[] _Reverse(byte[] data)
        {
            int len = data.Length;
            for (long i = 0; i < len / 2; ++i)
            {
                byte t;
                t = data[i];
                data[i] = data[len - i - 1];
                data[len - i - 1] = t;
            }

            return data;
        }
        #endregion
    }

    /// <summary>
    /// Provides a set of static methods for DateTime operations. This class cannot be inherited. 
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Unix epoch start date (lower boundary)
        /// </summary>
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Unix Millennium problem date (upper boundary)
        /// </summary>
        private static readonly DateTime _epochLimit = new DateTime(2038, 1, 19, 3, 14, 7, 0, DateTimeKind.Utc);

        /// <summary>
        /// Convert DateTime object to Unix timestamp signed integer value. 
        /// </summary>
        /// <param name="dateTime">DateTime value to convert</param>
        /// <returns>signed integer value, representing Unix timestamp (represented in UTC) converted from specifed DateTime value</returns>
        /// <exception cref="ArgumentOutOfRangeException">DateTime value can't be converted to Unix timestamp due out of signed int range</exception>
        public static int ConvertToUnixTimestamp(DateTime dateTime)
        {
            // ArgumentAssert.IsInRange(dateTime, _epoch, _epochLimit, Messages.Exception_ArgumentDateTimeOutOfRangeUnixTimestamp);
            TimeSpan diff = dateTime.ToUniversalTime() - _epoch;
            return Convert.ToInt32(Math.Floor(diff.TotalSeconds));
        }

        /// <summary>
        /// Convert Unix timestamp integer value to DateTime object.
        /// </summary>
        /// <param name="timestamp">signed integer value, specifies Unix timestamp</param>
        /// <returns>DateTime object (represented in local time) based on specified Unix timestamp value</returns>
        /// <exception cref="ArgumentOutOfRangeException">Unix timestamp value can't be converted to DateTime due out of signed int range</exception>
        public static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            // ArgumentAssert.IsGreaterOrEqual(timestamp, 0, Messages.Exception_ArgumentUnixTimestampOutOfRange);
            return (_epoch + TimeSpan.FromSeconds(timestamp)).ToLocalTime();
        }
    }
}
