using System.Collections.Generic;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;

namespace IClearlyHaveEnough
{
	[HarmonyPatch(typeof(Designator_Build))]
	[HarmonyPatch("DrawMouseAttachments")]
	class PatcherMouseAttachment
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (CodeInstruction instruction in instructions)
			{
				if (instruction.opcode == OpCodes.Ldfld && instruction.operand == typeof(Map).GetField("resourceCounter")) continue;
				if (instruction.opcode == OpCodes.Callvirt && instruction.operand == typeof(ResourceCounter).GetMethod("GetCount"))
				{
					yield return new CodeInstruction(OpCodes.Call, typeof(PatcherMouseAttachment).GetMethod("GetPresentOnMap"));
					continue;
				}
				if (instruction.opcode == OpCodes.Ldstr && instruction.operand.Equals("NotEnoughStoredLower"))
				{
					yield return new CodeInstruction(OpCodes.Ldstr, "NotEnoughPresentLower");
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
