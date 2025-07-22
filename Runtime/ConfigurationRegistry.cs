using System.Collections.Generic;
using UnityEngine;

namespace Foundation {
	public sealed class ConfigurationRegistry : ScriptableObject {
		public static ConfigurationRegistry Shared { get; private set; }

		// MARK: - Fields

		[SerializeField] internal List<AnyConfiguration> configurations = new();
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
				System.Type configurationType = configuration.GetType();
				configurationMap[configurationType] = configuration;
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