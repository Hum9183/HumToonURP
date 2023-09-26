using System;
using Hum.HumToon.Editor.Utils;
using NUnit.Framework;

namespace Hum.HumToon.Tests.Editor
{
    public class HumToonExtensionMethodsTests
    {
        [Test]
        [TestCase(0.0f, false)]
        [TestCase(0.5f, true)]
        [TestCase(1.0f, true)]
        public void TestToBool(float value, bool expectedBool)
        {
            bool result = value.ToBool();
            Assert.That(result, Is.EqualTo(expectedBool));
        }

        [Test]
        [TestCase(false, 0.0f)]
        [TestCase(true, 1.0f)]
        [TestCase(null, 0.0f)]
        public void TestToFloat(bool value, float expectedFloat)
        {
            float result = value.ToFloat();
            Assert.That(result, Is.EqualTo(expectedFloat));
        }

        [Test]
        [TestCase(0.0f, false)]
        [TestCase(1.0f, true)]
        [TestCase(0.99f, false)]
        [TestCase(1.01f, false)]
        [TestCase(1.01f, true, 0.1f)]
        [TestCase(0.99f, true, 0.1f)]
        public void TestIsOne(float value, bool expectedBool, float tolerance = 0.000000001f)
        {
            bool result = value.IsOne(tolerance);
            Assert.That(result, Is.EqualTo(expectedBool));
        }

        enum Season
        {
            Spring,
            Summer,
            Autumn,
            Winter
        }

        [Test]
        [TestCase(0, Season.Spring)]
        [TestCase(3, Season.Winter)]
        public void TestToEnum<T>(int value, T expectedEnum)
            where T : Enum
        {
            T result = value.ToEnum<T>();
            Assert.That(result, Is.EqualTo(expectedEnum));
        }
    }
}
