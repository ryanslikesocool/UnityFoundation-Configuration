using System.Collections.Generic;
using UnityEngine;

namespace Foundation {
	public sealed class ConfigurationRegistry : ScriptableObject {
		public static ConfigurationRegistry Shared { get; private set; }

		// MARK: - Fields

		[SerializeField, HideInInspector] internal List<AnyConfiguration> configurations = new();
		private Dictionary<System.Type, AnyConfiguration> configurationMap = default;

		// MARK: - Lifecyle

		private void OnEnable() {
			CreateConfigurationMap();

			Shared = this;
		}

		// MARK: -

		private void CreateConfigurationMap() {
			configurationMap = new();
			foreach (AnyConfiguration configuration in configurations) {
				TryRegisterConfiguration(configuration);
			}
		}

		private bool TryRegisterConfiguration<TConfiguration>(TConfiguration configuration) where TConfiguration : AnyConfiguration {
			// If passing in an object type-erased to `AnyConfiguration`,
			// `typeof(TConfiguration)` will return `AnyConfiguration`,
			// so instead, we have to use `configuration.GetType()`
			System.Type key = configuration.GetType();

			return configurationMap.TryAdd(key, configuration);
		}

		private bool UnregisterConfiguration<TConfiguration>(out TConfiguration configuration) where TConfiguration : AnyConfiguration {
			System.Type key = typeof(TConfiguration);

			if (
				configurationMap.TryGetValue(key, out AnyConfiguration untypedConfiguration)
				&& untypedConfiguration is TConfiguration typedConfiguration
			) {
				configuration = typedConfiguration;
				configurationMap.Remove(key);
				return true;
			} else {
				configuration = default;
				return false;
			}
		}

		public TConfiguration GetConfiguration<TConfiguration>() where TConfiguration : AnyConfiguration {
#if UNITY_EDITOR
			if (Application.isPlaying && configurations.Count != configurationMap?.Count) {
				CreateConfigurationMap();
			}
#endif

			return configurationMap[typeof(TConfiguration)] as TConfiguration;
		}
	}
}