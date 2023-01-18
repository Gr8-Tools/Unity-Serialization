using System;

namespace BinarySerialization.Extensions {
    public static class UnsafeUtilityExtensions {
        
        public static unsafe void* OffsetPtr(IntPtr ptr, int offset, int sizeOfType) {
            return IntPtr.Add(ptr, offset * sizeOfType).ToPointer();
        }
        public static unsafe void* OffsetPtr<T>(IntPtr ptr, int offset) where T : unmanaged {
            return OffsetPtr(ptr, offset, sizeof(T));
        }
        
        //

        public static unsafe void* OffsetPtr(void* ptr, int offset, int sizeOfType) {
            return OffsetPtr((IntPtr)ptr, offset, sizeOfType); 
        }
        
        public static unsafe void* OffsetPtr<T>(void* ptr, int offset) where T : unmanaged {
            return OffsetPtr(ptr, offset, sizeof(T)); 
        }
        
        //
        
        public static IntPtr OffsetIntPtr(IntPtr ptr, int offset, int sizeOfType) {
            return IntPtr.Add(ptr, offset * sizeOfType);
        }
        
        public static unsafe IntPtr OffsetIntPtr<T>(IntPtr ptr, int offset) where T : unmanaged {
            return OffsetIntPtr(ptr, offset, sizeof(T));
        }
        
        //
        
        public static unsafe IntPtr OffsetIntPtr(void* ptr, int offset, int sizeOfType) {
            return OffsetIntPtr((IntPtr)ptr, offset, sizeOfType); 
        }
        
        public static unsafe IntPtr OffsetIntPtr<T>(void* ptr, int offset) where T : unmanaged {
            return OffsetIntPtr<T>((IntPtr)ptr, offset); 
        }
        
        //
        
        public static unsafe ref T Ref<T>(IntPtr ptr) where T : unmanaged {
            return ref *(T*)ptr;
        }
        
        public static unsafe ref T OffsetRef<T>(IntPtr ptr, int offset, int sizeOfType) where T : unmanaged {
            return ref Ref<T>(ptr + offset * sizeOfType);
        }
        
        public static unsafe ref T OffsetRef<T>(IntPtr ptr, int offset) where T : unmanaged {
            return ref OffsetRef<T>(ptr, offset, sizeof(T));
        }
        
        
    }
}