using System.Reflection;
using Hum.HumToon.Editor.Language;
using NUnit.Framework;

namespace Hum.HumToon.Tests.Editor
{
    public class HumToonLanguageTests
    {
        private const string En = "Shader";
        private const string Jp = "シェーダ";
        private const string Cn = "着色器";
        private const string Empty = "";

        [Test]
        [TestCase(new string[] { En, Jp, Cn }, Language.English, Language.Japanese, Jp)]
        [TestCase(new string[] { En, Jp, Cn }, Language.Japanese, Language.Chinese, Cn)]
        [TestCase(new string[] { En, Jp, Empty }, Language.English, Language.Chinese, En)]
        [TestCase(new string[] { Empty, Empty, Empty }, Language.English, Language.English, Empty)]
        [TestCase(new string[] { null, null, null }, Language.English, Language.English, Empty)]
        public void TestSelectInternal(string[] texts, Language defaultLang, Language currentLang, string expectedStr)
        {
            var methodIndo = GetSelectInternalMethod();
            string resultStr = (string)methodIndo?.Invoke(null, new object[] { texts, defaultLang, currentLang });
            Assert.That(resultStr, Is.EqualTo(expectedStr));
        }

        [Test]
        [TestCase(new string[] { En, Jp,    }, Language.English, Language.English)]
        [TestCase(new string[] { En, Jp, Cn, Cn }, Language.English, Language.English)]
        [TestCase(new string[] {}, Language.English, Language.English)]
        public void TestSelectInternalException(string[] texts, Language defaultLang, Language currentLang)
        {
            var methodIndo = GetSelectInternalMethod();
            try
            {
                methodIndo?.Invoke(null, new object[] { texts, defaultLang, currentLang });
            }
            catch (TargetInvocationException e)
            {
                Assert.That(e.InnerException, Is.TypeOf<LanguageTextsOutOfRangeException>());
            }
        }

        private MethodInfo GetSelectInternalMethod()
        {
            var clsType = typeof(LanguageSelector);
            var methodIndo = clsType.GetMethod("SelectInternal", BindingFlags.NonPublic | BindingFlags.Static);
            return methodIndo;
        }

    }
}
