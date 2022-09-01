using System;

namespace BinarySerialization.Extensions {
    internal static class UnsafeUtilityExtensions {
        
        internal static unsafe void* OffsetPtr<T>(IntPtr ptr, int offset) where T : unmanaged {
            return IntPtr.Add(ptr, offset * sizeof(T)).ToPointer();
        }
        
        internal static unsafe void* OffsetPtr<T>(void* ptr, int offset) where T : unmanaged {
            return OffsetPtr<T>((IntPtr)ptr, offset); 
        }
        
        internal static unsafe IntPtr OffsetIntPtr<T>(IntPtr ptr, int offset) where T : unmanaged {
            return IntPtr.Add(ptr, offset * sizeof(T));
        }
        internal static unsafe IntPtr OffsetIntPtr<T>(void* ptr, int offset) where T : unmanaged {
            return OffsetIntPtr<T>((IntPtr)ptr, offset); 
        }
        
        internal static unsafe ref T OffsetRef<T>(IntPtr ptr, int offset) where T : unmanaged {
            return ref *(T*)(ptr + offset * sizeof(T));
        }
    }
}