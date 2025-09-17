using Foundation.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Foundation.Configuration.Editor {
	internal sealed class ConfigurationRegistryProvider : SettingsProvider {
		private SerializedObject configurationRegistry;
		private UnityEditor.Editor configurationRegistryEditor;

		public ConfigurationRegistryProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			configurationRegistry = GetSerializedConfiguration();
			configurationRegistryEditor = UnityEditor.Editor.CreateEditor(configurationRegistry.targetObject);
		}

		// MARK: - Draw

		public override void OnGUI(string searchContext) {
			configurationRegistryEditor.OnInspectorGUI();

			configurationRegistry.ApplyModifiedPropertiesWithoutUndo();
		}

		// MARK: - Serialization

		private static ConfigurationRegistry GetOrCreateConfiguration() {
			ConfigurationRegistry settings = LoadExistingConfiguration();
			if (settings == null) {
				const string defaultSettingsPath = "Assets/Plugins/Developed With Love/Unity Foundation - Configuration/Configuration Registry.asset";
				settings = ScriptableObject.CreateInstance<ConfigurationRegistry>();

				FoundationEditorUtility.CreateDirectory(defaultSettingsPath);

				AssetDatabase.CreateAsset(settings, defaultSettingsPath);
				AssetDatabase.SaveAssets();
			}
			return settings;
		}

		private static SerializedObject GetSerializedConfiguration()
			=> new(GetOrCreateConfiguration());

		private static ConfigurationRegistry LoadExistingConfiguration() {
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(ConfigurationRegistry)}");
			if (guids.Length == 0) {
				return null;
			}
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);
			return AssetDatabase.LoadAssetAtPath<ConfigurationRegistry>(path);
		}

		// MARK: - Settings Provider

		[SettingsProvider]
		public static SettingsProvider CreateProjectSettingsProvider() {
			const string MENU_PATH = "Project/Developed With Love/Configuration Registry";

			return new ConfigurationRegistryProvider(MENU_PATH, SettingsScope.Project);
		}
	}
}