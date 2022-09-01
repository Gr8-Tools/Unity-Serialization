using System;
using NUnit.Framework;
using Runtime;
using Tests.Runtime.Utils.Loggers;
using Tests.Runtime.Utils.Loggers.Utils;
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


        [TestCase(byte.MinValue, false, sbyte.MinValue)]
        [TestCase(128, true, 0)]
        [TestCase(byte.MaxValue, true, sbyte.MaxValue)]
        public void SimpleConversions(byte byteValue, bool boolValue, sbyte sbyteValue) {
            SerializationLogger.Log(
                $"[{nameof(SimpleConversions)}]: Prepare serialize info: [{byteValue};{boolValue};{sbyteValue}]");

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

            SerializationLogger.Log(
                $"[{nameof(SimpleConversions)}]: Deserialized: [{newByteValue};{newBoolValue};{newSbyteValue}]");

            Assert.AreEqual(byteValue, newByteValue, $"Received 'byte'-value '{newByteValue}' != '{byteValue}'");
            Assert.AreEqual(boolValue, newBoolValue, $"Received 'bool'-value '{newBoolValue}' != '{boolValue}'");
            Assert.AreEqual(sbyteValue, newSbyteValue, $"Received 'sbyte'-value '{newSbyteValue}' != '{sbyteValue}'");

            SerializationLogger.Log($"[{nameof(SimpleConversions)}]: Test finished ----------------------------------");
        }

        [TestCase(short.MinValue, int.MinValue, float.MinValue, double.MinValue)]
        [TestCase(0, 0, 0f, 0d)]
        [TestCase(-1, 1, 10.5f, 100.00003d)]
        [TestCase(short.MaxValue, int.MaxValue, float.MaxValue, double.MaxValue)]
        public void BigSystemTypeConversions(short shortValue, int intValue, float floatValue, double doubleValue) {
            SerializationLogger.Log(
                $"[{nameof(BigSystemTypeConversions)}]: Prepare serialize info: [{shortValue};{intValue};{floatValue};{doubleValue}]");

            int offset;
            byte[] buffer = new byte[sizeof(short) + sizeof(int) + sizeof(float) + sizeof(double)];

            string InternalSuccessSerializeLog<T>(T value) {
                return SuccessSerializeLog(value, buffer, offset);
            }

            string InternalFailSerializeLog<T>(T value, Exception ex) {
                return FailSerializeLog(value, buffer, offset, ex);
            }

            string InternalSuccessDeserializeLog<T>(T value) {
                return SuccessDeserializeLog(value, buffer, offset);
            }

            string InternalFailDeserializeLog<T>(T value, Exception ex) {
                return FailDeserializeLog(value, buffer, offset, ex);
            }

            offset = 0;
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => ExtendedBitConverter.Serialize(in shortValue, buffer, ref offset),
                SuccessMsgCall = () => InternalSuccessSerializeLog(shortValue),
                FailMsgCall = ex => InternalFailSerializeLog(shortValue, ex)
            });
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => ExtendedBitConverter.Serialize(in intValue, buffer, ref offset),
                SuccessMsgCall = () => InternalSuccessSerializeLog(intValue),
                FailMsgCall = ex => InternalFailSerializeLog(intValue, ex)
            });
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => ExtendedBitConverter.Serialize(in floatValue, buffer, ref offset),
                SuccessMsgCall = () => InternalSuccessSerializeLog(floatValue),
                FailMsgCall = ex => InternalFailSerializeLog(floatValue, ex)
            });
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => ExtendedBitConverter.Serialize(in doubleValue, buffer, ref offset),
                SuccessMsgCall = () => InternalSuccessSerializeLog(doubleValue),
                FailMsgCall = ex => InternalFailSerializeLog(doubleValue, ex)
            });

            // ExtendedBitConverter.Serialize(in shortValue, buffer, ref offset);
            // ExtendedBitConverter.Serialize(in intValue, buffer, ref offset);
            // ExtendedBitConverter.Serialize(in floatValue, buffer, ref offset);
            // ExtendedBitConverter.Serialize(in doubleValue, buffer, ref offset);

            SerializationLogger.Log(
                $"[{nameof(BigSystemTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

            offset = 0;
            short newShortValue = default;
            int newIntValue = default;
            float newFloatValue = default;
            double newDoubleValue = default;
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => buffer.Deserialize(ref offset, out newShortValue),
                SuccessMsgCall = () => InternalSuccessDeserializeLog(newShortValue),
                FailMsgCall = ex => InternalFailDeserializeLog(newShortValue, ex)
            });
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => buffer.Deserialize(ref offset, out newIntValue),
                SuccessMsgCall = () => InternalSuccessDeserializeLog(newIntValue),
                FailMsgCall = ex => InternalFailDeserializeLog(newIntValue, ex)
            });
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => buffer.Deserialize(ref offset, out newFloatValue),
                SuccessMsgCall = () => InternalSuccessDeserializeLog(newFloatValue),
                FailMsgCall = ex => InternalFailDeserializeLog(newFloatValue, ex)
            });
            SerializationLogger.LogAttempt(new LogAttemptArgs {
                Action = () => buffer.Deserialize(ref offset, out newDoubleValue),
                SuccessMsgCall = () => InternalSuccessDeserializeLog(newDoubleValue),
                FailMsgCall = ex => InternalFailDeserializeLog(newDoubleValue, ex)
            });

            // buffer.Deserialize(ref offset, out newShortValue);
            // buffer.Deserialize(ref offset, out newIntValue);
            // buffer.Deserialize(ref offset, out newFloatValue);
            // buffer.Deserialize(ref offset, out newDoubleValue);

            SerializationLogger.Log(
                $"[{nameof(BigSystemTypeConversions)}]: Deserialized: [{newShortValue};{newIntValue};{newFloatValue};{newDoubleValue}]");

            Assert.AreEqual(shortValue, newShortValue, $"Received 'short'-value '{newShortValue}' != '{shortValue}'");
            Assert.AreEqual(intValue, newIntValue, $"Received 'int'-value '{newIntValue}' != '{intValue}'");
            Assert.AreEqual(floatValue, newFloatValue, $"Received 'float'-value '{newFloatValue}' != '{floatValue}'");
            Assert.AreEqual(doubleValue,
                            newDoubleValue,
                            $"Received 'double'-value '{newDoubleValue}' != '{doubleValue}'");

            SerializationLogger.Log(
                $"[{nameof(BigSystemTypeConversions)}]: Test finished ----------------------------------");
        }

        [Test]
        public void UnityTypeConversions() {
            void UnityTypeConversionsInternal(Vector2 vector2, Vector2Int vector2Int, Vector3 vector3) {
                SerializationLogger.Log(
                    $"[{nameof(UnityTypeConversions)}]: Prepare serialize info: [{vector2};{vector2Int};{vector3}]");

                int offset;
                byte[] buffer = new byte[8 + 8 + 12];

                offset = 0;
                ExtendedBitConverter.Serialize(in vector2, buffer, ref offset);
                ExtendedBitConverter.Serialize(in vector2Int, buffer, ref offset);
                ExtendedBitConverter.Serialize(in vector3, buffer, ref offset);

                SerializationLogger.Log(
                    $"[{nameof(UnityTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

                offset = 0;
                buffer.Deserialize(ref offset, out Vector2 newVector2);
                buffer.Deserialize(ref offset, out Vector2Int newVector2Int);
                buffer.Deserialize(ref offset, out Vector3 newVector3);

                Assert.AreEqual(vector2, newVector2, $"Received 'Vector2'-value '{newVector2}' != '{vector2}'");
                Assert.AreEqual(vector2Int,
                                newVector2Int,
                                $"Received 'Vector2Int'-value '{newVector2Int}' != '{vector2Int}'");
                Assert.AreEqual(vector3, newVector3, $"Received 'Vector3'-value '{newVector3}' != '{vector3}'");
            }

            UnityTypeConversionsInternal(Vector2.zero, Vector2Int.zero, Vector3.zero);
            UnityTypeConversionsInternal(Vector2.left, Vector2Int.left, Vector3.left);
            UnityTypeConversionsInternal(Vector2.one, Vector2Int.one, Vector3.one);
            UnityTypeConversionsInternal(new Vector2(0.505f, 0.499f),
                                         new Vector2Int(5, 5),
                                         new Vector3(0.123f, -0.321f, 0.98765f));
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

#region Utils

        private static string SuccessSerializeLog<T>(T value, byte[] buffer, int offset) {
            return
                $"[Serialization] SUCCESS: Value='{value}' of Type='{typeof(T)}' wrapped in buffer [{string.Join(";", buffer)}]. NEW offset: '{offset}'";
        }

        private static string FailSerializeLog<T>(T value, byte[] buffer, int offset, Exception ex) {
            return
                $"[Serialization] FAILED: Value='{value}' of Type='{typeof(T)}' WASNOT wrapped in buffer [{string.Join(";", buffer)}]. Offset: '{offset}'. Exception: '{ex}'";
        }

        private static string SuccessDeserializeLog<T>(T value, byte[] buffer, int offset) {
            return
                $"[Deserialization] SUCCESS: Value='{value}' of Type='{typeof(T)}' handled from buffer [{string.Join(";", buffer)}]. NEW offset: '{offset}'";
        }

        private static string FailDeserializeLog<T>(T value, byte[] buffer, int offset, Exception ex) {
            return
                $"[Deserialization] FAILED: Value='{value}' of Type='{typeof(T)}' WASNOT handled in buffer [{string.Join(";", buffer)}]. Offset: '{offset}'. Exception: '{ex}'";
        }

#endregion
    }
}