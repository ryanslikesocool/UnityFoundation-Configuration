#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Foundation.Configuration.Editors {
	internal sealed class ConfigurationManager : OdinMenuEditorWindow {
		[MenuItem("Tools/Foundation/Configuration Manager")]
		private static void OpenEditor() => GetWindow<ConfigurationManager>();

		protected override OdinMenuTree BuildMenuTree() {
			OdinMenuTree tree = new OdinMenuTree();
			tree.AddAllAssetsAtPath("Configuration", "Assets/_Scriptable Objects/Configuration", typeof(AnyConfiguration), true, false);
			tree.SortMenuItemsByName();
			return tree;
		}
	}
}
#endif