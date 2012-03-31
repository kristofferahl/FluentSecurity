using System;
using System.Collections.Generic;

namespace FluentSecurity.Specification.Features.Helpers
{
	public class Givens<T> : List<Action<T>> where T : class {}
}