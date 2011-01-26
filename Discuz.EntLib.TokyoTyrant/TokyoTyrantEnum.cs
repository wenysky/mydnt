using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Discuz.EntLib.TokyoTyrant
{
    enum Command : byte
    {
        PUT = 0x10,
        PUTKEEP = 0x11,
        PUTCAT = 0x12,
        PUTSHL = 0x13,
        PUTNR = 0x18,
        OUT = 0x20,
        GET = 0x30,
        MGET = 0x31,
        VSIZ = 0x38,
        ITERINIT = 0x50,
        ITERNEXT = 0x51,
        FWMKEYS = 0x58,
        ADDINT = 0x60,
        ADDDOUBLE = 0x61,
        EXT = 0x68,
        SYNC = 0x70,
        OPT = 0x71,
        VANISH = 0x72,
        COPY = 0x73,
        RESTORE = 0x74,
        SETMST = 0x78,
        RNUM = 0x80,
        SIZE = 0x81,
        STAT = 0x88,
        MISC = 0x90
    }

    public enum ScriptOption : int
    {
        RecordLocking = 1 << 0, // # 脚本扩展项: 记录锁定
        GlobalLocking = 1 << 1 // # 脚本扩展项: 全局锁定
    }

    public enum MiscOption : int
    {
        OmitLog = 1 << 0 // # versatile function option: omission of the update log
    }

    /// <summary>
    /// 索引类型
    /// </summary>
    public enum IndexOption : int
    {
        LEXICAL = 0, // # 文本型索引
        DECIMAL = 1, // # 数值型索引
        TOKEN = 2, // # 标记倒排索引.   
        QGRAM = 3, // #QGram倒排索引.
        OPT = 9998, // # 9998, 对索引优化
        VOID = 9999, // # 9999, 移除索引.
        KEEP = 1 << 24 // # 16777216, 保持已有索引.  
    }
    
    public enum QueryOperation
    {
        STREQ = 0, // # 查询条件: 表示与操作对象的文字内容完全相同（=）
        STRINC = 1, // # 查询条件: 表示含有操作对象文字的内容（LIKE ‘%文字%’）
        STRBW = 2, // # 查询条件: 表示以操作对象的文字行列开始（LIKE ‘文字%’）
        STREW = 3, // # 查询条件: 表示到操作对象的文字行列结束（LIKE ‘%文字’）
        STRAND = 4, // # 查询条件: 表示包含操作对象的文字行列中右逗号分开部分的字段的全部（name LIKE ‘%文字㈠%’ AND name LIKE ‘%文字㈡%’）
        STROR = 5, // # 查询条件: 表示包含操作对象文字段中逗号分开部分的其中一部分（name LIKE ‘%文字㈠%’ OR name LIKE ‘%文字㈡%’）
        STROREQ = 6, // # 查询条件: 表示与操作对象文字段中逗号分开部分的其中某部分完全相同（ name = ‘文字㈠’ OR name =‘文字㈡’）
        STRRX = 7, // # 查询条件: 表与与常规表达式匹配
        NUMEQ = 8, // # 查询条件: 表示等于操作对象的数值（=）
        NUMGT = 9, // # 查询条件: 表示比操作对象的数值要大（>）
        NUMGE = 10, // # 查询条件: 表大于或等于操作对象的数值（>=）
        NUMLT = 11, // # 查询条件: 表示比操作对象的数值要小（<）
        NUMLE = 12, // # 查询条件: 表示小于或等于操作对象的数值（<=）
        NUMBT = 13, // # 查询条件: 表示其大小处于操作对象文字段中被逗号分开的两个数值的中间（between 100 and 200）
        NUMOREQ = 14, // # 查询条件: 表示其大小处于操作对象文字段中被逗号分开的两个数值的中间（between 100 and 200）
        NEGATE = 1 << 24, // # 查询条件: 负标志negation flag
        NOIDX = 1 << 25 // # 查询条件: 非索引标志
    }

    public enum QueryOrder
    {
        STRASC = 0, // # 排序类型: 表示按照文本型字段内的文本内容在字典中排列顺序的升序
        STRDESC = 1, // # 排序类型: 表示按照文本型字段内的文本内容在字典中排列顺序的降序
        NUMASC = 2, // # 排序类型: 表示按照数值大小的升序
        NUMDESC = 3 // # 排序类型: 表示按照数值大小的降序
    }
}
