using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.EntLib.SphinxClient
{
    /// <summary>
    /// Search results grouping function
    /// </summary>
    public enum GroupBy : int
    {
        SPH_GROUPBY_DAY = 0,
        SPH_GROUPBY_WEEK = 1,
        SPH_GROUPBY_MONTH = 2,
        SPH_GROUPBY_YEAR = 3,
        SPH_GROUPBY_ATTR = 4,
        SPH_GROUPBY_ATTRPAIR = 5
    }

    ///<summary>
    /// Search results sorting mode
    /// </summary>
    public enum SortMode : int
    {
        /// <summary>
        /// Sort results by relevance in descending order (best matches first)
        /// </summary>
        SPH_SORT_RELEVANCE = 0,
        /// <summary>
        /// Sort results by an attribute in descending order (bigger attribute values first)
        /// </summary>
        SPH_SORT_ATTR_DESC = 1,
        /// <summary>
        /// Sort results by an attribute in ascending order (smaller attribute values first)
        /// </summary>
        SPH_SORT_ATTR_ASC = 2,
        /// <summary>
        /// Sort results by time segments (last hour/day/week/month) in descending order, and then by relevance in descending order
        /// </summary>
        SPH_SORT_TIME_SEGMENTS = 3,
        /// <summary>
        /// Sort results by SQL-like sort expression, must contains attributes or internal attributes names with order direction specified by keyword (ASC/DESC)
        /// </summary>
        SPH_SORT_EXTENDED = 4,
        /// <summary>
        /// Sort results by an arithmetic expression
        /// </summary>
        SPH_SORT_EXPRESSION = 5
    }

    ///<summary>
    /// Search match mode
    /// </summary>
    public enum MatchMode : int
    {
        /* 区配方式 */
        SPH_MATCH_ALL = 0,
        SPH_MATCH_ANY = 1,
        SPH_MATCH_PHRASE = 2,
        SPH_MATCH_BOOLEAN = 3,
        SPH_MATCH_EXTENDED = 4
    }


    public enum AttrType : int
    {
        /* 属性类型 */
        SPH_ATTR_INTEGER = 1,
        SPH_ATTR_TIMESTAMP = 2,
        SPH_ATTR_ORDINAL = 3,
        SPH_ATTR_BOOL = 4,
        SPH_ATTR_FLOAT = 5,
        SPH_ATTR_MULTI = 0x40000000
    }

    /// <summary>
    /// 守护进程命令
    /// </summary>
    public enum CommandType : int
    {
        /* 属性类型 */
        SEARCHD_COMMAND_SEARCH = 0,
        SEARCHD_COMMAND_EXCERPT = 1,
        SEARCHD_COMMAND_UPDATE = 2
    }
    
    /// <summary>
    /// 守护进程命令版本信息
    /// </summary>
    public enum VerCommand : int
    {
        /* 属性类型 */
        VER_MAJOR_PROTO = 0x1,
        VER_COMMAND_SEARCH = 0x10f,
        VER_COMMAND_EXCERPT = 0x100,
        VER_COMMAND_UPDATE = 0x100
    }

    /// <summary>
    /// 过滤类型
    /// </summary>
    public enum FilterType : int
    {
        SPH_FILTER_VALUES = 0,
        SPH_FILTER_RANGE = 1,
        SPH_FILTER_FLOATRANGE = 2     
    }
    
    /// <summary>
    /// 守护进程（searchd）返回信息
    /// </summary>
    public enum ResultType : int
    {
       SEARCHD_OK = 0,
       SEARCHD_ERROR = 1,
       SEARCHD_RETRY = 2,
       SEARCHD_WARNING = 3 
    }
}
