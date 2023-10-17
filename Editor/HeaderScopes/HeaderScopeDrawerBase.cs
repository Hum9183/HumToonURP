using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScopes
{
    public abstract class HeaderScopeDrawerBase<T> : IHeaderScopeDrawer
        where T : IPropertiesContainer
    {
        protected readonly T PropContainer;
        private readonly Func<GUIContent> _headerStyleFunc;
        private readonly uint _expandable;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propContainer"></param>
        /// <param name="headerStyleFunc">GUIContentを取得する関数</param>
        /// <param name="expandable"></param>
        protected HeaderScopeDrawerBase(T propContainer, Func<GUIContent> headerStyleFunc, uint expandable)
        {
            PropContainer = propContainer;
            _headerStyleFunc = headerStyleFunc;
            _expandable = expandable;
        }

        public void SetProperties(MaterialProperty[] materialProperties)
        {
            PropertySetter.Set(PropContainer, materialProperties);
        }

        public void Draw(MaterialEditor materialEditor)
        {
            using var header = new MaterialHeaderScope(_headerStyleFunc?.Invoke(), _expandable, materialEditor); // NOTE: Draw header
            if (header.expanded is false)
                return;

            DrawInternal(materialEditor);
        }

        protected abstract void DrawInternal(MaterialEditor materialEditor);
    }
}
