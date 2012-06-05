using System;
using System.Configuration;
using System.Text;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	internal static class ExceptionFactory
	{
		public static Func<IRequestDescription> RequestDescriptionProvider;

		static ExceptionFactory()
		{
			Reset();
		}

		public static ConfigurationErrorsException CreateConfigurationErrorsException(string message)
		{
			var requestDesciption = RequestDescriptionProvider();
			var requestDescriptionString = CreateRequestDescriptionString(requestDesciption);
			return new ConfigurationErrorsException(message + requestDescriptionString);
		}

		public static string CreateRequestDescriptionString(IRequestDescription requestDescription)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			var areaName = requestDescription.AreaName.IsNullOrEmpty() ? "(not set)" : requestDescription.AreaName;
			stringBuilder.AppendLine("Area: {0}".FormatWith(areaName));
			stringBuilder.AppendLine("Controller: {0}".FormatWith(requestDescription.ControllerName));
			stringBuilder.Append("Action: {0}".FormatWith(requestDescription.ActionName));
			return stringBuilder.ToString();
		}

		public static void Reset()
		{
			RequestDescriptionProvider = () => ServiceLocator.Current.Resolve<IRequestDescription>();
		}
	}
}