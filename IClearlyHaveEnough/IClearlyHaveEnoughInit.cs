using Verse;
using Harmony;
using System.Reflection;

namespace IClearlyHaveEnough
{
	[StaticConstructorOnStartup]
	static class IClearlyHaveEnoughInit
	{
		static IClearlyHaveEnoughInit()
		{
			var harmonyInstance = HarmonyInstance.Create("com.chippedchap.iclearlyhaveenough");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
