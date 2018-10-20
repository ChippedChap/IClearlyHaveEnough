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
			var instrList = new List<CodeInstruction>(instructions);
			for (int i = 0; i < instrList.Count; i++)
			{
				if (instrList[i].opcode == OpCodes.Ldfld && instrList[i].operand == typeof(Map).GetField("resourceCounter")) continue;
				if (instrList[i].opcode == OpCodes.Callvirt && instrList[i].operand == typeof(ResourceCounter).GetMethod("GetCount"))
				{
					yield return new CodeInstruction(OpCodes.Call, typeof(PatcherMouseAttachment).GetMethod("GetPresentOnMap"));
					continue;
				}
				if (instrList[i].opcode == OpCodes.Ldstr && instrList[i].operand.Equals("NotEnoughStoredLower"))
				{
					yield return new CodeInstruction(OpCodes.Ldstr, "NotEnoughPresentLower");
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
