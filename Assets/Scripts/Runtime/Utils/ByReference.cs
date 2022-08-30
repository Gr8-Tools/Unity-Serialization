using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Runtime.Utils {
    // ByReference<T> is meant to be used to represent "ref T" fields. It is working
    // around lack of first class support for byref fields in C# and IL. The JIT and 
    // type loader has special handling for it that turns it into a thin wrapper around ref T.
    [Obsolete("Not used")]
    internal
#if !PROJECTN // readonly breaks codegen contract and asserts UTC
        readonly
#endif
        ref struct ByReference<T> where T : unmanaged {
        // CS0169: The private field '{blah}' is never used
#pragma warning disable 169
        private readonly IntPtr _value;
#pragma warning restore

        public unsafe ByReference(ref T value) { 
                _value = (IntPtr)UnsafeUtility.AddressOf(ref value);
        }

        public ByReference(IntPtr value) {
                _value = value;
        }

        public ref T Value {
                get {
                        unsafe {
                                return ref (*(T*)_value);
                        }
                }
        }

        public IntPtr IntPtrValue => _value;
        
        public unsafe void* VoidValue => (T*)_value;
    }
}