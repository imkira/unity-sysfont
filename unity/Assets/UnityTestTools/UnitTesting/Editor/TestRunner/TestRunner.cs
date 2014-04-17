using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityTest.UnitTestRunner;

namespace UnityTest
{
	public partial class UnitTestView
	{

		internal void UpdateTestInfo (ITestResult result)
		{
			FindTestResultByName (result.FullName).Update (result);
		}

		private UnitTestResult FindTestResultByName (string name)
		{
			var idx = testList.FindIndex (testResult => testResult.Test.FullName == name);
			return testList.ElementAt (idx);
		}

		public void Update ()
		{
			if (shouldUpdateTestList)
			{
				shouldUpdateTestList = false;
				Repaint ();
			}
		}

		private void RunTests (string[] tests)
		{
			if (!OkToRun ()) return;

			var currentScene = EditorApplication.currentScene;
			if (runTestOnANewScene || UnityEditorInternal.InternalEditorUtility.inBatchMode)
				EditorApplication.NewScene ();

			StartTestRun (tests, new TestRunnerEventListener (UpdateTestInfo), !runTestOnANewScene);
			shouldUpdateTestList = true;

			if (runTestOnANewScene)
			{
				if (!string.IsNullOrEmpty (currentScene))
					EditorApplication.OpenScene (currentScene);
				else
					EditorApplication.NewScene ();
			}

			if (Event.current != null)
				GUIUtility.ExitGUI ();
		}

		private static bool OkToRun ()
		{
			var okToRun = true;
			if (shouldRunTestOnANewScene)
			{
				if (shouldAutoSaveSceneBeforeRun)
					EditorApplication.SaveScene ();
				okToRun = EditorApplication.SaveCurrentSceneIfUserWantsTo ();
			}
			return okToRun;
		}

		public static void StartTestRun (string[] testsToRunList, ITestRunnerCallback eventListener, bool performUndo)
		{
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
			if (performUndo)
				Undo.RegisterSceneUndo ("UnitTestRunSceneSave");
#else
			var undoGroup = Undo.GetCurrentGroup ();
#endif
			var callbackList = new TestRunnerCallbackList ();
			if (eventListener != null) callbackList.Add (eventListener);
			try
			{
				foreach (var unitTestEngine in TestEngines)
				{
					unitTestEngine.RunTests (testsToRunList, callbackList);
				}
			}
			catch (Exception e)
			{
				Debug.LogException (e);
				callbackList.RunFinishedException (e);
			}
			finally
			{
				if (performUndo)
				{
					var undoStartTime = DateTime.Now;
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
					Undo.PerformUndo ();
#else
					Undo.RevertAllDownToGroup (undoGroup);
#endif
					if ((DateTime.Now - undoStartTime).Seconds > 1)
						Debug.LogWarning ("Undo after unit test run took " + (DateTime.Now - undoStartTime).Seconds + " seconds. Consider running unit tests on a new scene for better performance.");
				}
				EditorUtility.ClearProgressBar ();
				if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
					EditorApplication.Exit (0);
			}
		}

		private class TestRunnerEventListener : ITestRunnerCallback
		{
			private Action<ITestResult> updateCallback;

			public TestRunnerEventListener ( Action<ITestResult> updateCallback )
			{
				this.updateCallback = updateCallback;
			}

			public void TestStarted (string fullName)
			{
				EditorUtility.DisplayProgressBar ("Unit Tests Runner",
												fullName,
												1);
			}

			public void TestFinished (ITestResult result)
			{
				updateCallback (result);
			}

			public void RunStarted (string suiteName, int testCount)
			{
			}

			public void RunFinished ()
			{
				EditorUtility.ClearProgressBar ();
			}

			public void RunFinishedException (Exception exception)
			{
				RunFinished ();
			}
		}
	}
}
