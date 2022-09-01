using System;
using Tests.Runtime.Utils.Loggers.Utils;
using Tests.Runtime.Utils.Singletons;
using UnityEngine;

namespace Tests.Runtime.Utils.Loggers {
    internal class SerializationLogger : SingletonService<SerializationLogger> {
        private const bool ENABLE = true;
        
        internal static void Log(string msg) {
            if (ENABLE) {
                Debug.Log(msg);
            }
        }

        internal static void LogAttempt(Action action, Func<string> successMsg, Func<Exception, string> throwFailMsg) {
            try {
                action();
                Log(successMsg());
            } catch (Exception ex) {
                if (ENABLE) {
                    Log(throwFailMsg(ex));
                } else {
                    throw;
                }
            }
        }
        
        internal static void LogAttempt(LogAttemptArgs args) {
            try {
                args.Action();
                Log(args.SuccessMsgCall());
            } catch (Exception ex) {
                if (ENABLE) {
                    Log(args.FailMsgCall(ex));
                } else {
                    throw;
                }
            }
        }
    }
}