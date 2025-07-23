using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Foundation.Configuration.Editor {
	[CustomEditor(typeof(ConfigurationRegistry))]
	internal sealed class ConfigurationRegistryEditor : UnityEditor.Editor {
		private Vector2 scrollPosition = default;
		private ConfigurationRegistry registry = default;

		// MARK: - Lifecycle

		private void OnEnable() {
			registry = (ConfigurationRegistry)target;
		}

		// MARK: - GUI

		public override void OnInspectorGUI() {
			serializedObject.Update();

			OnConfigurationsGUI();

			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}

		private void OnConfigurationsGUI() {
			AnyConfiguration[] configurations = FindExistingConfigurations().ToArray();
			Array.Sort(configurations, (lhs, rhs) => lhs.name.CompareTo(rhs.name));

			using (EditorGUILayout.ScrollViewScope scrollViewScope = new(scrollPosition: scrollPosition)) {
				foreach (AnyConfiguration configuration in configurations) {
					OnConfigurationGUI(configuration);
				}

				scrollPosition = scrollViewScope.scrollPosition;
			}
		}

		private void OnConfigurationGUI(AnyConfiguration configuration) {
			using (new EditorGUILayout.HorizontalScope()) {
				using (EditorGUI.ChangeCheckScope changeCheckScope = new()) {
					bool currentState = registry.configurations.Contains(configuration);
					bool newState = EditorGUILayout.Toggle(
						label: GUIContent.none,
						value: currentState,
						options: GUILayout.Width(EditorGUIUtility.singleLineHeight)
					);

					if (changeCheckScope.changed) {
						UpdateState(currentState, newState);
					}
				}

				using (new EditorGUI.DisabledScope(disabled: true)) {
					EditorGUILayout.ObjectField(
						label: GUIContent.none,
						obj: configuration,
						objType: configuration.GetType(),
						allowSceneObjects: false
					);
				}
			}

			void UpdateState(bool previousValue, bool newValue) {
				switch (previousValue, newValue) {
					case (false, true):
						registry.configurations.Add(configuration);
						break;
					case (true, false):
						registry.configurations.Remove(configuration);
						break;
					default:
						break;
				}
			}
		}

		// MARK: -

		private static IEnumerable<AnyConfiguration> FindExistingConfigurations() {
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(AnyConfiguration)}");
			foreach (string guid in guids) {
				string path = AssetDatabase.GUIDToAssetPath(guid);
				yield return AssetDatabase.LoadAssetAtPath<AnyConfiguration>(path);
			}
		}
	}
}