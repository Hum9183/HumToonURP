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
            var scopes = CreateHeaderScopes();
            return scopes
                .Select(x => x.drawer)
                .Where(HumToonUtils.IsNotNull);
        }

        public IEnumerable<IHeaderScopeValidator> CreateValidators()
        {
            var scopes = CreateHeaderScopes();
            return scopes
                .Select(x => x.validator)
                .Where(HumToonUtils.IsNotNull);
        }

        private IEnumerable<(IHeaderScopeDrawer drawer, IHeaderScopeValidator validator)>
            CreateHeaderScopes()
        {
            return new List<(IHeaderScopeDrawer, IHeaderScopeValidator)>
            {
                (CreateSurfaceOptionsDrawer(), new SurfaceOptionsValidator()),
                (CreateBaseDrawer(), null),
                (CreateNormalDrawer(), new NormalValidator()),
                (CreateShadeDrawer(), new ShadeValidator()),
                (CreateRimLightDrawer(), new RimLightValidator()),
                (CreateEmissionDrawer(), new EmissionValidator()),
                (CreateMatCapDrawer(), new MatCapValidator()),
                (CreateLightDrawer(), null),
            };
        }

        private SurfaceOptionsDrawer CreateSurfaceOptionsDrawer()
        {
            return new SurfaceOptionsDrawer(
                new SurfaceOptionsPropertiesContainer(),
                () => SurfaceOptionsStyles.SurfaceOptionsFoldout,
                Convert.ToUInt32(Expandable.SurfaceOptions));
        }

        private BaseDrawer CreateBaseDrawer()
        {
            return new BaseDrawer(
                new BasePropertiesContainer(),
                () => BaseStyles.SurfaceInputsFoldout,
                Convert.ToUInt32(Expandable.Base));
        }

        private NormalDrawer CreateNormalDrawer()
        {
            return new NormalDrawer(
                new NormalPropertiesContainer(),
                () => NormalStyles.NormalFoldout,
                Convert.ToUInt32(Expandable.Normal));
        }

        private ShadeDrawer CreateShadeDrawer()
        {
            return new ShadeDrawer(
                new ShadePropertiesContainer(),
                () => ShadeStyles.ShadeFoldout,
                Convert.ToUInt32(Expandable.Shade));
        }

        private RimLightDrawer CreateRimLightDrawer()
        {
            return new RimLightDrawer(
                new RimLightPropertiesContainer(),
                () => RimLightStyles.RimLightFoldout,
                Convert.ToUInt32(Expandable.RimLight));
        }

        private EmissionDrawer CreateEmissionDrawer()
        {
            return new EmissionDrawer(
                new EmissionPropertiesContainer(),
                () => EmissionStyles.EmissionFoldout,
                Convert.ToUInt32(Expandable.Emission));
        }

        private MatCapDrawer CreateMatCapDrawer()
        {
            return new MatCapDrawer(
                new MatCapPropertiesContainer(),
                () => MatCapStyles.MatCapFoldout,
                Convert.ToUInt32(Expandable.MatCap));
        }

        private LightDrawer CreateLightDrawer()
        {
            return new LightDrawer(
                new LightPropertiesContainer(),
                () => LightStyles.LightFoldout,
                Convert.ToUInt32(Expandable.Light));
        }
    }
}
