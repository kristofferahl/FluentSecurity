//using System;
//using System.Collections.Generic;
//using System.Linq;
//using FluentSecurity.TestHelper.Specification.TestData;
//using NUnit.Framework;

//namespace FluentSecurity.TestHelper.Specification
//{
//    [TestFixture]
//    [Category("PolicyContainerExtensionsSpec")]
//    public class When_calling_expect_for_Sample_Index_on_policy_containers
//    {
//        private IEnumerable<IPolicyContainer> _policyContainers;

//        [SetUp]
//        public void Setup()
//        {
//            _policyContainers = FluentSecurityFactory.CreatePolicyContainers();
//        }

//        private PolicyExpectations Because()
//        {
//            return _policyContainers.Expect<SampleController>(x => x.Index());
//        }

//        [Test]
//        public void Should_have_one_expectation()
//        {
//            const int expectedExpectations = 1;

//            // Act
//            var expectations = Because();

//            // Assert
//            var totalExpectations = expectations.ExpectationsFor.Count();
//            Assert.That(totalExpectations, Is.EqualTo(expectedExpectations));
//        }

//        [Test]
//        public void Should_have_expectation_for_Sample_Index()
//        {
//            // Arrange
//            const string controller = "Sample";
//            const string action = "Index";
//            const int expectedExpectations = 1;

//            // Act
//            var expectations = Because();

//            // Assert
//            var matchingExpectations = expectations.ExpectationsFor.Count(x => x.Key.Equals(controller) && x.Value.Equals(action));
//            Assert.That(matchingExpectations, Is.EqualTo(expectedExpectations));
//        }

//        [Test]
//        public void Should_throw_when_policy_contianers_is_null()
//        {
//            // Arrange
//            const string expectedName = "policyContainers";
//            _policyContainers = null;

//            // Act
//            var result = Assert.Throws<ArgumentNullException>(() => Because());

//            // Assert
//            Assert.That(result.ParamName, Is.EqualTo(expectedName));
//        }
//    }

//    [TestFixture]
//    [Category("PolicyContainerExtensionsSpec")]
//    public class When_calling_expect_for_Sample_on_policy_containers
//    {
//        private IEnumerable<IPolicyContainer> _policyContainers;

//        [SetUp]
//        public void Setup()
//        {
//            _policyContainers = FluentSecurityFactory.CreatePolicyContainers();
//        }

//        private PolicyExpectations Because()
//        {
//            return _policyContainers.Expect<SampleController>();
//        }

//        [Test]
//        public void Should_have_4_expectations()
//        {
//            const int expectedExpectations = 4;

//            // Act
//            var expectations = Because();

//            // Assert
//            var totalExpectations = expectations.ExpectationsFor.Count();
//            Assert.That(totalExpectations, Is.EqualTo(expectedExpectations));
//        }

//        [Test]
//        public void Should_have_expectation_for_Sample_List()
//        {
//            // Arrange
//            const string controller = "Sample";
//            const string action = "List";
//            const int expectedExpectations = 1;

//            // Act
//            var expectations = Because();

//            // Assert
//            var matchingExpectations = expectations.ExpectationsFor.Count(x => x.Key.Equals(controller) && x.Value.Equals(action));
//            Assert.That(matchingExpectations, Is.EqualTo(expectedExpectations));
//        }

//        [Test]
//        public void Should_throw_when_policy_contianers_is_null()
//        {
//            // Arrange
//            const string expectedName = "policyContainers";
//            _policyContainers = null;

//            // Act
//            var result = Assert.Throws<ArgumentNullException>(() => Because());

//            // Assert
//            Assert.That(result.ParamName, Is.EqualTo(expectedName));
//        }
//    }
//}