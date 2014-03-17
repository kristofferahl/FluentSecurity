using System;

namespace FluentSecurity
{
	public static class CoreConfigurator
	{
		private static readonly object LockObject = new object();

		public static Guid CorrelationId { get; private set; }

		static CoreConfigurator()
		{
			CorrelationId = Guid.NewGuid();
		}

		public static void Reset()
		{
			lock (LockObject)
			{
				CorrelationId = Guid.NewGuid();
			}
		}
	}
}