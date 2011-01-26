using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Discuz.EntLib.TokyoTyrant
{
    public static class ProtocolHelper
    {
        public static int AsBigEndian(this int value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;
            else
                return ProtocolHelper.SwapInt32(value);
        }

        public static Int64 AsBigEndian(this Int64 value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;
            else
                return ProtocolHelper.SwapInt64(value);
        }

        public static string ReadUTF8String(this BinaryReader reader, int length)
        {
            var result = "";
            byte[] buf = new byte[length];
            var read = 0;
            var readCnt = 0;
            do
            {
                read = length - readCnt;
                read = reader.Read(buf, 0, read);
                result += System.Text.Encoding.UTF8.GetString(buf, 0, read);
                readCnt += read;
            } while (readCnt < length);
            return result;
        }


        public static short SwapInt16(short v)
        {
            return (short)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }

        public static ushort SwapUInt16(ushort v)
        {
            return (ushort)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }

        public static int SwapInt32(int v)
        {
            return (int)(((SwapInt16((short)v) & 0xffff) << 0x10) |
                          (SwapInt16((short)(v >> 0x10)) & 0xffff));
        }

        public static uint SwapUInt32(uint v)
        {
            return (uint)(((SwapUInt16((ushort)v) & 0xffff) << 0x10) |
                           (SwapUInt16((ushort)(v >> 0x10)) & 0xffff));
        }

        public static long SwapInt64(long v)
        {
            return (long)(((SwapInt32((int)v) & 0xffffffffL) << 0x20) |
                           (SwapInt32((int)(v >> 0x20)) & 0xffffffffL));
        }

        public static ulong SwapUInt64(ulong v)
        {
            return (ulong)(((SwapUInt32((uint)v) & 0xffffffffL) << 0x20) |
                            (SwapUInt32((uint)(v >> 0x20)) & 0xffffffffL));
        }
    }
}
