using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityTest.UnitTestRunner;

namespace UnityTest
{
	public static partial class Batch
	{
		private static string resultFilePathParam = "-resultFilePath=";
		private static string defaultResultFileName = "UnitTestResults.xml";

		public static void RunUnitTests ()
		{
			var resultFilePath = GetParameterArgument (resultFilePathParam) ?? Directory.GetCurrentDirectory ();
			if (Directory.Exists (resultFilePath))
				resultFilePath = Path.Combine (resultFilePath, defaultResultFileName);
			EditorApplication.NewScene ();
			UnitTestView.StartTestRun (new string[0], new TestRunnerEventListener (resultFilePath), false );
		}

		private static string GetParameterArgument ( string parameterName )
		{
			foreach (var arg in Environment.GetCommandLineArgs ())
			{
				if (arg.ToLower ().StartsWith (parameterName.ToLower ()))
				{
					return arg.Substring (parameterName.Length);
				}
			}
			return null;
		}

		private class TestRunnerEventListener : ITestRunnerCallback
		{
			private string resultFilePath;
			private List<ITestResult> results = new List<ITestResult> ();

			public TestRunnerEventListener (string resultFilePath)
			{
				this.resultFilePath = resultFilePath;
			}

			public void TestFinished (ITestResult test)
			{
				results.Add (test);
			}

			public void RunFinished ()
			{
				var resultDestiantion = Application.dataPath;
				if (!string.IsNullOrEmpty (resultFilePath))
					resultDestiantion = resultFilePath;
				var fileName = Path.GetFileName (resultDestiantion);
				if (!string.IsNullOrEmpty (fileName))
					resultDestiantion = resultDestiantion.Substring (0, resultDestiantion.Length - fileName.Length);
				else
					fileName = "UnitTestResults.xml";
#if !UNITY_METRO
				var resultWriter = new XmlResultWriter ("Unit Tests", results.ToArray ());
				resultWriter.WriteToFile (resultDestiantion, fileName);
#endif
			}

			public void TestStarted (string fullName)
			{
			}

			public void RunStarted (string suiteName, int testCount)
			{
			}

			public void RunFinishedException (Exception exception)
			{
				throw exception;
			}
		}
	}
}
