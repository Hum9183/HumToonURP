using UnityEditor;

namespace Hum.HumToon.Editor.HeaderScopes
{
    public interface IHeaderScopeDrawer
    {
        void SetProperties();
        void Draw(MaterialEditor materialEditor);
    }
}
