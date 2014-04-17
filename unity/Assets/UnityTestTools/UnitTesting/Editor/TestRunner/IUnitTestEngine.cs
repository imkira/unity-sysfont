namespace UnityTest
{
	public interface IUnitTestEngine
	{
		UnitTestResult[] GetTests ();
		void RunTests(string[] tests, UnitTestRunner.ITestRunnerCallback testRunnerEventListener);
	}
}
