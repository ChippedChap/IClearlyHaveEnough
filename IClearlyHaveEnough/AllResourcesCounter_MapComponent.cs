using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IClearlyHaveEnough
{
	class AllResourcesCounter_MapComponent : MapComponent
	{
		private Dictionary<ThingDef, int> resourceCounts = new Dictionary<ThingDef, int>();
		private HashSet<ThingDef> resourceThingDefs = new HashSet<ThingDef>();

		public AllResourcesCounter_MapComponent(Map map) : base(map)
		{
		}

		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				UpdateResourceCounts();
			}
		}

		public override void FinalizeInit()
		{
			RefillDefs();
			UpdateResourceCounts();
		}

		public bool ShouldTrackThing(Thing thing)
		{
			return resourceThingDefs.Contains(thing.def);
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
			foreach (ThingDef resourceDef in resourceThingDefs)
			{
				List <Thing> listOfCurDef = map.listerThings.ThingsOfDef(resourceDef);
				int resourceCount = 0;
				for (int j = 0; j < listOfCurDef.Count; j++)
				{
					resourceCount += listOfCurDef[j].stackCount;
				}
				resourceCounts.Add(resourceDef, resourceCount);
			}
		}

		private void RefillDefs()
		{
			resourceThingDefs.Clear();
			resourceThingDefs = new HashSet<ThingDef>(DefDatabase<ThingDef>.AllDefs.Where(def => def.CountAsResource));
		}
	}
}
