using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentSecurity.Configuration;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using FluentSecurity.Specification.TestData.Controllers.AssemblyScannerControllers;
using FluentSecurity.Specification.TestData.Controllers.AssemblyScannerControllers.Exclude;
using FluentSecurity.Specification.TestData.Controllers.AssemblyScannerControllers.Include;
using FluentSecurity.Specification.TestData.Controllers.BaseControllers;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_creating_a_new_ConfigurationExpression
	{
		private static ConfigurationExpression Because()
		{
			return new RootConfiguration();
		}

		[Test]
		public void Should_not_contain_any_policycontainers()
		{
			// Act
			var configurationExpression = Because();

			// Assert
			var containers = configurationExpression.Runtime.PolicyContainers.Count();
			Assert.That(containers, Is.EqualTo(0));
		}

		[Test]
		public void Should_have_PolicyAppender_set_to_DefaultPolicyAppender()
		{
			// Arrange
			var expectedPolicyAppenderType = typeof(DefaultPolicyAppender);

			// Act
			var configurationExpression = Because();

			// Assert
			Assert.That(configurationExpression.PolicyAppender, Is.TypeOf(expectedPolicyAppenderType));
		}

		[Test]
		public void Should_have_Advanced_set_to_AdvancedConfigurationExpression()
		{
			// Arrange
			var expectedType = typeof(AdvancedConfiguration);

			// Act
			var configurationExpression = Because();

			// Assert
			Assert.That(configurationExpression.Advanced, Is.TypeOf(expectedType));
			Assert.That(configurationExpression.Advanced, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_Blog_Index
	{
		private ConfigurationExpression _configurationExpression;

		[SetUp]
		public void SetUp()
		{
			_configurationExpression = new RootConfiguration();
			_configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
		}

		private void Because()
		{
			_configurationExpression.For<BlogController>(x => x.Index());
		}

		[Test]
		public void Should_have_policycontainer_for_Blog_Index()
		{
			// Act
			Because();

			// Assert
			var policyContainer = _configurationExpression.Runtime.PolicyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), "Index");
			
			Assert.That(policyContainer, Is.Not.Null);
			Assert.That(_configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_PolicyAppender_set_to_PolicyAppender()
		{
			// Act
			Because();

			// Assert
			var policyContainer = (PolicyContainer) _configurationExpression.Runtime.PolicyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), "Index");
			Assert.That(policyContainer.PolicyAppender, Is.TypeOf(typeof(DefaultPolicyAppender)));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_aliased_action
	{
		[Test]
		public void Should_have_policycontainer_for_AliasedController_ActualAction()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<AliasedController>(x => x.ActualAction());

			// Assert
			var policyContainer = configurationExpression.Runtime.PolicyContainers.First();

			Assert.That(policyContainer.ActionName, Is.EqualTo("AliasedAction"));
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		private class AliasedController : Controller
		{
			[ActionName("AliasedAction")]
			public ActionResult ActualAction()
			{
				return null;
			}
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_void_action
	{
		[Test]
		public void Should_have_policycontainer_for_controller_with_void_action()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<ParentVoidActionController>(x => x.VoidAction());

			// Assert
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_policycontainer_for_void_action_from_parent_controller()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<ChildVoidActionController>(x => x.VoidAction());

			// Assert
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		private class ParentVoidActionController : Controller
		{
			public void VoidAction()
			{
				
			}
		}

		private class ChildVoidActionController:ParentVoidActionController
		{
			
		}
	}

	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_using_ByController_convention
	{
		[Test]
		public void Should_have_policycontainer_for_all_actions_including_void_actions()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<ParentVoidActionController>();

			// Assert
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(2));
		}

		public void Should_have_policycontainer_for_all_actions_including_inherited_void_actions()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<ChildVoidActionController>();

			// Assert
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(2));
		}

		private class ParentVoidActionController : Controller
		{
			public void VoidAction()
			{

			}

			public ActionResult DummyAction()
			{
				return new EmptyResult();
			}
		}

		private class ChildVoidActionController : ParentVoidActionController
		{
			public void VoidAction()
			{

			}

			public ActionResult DummyAction()
			{
				return new EmptyResult();
			}
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_Blog_Index_and_AddPost
	{
		[Test]
		public void Should_have_policycontainer_for_Blog_Index_and_AddPost()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.For<BlogController>(x => x.Index());
			configurationExpression.For<BlogController>(x => x.AddPost());

			// Assert
			var policyContainers = configurationExpression.Runtime.PolicyContainers.ToList();
			Assert.That(policyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), "AddPost"), Is.Not.Null);
			Assert.That(policyContainers.Count, Is.EqualTo(2));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_async_actions
	{
		[Test]
		public void Should_have_policycontainer_for_TaskController_LongRunningAction()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<TaskController>(x => x.LongRunningAction());

			// Assert
			var policyContainer = configurationExpression.Runtime.PolicyContainers.First();

			Assert.That(policyContainer.ControllerName, Is.EqualTo(NameHelper.Controller<TaskController>()));
			Assert.That(policyContainer.ActionName, Is.EqualTo("LongRunningAction"));
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_policycontainer_for_TaskController_LongRunningJsonAction()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<TaskController>(x => x.LongRunningJsonAction());

			// Assert
			var policyContainer = configurationExpression.Runtime.PolicyContainers.First();

			Assert.That(policyContainer.ControllerName, Is.EqualTo(NameHelper.Controller<TaskController>()));
			Assert.That(policyContainer.ActionName, Is.EqualTo("LongRunningJsonAction"));
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		private class TaskController : AsyncController
		{
			public Task<ActionResult> LongRunningAction()
			{
				return null;
			}

			public Task<JsonResult> LongRunningJsonAction()
			{
				return null;
			}
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_the_Blog_controller
	{
		private ConfigurationExpression _configurationExpression;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
		}

		private void Because()
		{
			_configurationExpression.For<BlogController>();
		}

		[Test]
		public void Should_have_policycontainers_for_all_actions()
		{
			// Arrange
			var expectedControllerName = NameHelper.Controller<BlogController>();

			// Act
			Because();

			// Assert
			var policyContainers = _configurationExpression.Runtime.PolicyContainers.ToList();
			Assert.That(policyContainers.GetContainerFor(expectedControllerName, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(expectedControllerName, "ListPosts"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(expectedControllerName, "AddPost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(expectedControllerName, "EditPost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(expectedControllerName, "DeletePost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(expectedControllerName, "AjaxList"), Is.Not.Null);
		}

		[Test]
		public void Should_have_6_policycontainers()
		{
			// Act
			Because();

			// Assert
			Assert.That(_configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(6));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_controller_with_aliased_action
	{
		[Test]
		public void Should_have_policycontainer_for_AliasedController_ActualAction()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			configurationExpression.For<AliasedController>();

			// Assert
			var policyContainer = configurationExpression.Runtime.PolicyContainers.First();

			Assert.That(policyContainer.ActionName, Is.EqualTo("AliasedAction"));
			Assert.That(configurationExpression.Runtime.PolicyContainers.Count(), Is.EqualTo(1));
		}

		private class AliasedController : Controller
		{
			[ActionName("AliasedAction")]
			public ActionResult ActualAction()
			{
				return null;
			}
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_calling_assembly : AssemblyScannerAssemblySpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions()
		{
			// Act & assert
			Because(configurationExpression =>
				configurationExpression.ForAllControllers()
				);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_specific_assembly : AssemblyScannerAssemblySpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions()
		{
			// Act & assert
			var assemblyWithoutControllers = typeof (SecurityConfigurator).Assembly;
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInAssembly(GetType().Assembly, assemblyWithoutControllers)
				);
		}

		[Test]
		public void Should_throw_when_assemblies_is_null()
		{
			// Act & assert
			Assert.Throws<ArgumentNullException>(() =>
				Because(configurationExpression =>
					configurationExpression.ForAllControllersInAssembly(null)
					)
				);
		}

		[Test]
		public void Should_throw_when_assembly_list_contains_null_assembly()
		{
			// Arrange
			var assemblies = new List<Assembly>
			{
				GetType().Assembly,
				null
			}.ToArray();

			// Act & assert
			Assert.Throws<ArgumentException>(() =>
				Because(configurationExpression =>
					configurationExpression.ForAllControllersInAssembly(assemblies)
					)
				);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_assembly_containing_type : AssemblyScannerAssemblySpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions()
		{
			// Act & assert
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInAssemblyContainingType<SomeClass>()
				);
		}

		internal class SomeClass {}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_inheriting : AssemblyScannerBaseSpecification
	{
		[Test]
		public void Should_have_policycontainers_for_base_and_inheriting_controllers_and_all_actions()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<IneritingBaseController>();
			var baseController = NameHelper.Controller<BaseController>();

			// Act
			Because(configurationExpression =>
					configurationExpression.ForAllControllersInheriting<BaseController>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(3));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(baseController, "InheritedAction"), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_base_and_inheriting_controllers_and_specific_action()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<IneritingBaseController>();
			var baseController = NameHelper.Controller<BaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<BaseController>(x => x.InheritedAction())
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(2));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(baseController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_inheriting_controllers_and_all_actions()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<IneritingAbstractBaseController>();
			var baseController = NameHelper.Controller<AbstractBaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<AbstractBaseController>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(2));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(baseController, "InheritedAction"), Is.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_inheriting_controllers_and_specific_action()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<IneritingAbstractBaseController>();
			var baseController = NameHelper.Controller<AbstractBaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<AbstractBaseController>(x => x.InheritedAction())
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(1));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Null);
			Assert.That(PolicyContainers.GetContainerFor(baseController, "InheritedAction"), Is.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_base_and_inheriting_controllers_and_all_actions_in_specified_assemblies()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<IneritingBaseController>();
			var baseController = NameHelper.Controller<BaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<BaseController>(GetType().Assembly, typeof(SecurityConfigurator).Assembly)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(3));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(baseController, "InheritedAction"), Is.Not.Null);
		}

		[Test]
		public void Should_have_no_policycontainers_for_base_and_inheriting_controllers_in_specified_assemblies()
		{
			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<BaseController>(typeof(SecurityConfigurator).Assembly)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_have_policycontainers_for_inheriting_controllers_and_all_actions_in_specified_assemblies()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<IneritingAbstractBaseController>();
			var baseController = NameHelper.Controller<AbstractBaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<AbstractBaseController>(GetType().Assembly, typeof(SecurityConfigurator).Assembly)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(2));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(baseController, "InheritedAction"), Is.Null);
		}

		[Test]
		public void Should_have_no_policycontainers_for_inheriting_controllers_in_specified_assemblies()
		{
			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<AbstractBaseController>(typeof(SecurityConfigurator).Assembly)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_have_policycontainers_for_inheriting_controllers_of_generic_base_controller_of_FirstInheritingEntity_FirstInheritingBaseViewModel()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<FirstInheritingGenericBaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<GenericBaseController<FirstInheritingEntity, FirstInheritingBaseViewModel>>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(2));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_inheriting_controllers_of_generic_base_controller_of_SecondInheritingEntity_SecondInheritingBaseViewModel()
		{
			// Arrange
			var inerhitingController = NameHelper.Controller<SecondInheritingGenericBaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<GenericBaseController<SecondInheritingEntity, SecondInheritingBaseViewModel>>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(2));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController, "FirstClassAction"), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_inheriting_controllers_of_generic_base_controller_of_BaseEntity_BaseViewModel()
		{
			// Arrange
			var inerhitingController1 = NameHelper.Controller<FirstInheritingGenericBaseController>();
			var inerhitingController2 = NameHelper.Controller<SecondInheritingGenericBaseController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<GenericBaseController<BaseEntity, BaseViewModel>>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(4));
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController1, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController1, "FirstClassAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController2, "InheritedAction"), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(inerhitingController2, "FirstClassAction"), Is.Not.Null);
		}

		[Test]
		public void Should_have_no_policycontainers_for_inheriting_controllers_of_generic_base_controller_of_BaseEntity_OtherViewModel()
		{
			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInheriting<GenericBaseController<BaseEntity, OtherViewModel>>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_throw_when_action_expresion_is_null()
		{
			var expression = new RootConfiguration();
			Expression<Func<AbstractBaseController, object>> actionExpression = null;
			Assert.Throws<ArgumentNullException>(() => expression.ForAllControllersInheriting(actionExpression));
		}

		[Test]
		public void Should_throw_when_assemblies_is_null()
		{
			var expression = new RootConfiguration();
			Assert.Throws<ArgumentNullException>(() => expression.ForAllControllersInheriting<AbstractBaseController>(x => x.InheritedAction(), null));
		}

		[Test]
		public void Should_throw_when_assemblies_contains_null()
		{
			var expression = new RootConfiguration();
			Assert.Throws<ArgumentException>(() => expression.ForAllControllersInheriting<AbstractBaseController>(x => x.InheritedAction(), null, null));
		}

		public class OtherViewModel : BaseViewModel {}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_namespace_containing_type : AssemblyScannerBaseSpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions_in_namespace_of_ClassInRootNamespace()
		{
			// Arrange
			const string index = "Index";
			var root = NameHelper.Controller<RootController>();
			var include = NameHelper.Controller<IncludedController>();
			var exclude = NameHelper.Controller<ExcludedController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInNamespaceContainingType<ClassInRootNamespace>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(3));
			Assert.That(PolicyContainers.GetContainerFor(root, index), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(include, index), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(exclude, index), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions_in_namespace_of_ClassInInvcludeNamespace()
		{
			// Arrange
			const string index = "Index";
			var include = NameHelper.Controller<IncludedController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInNamespaceContainingType<ClassInIncludeNamespace>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(1));
			Assert.That(PolicyContainers.GetContainerFor(include, index), Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_actions_matching : AssemblyScannerBaseSpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_Delete_actions_in_calling_assembly()
		{
			// Arrange
			const string expectedActionName = "DeletePost";

			// Act
			Because(configurationExpression =>
				configurationExpression.ForActionsMatching(x => x.ActionName == expectedActionName)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(1));
			Assert.That(PolicyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), expectedActionName), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_all_actions_starting_with_Edit_in_the_specified_assembly()
		{
			// Arrange
			const string expectedActionName = "EditPost";

			// Act
			Because(configurationExpression =>
				configurationExpression.ForActionsMatching(x => x.ActionName.StartsWith("Edit"), GetType().Assembly)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(1));
			Assert.That(PolicyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), expectedActionName), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_all_Index_actions_where_controller_is_BlogController()
		{
			// Arrange
			const string expectedActionName = "Index";

			// Act
			Because(configurationExpression =>
				configurationExpression.ForActionsMatching(x =>
					x.ActionName == expectedActionName &&
					x.ControllerType == typeof(BlogController)
					)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(1));
			Assert.That(PolicyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), expectedActionName), Is.Not.Null);
		}

		[Test]
		public void Should_not_have_any_policycontainers()
		{
			// Act & Assert
			Because(configurationExpression =>
				configurationExpression.ForActionsMatching(x => false)
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_expose_Controller_Action_and_ActionResult()
		{
			// Act & Assert
			Because(configurationExpression =>
				configurationExpression.ForActionsMatching(x =>
				{
					Assert.That(x.ControllerType, Is.Not.Null);
					Assert.That(x.ActionName, Is.Not.Empty);
					Assert.True(x.ActionResultType.IsControllerActionReturnType());
					return false;
				}));

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(0));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_scanning_for_profiles
	{
		[Test]
		public void Should_scan_for_profiles_avoiding_infinite_loop()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.Scan(scan =>
			{
				scan.AssembliesFromApplicationBaseDirectory(assembly => assembly.Equals(GetType().Assembly));
				scan.LookForProfiles();
			});

			// Assert
			Assert.That(configurationExpression.Runtime.Profiles.Count(), Is.EqualTo(3));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_applying_a_profile
	{
		[Test]
		public void Should_add_profile()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ApplyProfile<EmptyProfile>();

			// Assert
			Assert.That(configurationExpression.Runtime.Profiles.Single(), Is.EqualTo(typeof(EmptyProfile)));
		}

		[Test]
		public void Should_only_add_unique_profiles()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ApplyProfile<EmptyProfile>();
			configurationExpression.ApplyProfile<EmptyProfile>();

			// Assert
			Assert.That(configurationExpression.Runtime.Profiles.Single(), Is.EqualTo(typeof(EmptyProfile)));
		}

		public class EmptyProfile : SecurityProfile
		{
			public override void Configure() {}
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_pass_null_to_GetAuthenticationStatusFrom
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.GetAuthenticationStatusFrom(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_add_policies_before_specifying_a_function_returning_roles
	{
		[Test]
		public void Should_throw_ConfigurationErrorsException()
		{
			// Arrange
			var configurationExpression = new RootConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
			configurationExpression.For<BlogController>(x => x.Index());

			// Assert
			Assert.Throws<ConfigurationErrorsException>(() => configurationExpression.GetRolesFrom(StaticHelper.GetRolesExcludingOwner));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_pass_null_to_GetRolesFrom
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.GetRolesFrom(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_policyappender_to_null
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.SetPolicyAppender(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_servicelocator_to_null
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_policyappender_to_instance_of_DefaultPolicyAppender
	{
		[Test]
		public void Should_have_policyappender_set_to_instance_of_DefaultPolicyAppender()
		{
			// Arrange
			var expectedPolicyAppender = new DefaultPolicyAppender();
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.SetPolicyAppender(expectedPolicyAppender);

			// Assert
			configurationExpression.For<BlogController>(x => x.Index());
			var policyContainer = (PolicyContainer) configurationExpression.Runtime.PolicyContainers.GetContainerFor(NameHelper.Controller<BlogController>(), "Index");
			Assert.That(policyContainer.PolicyAppender, Is.EqualTo(expectedPolicyAppender));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_specify_a_default_policy_violation_handler
	{
		[Test]
		public void Should_clear_conflicting_conventions_and_add_convention_for_lazy_default_PolicyViolationHandler()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
			configurationExpression.Advanced.Conventions(conventions => 
			{
				conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
				conventions.Add(new DefaultPolicyViolationHandlerIsInstanceConvention<AnyPolicyViolationHandler>(() => new AnyPolicyViolationHandler()));
				conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<AnyPolicyViolationHandler>());
			});

			// Act
			configurationExpression.DefaultPolicyViolationHandlerIs<CustomDefaultPolicyViolationHandler>();

			// Assert
			var appliedConventions = configurationExpression.Runtime.Conventions.OfType<IPolicyViolationHandlerConvention>().ToList();
			Assert.That(appliedConventions.Count(), Is.EqualTo(2));
			Assert.That(appliedConventions.First(), Is.TypeOf<FindByPolicyNameConvention>());
			Assert.That(appliedConventions.Last(), Is.TypeOf<DefaultPolicyViolationHandlerIsOfTypeConvention<CustomDefaultPolicyViolationHandler>>());
		}

		[Test]
		public void Should_clear_conflicting_conventions_and_add_convention_for_lazy_default_PolicyViolationHandler_instance()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
			configurationExpression.Advanced.Conventions(conventions =>
			{
				conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
				conventions.Add(new DefaultPolicyViolationHandlerIsInstanceConvention<AnyPolicyViolationHandler>(() => new AnyPolicyViolationHandler()));
				conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<AnyPolicyViolationHandler>());
			});

			// Act
			configurationExpression.DefaultPolicyViolationHandlerIs(() => new CustomDefaultPolicyViolationHandler());

			// Assert
			var appliedConventions = configurationExpression.Runtime.Conventions.OfType<IPolicyViolationHandlerConvention>().ToList();
			Assert.That(appliedConventions.Count(), Is.EqualTo(2));
			Assert.That(appliedConventions.First(), Is.TypeOf<FindByPolicyNameConvention>());
			Assert.That(appliedConventions.Last(), Is.TypeOf<DefaultPolicyViolationHandlerIsInstanceConvention<CustomDefaultPolicyViolationHandler>>());
		}

		public class AnyPolicyViolationHandler : DefaultPolicyViolationHandler {}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_external_servicelocators : ConfigurationExpressionSpecification
	{
		[Test]
		public void Should_resolve_all_instances_from_services_locator()
		{
			// Arrange
			var concreteTypes = new List<ConcreteType1> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			FakeIoC.GetAllInstancesProvider = () => concreteTypes;
			Func<Type, IEnumerable<object>> servicesLocator = FakeIoC.GetAllInstances;
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(servicesLocator);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(concreteTypes));
		}

		[Test]
		public void Should_resolve_single_instance_from_services_locator()
		{
			// Arrange
			var concreteTypes = new List<ConcreteType1> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			FakeIoC.GetAllInstancesProvider = () => concreteTypes;
			Func<Type, IEnumerable<object>> servicesLocator = FakeIoC.GetAllInstances;
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(servicesLocator);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.Resolve(typeof(ConcreteType1)), Is.EqualTo(concreteTypes.First()));
		}

		[Test]
		public void Should_resolve_all_instances_from_services_locator_and_resolve_single_instance_from_single_service_locator()
		{
			// Arrange
			var concreteTypesInServiceLocator1 = new List<object> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			var concreteTypesInServiceLocator2 = new List<object> { new ConcreteType1(), new ConcreteType2() };

			FakeIoC.GetAllInstancesProvider = () => concreteTypesInServiceLocator1;
			FakeIoC.GetInstanceProvider = () => concreteTypesInServiceLocator2;

			Func<Type, IEnumerable<object>> servicesLocator = FakeIoC.GetAllInstances;
			Func<Type, object> singleServiceLocator = FakeIoC.GetInstance;

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(servicesLocator, singleServiceLocator);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(concreteTypesInServiceLocator1));
			Assert.That(externalServiceLocator.Resolve(typeof(ConcreteType2)), Is.EqualTo(concreteTypesInServiceLocator2.Last()));
		}

		[Test]
		public void Should_resolve_everything_from_implementation_of_ISecurityServiceLocator()
		{
			// Arrange
			IEnumerable<object> expectedTypes = new List<object> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			object expectedType = new ConcreteType2();

			var securityServiceLocator = new Mock<ISecurityServiceLocator>();
			securityServiceLocator.Setup(x => x.ResolveAll(typeof(ConcreteType1))).Returns(expectedTypes);
			securityServiceLocator.Setup(x => x.Resolve(typeof(ConcreteType2))).Returns(expectedType);

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(securityServiceLocator.Object);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(expectedTypes));
			Assert.That(externalServiceLocator.Resolve(typeof(ConcreteType2)), Is.EqualTo(expectedType));
			securityServiceLocator.VerifyAll();
		}

		[Test]
		public void Should_throw_when_serviceslocator_is_null()
		{
			// Arrange
			Func<Type, IEnumerable<object>> servicesLocator = null;
			Func<Type, object> singleServiceLocator = FakeIoC.GetInstance;

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act & assert
			var exception = Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(servicesLocator, singleServiceLocator));
			Assert.That(exception.ParamName, Is.EqualTo("servicesLocator"));
		}

		[Test]
		public void Should_throw_when_securityservicelocator_is_null()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act & assert
			var exception = Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(null));
			Assert.That(exception.ParamName, Is.EqualTo("securityServiceLocator"));
		}

		private class ConcreteType1 { }
		private class ConcreteType2 { }
	}

	public abstract class ConfigurationExpressionSpecification
	{
		protected static ISecurityServiceLocator GetExternalServiceLocator(ConfigurationExpression configurationExpression)
		{
			return configurationExpression.Runtime.ExternalServiceLocator;
		}
	}
}