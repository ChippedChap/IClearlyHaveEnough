using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace IClearlyHaveEnough.Patches
{
	[HarmonyPatch(typeof(Designator_Build))]
	[HarmonyPatch("DrawPanelReadout")]
	class PatcherDrawPanelReadout
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (CodeInstruction instruction in instructions)
			{
				if (instruction.opcode == OpCodes.Ldfld && instruction.operand == typeof(Map).GetField("resourceCounter")) continue;
				if (instruction.opcode == OpCodes.Callvirt && instruction.operand == typeof(ResourceCounter).GetMethod("GetCount"))
				{
					yield return new CodeInstruction(OpCodes.Call, typeof(PatcherDrawPanelReadout).GetMethod("GetPresentOnMap"));
					continue;
				}
				yield return instruction;
			}
		}

		public static int GetPresentOnMap(Map map, ThingDef def)
		{
			return map.GetComponent<AllResourcesCounter_MapComponent>().GetCount(def);
		}
	}
}
