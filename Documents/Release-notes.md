# Release notes

## FluentSecurity 2.1.0

- ( **Fixed** ) Fixed issue with assemlbyscanning in SecurityDoctor.
- ( **New** ) Added option for disabling scanning for ISecurityEventListener's during configuration.
- ( **New** ) Added configuration option for configuring the assembly scanner used when scanning for ISecurityEventListener's.
- ( **New** ) Added support for IncludeAssembly and ExcludeAssembly with file predicates when using the AssemblyScanner.
- ( **New** ) Added support for ExcludeNamespaceContainingType\<T\> when using the AssemblyScanner.

## FluentSecurity 2.0.0

- ( **Fixed** ) Fixed issue with invariant culture (thanks to tsvayer).
- ( **Fixed** ) Fixed issue with RequireRolePolicy and RequireAllRolesPolicy resolving roles 3 times/execution.
- ( **Fixed** ) Fixed spelling of ISecurityContext methods (thanks to MariusSchulz).
- ( **Fixed** ) Made the constructor of PolicyViolationException public to enable unit testing of violation handlers.
- ( **Removed** ) Removed RemovePoliciesFor from ConfigurationExpression. Use RemovePolicy on IPolicyContainer and IConventionPolicyContainer.
- ( **Removed** ) Removed PolicyViolationException\<TSecurityPolicy\> as exceptions should be created based on PolicyResult.
- ( **Removed** ) Removed PolicyExecutionMode and changed the default policy execution behavior to "stop on first violation".
- ( **Removed** ) Removed PolicyAppender property from IPolicyContainer interface as it's not really useful to anyone.
- ( **Change** ) HandleSecurityAttribute now implements IAuthorizationFilter instead of inheriting from ActionFilterAttribute.
- ( **Change** ) Created RequireAnyRolePolicy and marked RequireRolePolicy as deprecated.
- ( **Change** ) Created RequireAnyRole extension and marked RequireRole extension as deprecated.
- ( **Change** ) Moved IgnoreMissingConfiguration option to Advanced property.
- ( **Change** ) Method HandleSecurityFor on ISecurityHandler now takes an instance of ISecurityContext as the last parameter.
- ( **Change** ) Moved SecurityContextWrapper to the FluentSecurity.Policy.Contexts namespace.
- ( **Change** ) Moved PolicyViolationHandlerSelector to the FluentSecurity.Policy.ViolationHandlers namespace.
- ( **Change** ) Made the Each\<T\>, GetActionMethods and GetAreaName extensions internal as they were never intended to be public.
- ( **Change** ) Cleaned up the configuration API by hiding all the IList<T> related methods from ConfigurationExpression.
- ( **Change** ) Split IPolicyContainer in two (IPolicyContainer and IPolicyContainerConfiguration).
- ( **Change** ) Changed policy extensions so that they extend IPolicyContainerConfiguration instead of IPolicyContainer and IConventionPolicyContainer.
- ( **Change** ) Moved IWhatDoIHaveBuilder and DefaultWhatDoIHaveBuilder to Diagnostics namespace.
- ( **Change** ) Moved DefaultPolicyAppender to Configuration namespace.
- ( **Change** ) Moved ITypeScanner interface to Scanning.TypeScanners namespace.
- ( **Change** ) Added ISecurityRuntime property to ISecurityContext.
- ( **New** ) Added support for using AllowAny to add an IgnorePolicy.
- ( **New** ) Added support for using ActionNameAttribute (thanks to Chandu).
- ( **New** ) Added HttpUnauthorizedPolicyViolationHandler for returning standard mvc HttpUnauthorizedResult.
- ( **New** ) Added support for async controllers (thanks to jansaris).
- ( **New** ) Added support for void actions (thanks to Chandu).
- ( **New** ) Added support for securing controllers based on inheritance - Base controllers (thanks to Ridermansb).
- ( **New** ) Added support for securing controllers based on action name and controller type.
- ( **New** ) Added support for asserting all controller action have been configured using AssertAllActionsAreConfigured (thanks to Chandu).
- ( **New** ) Made ISecurityContext available for violation handlers through the SecurityContext property of PolicyViolationException.
- ( **New** ) Exposed conventions through Conventions property on IAdvancedConfiguration and AdvancedConfiguration.
- ( **New** ) Improved diagnostics using the diagnostics pipeline and Glimpse tab (thanks to nikmd23).

### Areas/Profiles
- ( **New** ) Added support for creating profiles to help manage large/area configurations by inheriting from SecurityProfile.
- ( **New** ) Added support for applying profiles using ApplyProfile\<TSecurityProfile\>.
- ( **New** ) Added support for scanning for profiles using the Scan method and LookForProfiles.

### Caching
- ( **New** ) Added support for changing the default cache lifecycle of policy results using the Advanced.SetDefaultResultsCacheLifecycle option.
- ( **New** ) Added support for providing custom cache keys for policies using the ICacheKeyProvider interface.
- ( **New** ) Added support for specifying a cache lifecycle using Cache\<TSecurityPolicy\>. Supported lifecycles are DoNotCache, PerHttpRequest, PerHttpSession.
- ( **New** ) Added support for specifying a cache level using Cache\<TSecurityPolicy\>. Supported levels are Controller, ControllerAction and Policy.
- ( **New** ) Added support for clearing cache strategies using ClearCacheStrategies and ClearStrategyFor\<TSecurityPolicy\>.
- ( **New** ) Added support for specifying cache lifecycle and strategy using method chaining after AddPolicy\<TSecurityPolicy\>.

### ISecurityContext
- ( **New** ) Extended ISecurityContext with a Data property (dynamic) for adding and reading custom context data at runtime.
- ( **New** ) A RouteValueDictionary is by default added to ISecurityContext.Data.RouteValues when using the HandleSecurityAttribute.
- ( **New** ) Added support for modifying the ISecurityContext on creation using configuration.Advanced.ModifySecurityContext.
- ( **New** ) Added support for working with a typed ISecurityContext in policies by inheriting from SecurityPolicyBase\<TSecurityContext\>.
- ( **New** ) Added a MvcSecurityContext wrapper over ISecurityContext that makes it easier accessing route values in policies.
- ( **New** ) Extended ISecurityContext with an Id property for diagnostics and debugging purposes.

### AddPolicy\<T\> and dependency injection for policies
- ( **New** ) Added support for adding policies using AddPolicy\<T\>.
- ( **New** ) Added support for resolving policies from an IoC-container.
- ( **New** ) Added extension for getting the actual type of a policy (ISecurityPolicy.GetPolicyType).
- ( **New** ) Added support for verifying existence of policies added using AddPolicy\<T\> (*TestHelper*).
- ( **New** ) Added support for caching of policies added using AddPolicy\<T\>.

### Policy violation handlers
- ( **Change** ) Extracted existing conventions from PolicyViolationHandlerSelector to FindByPolicyNameConvention and FindDefaultPolicyViolationHandlerByNameConvention (IPolicyViolationHandlerConvention).
- ( **New** ) Added support for setting a default policy violation handler (DefaultPolicyViolationHandlerIsInstanceConvention, DefaultPolicyViolationHandlerIsOfTypeConvention).
- ( **New** ) Added support for specifying how violations are handled using configuration.Advanced.Violations(violations => {}).
- ( **New** ) Added support for adding and removing conventions using configuration.Advanced.Violations(violations => {}).
- ( **New** ) Added conventions PolicyTypeToPolicyViolationHandlerInstanceConvention, PolicyTypeToPolicyViolationHandlerTypeConvention, PredicateToPolicyViolationHandlerInstanceConvention and PredicateToPolicyViolationHandlerTypeConvention.
- ( **New** ) Added base classes (PolicyViolationHandlerFilterConvention, PolicyViolationHandlerTypeConvention, LazyInstancePolicyViolationHandlerConvention, LazyTypePolicyViolationHandlerConvention) to help with creating custom conventions (IPolicyViolationHandlerConvention).