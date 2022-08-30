using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Runtime.Extensions;
using Unity.Collections.LowLevel.Unsafe;

namespace Runtime.Utils {
    public readonly ref struct Span<T> where T : unmanaged {
        internal readonly ByReference<T> _pointer;
        private readonly int _length;
        
        /// <summary>
        /// Creates a new span over the portion of the target array beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the span.</param>
        /// <param name="length">The number of items in the span.</param>
        /// <remarks>Returns default when <paramref name="array"/> is null.</remarks>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array, int start, int length)
        {
            if (array == null) {
                if (start != 0 || length != 0)
                    throw new ArgumentOutOfRangeException(nameof(array));
                this = default;
                return; // returns default
            }
#if !NET_STANDARD_2_0
            if (default(T)! == null && array.GetType() != typeof(T[])) // TODO-NULLABLE: default(T) == null warning (https://github.com/dotnet/roslyn/issues/34757)
                ThrowHelper.ThrowArrayTypeMismatchException();
#endif
            
#if BIT64
            // See comment in Span<T>.Slice for how this works.
            if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)array.Length)
                throw new ArgumentOutOfRangeException(nameof(array));
#else
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                throw new ArgumentOutOfRangeException(nameof(array));
#endif

            // unsafe {
            //     void* ptr;
            //     UnsafeUtility.CopyObjectAddressToPtr(array, &ptr);
            //     
            //     _pointer = new ByReference<T>((IntPtr)ptr + start * sizeof(T));
            // }

            //_pointer = new ByReference<T>(ref array.GetRawSzArrayData<T>());
            _pointer = new ByReference<T>(Marshal.UnsafeAddrOfPinnedArrayElement(array, start));
            _length = length;
        }
        
        /// <summary>
        /// Returns a reference to specified element of the Span.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Thrown when index less than 0 or index greater than or equal to Length
        /// </exception>
        public ref T this[int index]
        {
            
#if PROJECTN
            get {
                return ref UnsafeUtilityExtensions.OffsetRef<T>(_pointer.IntPtrValue, index);
            }
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if ((uint)index >= (uint)_length) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return ref UnsafeUtilityExtensions.OffsetRef<T>(_pointer.IntPtrValue, index);
            }
#endif
        }
    }
}