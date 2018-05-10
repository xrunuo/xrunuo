using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.Testing
{
	public class TestRunner
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static int m_Failed = 0;

		public static void RunTests()
		{
			var testCaseTypes = ScriptCompiler.FindTypesByAttribute<TestCaseAttribute>();

			log.Info("Running {0} test suites", testCaseTypes.Count());

			foreach (var testCaseType in testCaseTypes)
			{
				object testCase = Activator.CreateInstance(testCaseType);

				RunTestMethods(testCase);
			}

			if (m_Failed == 0)
			{
				log.Info("Test passed");
			}
			else
			{
				log.Error("Tests failed ({0} errors)", m_Failed);
			}
		}

		private static void RunTestMethods(object testCase)
		{
			IEnumerable<MethodInfo> methods = testCase.GetType().GetMethods()
				.Where(m => m.GetCustomAttribute<TestAttribute>() != null);

			foreach (var method in methods)
			{
				try
				{
					method.Invoke(testCase, new object[] { });
					log.Info(testCase.GetType().FullName + "::" + method.Name + " - PASS");
				}
				catch (Exception e)
				{
					log.Error(testCase.GetType().FullName + "::" + method.Name + " - FAIL");
					m_Failed++;
				}
			}
		}
	}
}
