using NUnit.Core;

namespace UnityTest
{
	public static class NUnitExtensions
	{
		public static UnitTestResult UnitTestResult ( this NUnit.Core.TestResult result, string currentAssemblyPath )
		{
			return new UnitTestResult()
			{
				Executed = result.Executed,
				ResultState = (TestResultState)result.ResultState,
				Message = result.Message,
				StackTrace = result.StackTrace,
				Duration = result.Time,
				Test = new UnitTestInfo (result.Test.TestName.FullName),
				AssemblyPath = currentAssemblyPath
			};
		}
	}
}
