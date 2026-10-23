#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Foundation.Configuration.Editor {
	internal sealed class ConfigurationManager : OdinMenuEditorWindow {
		[MenuItem("Tools/Foundation/Configuration Manager")]
		private static void OpenEditor() {
			ConfigurationManager window = GetWindow<ConfigurationManager>();
			window.titleContent = new UnityEngine.GUIContent("Configuration Manager", EditorGUIUtility.FindTexture("Settings"));
		}

		protected override OdinMenuTree BuildMenuTree() {
			OdinMenuTree tree = new OdinMenuTree();
			tree.AddAllAssetsAtPath(null, "Assets", typeof(AnyConfiguration), true, false);
			tree.SortMenuItemsByName();

			foreach (OdinMenuItem item in tree.RootMenuItem.GetChildMenuItemsRecursive(includeSelf: false)) {
				foreach (string omittedPrefix in OmittedPrefixes) {
					item.Name = item.Name.RemovePrefix(omittedPrefix);
				}
			}

			return tree;
		}

		// MARK: - Constants

		private static readonly string[] OmittedPrefixes = new string[] {
			"Configuration_",
			"CFG_",
		};
	}
}
#endif