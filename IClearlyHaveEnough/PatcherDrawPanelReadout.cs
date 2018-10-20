using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace IClearlyHaveEnough
{
	[HarmonyPatch(typeof(Designator_Build))]
	[HarmonyPatch("DrawPanelReadout")]
	class PatcherDrawPanelReadout
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var instrList = new List<CodeInstruction>(instructions);
			for (int i = 0; i < instrList.Count; i++)
			{
				if (instrList[i].opcode == OpCodes.Ldfld && instrList[i].operand == typeof(Map).GetField("resourceCounter")) continue;
				if (instrList[i].opcode == OpCodes.Callvirt && instrList[i].operand == typeof(ResourceCounter).GetMethod("GetCount"))
				{
					yield return new CodeInstruction(OpCodes.Call, typeof(PatcherDrawPanelReadout).GetMethod("GetPresentOnMap"));
					continue;
				}
				yield return instrList[i];
			}
		}

		public static int GetPresentOnMap(Map map, ThingDef def)
		{
			return map.GetComponent<AllResourcesCounter_MapComponent>().GetCount(def);
		}
	}
}
