using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace Discuz.Data
{
    /// <summary>
    /// Oracle数据库的Discuz!NT接口, 有关Oracle的更多信息见 http://www.oracle.com/technology/software/tech/windows/odpnet/index.html
    /// </summary>
    public class OracleProvider : Discuz.Data.IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return OracleClientFactory.Instance;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if ((cmd as OracleCommand) != null)
            {
                OracleCommandBuilder.DeriveParameters(cmd as OracleCommand);
            }
        }

        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            OracleParameter param;

            if (Size > 0)
                param = new OracleParameter(ParamName, (OracleType)DbType, Size);
            else
                param = new OracleParameter(ParamName, (OracleType)DbType);

            return param;
        }


        public bool IsFullTextSearchEnabled()
        {
            return true;
        }

        public bool IsCompactDatabase()
        {
            return true;
        }

        public bool IsBackupDatabase()
        {
            return false;
        }

    }
}
