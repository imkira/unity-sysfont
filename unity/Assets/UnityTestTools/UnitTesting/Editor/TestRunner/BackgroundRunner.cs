using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityTest.UnitTestRunner;

namespace UnityTest
{
	[InitializeOnLoad]
	public partial class UnitTestView
	{
		private static int nextCheck;
		private static string uttRecompile = "UTT-recompile";
		static bool shouldRunOnRecompilation { get { return EditorPrefs.GetBool ("UTR-runOnRecompilation"); } }

		static UnitTestView ()
		{
			EditorApplication.update += BackgroudRunner;
		}

		private static void BackgroudRunner ()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				EditorApplication.update -= BackgroudRunner;
				return;
			}
			if (EditorApplication.isCompiling)
			{
				if (!shouldRunOnRecompilation) return;
				EditorPrefs.SetString (uttRecompile, Application.dataPath);
				EditorApplication.update -= BackgroudRunner;
				return;
			}
			var t = (int) Time.realtimeSinceStartup;
			if (t < nextCheck)
				return;
			nextCheck = t + 1;

			if (!shouldRunOnRecompilation) return;

			if (EditorPrefs.HasKey (uttRecompile))
			{
				var recompile = EditorPrefs.GetString (uttRecompile);
				if (recompile == Application.dataPath && OkToRun ())
				{
					var currentScene = EditorApplication.currentScene;

					if (shouldRunTestOnANewScene || UnityEditorInternal.InternalEditorUtility.inBatchMode)
						EditorApplication.NewScene ();

					StartTestRun (new string[0], new BackgroundTestRunnerEventListener (), !shouldRunTestOnANewScene);
					
					if (shouldRunTestOnANewScene)
					{
						if (!string.IsNullOrEmpty (currentScene))
							EditorApplication.OpenScene (currentScene);
						else
							EditorApplication.NewScene ();
					}
				}
				EditorPrefs.DeleteKey (uttRecompile);
			}
		}

		private class BackgroundTestRunnerEventListener : ITestRunnerCallback
		{
			private bool anyFailed;
			private UnitTestView runnerWindow; 
			public BackgroundTestRunnerEventListener ()
			{
				var objs = Resources.FindObjectsOfTypeAll (typeof (UnitTestView));
				if (objs != null && objs.Count () == 1)
					runnerWindow = objs.Single () as UnitTestView;
			}

			public void TestFinished ( ITestResult result )
			{
				if(runnerWindow!=null) runnerWindow.UpdateTestInfo (result);
				if (result.Executed && !result.IsSuccess)
					anyFailed = true;
			}

			public void TestStarted (string fullName){}
			public void RunStarted (string suiteName, int testCount){}
			public void RunFinished ()
			{
				if (anyFailed) Debug.LogWarning ("Unit tests failed");
			}
			public void RunFinishedException (Exception exception)
			{
				RunFinished ();
			}
		}
	}
}
