using System;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime {
    public static class ExtendedBitConverter{
        private const byte FALSE_BYTE = 0; 
        private const byte TRUE_BYTE = 1;

#region Serialize

        /// <summary>
        /// Write value <paramref name="value"/> of type 'byte' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in byte value, byte[] target, ref int offset) {
            target[offset++] = value;
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'bool' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in bool value, byte[] target, ref int offset) {
            target[offset++] = value ? TRUE_BYTE : FALSE_BYTE;
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'sbyte' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in sbyte value, byte[] target, ref int offset) {
            target[offset++] = (byte)(value - sbyte.MinValue);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'short' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in short value, byte[] target, ref int offset) {
            const int length = sizeof(short);
            SerializeDefault(in value, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'int' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in int value, byte[] target, ref int offset) {
            const int length = sizeof(int);
            SerializeDefault(in value, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'float' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in float value, byte[] target, ref int offset) {
            const int length = sizeof(float);
            SerializeDefault(in value, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'double' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in double value, byte[] target, ref int offset) {
            const int length = sizeof(double);
            SerializeDefault(in value, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'Vector2' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in Vector2 value, byte[] target, ref int offset) {
            const int length = sizeof(float);
            SerializeDefault(in value.x, target, ref offset, length);
            SerializeDefault(in value.y, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'Vector2Int' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in Vector2Int value, byte[] target, ref int offset) {
            const int length = sizeof(int);
            SerializeDefault(value.x, target, ref offset, length);
            SerializeDefault(value.y, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'Vector3' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in Vector3 value, byte[] target, ref int offset) {
            const int length = sizeof(int);
            SerializeDefault(in value.x, target, ref offset, length);
            SerializeDefault(in value.y, target, ref offset, length);
            SerializeDefault(in value.z, target, ref offset, length);
        }
        
        /// <summary>
        /// Write value <paramref name="value"/> of type 'Guid' into byte-array <paramref name="target"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Serialize(in Guid value, byte[] target, ref int offset) {
            const int length = 16;
            SerializeDefault(in value, target, ref offset, length);
        }

#endregion

#region Deserialize

        /// <summary>
        /// Read value <paramref name="value"/> of type 'byte' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out byte value) {
            value = source[offset++];
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'bool' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out bool value) {
            value = source[offset++] == TRUE_BYTE;
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'sbyte' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out sbyte value) {
            value = (sbyte)(source[offset++] + sbyte.MinValue);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'short' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out short value) {
            const int lenght = sizeof(short);
            DeserializeDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'int' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out int value) {
            const int lenght = sizeof(int);
            DeserializeDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'float' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out float value) {
            const int lenght = sizeof(float);
            DeserializeDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'double' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out double value) {
            const int lenght = sizeof(double);
            DeserializeDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'Vector2' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Vector2 value) {
            const int lenght = sizeof(float);
            DeserializeDefault(source, ref offset, out float xValue, lenght);
            DeserializeDefault(source, ref offset, out float yValue, lenght);

            value = new Vector2(xValue, yValue);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'Vector2Int' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Vector2Int value) {
            const int lenght = sizeof(float);
            DeserializeDefault(source, ref offset, out int xValue, lenght);
            DeserializeDefault(source, ref offset, out int yValue, lenght);

            value = new Vector2Int(xValue, yValue);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'Vector3' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Vector3 value) {
            const int lenght = sizeof(float);
            DeserializeDefault(source, ref offset, out float xValue, lenght);
            DeserializeDefault(source, ref offset, out float yValue, lenght);
            DeserializeDefault(source, ref offset, out float zValue, lenght);

            value = new Vector3(xValue, yValue, zValue);
        }

        /// <summary>
        /// Read value <paramref name="value"/> of type 'Guid' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Guid value) {
            const int lenght = sizeof(float);
            DeserializeDefault(source, ref offset, out value, lenght);
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