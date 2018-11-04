using Harmony;
using RimWorld;
using Verse;

namespace IClearlyHaveEnough
{
	[HarmonyPatch(typeof(MainTabWindow_Architect))]
	[HarmonyPatch("PostOpen")]
	class UpdateOnArchitectOpen
	{
		static void Postfix()
		{
			Find.CurrentMap.GetComponent<AllResourcesCounter_MapComponent>().UpdateResourceCounts();
		}
	}
}
