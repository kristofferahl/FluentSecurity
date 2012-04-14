# Future release notes for FluentSecurity 2.0

**NOTE: These changes/features will not be available until version 2.0 is released.**

- (**Removed**) Removed RemovePoliciesFor from ConfigurationExpression. Use RemovePolicy on IPolicyContainer and IConventionPolicyContainer.
- (**Removed**) Removed PolicyViolationException\<TSecurityPolicy\> as exceptions should be created based on PolicyResult.
- (**Removed**) Removed PolicyExecutionMode and changed the default policy execution behavior to "stop on first violation".
- (**New**) Added support for using AllowAny to add an IgnorePolicy.
- (**New**) Added support for changing the default cache lifecycle of policy results using the Advanced.SetDefaultResultsCacheLifecycle option.
- (**New**) Added support for providing custom cache keys for policies using the ICacheKeyProvider interface.
- (**New**) Added support for specifying a cache lifecycle using CacheResultsOf\<TSecurityPolicy\>. Supported lifecycles are DoNotCache, PerHttpRequest, PerHttpSession.
- (**New**) Added support for specifying a cache level using CacheResultsOf\<TSecurityPolicy\>. Supported levels are Controller, ControllerAction and Policy.
- (**New**) Added support for clearing cache strategies using ClearCacheStrategies and ClearStrategiesFor\<TSecurityPolicy\>.
- (**New**) Extended ISecurityContext with a Data property (dynamic) for adding and reading custom context data at runtime.
- (**New**) A RouteValueDictionary is by default added to ISecurityContext.Data.RouteValues when using the HandleSecurityAttribute.
- (**New**) Added support for modifying the ISecurityContext on creation using configuration.Advanced.ModifySecurityContext.
- (**New**) Added support for working with a typed ISecurityContext in policies by inheriting from SecurityPolicyBase\<TSecurityContext\>.
- (**New**) Added a MvcSecurityContext wrapper over ISecurityContext that makes it easier accessing route values in policies.
- (**Change**) Method HandleSecurityFor on ISecurityHandler now takes an instance of ISecurityContext as the last parameter.
- (**Change**) Moved SecurityContextWrapper to the FluentSecurity.Policy.Contexts namespace.