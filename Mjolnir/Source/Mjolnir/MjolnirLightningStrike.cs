using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace Mjolnir
{
    [StaticConstructorOnStartup]
    public class MjolnirLightningStrike : WeatherEvent_LightningFlash
    {
        private IntVec3 strikeLoc = IntVec3.Invalid;

        private Mesh boltMesh;

        private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);

        public MjolnirLightningStrike(Map map) : base(map)
        {
        }

        public MjolnirLightningStrike(Map map, IntVec3 forcedStrikeLoc) : base(map)
        {
            this.strikeLoc = forcedStrikeLoc;
        }

        public override void FireEvent()
        {
            base.FireEvent();
            if (!this.strikeLoc.IsValid)
            {
                this.strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(this.map) && !this.map.roofGrid.Roofed(sq), this.map, 1000);
            }
            this.boltMesh = LightningBoltMeshPool.RandomBoltMesh;
            if (!this.strikeLoc.Fogged(this.map))
            {
                GenExplosion.DoExplosion(this.strikeLoc, this.map, 1.9f, DamageDefOf.Flame, null, 50, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
                Vector3 loc = this.strikeLoc.ToVector3Shifted();
                for (int i = 0; i < 4; i++)
                {
                    MoteMaker.ThrowSmoke(loc, this.map, 1.5f);
                    MoteMaker.ThrowMicroSparks(loc, this.map);
                    MoteMaker.ThrowLightningGlow(loc, this.map, 1.5f);
                }
            }
            SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, this.map, false), MaintenanceType.None);
            SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        }

        public override void WeatherEventDraw()
        {
            Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(MjolnirLightningStrike.LightningMat, base.LightningBrightness), 0);
        }
    }
}