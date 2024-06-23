using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TestMod.Patches;
using UnityEngine;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyTitle("ClassLibrary1")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ClassLibrary1")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("0a164a77-b539-4063-b68b-3341f0194610")]
[assembly: AssemblyFileVersion("1.0.0.0")]
//[assembly: TargetFramework(".NETFramework,Version=v4.8", FrameworkDisplayName = ".NET Framework 4.8")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace TestMod
{
	[BepInPlugin("NoOldBird", "No Old Bird Mod", "1.0.0")]
	public class NoOldBird : BaseUnityPlugin
	{
		private const string modGUID = "NoOldBirdMod";

		private const string modName = "No Old Bird Mod";

		private const string modVersion = "1.0.0";

		private readonly Harmony harmony = new Harmony("NoOldBirdMod");

		private static NoOldBird Instance;

		internal ManualLogSource mls;

		private void Awake()
		{
			if ((Object)(object)Instance == (Object)null)
			{
				Instance = this;
			}
			mls = BepInEx.Logging.Logger.CreateLogSource("NoOldBirdMod");
			mls.LogInfo((object)"The No Old Bird Mod is now active!");
			harmony.PatchAll(typeof(NoOldBird));
			harmony.PatchAll(typeof(NoOldBirdAtLoadPatch));
		}
	}
}
namespace TestMod.Patches
{
	[HarmonyPatch(typeof(RoundManager))]
	internal class NoOldBirdAtLoadPatch
	{
		[HarmonyPatch("LoadNewLevelWait")]
		[HarmonyPostfix]
		private static void RemoveOldBirdBeforePatch(ref SelectableLevel ___currentLevel)
		{
			List<SpawnableEnemyWithRarity> outsideEnemies = ___currentLevel.OutsideEnemies;
			SpawnableEnemyWithRarity val = null;
			foreach (SpawnableEnemyWithRarity item in outsideEnemies)
			{
				if (item.enemyType.enemyName == "RadMech")
				{
					val = item;
				}
			}
			if (val != null)
			{
				outsideEnemies.Remove(val);
				BepInEx.Logging.Logger.CreateLogSource("NoOldBirdMod").LogInfo((object)"Old Bird removed!");
			}
		}
	}
}
