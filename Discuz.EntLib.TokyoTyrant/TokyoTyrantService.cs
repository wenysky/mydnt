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
    /// <summary>
    /// TokyoTyrant客户端
    /// </summary>
    public class TokyoTyrantService
    {
        /// <summary>
        /// 存储单一键值(key/value)对记录. 
        /// 如数据库中已有该键值，则"overwrite"为"true"时用新值进行覆盖.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="overwrite">如为false,该方法将会抛出异常标示该键已经存在</param>
        public static void Put(TcpClientIOPool pool, string key, string value, bool overwrite)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((overwrite ? (byte)Command.PUT : (byte)Command.PUTKEEP));
                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    byte[] valuebytes = System.Text.Encoding.UTF8.GetBytes(value);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(valuebytes.Length.AsBigEndian());
                    writer.Write(keybytes);
                    writer.Write(valuebytes);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        ///将value追加到已经存在的key原值之后, 第二个参数只有在key是字符串时有效, 如果记录不存在, 创建新的记录. 
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Concatenate(TcpClientIOPool pool, string key, string value)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.PUTCAT);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    byte[] valuebytes = System.Text.Encoding.UTF8.GetBytes(value);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(valuebytes.Length.AsBigEndian());
                    writer.Write(keybytes);
                    writer.Write(valuebytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        /// 连接一条记录并自左端开始截掉width个字符. 如果记录不存在, 创建新的记录.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="width"></param>
        public static void ConcatenateShiftLeft(TcpClientIOPool pool, string key, string value, int width)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.PUTSHL);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    byte[] valuebytes = System.Text.Encoding.UTF8.GetBytes(value);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(valuebytes.Length.AsBigEndian());
                    writer.Write(width.AsBigEndian());
                    writer.Write(keybytes);
                    writer.Write(valuebytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        /// 快速存储键值对(不再获取服务端返回信息). 如键值已存在则将被覆盖
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void PutFast(TcpClientIOPool pool, string key, string value)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.PUTNR);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    byte[] valuebytes = System.Text.Encoding.UTF8.GetBytes(value);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(valuebytes.Length.AsBigEndian());
                    writer.Write(keybytes);
                    writer.Write(valuebytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        tcpClientIO.Write(m.ToArray(), 0, (int)m.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定键的记录.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        public static void Delete(TcpClientIOPool pool, string key)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.OUT);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(keybytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }


        /// <summary>
        /// 获取指定键的值(string)
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <returns>返回值(string)</returns>
        public static string Get(TcpClientIOPool pool, string key)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.GET);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(keybytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int valSize = reader.ReadInt32().AsBigEndian();
                            if (valSize >= 0)
                                return reader.ReadUTF8String(valSize);
                            else
                                throw new TTReceiveException();
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定键的整型记录.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <returns>返回整型记录值</returns>
        public static int GetInteger(TcpClientIOPool pool, string key)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.GET);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(keybytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int valSize = reader.ReadInt32().AsBigEndian();
                            if (valSize == 4)
                                return reader.ReadInt32();
                            else
                                throw new TTReceiveException();
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// 获取(组)键列表的多条记录
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="keys">键</param>
        /// <returns>返回记录集(dictionary类)</returns>
        public static IDictionary<string, string> GetMultiple(TcpClientIOPool pool, string[] keys)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.MGET);
                    writer.Write(keys.Length.AsBigEndian());
                    foreach (var key in keys)
                    {
                        byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                        writer.Write(keybytes.Length.AsBigEndian());
                        writer.Write(keybytes);
                    }
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int rnum = reader.ReadInt32().AsBigEndian();
                            if (rnum < 0) throw new TTReceiveException();
                            for (int i = 0; i < rnum; i++)
                            {
                                int keySize = reader.ReadInt32().AsBigEndian();
                                int valSize = reader.ReadInt32().AsBigEndian();
                                if (keySize > 0 && valSize > 0)
                                {
                                    var keyStr = reader.ReadUTF8String(keySize);
                                    var valStr = reader.ReadUTF8String(valSize);
                                    result.Add(keyStr, valStr);
                                }
                                else if (keySize < 0 || valSize < 0)
                                    throw new TTReceiveException();
                            }
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// 获取记录值的大小
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <returns>获取指定记录的大小</returns>
        public static int GetSize(TcpClientIOPool pool, string key)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.VSIZ);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(keybytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                            return reader.ReadInt32().AsBigEndian();
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化iterator.
        /// 用于按键遍历数据库中的每条记录
        /// </summary>
        /// <param name="pool">链接池</param>
        public static void IteratorInitialize(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.ITERINIT);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前iterator的下一条记录
        /// 如成功，则返回值是下一个键，否则为空null（遍历结束）. 
        /// It is possible to access every record by iteration of calling this method.  
        /// It is allowed to update or remove records whose keys are fetched while the iteration. 
        /// However, it is not assured if updating the database is occurred while the iteration. 
        /// Besides, the order of this traversal access method is arbitrary, so it is not assured that the order of storing matches the one of the traversal access.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <returns>键信息或空(无记录时)</returns>
        public static string IteratorNext(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.ITERNEXT);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int keySize = reader.ReadInt32().AsBigEndian();
                            if (keySize <= 0)
                                return null;
                            else
                                return reader.ReadUTF8String(keySize);
                        }
                        else
                            return null;
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定前缀的键.该操作要扫描整个数据库可能执行很慢
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="prefix">前缀</param>
        /// <param name="max">最大的返回键数.如为负则不限制</param>
        /// <returns></returns>
        public static string[] GetMatchingKeys(TcpClientIOPool pool, string prefix, int max)
        {
            IList<string> result = new List<string>();
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.FWMKEYS);

                    byte[] prefixbytes = System.Text.Encoding.UTF8.GetBytes(prefix);
                    writer.Write(prefixbytes.Length.AsBigEndian());
                    writer.Write(max.AsBigEndian());
                    writer.Write(prefixbytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int knum = reader.ReadInt32().AsBigEndian();
                            if (knum < 0) throw new TTReceiveException();
                            for (int i = 0; i < knum; i++)
                            {
                                int keySize = reader.ReadInt32().AsBigEndian();
                                if (keySize > 0)
                                {
                                    var keyStr = reader.ReadUTF8String(keySize);
                                    result.Add(keyStr);
                                }
                                else if (keySize < 0)
                                    throw new TTReceiveException();
                            }
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                    return result.ToArray();
                }
            }
        }

        /// <summary>
        /// 指定键的递增数(如键不存在，则创建新的记录).
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>新递增的记录值</returns>
        public static int IncrementInteger(TcpClientIOPool pool, string key, int value)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.ADDINT);

                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(value.AsBigEndian());
                    writer.Write(keybytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                            return reader.ReadInt32().AsBigEndian();
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// 在服务端执行lua 脚本并返回结果(string)
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="name">脚本名</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="options">脚本的锁选项</param>
        /// <returns></returns>
        public static string ExecuteScript(TcpClientIOPool pool, string name, string key, string value, ScriptOption options)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.EXT);

                    byte[] namebytes = System.Text.Encoding.UTF8.GetBytes(name);
                    byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(key);
                    byte[] valuebytes = System.Text.Encoding.UTF8.GetBytes(value);
                    writer.Write(namebytes.Length.AsBigEndian());
                    writer.Write(((int)options).AsBigEndian());
                    writer.Write(keybytes.Length.AsBigEndian());
                    writer.Write(valuebytes.Length.AsBigEndian());
                    writer.Write(namebytes);
                    writer.Write(keybytes);
                    writer.Write(valuebytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        BinaryReader reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int valSize = reader.ReadInt32().AsBigEndian();
                            if (valSize >= 0)
                                return reader.ReadUTF8String(valSize);
                            else
                                throw new TTReceiveException();
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// Synchronize updated contents with the file and the device.
        /// </summary>
        /// <param name="pool">链接池</param>
        public static void Synchronize(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.SYNC);
                    writer.Flush();
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        /// 优化存储
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="parameters">specifies the string of the tuning parameters.</param>
        public static void Optimize(TcpClientIOPool pool, string parameters)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.OPT);

                    byte[] parametersbytes = System.Text.Encoding.UTF8.GetBytes(parameters);
                    writer.Write(parametersbytes.Length.AsBigEndian());
                    writer.Write(parametersbytes);

                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        /// 清空数据库(所有记录)
        /// </summary>
        /// <param name="pool">链接池</param>
        public static void Vanish(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.VANISH);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        /// Copy the database file.
        /// The database file is assured to be kept synchronized and not modified while the copying or executing operation is in progress.  
        /// So, this method is useful to create a backup file of the database file.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="path"> specifies the path of the destination file.  If it begins with `@', the trailing substring is executed as a command line.</param>
        public static void Copy(TcpClientIOPool pool, string path)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.COPY);

                    byte[] pathbytes = System.Text.Encoding.UTF8.GetBytes(path);
                    writer.Write(pathbytes.Length.AsBigEndian());
                    writer.Write(pathbytes);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        byte code = tcpClientIO.WriteAndReadByte(m.ToArray(), 0, (int)m.Length);
                        if (code != 0)
                            throw new TTMiscException();
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据库的记录条数
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <returns>记录条数(Int64类型)</returns>
        public static Int64 GetRecordCount(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.RNUM);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        var reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int high = reader.ReadInt32().AsBigEndian();
                            int low = reader.ReadInt32().AsBigEndian();
                            Int64 result = (high << 32) + low;
                            return result;
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据库大小
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <returns>数据库大小(Int64类型)</returns>
        public static Int64 GetDatabaseSize(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.SIZE);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        var reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int high = reader.ReadInt32().AsBigEndian();
                            int low = reader.ReadInt32().AsBigEndian();
                            Int64 result = (high << 32) + low;
                            return result;
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }


        /// <summary>
        /// 获取数据库的状态
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <returns>返回数据状态(IDictionary类型)</returns>
        public static IDictionary<string, string> GetDatabaseStatus(TcpClientIOPool pool)
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.STAT);
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        var reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int statSize = reader.ReadInt32().AsBigEndian();
                            if (statSize <= 0)
                                return null;
                            else
                            {
                                var tsv = reader.ReadUTF8String(statSize);
                                Dictionary<string, string> result = new Dictionary<string, string>();
                                var lines = tsv.Split('\n');
                                foreach (var line in lines)
                                {
                                    var columns = line.Split('\t');
                                    if (columns.Length >= 2)
                                        result.Add(columns[0], columns[1]);
                                }
                                return result;
                            }
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
        }

        /// <summary>
        /// Call a versatile function for miscellaneous operations.
        /// </summary>
        /// <param name="operation">Specifies the name of the function.  
        /// All databases support "putlist", "outlist", and "getlist". 
        /// "putlist" is to store records. It receives keys and values one after the other, and returns an empty list. 
        /// "outlist" is to remove records.  It receives keys, and returns an empty array.  
        /// "getlist" is to retrieve records.  It receives keys, and returns keys and values of corresponding records one after the other.  
        /// Table database supports "setindex", "search", and "genuid"</param>
        /// <param name="pool">链接池</param>
        /// <param name="args">The arguments.</param>
        /// <param name="option">MiscOptions bit mask</param>
        /// <returns>Returns an array of the results.</returns>
        public static string[] Misc(TcpClientIOPool pool, string operation, string[] args, MiscOption option)
        {
            List<string> result = new List<string>();
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.MISC);

                    byte[] operationbytes = System.Text.Encoding.UTF8.GetBytes(operation);
                    writer.Write(operationbytes.Length.AsBigEndian());
                    writer.Write(((int)option).AsBigEndian());
                    writer.Write(args.Length.AsBigEndian());
                    writer.Write(operationbytes);
                    foreach (string a in args)
                    {
                        var val = a;
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(val);
                        writer.Write(bytes.Length.AsBigEndian());
                        writer.Write(bytes);
                    }
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        var reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                        {
                            int rnum = reader.ReadInt32().AsBigEndian();
                            int size = 0;
                            for (int i = 0; i < rnum; i++)
                            {
                                size = reader.ReadInt32().AsBigEndian();
                                if (size > 0)
                                {
                                    var item = reader.ReadUTF8String(size);
                                    result.Add(item);
                                }
                                else if (size < 0)
                                    throw new TTReceiveException();
                            }
                        }
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 删除指定组键的记录
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="keys">键</param>
        public static void DeleteMultiple(TcpClientIOPool pool, string[] keys)
        {
            Misc(pool, "outlist", keys, 0);
        }

        /// <summary>
        /// 一次添加多项组键记录
        /// </summary>
        /// <param name="items">项集合</param>
        /// <param name="pool">链接池</param>
        public static void PutMultiple(TcpClientIOPool pool, IDictionary<string, string> items)
        {
            IList<string> args = new List<string>();
            foreach (var k in items.Keys)
            {
                args.Add(k);
                args.Add(items[k]);
            }
            Misc(pool, "putlist", args.ToArray(), 0);
        }

        /// <summary>
        /// 添加单条多列记录
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="key">键</param>
        /// <param name="column">列</param>
        /// <param name="overwrite">如为true则覆盖</param>
        public static void PutColumns(TcpClientIOPool pool, string key, IDictionary<string, string> columns, bool overwrite)
        {
            List<string> args = new List<string>();
            args.Add(key);
            foreach (var k in columns.Keys)
            {
                args.Add(k);
                args.Add(columns[k]);
            }
            Misc(pool, (overwrite ? "put" : "putkeep"), args.ToArray(), 0);
        }

        static void PutColumns(TcpClientIOPool pool, string key, string[] args, bool overwrite)
        {
            Misc(pool, (overwrite ? "put" : "putkeep"), args, 0);
        }

        /// <summary>
        /// 添加(多列)记录dictionary.与PutColumns速度相当.
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="records">The records.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public static void PutColumnRecords(TcpClientIOPool pool, IDictionary<string, IDictionary<string, string>> records, bool overwrite)
        {

            var argList = new List<string[]>();
            foreach (var key in records.Keys)
            {
                var args = new List<string>();
                args.Add(key);
                var record = records[key];
                foreach (var k in record.Keys)
                {
                    args.Add(k);
                    args.Add(record[k]);
                }
                argList.Add(args.ToArray());
            }

            for (int i = 0; i < argList.Count; i++)
            {
                Misc(pool, (overwrite ? "put" : "putkeep"), argList[i], 0);
            }
        }

        /// <summary>
        /// 获取一组键的记录信息(含列信息).
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public static IDictionary<string, IDictionary<string, string>> GetColumns(TcpClientIOPool pool, string[] keys)
        {
            var result = new Dictionary<string, IDictionary<string, string>>();
            var records = GetMultiple(pool, keys);
            foreach (var key in records.Keys)
            {
                var values = ((string)records[key]).Split('\0');
                var col = new Dictionary<string, string>();
                int i = 0;
                while (i < values.Length - 1)
                {
                    col.Add(values[i], values[i + 1]);
                    i += 2;
                }
                result.Add(key, col);
            }
            return result;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="name">要索引的列. 如索引存在,它会被重建. 空字符串即是主键..</param>
        /// <param name="type">
        /// 索引类型: 
        /// `IndexOptions.ITLEXICAL' for lexical string, 
        /// `IndexOptions.ITDECIMAL' for decimal string.  
        /// `IndexOptions.ITOPT', the index is optimized.  
        /// `IndexOptions.ITVOID', the index is removed.  
        /// `IndexOptions.ITKEEP' is added by bitwise-or and the index exists, this method merely returns failure.
        /// </param>
        public static void SetIndex(TcpClientIOPool pool, string name, IndexOption type)
        {
            List<string> args = new List<string>();
            args.Add(name);
            args.Add(((int)type).ToString());
            Misc(pool, "setindex", args.ToArray(), 0);
        }

        public static string[] QueryKeys(TcpClientIOPool pool, Query query)
        {
            var args = query.GetArgs();
            return Misc(pool, "search", args, MiscOption.OmitLog);
        }

        public static void QueryDelete(TcpClientIOPool pool, Query query)
        {
            List<string> argsList = query.GetArgs().ToList<string>();
            argsList.Add("out");
            Misc(pool, "search", argsList.ToArray(), 0);
        }


        public static IDictionary<string, IDictionary<string, string>> QueryRecords(TcpClientIOPool pool, Query query)
        {
            var result = new Dictionary<string, IDictionary<string, string>>();
            List<string> argsList = query.GetArgs().ToList<string>();
            argsList.Add("get");
            var args = Misc(pool, "search", argsList.ToArray(), MiscOption.OmitLog);
            foreach (var arg in args)
            {
                var col = new Dictionary<string, string>();
                var cary = arg.Split('\0');
                var cnum = cary.Length - 1;
                var i = 0;
                Dictionary<string, string> columns = null;
                while (i < cnum)
                {
                    var key = cary[i];
                    var value = cary[i + 1];
                    if (i == 0)
                    {
                        columns = new Dictionary<string, string>();
                        result.Add(value, columns);
                    }
                    else
                    {
                        columns.Add(key, value);
                    }
                    i += 2;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定查询条件记录数
        /// </summary>
        /// <param name="pool">链接池</param>
        /// <param name="query">查询</param>
        /// <returns>记录数</returns>
        public static int QueryRecordsCount(TcpClientIOPool pool, Query query)
        {
            int result = 0;
            List<string> argsList = query.GetArgs().ToList<string>();
            argsList.Add("get");
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m, System.Text.Encoding.UTF8))
                {
                    writer.Write((byte)0xC8);
                    writer.Write((byte)Command.RNUM);
                    writer.Write("search".Length.AsBigEndian());
                    writer.Write(((int)MiscOption.OmitLog).AsBigEndian());
                    writer.Write(argsList.ToArray().Length.AsBigEndian());
                    writer.Write(System.Text.Encoding.UTF8.GetBytes("search"));
                    foreach (string a in argsList.ToArray())
                    {
                        var val = a;
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(val);
                        writer.Write(bytes.Length.AsBigEndian());
                        writer.Write(bytes);
                    }
                    using (TcpClientIO tcpClientIO = pool.GetTcpClient())
                    {
                        var reader = tcpClientIO.WriteAndGetReader(m.ToArray(), 0, (int)m.Length);
                        byte code = reader.ReadByte();
                        if (code == 0)
                            result = reader.ReadInt32().AsBigEndian();
                        else
                            throw TokyoTyrantException.FromErrorCode(code);
                    }
                }
            }
            return result;
        }
    }


}
