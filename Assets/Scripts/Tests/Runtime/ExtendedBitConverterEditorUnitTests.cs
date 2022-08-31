using System;
using NUnit.Framework;
using Runtime;
using Tests.Runtime.Utils.Loggers;
using UnityEngine;

namespace Tests.Runtime {
    public class ExtendedBitConverterEditorUnitTests {
        // // A Test behaves as an ordinary method
        // [Test]
        // public void ExtendedBitConverterEditorUnitTestsSimplePasses() {
        //     // Use the Assert class to test conditions
        // }
        //
        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator ExtendedBitConverterEditorUnitTestsWithEnumeratorPasses() {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }

        [Test]
        public void SimpleConversions() {
            void SimpleConversionsInternal(byte byteValue, bool boolValue, sbyte sbyteValue) {
                SerializationLogger.Log($"[{nameof(SimpleConversions)}]: Prepare serialize info: [{byteValue};{boolValue};{sbyteValue}]");

                int offset;
                byte[] buffer = new byte[3];

                offset = 0;
                ExtendedBitConverter.Serialize(in byteValue, buffer, ref offset);
                ExtendedBitConverter.Serialize(in boolValue, buffer, ref offset);
                ExtendedBitConverter.Serialize(in sbyteValue, buffer, ref offset);
            
                SerializationLogger.Log($"[{nameof(SimpleConversions)}]: Serialized info: [{string.Join(";", buffer)}]");
                
                offset = 0;
                buffer.Deserialize(ref offset, out byte newByteValue);
                buffer.Deserialize(ref offset, out bool newBoolValue);
                buffer.Deserialize(ref offset, out sbyte newSbyteValue);
                
                SerializationLogger.Log($"[{nameof(SimpleConversions)}]: Deserialized: [{newByteValue};{newBoolValue};{newSbyteValue}]");

                Assert.AreEqual(byteValue, newByteValue, $"Received 'byte'-value '{newByteValue}' != '{byteValue}'");
                Assert.AreEqual(boolValue, newBoolValue, $"Received 'bool'-value '{newBoolValue}' != '{boolValue}'");
                Assert.AreEqual(sbyteValue, newSbyteValue, $"Received 'sbyte'-value '{newSbyteValue}' != '{sbyteValue}'");
                
                SerializationLogger.Log($"[{nameof(SimpleConversions)}]: Test finished ----------------------------------");
            }

            SimpleConversionsInternal(byte.MinValue, false, sbyte.MinValue);
            SimpleConversionsInternal(128, true, 0);
            SimpleConversionsInternal(byte.MaxValue, true, sbyte.MaxValue);
        }

        [Test]
        public void BigSystemTypeConversions() {
            void BigSystemTypeConversionsInternal(short shortValue, int intValue, float floatValue, double doubleValue) {
                SerializationLogger.Log($"[{nameof(BigSystemTypeConversions)}]: Prepare serialize info: [{shortValue};{intValue};{floatValue};{doubleValue}]");

                int offset;
                byte[] buffer = new byte[sizeof(short) + sizeof(int) + sizeof(float) + sizeof(double)];

                offset = 0;
                ExtendedBitConverter.Serialize(in shortValue, buffer, ref offset);
                ExtendedBitConverter.Serialize(in intValue, buffer, ref offset);
                ExtendedBitConverter.Serialize(in floatValue, buffer, ref offset);
                ExtendedBitConverter.Serialize(in doubleValue, buffer, ref offset);
            
                SerializationLogger.Log($"[{nameof(BigSystemTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

                offset = 0;
                buffer.Deserialize(ref offset, out short newShortValue);
                buffer.Deserialize(ref offset, out int newIntValue);
                buffer.Deserialize(ref offset, out float newFloatValue);
                buffer.Deserialize(ref offset, out double newDoubleValue);
                
                SerializationLogger.Log($"[{nameof(SimpleConversions)}]: Deserialized: [{newShortValue};{newIntValue};{newFloatValue};{newDoubleValue}]");

                Assert.AreEqual(shortValue, newShortValue, $"Received 'short'-value '{newShortValue}' != '{shortValue}'");
                Assert.AreEqual(intValue, newIntValue, $"Received 'int'-value '{newIntValue}' != '{intValue}'");
                Assert.AreEqual(floatValue, newFloatValue, $"Received 'float'-value '{newFloatValue}' != '{floatValue}'");
                Assert.AreEqual(doubleValue, newDoubleValue, $"Received 'double'-value '{newDoubleValue}' != '{doubleValue}'");

                SerializationLogger.Log($"[{nameof(BigSystemTypeConversions)}]: Test finished ----------------------------------");
            }
            
            BigSystemTypeConversionsInternal(short.MinValue, int.MinValue, float.MinValue, double.MinValue);
            BigSystemTypeConversionsInternal(0, 0, 0, 0);
            BigSystemTypeConversionsInternal(-1, 1, 10.5f, 100.00003d);
            BigSystemTypeConversionsInternal(short.MaxValue, int.MaxValue, float.MaxValue, double.MaxValue);
        }

        [Test]
        public void UnityTypeConversions() {
            void UnityTypeConversionsInternal(Vector2 vector2, Vector2Int vector2Int, Vector3 vector3) {
                SerializationLogger.Log($"[{nameof(UnityTypeConversions)}]: Prepare serialize info: [{vector2};{vector2Int};{vector3}]");

                int offset;
                byte[] buffer = new byte[8 + 8 + 12];

                offset = 0;
                ExtendedBitConverter.Serialize(in vector2, buffer, ref offset);
                ExtendedBitConverter.Serialize(in vector2Int, buffer, ref offset);
                ExtendedBitConverter.Serialize(in vector3, buffer, ref offset);
            
                SerializationLogger.Log($"[{nameof(UnityTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

                offset = 0;
                buffer.Deserialize(ref offset, out Vector2 newVector2);
                buffer.Deserialize(ref offset, out Vector2Int newVector2Int);
                buffer.Deserialize(ref offset, out Vector3 newVector3);

                Assert.AreEqual(vector2, newVector2, $"Received 'Vector2'-value '{newVector2}' != '{vector2}'");
                Assert.AreEqual(vector2Int, newVector2Int, $"Received 'Vector2Int'-value '{newVector2Int}' != '{vector2Int}'");
                Assert.AreEqual(vector3, newVector3, $"Received 'Vector3'-value '{newVector3}' != '{vector3}'");
            }
            
            UnityTypeConversionsInternal(Vector2.zero, Vector2Int.zero, Vector3.zero);
            UnityTypeConversionsInternal(Vector2.left, Vector2Int.left, Vector3.left);
            UnityTypeConversionsInternal(Vector2.one, Vector2Int.one, Vector3.one);
            UnityTypeConversionsInternal(new Vector2(0.505f, 0.499f), new Vector2Int(5, 5), new Vector3(0.123f, -0.321f, 0.98765f));
        }

        [Test]
        public void GuidConversion() {
            void GuidConversionInternal(Guid initialGuid) {
                SerializationLogger.Log($"[{nameof(GuidConversion)}]: Prepare serialize info: [{initialGuid}]");

                int offset;
                byte[] buffer = new byte[16];

                offset = 0;
                ExtendedBitConverter.Serialize(in initialGuid, buffer, ref offset);
                
                SerializationLogger.Log($"[{nameof(GuidConversion)}]: Serialized info: [{string.Join(";", buffer)}]");
                
                offset = 0;
                buffer.Deserialize(ref offset, out Guid newGuid);
                
                Assert.AreEqual(initialGuid, newGuid, $"Received 'Vector2'-value '{newGuid}' != '{initialGuid}'");
            }
            
            GuidConversionInternal(Guid.Empty);
            GuidConversionInternal(Guid.NewGuid());
        }
    }
}