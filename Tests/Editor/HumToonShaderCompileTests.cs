using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Tests.Editor
{
    public class HumToonShaderCompileTests
    {
        private const string HumToonGUID = "e2deff3aad3b5ac4baad4f9622f2deea";

        [Test]
        public void TestHumToonShaderCompile()
        {
            string path = AssetDatabase.GUIDToAssetPath(HumToonGUID);
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
            AssetDatabase.ImportAsset(path);

            Assert.True(shader.isSupported);
            Assert.False(ShaderUtil.ShaderHasError(shader));
        }
    }
}
