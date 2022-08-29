using System;

namespace Runtime.Extensions {
    internal static class UnsafeUtilityExtensions {
        internal static ref T OffsetRef<T>(IntPtr ptr, int index) where T : unmanaged {
            unsafe {
                return ref *(T*)(ptr + index * sizeof(T));
            }
        }
    }
}