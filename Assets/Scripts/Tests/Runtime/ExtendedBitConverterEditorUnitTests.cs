using System;
using System.Collections.Generic;
using System.IO;
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
                SerializationLogger.Log($"[{nameof(UnityTypeConversions)}]: Prepare serialize info: [{vector2};{vector2Int};{vector3}]");

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

                Assert.AreEqual(initialGuid, newGuid, $"Received 'Guid'-value '{newGuid}' != '{initialGuid}'");
            }

            GuidConversionInternal(Guid.Empty);
            GuidConversionInternal(Guid.NewGuid());
        }
        
        [TestCase(byte.MinValue, false, sbyte.MinValue, short.MinValue, int.MinValue, float.MinValue, double.MinValue)]
        [TestCase(128, true, 0, 0, 0, 0f, 0d)]
        [TestCase(byte.MaxValue, true, sbyte.MaxValue, -1, 1, float.NaN, double.NegativeInfinity)]
        [TestCase(byte.MaxValue, true, sbyte.MaxValue, short.MaxValue, int.MaxValue, float.MaxValue, double.MaxValue)]
        public void UniversalBaseTypeConversions(byte byteValue, bool boolValue, sbyte sbyteValue, short shortValue, int intValue, float floatValue, double doubleValue) {
            SerializationLogger.Log($"[{nameof(UniversalBaseTypeConversions)}]: Prepare serialize info: [{byteValue};{boolValue};{sbyteValue};{shortValue};{intValue};{floatValue};{doubleValue}]");

            int offset;
            byte[] buffer = new byte[3 + sizeof(short) + sizeof(int) + sizeof(float) + sizeof(double)];

            offset = 0;
            ExtendedBitConverter.SerializeDefault(in byteValue, buffer, ref offset, sizeof(byte));
            ExtendedBitConverter.SerializeDefault(in boolValue, buffer, ref offset, sizeof(bool));
            ExtendedBitConverter.SerializeDefault(in sbyteValue, buffer, ref offset, sizeof(sbyte));
            ExtendedBitConverter.SerializeDefault(in shortValue, buffer, ref offset, sizeof(short));
            ExtendedBitConverter.SerializeDefault(in intValue, buffer, ref offset, sizeof(int));
            ExtendedBitConverter.SerializeDefault(in floatValue, buffer, ref offset, sizeof(float));
            ExtendedBitConverter.SerializeDefault(in doubleValue, buffer, ref offset, sizeof(double));
            
            SerializationLogger.Log($"[{nameof(UniversalBaseTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

            offset = 0;
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out byte newByteValue, sizeof(byte));
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out bool newBoolValue, sizeof(bool));
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out sbyte newSbyteValue, sizeof(sbyte));
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out short newShortValue, sizeof(short));
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out int newIntValue, sizeof(int));
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out float newFloatValue, sizeof(float));
            ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out double newDoubleValue, sizeof(double));
            

            SerializationLogger.Log(
                $"[{nameof(UniversalBaseTypeConversions)}]: Deserialized: [{newByteValue};{newBoolValue};{newSbyteValue};{shortValue};{intValue};{floatValue};{doubleValue}]");

            Assert.AreEqual(byteValue, newByteValue, $"Received 'byte'-value '{newByteValue}' != '{byteValue}'");
            Assert.AreEqual(boolValue, newBoolValue, $"Received 'bool'-value '{newBoolValue}' != '{boolValue}'");
            Assert.AreEqual(sbyteValue, newSbyteValue, $"Received 'sbyte'-value '{newSbyteValue}' != '{sbyteValue}'");
            Assert.AreEqual(shortValue, newShortValue, $"Received 'short'-value '{newShortValue}' != '{shortValue}'");
            Assert.AreEqual(intValue, newIntValue, $"Received 'int'-value '{newIntValue}' != '{intValue}'");
            Assert.AreEqual(floatValue, newFloatValue, $"Received 'float'-value '{newFloatValue}' != '{floatValue}'");
            Assert.AreEqual(doubleValue, newDoubleValue, $"Received 'double'-value '{newDoubleValue}' != '{doubleValue}'");

            SerializationLogger.Log($"[{nameof(UniversalBaseTypeConversions)}]: Test finished ----------------------------------");
        }
        
        [Test]
        public void UniversalStructTypeConversions() {
            void UniversalStructTypeConversionsInternal(Vector2 vector2, Vector2Int vector2Int, Vector3 vector3, Guid guid) {
                SerializationLogger.Log(
                    $"[{nameof(UniversalStructTypeConversions)}]: Prepare serialize info: [{vector2};{vector2Int};{vector3};{guid}]");

                int offset;
                byte[] buffer = new byte[8 + 8 + 12 + 16];

                offset = 0;
                ExtendedBitConverter.SerializeDefault(in vector2, buffer, ref offset, 8);
                ExtendedBitConverter.SerializeDefault(in vector2Int, buffer, ref offset, 8);
                ExtendedBitConverter.SerializeDefault(in vector3, buffer, ref offset, 12);
                ExtendedBitConverter.SerializeDefault(in guid, buffer, ref offset, 16);

                SerializationLogger.Log($"[{nameof(UniversalStructTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

                offset = 0;
                ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out Vector2 newVector2, 8);
                ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out Vector2Int newVector2Int, 8);
                ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out Vector3 newVector3, 12);
                ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out Guid newGuid, 16);

                SerializationLogger.Log($"[{nameof(UniversalStructTypeConversions)}]: Deserialized: [{newVector2};{newVector2Int};{newVector3};{newGuid}]");
                
                Assert.AreEqual(vector2, newVector2, $"Received 'Vector2'-value '{newVector2}' != '{vector2}'");
                Assert.AreEqual(vector2Int, newVector2Int, $"Received 'Vector2Int'-value '{newVector2Int}' != '{vector2Int}'");
                Assert.AreEqual(vector3, newVector3, $"Received 'Vector3'-value '{newVector3}' != '{vector3}'");
                Assert.AreEqual(guid, newGuid, $"Received 'Guid'-value '{newGuid}' != '{guid}'");

                SerializationLogger.Log($"[{nameof(UniversalStructTypeConversions)}]: Test finished ----------------------------------");
            }
            
            UniversalStructTypeConversionsInternal(Vector2.zero, Vector2Int.zero, Vector3.zero, Guid.Empty);
            UniversalStructTypeConversionsInternal(Vector2.left, Vector2Int.left, Vector3.left, Guid.NewGuid());
            UniversalStructTypeConversionsInternal(Vector2.one, Vector2Int.one, Vector3.one, Guid.NewGuid());
            UniversalStructTypeConversionsInternal( Vector2.negativeInfinity, new Vector2Int(5, 5), Vector3.positiveInfinity, Guid.NewGuid());
        }
        
        [Test]
        public void ReadonlyStructTypeConversions() {
             void ReadonlyStructTypeConversionsInternal(ReadonlyStruct value) {
                SerializationLogger.Log($"[{nameof(ReadonlyStructTypeConversions)}]: Prepare serialize info: [{value}]");

                int valueSize = 0;
                unsafe {
                    valueSize = sizeof(ReadonlyStruct);
                    SerializationLogger.Log($"Size of '{nameof(ReadonlyStruct)}': {valueSize}");
                }
                
                int offset;
                byte[] buffer = new byte[valueSize];

                offset = 0;
                ExtendedBitConverter.SerializeDefault(in value, buffer, ref offset);
                SerializationLogger.Log($"[{nameof(ReadonlyStructTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

                offset = 0;
                ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out ReadonlyStruct newValue);
                SerializationLogger.Log($"[{nameof(ReadonlyStructTypeConversions)}]: Deserialized: [{newValue}]");
                
                Assert.AreEqual(value, newValue, $"Received 'ReadonlyStruct'-value '{newValue}' != '{value}'");
                
                SerializationLogger.Log($"[{nameof(ReadonlyStructTypeConversions)}]: Test finished ----------------------------------");
            }
            
            ReadonlyStructTypeConversionsInternal(new ReadonlyStruct(byte.MinValue, false, sbyte.MinValue, short.MinValue, int.MinValue, float.MinValue, double.MinValue, Vector2.zero, Vector2Int.zero, Vector3.zero, Guid.Empty));
            ReadonlyStructTypeConversionsInternal(new ReadonlyStruct(128, true, 0, 0, 0, 0f, 0d, Vector2.left, Vector2Int.left, Vector3.left, Guid.NewGuid()));
            ReadonlyStructTypeConversionsInternal(new ReadonlyStruct(byte.MaxValue, true, sbyte.MaxValue, -1, 1, 10.5f, 100.00003d, Vector2.one, Vector2Int.one, Vector3.one, Guid.NewGuid()));
            ReadonlyStructTypeConversionsInternal(new ReadonlyStruct(byte.MaxValue, true, sbyte.MaxValue, short.MaxValue, int.MaxValue, float.MaxValue, double.MaxValue, new Vector2(0.505f, 0.499f), new Vector2Int(5, 5), new Vector3(0.123f, -0.321f, 0.98765f), Guid.NewGuid()));
        }
        
        [Test]
        public void PropertyStructTypeConversions() {
             void PropertyStructTypeConversionsInternal(PropertyStruct value) {
                SerializationLogger.Log($"[{nameof(PropertyStructTypeConversions)}]: Prepare serialize info: [{value}]");

                int valueSize = 0;
                unsafe {
                    valueSize = sizeof(PropertyStruct);
                    SerializationLogger.Log($"Size of '{nameof(PropertyStruct)}': {valueSize}");
                }
                
                int offset;
                byte[] buffer = new byte[valueSize];

                offset = 0;
                ExtendedBitConverter.SerializeDefault(in value, buffer, ref offset);
                SerializationLogger.Log($"[{nameof(PropertyStructTypeConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

                offset = 0;
                ExtendedBitConverter.DeserializeDefault(buffer, ref offset, out PropertyStruct newValue);
                SerializationLogger.Log($"[{nameof(PropertyStructTypeConversions)}]: Deserialized: [{newValue}]");
                
                Assert.AreEqual(value, newValue, $"Received 'PropertyStruct'-value '{newValue}' != '{value}'");
                
                SerializationLogger.Log($"[{nameof(PropertyStructTypeConversions)}]: Test finished ----------------------------------");
            }
            
            PropertyStructTypeConversionsInternal(new PropertyStruct {
                ByteValue = byte.MinValue, 
                BoolValue = false, 
                SbyteValue = sbyte.MinValue, 
                ShortValue = short.MinValue, 
                IntValue = int.MinValue,
                FloatValue = float.MinValue,
                DoubleValue = double.MinValue,
                Vector2Value = Vector2.zero,
                Vector2IntValue = Vector2Int.zero,
                Vector3Value = Vector3.zero,
                GuidValue = Guid.Empty
            });
            PropertyStructTypeConversionsInternal(new PropertyStruct {
                ByteValue = 128,
                BoolValue = true,
                SbyteValue = 0,
                ShortValue = 0,
                IntValue = 0,
                FloatValue = 0f,
                DoubleValue = 0d,
                Vector2Value = Vector2.left,
                Vector2IntValue = Vector2Int.left,
                Vector3Value = Vector3.left,
                GuidValue = Guid.NewGuid()
            });
            PropertyStructTypeConversionsInternal(new PropertyStruct {
                ByteValue = byte.MaxValue, 
                BoolValue = true,
                SbyteValue = sbyte.MaxValue,
                ShortValue = -1,
                IntValue = -1,
                FloatValue = 10.5f,
                DoubleValue = -100.00003d,
                Vector2Value = Vector2.one,
                Vector2IntValue = Vector2Int.one,
                Vector3Value = Vector3.one,
                GuidValue = Guid.NewGuid()
            });
            PropertyStructTypeConversionsInternal(new PropertyStruct {
                ByteValue = byte.MaxValue,
                BoolValue = true,
                SbyteValue = sbyte.MaxValue,
                ShortValue = short.MaxValue,
                IntValue = int.MaxValue,
                FloatValue = float.MaxValue,
                DoubleValue = double.MaxValue,
                Vector2Value = new Vector2(0.505f, 0.499f),
                Vector2IntValue = new Vector2Int(5, 5), 
                Vector3Value = new Vector3(0.123f, -0.321f, 0.98765f), 
                GuidValue = Guid.NewGuid()
            });
        }
        
#region Utils
        
        private readonly struct ReadonlyStruct {
            private readonly byte _byteValue;
            private readonly bool _boolValue;
            private readonly sbyte _sbyteValue;
            
            private readonly short _shortValue;
            private readonly int _intValue;
            private readonly float _floatValue;
            private readonly double _doubleValue;
            
            private readonly Vector2 _vector2Value;
            private readonly Vector2Int _vector2IntValue;
            private readonly Vector3 _vector3Value;
            private readonly Guid _guidValue;
            public ReadonlyStruct(byte byteValue, bool boolValue, sbyte sbyteValue, short shortValue, int intValue, float floatValue, double doubleValue, Vector2 vector2Value, Vector2Int vector2IntValue, Vector3 vector3Value, Guid guidValue) {
                _byteValue = byteValue;
                _boolValue = boolValue;
                _sbyteValue = sbyteValue;
                _shortValue = shortValue;
                _intValue = intValue;
                _floatValue = floatValue;
                _doubleValue = doubleValue;
                _vector2Value = vector2Value;
                _vector2IntValue = vector2IntValue;
                _vector3Value = vector3Value;
                _guidValue = guidValue;
            }

            public override string ToString() {
                return $"[Byte='{_byteValue}'][Bool='{_boolValue}'][Sbyte='{_sbyteValue}']" +
                       $"[Short='{_shortValue}'][Int='{_intValue}'][Float='{_floatValue}'][Double='{_doubleValue}']" +
                       $"[Vector2='{_vector2Value}'][Vector2Int='{_vector2IntValue}'][Vector3='{_vector3Value}'][Guid='{_guidValue}']";
            }
        }
        
        private struct PropertyStruct {
            internal byte ByteValue { get; set; }
            internal bool BoolValue { get; set; }
            internal sbyte SbyteValue { get; set; }
            internal short ShortValue { get; set; }
            internal int IntValue { get; set; }
            internal float FloatValue { get; set; }
            internal double DoubleValue { get; set; }
            internal Vector2 Vector2Value { get; set; }
            internal Vector2Int Vector2IntValue { get; set; }
            internal Vector3 Vector3Value { get; set; }
            internal Guid GuidValue { get; set; }

            public override string ToString() {
                return $"[Byte='{ByteValue}'][Bool='{BoolValue}'][Sbyte='{SbyteValue}']" +
                       $"[Short='{ShortValue}'][Int='{IntValue}'][Float='{FloatValue}'][Double='{DoubleValue}']" +
                       $"[Vector2='{Vector2Value}'][Vector2Int='{Vector2IntValue}'][Vector3='{Vector3Value}'][Guid='{GuidValue}']";
            }
        }

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