using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ProjectAnomalySyndicate
{
    public class CompProperties_FacilityObeliskSuppressor : CompProperties_Facility
    {
        public float activityDecreasePerTick = 1f;
        public CompProperties_FacilityObeliskSuppressor()
        {
            compClass = typeof(CompFacilityObeliskSuppressor);
        }


    }
    public class CompFacilityObeliskSuppressor : CompFacilityInactiveWhenElectricityDisabled
    {
        public new CompProperties_FacilityObeliskSuppressor Props => (CompProperties_FacilityObeliskSuppressor)props;
        public override void CompTick()
        {
            base.CompTick();
            if (CanBeActive)
            {
                foreach (Building building in LinkedBuildings)
                {
                    CompObelisk comp = building.TryGetComp<CompObelisk>();
                    comp?.ActivityComp.AdjustActivity(comp?.ActivityComp.suppressIfAbove < comp?.ActivityComp.ActivityLevel ? - Props.activityDecreasePerTick : 0);
                }
            }

        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            if(LinkedBuildings.Count > 0)
            {
                stringBuilder.Append("\n" + "SuppressingObelisk".Translate() + " ");
                stringBuilder.Append(string.Join(", ", LinkedBuildings.Select(building => building.LabelCap)));
            }
            else
            {
                stringBuilder.Append("\n" + "NotSuppressingObelisk".Translate());
            }
            

            return stringBuilder.ToString();
          
        }

    }

}
