# Future release notes for FluentSecurity 2.0

**NOTE: These changes/features will not be available until version 2.0 is released.**

- (**Fixed**) Fixed issue with invariant culture.
- (**Fixed**) Fixed issue with RequireRolePolicy and RequireAllRolesPolicy resolving roles 3 times/execution.
- (**Fixed**) Fixed spelling of ISecurityContext methods.
- (**Removed**) Removed RemovePoliciesFor from ConfigurationExpression. Use RemovePolicy on IPolicyContainer and IConventionPolicyContainer.
- (**Removed**) Removed PolicyViolationException\<TSecurityPolicy\> as exceptions should be created based on PolicyResult.
- (**Removed**) Removed PolicyExecutionMode and changed the default policy execution behavior to "stop on first violation".
- (**Removed**) Removed PolicyAppender property from IPolicyContainer interface as it's not really useful to anyone.
- (**Change**) Method HandleSecurityFor on ISecurityHandler now takes an instance of ISecurityContext as the last parameter.
- (**Change**) Moved SecurityContextWrapper to the FluentSecurity.Policy.Contexts namespace.
- (**Change**) Moved PolicyViolationHandlerSelector to the FluentSecurity.Policy.ViolationHandlers namespace.
- (**Change**) Made the Each\<T\>, GetActionMethods and GetAreaName extensions internal as they were never intended to be public.
- (**Change**) Cleaned up the configuration API by hiding all the IList<T> related methods from ConfigurationExpression.
- (**Change**) Split IPolicyContainer in two (IPolicyContainer and IPolicyContainerConfiguration).
- (**Change**) Changed policy extensions so that they extend IPolicyContainerConfiguration instead of IPolicyContainer and IConventionPolicyContainer.
- (**Change**) Moved IWhatDoIHaveBuilder and DefaultWhatDoIHaveBuilder to Diagnostics namespace.
- (**Change**) Moved DefaultPolicyAppender to Configuration namespace.
- (**New**) Added support for using AllowAny to add an IgnorePolicy.
- (**New**) Exposed conventions through Conventions property on IAdvancedConfiguration and AdvancedConfiguration.

## Caching
- (**New**) Added support for changing the default cache lifecycle of policy results using the Advanced.SetDefaultResultsCacheLifecycle option.
- (**New**) Added support for providing custom cache keys for policies using the ICacheKeyProvider interface.
- (**New**) Added support for specifying a cache lifecycle using CacheResultsOf\<TSecurityPolicy\>. Supported lifecycles are DoNotCache, PerHttpRequest, PerHttpSession.
- (**New**) Added support for specifying a cache level using CacheResultsOf\<TSecurityPolicy\>. Supported levels are Controller, ControllerAction and Policy.
- (**New**) Added support for clearing cache strategies using ClearCacheStrategies and ClearStrategyFor\<TSecurityPolicy\>.

## ISecurityContext
- (**New**) Extended ISecurityContext with a Data property (dynamic) for adding and reading custom context data at runtime.
- (**New**) A RouteValueDictionary is by default added to ISecurityContext.Data.RouteValues when using the HandleSecurityAttribute.
- (**New**) Added support for modifying the ISecurityContext on creation using configuration.Advanced.ModifySecurityContext.
- (**New**) Added support for working with a typed ISecurityContext in policies by inheriting from SecurityPolicyBase\<TSecurityContext\>.
- (**New**) Added a MvcSecurityContext wrapper over ISecurityContext that makes it easier accessing route values in policies.

## AddPolicy\<T\> and dependency injection for policies
- (**New**) Added support for adding policies using AddPolicy\<T\>.
- (**New**) Added support for resolving policies from an IoC-container.
- (**New**) Added extension for getting the actual type of a policy (ISecurityPolicy.GetPolicyType).
- (**New**) Added support for verifying existence of policies added using AddPolicy\<T\> (*TestHelper*).
- (**New**) Added support for caching of policies added using AddPolicy\<T\>.

## Policy violation handlers
- (**Change**) Extracted existing conventions from PolicyViolationHandlerSelector to FindByPolicyNameConvention and FindDefaultPolicyViolationHandlerByNameConvention (IPolicyViolationHandlerConvention).
- (**New**) Added support for setting a default policy violation handler (DefaultPolicyViolationHandlerIsInstanceConvention, DefaultPolicyViolationHandlerIsOfTypeConvention).
- (**New**) Added support for specifying how violations are handled using configuration.Advanced.Violations(violations => {}).
- (**New**) Added support for adding and removing conventions using configuration.Advanced.Violations(violations => {}).
- (**New**) Added conventions PolicyTypeToPolicyViolationHandlerInstanceConvention, PolicyTypeToPolicyViolationHandlerTypeConvention, PredicateToPolicyViolationHandlerInstanceConvention and PredicateToPolicyViolationHandlerTypeConvention.
- (**New**) Added base classes (PolicyViolationHandlerFilterConvention, PolicyViolationHandlerTypeConvention, LazyInstancePolicyViolationHandlerConvention, LazyTypePolicyViolationHandlerConvention) to help with creating custom conventions (IPolicyViolationHandlerConvention).