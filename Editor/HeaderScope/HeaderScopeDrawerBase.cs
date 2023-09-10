using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;

namespace HumToon.Editor
{
    public abstract class HeaderScopeDrawerBase<T> : IHeaderScopeDrawer
        where T : IPropertyContainer
    {
        protected readonly T PropContainer;
        private readonly GUIContent _headerStyle;
        private readonly uint _expandable;

        protected HeaderScopeDrawerBase(T propContainer, GUIContent headerStyle, uint expandable)
        {
            PropContainer = propContainer;
            _headerStyle = headerStyle;
            _expandable = expandable;
        }

        public void SetProperties()
        {
            PropContainer.Set();
        }

        public void Draw(MaterialEditor materialEditor)
        {
            using var header = new MaterialHeaderScope(_headerStyle, _expandable, materialEditor); // NOTE: Draw header
            if (header.expanded is false)
                return;

            DrawInternal(materialEditor);
        }

        protected abstract void DrawInternal(MaterialEditor materialEditor);
    }
}
