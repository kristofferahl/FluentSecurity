using System;

namespace FluentSecurity.Specification.TestData
{
	public class MockedHelper
	{
		/// <summary>
		/// Must be mocked and may not be user for anything else then mocking
		/// </summary>
		public virtual bool IsAuthenticatedReturnsTrue()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Must be mocked and may not be user for anything else then mocking
		/// </summary>
		public virtual object[] GetRoles()
		{
			throw new NotImplementedException();
		}
	}
}