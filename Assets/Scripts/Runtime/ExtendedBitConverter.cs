using System;
using Runtime.Extensions;
using Runtime.Utils;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Runtime {
    public static class ExtendedBitConverter{
        private const byte FALSE_BYTE = 0; 
        private const byte TRUE_BYTE = 1;
        private const int SIZE_OF_BYTE = 8;

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
            ReadDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'int' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out int value) {
            const int lenght = sizeof(int);
            ReadDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'float' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out float value) {
            const int lenght = sizeof(float);
            ReadDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'double' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out double value) {
            const int lenght = sizeof(double);
            ReadDefault(source, ref offset, out value, lenght);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'Vector2' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Vector2 value) {
            const int lenght = sizeof(float);
            ReadDefault(source, ref offset, out float xValue, lenght);
            ReadDefault(source, ref offset, out float yValue, lenght);

            value = new Vector2(xValue, yValue);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'Vector2Int' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Vector2Int value) {
            const int lenght = sizeof(float);
            ReadDefault(source, ref offset, out int xValue, lenght);
            ReadDefault(source, ref offset, out int yValue, lenght);

            value = new Vector2Int(xValue, yValue);
        }
        
        /// <summary>
        /// Read value <paramref name="value"/> of type 'Vector3' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Vector3 value) {
            const int lenght = sizeof(float);
            ReadDefault(source, ref offset, out float xValue, lenght);
            ReadDefault(source, ref offset, out float yValue, lenght);
            ReadDefault(source, ref offset, out float zValue, lenght);

            value = new Vector3(xValue, yValue, zValue);
        }

        /// <summary>
        /// Read value <paramref name="value"/> of type 'Guid' from byte-array <paramref name="source"/> starting with <paramref name="offset"/> position
        /// <para>offset is increased with required size</para>
        /// </summary>
        public static void Deserialize(this byte[] source, ref int offset, out Guid value) {
            const int lenght = sizeof(float);
            ReadDefault(source, ref offset, out value, lenght);
        }
        
#endregion



#region Utils

        private static void SerializeDefault<T>(in T value, byte[] target, ref int offset, in int length) {
            unsafe {
                fixed(void* ptr = target) {
                    void* writePtr = UnsafeUtilityExtensions.OffsetPtr<byte>(ptr, offset);
                    UnsafeUtility.WriteArrayElement(writePtr, 0, value);
                }
            }
            
            offset += length;
        }

        private static void ReadDefault<T>(byte[] source, ref int offset, out T value, in int length) {
            unsafe {
                fixed(void* ptr = source) {
                    void* readPtr = UnsafeUtilityExtensions.OffsetPtr<byte>(ptr, offset);
                    value = UnsafeUtility.ReadArrayElement<T>(readPtr, 0);
                }
            }
            
            offset += length;
        }

#endregion
    }
}