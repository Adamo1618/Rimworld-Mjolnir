using System;
using Verse;
using RimWorld;

namespace RimWorld
{
    public class Verb_MjolnirLightningStrike : Verb
    {
        protected override bool TryCastShot()
        {
            if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
            {
                return false;
            }
            Map ThisMap = this.caster.Map;
            IntVec3 intVec = new IntVec3(this.currentTarget.Cell.x, this.currentTarget.Cell.y, this.currentTarget.Cell.z);
            ThisMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(ThisMap, intVec));
            GenExplosion.DoExplosion(intVec, ThisMap, 1.9f, DamageDefOf.Flame, null, 50, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
            return true;
        }
        public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
        {
            needLOSToCenter = false;
            return 15f;
        }
    }
}