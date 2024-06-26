#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Foundation.Configuration.Editors {
	internal sealed class ConfigurationManager : OdinMenuEditorWindow {
		[MenuItem("Tools/Foundation/Configuration Manager")]
		private static void OpenEditor() => GetWindow<ConfigurationManager>();

		protected override OdinMenuTree BuildMenuTree() {
			OdinMenuTree tree = new OdinMenuTree();
			tree.AddAllAssetsAtPath(null, "Assets/_Scriptable Objects/Configuration", typeof(AnyConfiguration), true, false);
			tree.SortMenuItemsByName();

			foreach (OdinMenuItem item in tree.RootMenuItem.GetChildMenuItemsRecursive(includeSelf: false)) {
				item.Name = item.Name.RemovePrefix("Configuration_").RemovePrefix("CFG_");
			}

			return tree;
		}
	}
}
#endif