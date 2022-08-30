using NUnit.Framework;
using Runtime;
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
            Debug.Log($"[{nameof(SimpleConversions)}]: Prepare serialize info: [{byteValue};{boolValue};{sbyteValue}]");

            int offset;
            byte[] buffer = new byte[3];

            offset = 0;
            ExtendedBitConverter.Serialize(in byteValue, buffer, ref offset);
            ExtendedBitConverter.Serialize(in boolValue, buffer, ref offset);
            ExtendedBitConverter.Serialize(in sbyteValue, buffer, ref offset);
            
            Debug.Log($"[{nameof(SimpleConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

            offset = 0;
            buffer.Deserialize(ref offset, out byte newByteValue);
            buffer.Deserialize(ref offset, out bool newBoolValue);
            buffer.Deserialize(ref offset, out sbyte newSbyteValue);

            Assert.AreEqual(byteValue, newByteValue, $"Received 'byte'-value '{newByteValue}' != '{byteValue}'");
            Assert.AreEqual(boolValue, newBoolValue, $"Received 'bool'-value '{newBoolValue}' != '{boolValue}'");
            Assert.AreEqual(sbyteValue, newSbyteValue, $"Received 'sbyte'-value '{newSbyteValue}' != '{sbyteValue}'");
        }

        [TestCase(short.MinValue, int.MinValue, float.MinValue, double.MinValue)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(-1, 1, 10.5f, 100.00003d)]
        [TestCase( short.MaxValue, int.MaxValue, float.MaxValue, double.MaxValue)]
        public void BigSystemTypeConversions(short shortValue, int intValue, float floatValue, double doubleValue) {
            Debug.Log($"[{nameof(BigSystemTypeConversions)}]: Prepare serialize info: [{shortValue};{intValue};{floatValue};{doubleValue}]");

            int offset;
            byte[] buffer = new byte[sizeof(short) + sizeof(int) + sizeof(float) + sizeof(double)];

            offset = 0;
            ExtendedBitConverter.Serialize(in shortValue, buffer, ref offset);
            ExtendedBitConverter.Serialize(in intValue, buffer, ref offset);
            ExtendedBitConverter.Serialize(in floatValue, buffer, ref offset);
            ExtendedBitConverter.Serialize(in doubleValue, buffer, ref offset);
            
            Debug.Log($"[{nameof(SimpleConversions)}]: Serialized info: [{string.Join(";", buffer)}]");

            offset = 0;
            buffer.Deserialize(ref offset, out short newShortValue);
            buffer.Deserialize(ref offset, out int newIntValue);
            buffer.Deserialize(ref offset, out float newFloatValue);
            buffer.Deserialize(ref offset, out double newDoubleValue);

            Assert.AreEqual(shortValue, newShortValue, $"Received 'short'-value '{newShortValue}' != '{shortValue}'");
            Assert.AreEqual(intValue, newIntValue, $"Received 'int'-value '{newIntValue}' != '{intValue}'");
            Assert.AreEqual(floatValue, newFloatValue, $"Received 'float'-value '{newFloatValue}' != '{floatValue}'");
            Assert.AreEqual(doubleValue, newDoubleValue, $"Received 'double'-value '{newDoubleValue}' != '{doubleValue}'");
        }
    }
}