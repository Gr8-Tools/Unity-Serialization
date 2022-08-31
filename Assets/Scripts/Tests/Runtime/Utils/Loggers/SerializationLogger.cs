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
    }
}