namespace FluentSecurity
{
	public interface IRequestDescription
	{
		string AreName { get; }
		string ControllerName { get; }
		string ActionName { get; }
	}
}