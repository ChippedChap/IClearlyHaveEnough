using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IClearlyHaveEnough
{
	class AllResourcesCounter_MapComponent : MapComponent
	{
		private Dictionary<ThingDef, int> resourceCounts = new Dictionary<ThingDef, int>();
		private List<ThingDef> resourceThingDefs = new List<ThingDef>();

		public AllResourcesCounter_MapComponent(Map map) : base(map)
		{
			RefillDefs();
			UpdateResourceCounts();
		}

		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				UpdateResourceCounts();
			}
		}

		public int GetCount(ThingDef def)
		{
			if (resourceCounts.ContainsKey(def))
			{
				return resourceCounts[def];
			}
			else
			{
				if (def.resourceReadoutPriority != ResourceCountPriority.Uncounted)
				{
					Log.Error("AllResourcesCounter_MapComponent from mod IClearlyHaveEnough was requested for a count of a ThingDef that does not have a key.");
				}
				return 0;
			}
		}

		public void UpdateResourceCounts()
		{
			resourceCounts.Clear();
			for (int i = 0; i < resourceThingDefs.Count; i++)
			{
				List <Thing> listOfCurDef = map.listerThings.ThingsOfDef(resourceThingDefs[i]);
				int resourceCount = 0;
				for (int j = 0; j < listOfCurDef.Count; j++)
				{
					resourceCount += listOfCurDef[j].stackCount;
				}
				resourceCounts.Add(resourceThingDefs[i], resourceCount);
			}
		}

		private void RefillDefs()
		{
			resourceThingDefs.Clear();
			resourceThingDefs.AddRange(DefDatabase<ThingDef>.AllDefs.Where(def => def.CountAsResource));
		}
	}
}
