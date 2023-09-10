using UnityEditor;
using UnityEngine;

namespace HumToon.Editor
{
    public interface IHeaderScopeDrawer
    {
        void SetProperties();
        void Draw(MaterialEditor materialEditor);
    }
}
