using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Core;
using NUnit.Core.Filters;
using System.Linq;

namespace UnityTest
{
	public class NUnitTestEngine : IUnitTestEngine
	{
		private string[] assembliesWithTests = new[]
			{
				//"Assembly-CSharp", 
				"Assembly-CSharp-Editor", 
				//"Assembly-Boo", 
				"Assembly-Boo-Editor", 
				//"Assembly-UnityScript", 
				"Assembly-UnityScript-Editor"
			};

		public UnitTestResult[] GetTests()
		{
			return ReloadTestList();
		}

		private UnitTestResult[] ReloadTestList ()
		{
			List<String> assemblies = GetAssemblies ();
			TestSuite suite = PrepareTestSuite (assemblies);
			return ParseTestList (suite, "");
		}

		private UnitTestResult[] ParseTestList ( NUnit.Core.Test test, string currentAssemblyPath )
		{
			if (test.IsSuite)
			{
				var tests = new List<UnitTestResult> ();
				foreach (var obj in test.Tests)
				{
					if (obj is TestAssembly && File.Exists ((obj as TestAssembly).TestName.FullName))
						currentAssemblyPath = (obj as TestAssembly).TestName.FullName;

					if (obj is NUnit.Core.Test)
					{
						var results = ParseTestList (obj as NUnit.Core.Test, currentAssemblyPath);
						tests.AddRange (results);
					}
				}
				return tests.ToArray ();
			}
			else
			{
				return new[] { CreateNewTestResult (test as TestMethod, currentAssemblyPath) };
			}
		}

		private UnitTestResult CreateNewTestResult ( TestMethod test, string currentAssemblyPath )
		{
			return new UnitTestResult ()
			{
				Message = "",
				StackTrace = "",
				Duration = 0,
				Test = CreateUnitTestInfo(test),
				AssemblyPath = currentAssemblyPath
			};
		}

		private UnitTestInfo CreateUnitTestInfo (TestMethod test)
		{
			//this shouldn't be initialize by constructor
			//return test.UnitTestInfo ();
			return new UnitTestInfo (test);
		}

		public void RunTests(string[] tests, UnitTestRunner.ITestRunnerCallback testRunnerEventListener)
		{
			List<String> assemblies = GetAssemblies();
			TestSuite suite = PrepareTestSuite(assemblies);

			ITestFilter filter = TestFilter.Empty;
			if (tests != null && tests.Any())
				filter = new SimpleNameFilter(tests);
			
			if (testRunnerEventListener!=null)
				testRunnerEventListener.RunStarted(suite.TestName.FullName, suite.TestCount);
			
			ExecuteTestSuite(suite, testRunnerEventListener, filter);
			
			if (testRunnerEventListener!=null)
				testRunnerEventListener.RunFinished();
		}

		private List<String> GetAssemblies()
		{
			var assemblyList = new List<String>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				var assemblyName = assembly.GetName().Name;
				if (assembliesWithTests.Contains(assemblyName)) assemblyList.Add (assembly.Location);
			}

			return assemblyList;
		}

		private TestSuite PrepareTestSuite(List<String> assemblyList)
		{
			CoreExtensions.Host.InitializeService();
			var testPackage = new TestPackage("Unity", assemblyList);
			var builder = new TestSuiteBuilder();
			TestExecutionContext.CurrentContext.TestPackage = testPackage;
			TestSuite suite = builder.Build(testPackage);
			return suite;
		}

		private void ExecuteTestSuite(TestSuite suite, UnitTestRunner.ITestRunnerCallback testRunnerEventListener, ITestFilter filter)
		{
			EventListener eventListener;
			if (testRunnerEventListener == null)
				eventListener = new NullListener ();
			else
				eventListener = new TestRunnerEventListener (testRunnerEventListener);
			suite.Run(eventListener, filter);
		}

		private class TestRunnerEventListener : EventListener
		{
			private UnitTestRunner.ITestRunnerCallback testRunnerEventListener;

			public TestRunnerEventListener(UnitTestRunner.ITestRunnerCallback testRunnerEventListener)
			{
				this.testRunnerEventListener = testRunnerEventListener;
			}

			public void RunStarted(string name, int testCount)
			{
				testRunnerEventListener.RunStarted(name, testCount);
			}

			public void RunFinished(NUnit.Core.TestResult result)
			{
				testRunnerEventListener.RunFinished();
			}

			public void RunFinished(Exception exception)
			{
				testRunnerEventListener.RunFinishedException(exception);
			}

			public void TestStarted(NUnit.Core.TestName testName)
			{
				testRunnerEventListener.TestStarted(testName.FullName);
			}

			public void TestFinished(NUnit.Core.TestResult result)
			{
				testRunnerEventListener.TestFinished(result.UnitTestResult(""));
			}

			public void SuiteStarted(NUnit.Core.TestName testName)
			{
			}

			public void SuiteFinished(NUnit.Core.TestResult result)
			{
			}

			public void UnhandledException(Exception exception)
			{
			}

			public void TestOutput(NUnit.Core.TestOutput testOutput)
			{
			}
		}
	}
}
