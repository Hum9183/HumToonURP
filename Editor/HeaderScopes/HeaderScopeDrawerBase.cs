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
        private readonly GUIContent _headerStyle;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propContainer"></param>
        /// <param name="headerStyle"></param>
        /// <param name="expandable"></param>
        protected HeaderScopeDrawerBase(T propContainer, GUIContent headerStyle, uint expandable)
        {
            PropContainer = propContainer;
            _headerStyle = headerStyle;
            _expandable = expandable;
        }

        public void SetProperties(MaterialProperty[] materialProperties)
        {
            PropContainer.Set(materialProperties);
        }

        public void Draw(MaterialEditor materialEditor)
        {
            GUIContent label = _headerStyle ?? _headerStyleFunc?.Invoke();

            using var header = new MaterialHeaderScope(label, _expandable, materialEditor); // NOTE: Draw header
            if (header.expanded is false)
                return;

            DrawInternal(materialEditor);
        }

        protected abstract void DrawInternal(MaterialEditor materialEditor);
    }
}
