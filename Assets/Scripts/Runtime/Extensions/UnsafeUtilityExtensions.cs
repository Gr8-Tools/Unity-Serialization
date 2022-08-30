using System;
using Runtime.Utils;
using Unity.Collections.LowLevel.Unsafe;

namespace Runtime.Extensions {
    internal static class UnsafeUtilityExtensions {
        internal static ref T OffsetRef<T>(IntPtr ptr, int index) where T : unmanaged {
            unsafe {
                return ref *(T*)(ptr + index * sizeof(T));
            }
        }
        
        [Obsolete("Not used")]
        internal static ref T GetRawSzArrayData<T>(this Array array) {
            return ref UnsafeUtility.As<byte, T>(ref UnsafeUtility.As<Array, RawSzArrayData> (ref array).Data);
        }
    }
}