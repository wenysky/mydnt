using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.EntLib.TokyoTyrant
{

    public class TokyoTyrantException : Exception { 
        
        public TokyoTyrantException(string message) : base(message) {
        }

        public static TokyoTyrantException FromErrorCode(int errorCode) {
            switch (errorCode) {
                case 1: // EINVALID = 1 # error code: 无效操作
                    return new TTInvalidOperationException(); 
                case 2:  // ENOHOST = 2 # error code: 主机无效
                    return new TTNoHostException();
                case 3: // EREFUSED = 3 # error code: 链接拒绝
                    return new TTConnectionRefusedException();
                case 4:  // ESEND = 4 # error code: 发送错误
                    return new TTSendException();
                case 5:  // ERECV = 5 # error code: 接收错误
                    return new TTReceiveException();
                case 6:  // EKEEP = 6 # error code: 记录已存在
                    return new TTExistingRecordException();
                case 7:  // ENOREC = 7 # error code: 未发现记录
                    return new TTRecordNotFoundException();
                case 9999: // EMISC = 9999 # error code: miscellaneous error
                    return new TTMiscException();
                default:
                    return new TokyoTyrantException("Error " + errorCode.ToString());
            }
        }
        
    
    }
    
    public class TTNotConnectedException : TokyoTyrantException { public TTNotConnectedException() : base("Not Connected") { } }
    public class TTInvalidOperationException : TokyoTyrantException { public TTInvalidOperationException() : base("Invalid Operation") { } }
    public class TTNoHostException : TokyoTyrantException { public TTNoHostException() : base("Host not found") { } }
    public class TTConnectionRefusedException : TokyoTyrantException { public TTConnectionRefusedException() : base("Connection refused") { } }    
    public class TTSendException : TokyoTyrantException { public TTSendException() : base("Send error") { } }
    public class TTReceiveException : TokyoTyrantException { public TTReceiveException() : base("Receive error") { } }
    public class TTExistingRecordException : TokyoTyrantException { public TTExistingRecordException() : base("Existing record") { } }
    public class TTRecordNotFoundException : TokyoTyrantException { public TTRecordNotFoundException() : base("Record not found") { } }
    public class TTMiscException : TokyoTyrantException { public TTMiscException() : base("Miscellaneous error") { } }
    




}
