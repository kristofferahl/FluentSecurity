using System;

namespace FluentSecurity.Configuration
{
	internal class ProfileImport
	{
		public ProfileImport(Type type)
		{
			Type = type;
		}

		public Type Type { get; private set; }
		public bool Completed { get; private set; }

		public void MarkCompleted()
		{
			Completed = true;
		}
	}
}