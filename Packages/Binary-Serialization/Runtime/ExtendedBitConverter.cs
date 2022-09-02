using System;
using System.Text;
using BinarySerialization.Extensions;

namespace BinarySerialization {
    public static class ExtendedBitConverter{

#region Serialize
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'string' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>Offset is increased with required size</para>
        /// <para>It uses Encoding.UTF8 for conversion</para>
        /// </summary>
        public static void Serialize(in string value, byte[] target, ref int offset) {
            if (target.Length < offset + Encoding.UTF8.GetByteCount(value)) {
                throw new ArgumentOutOfRangeException(nameof(target));
            }

            var encodedBytes = Encoding.UTF8.GetBytes(value, 0, value.Length, target, offset);
            offset += encodedBytes;
        }

#endregion

#region Deserialize
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'string' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>Offset is increased with required size</para>
        /// /// <para>It uses Encoding.UTF8 for conversion</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out string value, int length = -1) {
            if (source == null) {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (length < 0) {
                length = source.Length - offset;
                if (length < 0) {
                    throw new ArgumentOutOfRangeException(nameof(source));
                }
            }

            if (length == 0) {
                value = string.Empty;
                return;
            }

            value = Encoding.UTF8.GetString(source, offset, length);
            offset += length;
        }

#endregion
        
#region Utils

        /// <summary>
        /// Write value <paramref name="value"/> of any unmanaged type to <see cref="byte"/>-array <paramref name="target"/> starting with <paramref name="offset"/>
        /// <para><paramref name="offset"/> will be increased with required size</para>
        /// </summary>
        /// <param name="value">Write value</param>
        /// <param name="target">Target byte-destination</param>
        /// <param name="offset">Start write position. Will be increased on <paramref name="length"/></param>
        /// <param name="length">The required bytes count to write value <paramref name="value"/> in <paramref name="target"/></param>
        /// <typeparam name="T">Unmanaged type of <paramref name="value"/></typeparam>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one or any last bytes of <paramref name="value"/> can't be written in <paramref name="target"/></exception>
        public static void SerializeDefault<T>(in T value, byte[] target, ref int offset, int length = 0) where T : unmanaged {
            if (length == 0) {
                unsafe {
                    length = sizeof(T);
                }
            }
            
            if (length + offset > target.Length) {
                throw new ArgumentOutOfRangeException(nameof(target),
                                                      $"Invalid size of buffer: require {length} bytes starting from {offset} index, but length is only '{target.Length}'");
            }
            
            var arraySegment = new ArraySegment<byte>(target, offset, length);
            SerializeDefault(in value, in arraySegment, ref offset);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of any unmanaged type to <seealso cref="ArraySegment{T}"/><paramref name="target"/>
        /// <para> <paramref name="offset"/> will be increased with required size of <see cref="ArraySegment{T}.Count"/></para>
        /// </summary>
        /// <param name="value">Write value</param>
        /// <param name="target">Target <see cref="ArraySegment{T}"/></param>
        /// <param name="offset">Will be increased on <see cref="ArraySegment{T}.Count"/></param>
        /// <typeparam name="T">Unmanaged type of <paramref name="value"/></typeparam>
        public static void SerializeDefault<T>(in T value, in ArraySegment<byte> target, ref int offset) where T : unmanaged {
            unsafe {
                fixed(void* ptr = target.Array) {
                    void* writePtr = UnsafeUtilityExtensions.OffsetPtr<byte>(ptr, target.Offset);
                    *(T*)writePtr = value;
                }
            }
            
            offset += target.Count;
        }

        /// <summary>
        /// Read value <paramref name="value"/> of any unmanaged type from <see cref="byte"/>-array <paramref name="source"/> starting with <paramref name="offset"/>
        /// <para>offset will be increased with required size</para>
        /// </summary>
        /// <param name="value">Read value</param>
        /// <param name="source">Source byte-destination</param>
        /// <param name="offset">Start read position. Will be increased on <paramref name="length"/></param>
        /// <param name="length">The required bytes count to read value <paramref name="value"/> from <paramref name="source"/></param>
        /// <typeparam name="T">Unmanaged type of <paramref name="value"/></typeparam>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if one or any last bytes of <paramref name="value"/> can't be read from<paramref name="source"/></exception>
        public static void DeserializeDefault<T>(byte[] source, ref int offset, out T value, int length = 0) where T : unmanaged {
            if (length == 0) {
                unsafe {
                    length = sizeof(T);
                }
            }
            
            if (length + offset > source.Length) {
                throw new ArgumentOutOfRangeException(nameof(source),
                                                      $"Invalid size of buffer: require {length} bytes starting from {offset} index, but length is only '{source.Length}'");
            }
            
            var arraySegment = new ArraySegment<byte>(source, offset, length);
            DeserializeDefault(in arraySegment, ref offset, out value);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of any unmanaged type from <see cref="ArraySegment{T}"/><paramref name="source"/>
        /// <para> <paramref name="offset"/> will be increased with required size of <see cref="ArraySegment{T}.Count"/></para>
        /// </summary>
        /// <param name="value">Read value</param>
        /// <param name="source">Source <see cref="ArraySegment{T}"/></param>
        /// <param name="offset">Will be increased on <see cref="ArraySegment{T}.Count"/></param>
        /// <typeparam name="T">Unmanaged type of <paramref name="value"/></typeparam>
        public static void DeserializeDefault<T>(in ArraySegment<byte> source, ref int offset, out T value) where T : unmanaged {
            unsafe {
                fixed(void* ptr = source.Array) {
                    void* readPtr = UnsafeUtilityExtensions.OffsetPtr<byte>(ptr, source.Offset);
                    value = *(T*)readPtr;
                }
            }
            
            offset += source.Count;
        }

#endregion
    }
}