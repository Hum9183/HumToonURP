using System;
using UnityEditor;
using UnityEngine;

namespace Hum.HumToon.Editor.HeaderScopes.Base
{
    public class BaseDrawer : HeaderScopeDrawerBase<BasePropertiesContainer>
    {
        public BaseDrawer(BasePropertiesContainer propContainer, Func<GUIContent> headerStyleFunc, uint expandable)
            : base(propContainer, headerStyleFunc, expandable)
        {
        }

        protected override void DrawInternal(MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(BaseStyles.BaseMap, PropContainer.BaseMap, PropContainer.BaseColor);
            materialEditor.TextureScaleOffsetProperty(PropContainer.BaseMap);
        }
    }
}
