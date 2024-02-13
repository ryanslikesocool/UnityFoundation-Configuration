namespace Foundation.Configuration {
	public interface IConfigurable<Configuration> where Configuration : IConfiguration {
		Configuration configuration { get; }
	}
}