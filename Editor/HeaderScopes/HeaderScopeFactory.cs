using System;
using System.Collections.Generic;
using System.Linq;
using Hum.HumToon.Editor.HeaderScopes.Base;
using Hum.HumToon.Editor.HeaderScopes.Emission;
using Hum.HumToon.Editor.HeaderScopes.Light;
using Hum.HumToon.Editor.HeaderScopes.MatCap;
using Hum.HumToon.Editor.HeaderScopes.Normal;
using Hum.HumToon.Editor.HeaderScopes.RimLight;
using Hum.HumToon.Editor.HeaderScopes.Shade;
using Hum.HumToon.Editor.HeaderScopes.SurfaceOptions;
using Hum.HumToon.Editor.Utils;

namespace Hum.HumToon.Editor.HeaderScopes
{
    // TODO: VContainer等のライブラリを使用することを検討
    public class HeaderScopeFactory
    {
        public IEnumerable<IHeaderScopeDrawer> CreateDrawers()
        {
            var scopes = CreateHeaderScopes(new PropertySetter());
            return scopes
                .Select(x => x.drawer)
                .Where(HumToonUtils.IsNotNull);;
        }

        public IEnumerable<IHeaderScopeValidator> CreateValidators()
        {
            var scopes = CreateHeaderScopes();
            return scopes
                .Select(x => x.validator)
                .Where(HumToonUtils.IsNotNull);
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
                (CreateRimLightDrawer(propSetter), new RimLightValidator()),
                (CreateEmissionDrawer(propSetter), new EmissionValidator()),
                (CreateMatCapDrawer(propSetter), new MatCapValidator()),
                (CreateLightDrawer(propSetter), null),
            };
        }

        private SurfaceOptionsDrawer CreateSurfaceOptionsDrawer(PropertySetter propSetter)
        {
            return new SurfaceOptionsDrawer(
                new SurfaceOptionsPropertiesContainer(propSetter),
                () => SurfaceOptionsStyles.SurfaceOptionsFoldout,
                Convert.ToUInt32(Expandable.SurfaceOptions));
        }

        private BaseDrawer CreateBaseDrawer(PropertySetter propSetter)
        {
            return new BaseDrawer(
                new BasePropertiesContainer(propSetter),
                () => BaseStyles.SurfaceInputsFoldout,
                Convert.ToUInt32(Expandable.Base));
        }

        private NormalDrawer CreateNormalDrawer(PropertySetter propSetter)
        {
            return new NormalDrawer(
                new NormalPropertiesContainer(propSetter),
                () => NormalStyles.NormalFoldout,
                Convert.ToUInt32(Expandable.Normal));
        }

        private ShadeDrawer CreateShadeDrawer(PropertySetter propSetter)
        {
            return new ShadeDrawer(
                new ShadePropertiesContainer(propSetter),
                () => ShadeStyles.ShadeFoldout,
                Convert.ToUInt32(Expandable.Shade));
        }

        private RimLightDrawer CreateRimLightDrawer(PropertySetter propSetter)
        {
            return new RimLightDrawer(
                new RimLightPropertiesContainer(propSetter),
                () => RimLightStyles.RimLightFoldout,
                Convert.ToUInt32(Expandable.RimLight));
        }

        private EmissionDrawer CreateEmissionDrawer(PropertySetter propSetter)
        {
            return new EmissionDrawer(
                new EmissionPropertiesContainer(propSetter),
                () => EmissionStyles.EmissionFoldout,
                Convert.ToUInt32(Expandable.Emission));
        }

        private MatCapDrawer CreateMatCapDrawer(PropertySetter propSetter)
        {
            return new MatCapDrawer(
                new MatCapPropertiesContainer(propSetter),
                () => MatCapStyles.MatCapFoldout,
                Convert.ToUInt32(Expandable.MatCap));
        }

        private LightDrawer CreateLightDrawer(PropertySetter propSetter)
        {
            return new LightDrawer(
                new LightPropertiesContainer(propSetter),
                () => LightStyles.LightFoldout,
                Convert.ToUInt32(Expandable.Light));
        }
    }
}
