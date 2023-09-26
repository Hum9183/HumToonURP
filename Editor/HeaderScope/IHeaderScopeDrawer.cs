using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScope
{
    public interface IHeaderScopeDrawer
    {
        void SetProperties();
        void Draw(MaterialEditor materialEditor);
    }
}
