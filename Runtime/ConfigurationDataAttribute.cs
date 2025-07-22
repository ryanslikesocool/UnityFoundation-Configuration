#if ODIN_INSPECTOR
using System;

namespace Foundation {
	/// <summary>
	/// An attribute used to mark scriptable objects so they can be displayed in the configuration manager editor window.
	/// </summary>
	public sealed class ConfigurationDataAttribute : Attribute { }
}
#endif