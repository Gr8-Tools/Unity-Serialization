using System;

namespace Tests.Runtime.Utils.Loggers.Utils {
    internal ref struct LogAttemptArgs {
        internal Action Action { get; set; }
        internal Func<string> SuccessMsgCall { get; set; }
        internal Func<Exception, string> FailMsgCall { get; set; }
    }
}