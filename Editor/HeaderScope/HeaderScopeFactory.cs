using System;
using System.Linq;
using System.Collections.Generic;

namespace HumToon.Editor
{
    public class HeaderScopeFactory
    {
        public IEnumerable<IHeaderScopeDrawer> CreateDrawers(PropertySetter propSetter)
        {
            var scopes = CreateHeaderScopes(propSetter);
            return scopes
                .Select(x => x.drawer)
                .Where(Utils.IsNotNull);;
        }

        public IEnumerable<IHeaderScopeValidator> CreateValidators()
        {
            var scopes = CreateHeaderScopes();
            return scopes
                .Select(x => x.validator)
                .Where(Utils.IsNotNull);
        }

        private IEnumerable<(IHeaderScopeDrawer drawer, IHeaderScopeValidator validator)>
            CreateHeaderScopes(PropertySetter propSetter = null)
        {
            return new List<(IHeaderScopeDrawer, IHeaderScopeValidator)>
            {
                (CreateSurfaceOptionsDrawer(propSetter), new SurfaceOptionsValidator()),
                (CreateBaseDrawer(propSetter), null),
                (CreateNormalDrawer(propSetter), new NormalValidator()),
                (CreateShadeDrawer(propSetter), new ShadeValidator()),
                (CreateMatCapDrawer(propSetter), new MatCapValidator()),
                (CreateLightDrawer(propSetter), null),
            };
        }

        private SurfaceOptionsDrawer CreateSurfaceOptionsDrawer(PropertySetter propSetter)
        {
            return new SurfaceOptionsDrawer(
                new SurfaceOptionsPropertyContainer(propSetter),
                SurfaceOptionsStyles.SurfaceOptionsFoldout,
                Convert.ToUInt32(HumToon.Editor.Expandable.SurfaceOptions));
        }

        private BaseDrawer CreateBaseDrawer(PropertySetter propSetter)
        {
            return new BaseDrawer(
                new BasePropertyContainer(propSetter),
                BaseStyles.SurfaceInputsFoldout,
                Convert.ToUInt32(HumToon.Editor.Expandable.Base));
        }

        private NormalDrawer CreateNormalDrawer(PropertySetter propSetter)
        {
            return new NormalDrawer(
                new NormalPropertyContainer(propSetter),
                NormalStyles.NormalFoldout,
                Convert.ToUInt32(HumToon.Editor.Expandable.Normal));
        }

        private ShadeDrawer CreateShadeDrawer(PropertySetter propSetter)
        {
            return new ShadeDrawer(
                new ShadePropertyContainer(propSetter),
                ShadeStyles.ShadeFoldout,
                Convert.ToUInt32(HumToon.Editor.Expandable.Shade));
        }

        private MatCapDrawer CreateMatCapDrawer(PropertySetter propSetter)
        {
            return new MatCapDrawer(
                new MatCapPropertyContainer(propSetter),
                MatCapStyles.MatCapFoldout,
                Convert.ToUInt32(HumToon.Editor.Expandable.MatCap));
        }

        private LightDrawer CreateLightDrawer(PropertySetter propSetter)
        {
            return new LightDrawer(
                new LightPropertyContainer(propSetter),
                LightStyles.LightFoldout,
                Convert.ToUInt32(HumToon.Editor.Expandable.Light));
        }
    }
}
