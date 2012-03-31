Feature: Caching of PolicyResults
	In order to improve performance of applications using FluentSecurity
	As a developer
	I want the ability to cache results of policies

Scenario: Cache results of policy for all controllers

	Given the cache strategy of all controllers is set to PerHttpRequest for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "*_*_WriterPolicy" 


Scenario: Cache results of policy for specific controller

	Given the cache strategy of BlogController is set to PerHttpRequest for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "BlogController_*_WriterPolicy" 


Scenario: Cache results of policy for specific controller and action

	Given the cache strategy of BlogController AddPost is set to PerHttpRequest for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "BlogController_AddPost_WriterPolicy"


Scenario: Override cache lifecycle of policy for specific controller and action

	Given the cache strategy of BlogController is set to PerHttpRequest for WriterPolicy
	And the cache strategy of BlogController AddPost is set to DoNotCache for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should not cache result 


Scenario: Override cache lifecyle of policy for specific controller

	Given the cache strategy of BlogController AddPost is set to PerHttpSession for WriterPolicy
	And the cache strategy of BlogController is set to PerHttpRequest for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "BlogController_*_WriterPolicy"


#Scenario: Override cache lifecycle of policy for controller action matching convention


Scenario: Override cache level of policy for all controllers

	Given the cache strategy of all controllers is set to PerHttpRequest by ControllerAction for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "BlogController_AddPost_WriterPolicy"


Scenario: Override cache level of policy for specific controller

	Given the cache strategy of BlogController is set to PerHttpRequest by ControllerAction for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "BlogController_AddPost_WriterPolicy"


Scenario: Override cache level of policy for specific controller action

	Given the cache strategy of BlogController AddPost is set to PerHttpRequest by Policy for WriterPolicy
	When enforcing WriterPolicy for BlogController AddPost
	Then it should cache result PerHttpRequest
	Then it should cache result with key "*_*_WriterPolicy"


#Scenario: Clear cache strategies for specific controller


#Scenario: Clear cache strategies for specific controller action