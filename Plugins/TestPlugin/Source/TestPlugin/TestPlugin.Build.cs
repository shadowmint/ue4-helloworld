using System;

namespace UnrealBuildTool.Rules {
	public class TestPlugin : ModuleRules {
		public TestPlugin(TargetInfo target) {
			SetupLocal(target);
		}

		/** Perform all the normal module setup for plugin local c++ files. */
		private void SetupLocal(TargetInfo target) {
			PublicIncludePaths.AddRange(new string[] {"Developer/TestPlugin/Public" });
			PrivateIncludePaths.AddRange(new string[] {"Developer/TestPlugin/Private" });
			PublicDependencyModuleNames.AddRange(new string[] {"Core"});
			PrivateDependencyModuleNames.AddRange(new string[] {});
			DynamicallyLoadedModuleNames.AddRange(new string[] {});
		}
	}
}
