using System;

namespace BinarySerialization.Extensions {
    public static class UnsafeUtilityExtensions {
        
        public static unsafe void* OffsetPtr<T>(IntPtr ptr, int offset) where T : unmanaged {
            return IntPtr.Add(ptr, offset * sizeof(T)).ToPointer();
        }
        
        public static unsafe void* OffsetPtr<T>(void* ptr, int offset) where T : unmanaged {
            return OffsetPtr<T>((IntPtr)ptr, offset); 
        }
        
        public static unsafe IntPtr OffsetIntPtr<T>(IntPtr ptr, int offset) where T : unmanaged {
            return IntPtr.Add(ptr, offset * sizeof(T));
        }
        public static unsafe IntPtr OffsetIntPtr<T>(void* ptr, int offset) where T : unmanaged {
            return OffsetIntPtr<T>((IntPtr)ptr, offset); 
        }
        
        public static unsafe ref T OffsetRef<T>(IntPtr ptr, int offset) where T : unmanaged {
            return ref *(T*)(ptr + offset * sizeof(T));
        }
    }
}