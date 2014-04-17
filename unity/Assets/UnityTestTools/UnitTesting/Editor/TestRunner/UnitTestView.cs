using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityTest
{
	[Serializable]
	public partial class UnitTestView : EditorWindow
	{
		private static class Styles
		{
			public static GUIStyle buttonLeft;
			public static GUIStyle buttonMid;
			public static GUIStyle buttonRight;
			static Styles ()
			{
				buttonLeft = GUI.skin.FindStyle (GUI.skin.button.name + "left");
				buttonMid = GUI.skin.FindStyle (GUI.skin.button.name + "mid");
				buttonRight = GUI.skin.FindStyle (GUI.skin.button.name + "right");
			}
		}

		private static List<IUnitTestEngine> testEngines = null;
		private static IEnumerable<IUnitTestEngine> TestEngines {
			get
			{
				if(testEngines==null || testEngines.Count == 0)
					InstantiateUnitTestEngines ();
				return testEngines;
			}
		}

		[SerializeField]
		private List<UnitTestResult> testList = new List<UnitTestResult> ();

		#region renderer list
		[SerializeField]
		private int selectedRenderer;
		[SerializeField]
		private List<GroupedByHierarchyRenderer> rendererList = new List<GroupedByHierarchyRenderer> ()
		{
			new GroupedByHierarchyRenderer()
		};
		#endregion

		#region runner steering vars
		private Vector2 testListScroll, testInfoScroll, toolbarScroll;
		private bool shouldUpdateTestList;
		#endregion
		
		#region runner options vars
		private bool optionsFoldout;
		private bool runOnRecompilation;
		private bool horizontalSplit = true;
		private bool autoSaveSceneBeforeRun;
		private bool runTestOnANewScene;
		#endregion

		#region test filter vars
		private bool filtersFoldout;
		private string testFilter = "";
		private bool showFailed = true;
		private bool showIgnored = true;
		private bool showNotRun = true;
		private bool showSucceeded = true;
		private Rect toolbarRect;
		#endregion

		#region GUI Contents
		private readonly GUIContent guiRunSelectedTestsIcon = new GUIContent (Icons.runImg, "Run selected tests");
		private readonly GUIContent guiRunAllTestsIcon = new GUIContent (Icons.runAllImg, "Run all tests");
		private readonly GUIContent guiRerunFailedTestsIcon = new GUIContent (Icons.runFailedImg, "Rerun failed tests");
		private readonly GUIContent guiOptionButton = new GUIContent ("Options", Icons.gearImg);
		private readonly GUIContent guiRunOnRecompile = new GUIContent ("Run on recompile", "Run on recompile");
		private readonly GUIContent guiShowDetailsBelowTests = new GUIContent ("Show details below tests", "Show run details below test list");
		private readonly GUIContent guiRunTestsOnNewScene = new GUIContent ("Run tests on a new scene", "Run tests on a new scene");
		private readonly GUIContent guiAutoSaveSceneBeforeRun = new GUIContent ("Autosave scene", "The runner will automaticall save current scene changes before it starts");
		#endregion

		static bool shouldRunTestOnANewScene { get { return EditorPrefs.GetBool ("UTR-runTestOnANewScene"); } }
		static bool shouldAutoSaveSceneBeforeRun { get { return EditorPrefs.GetBool ("UTR-autoSaveSceneBeforeRun"); } }

		public UnitTestView ()
		{
			title = "Unit Tests Runner";

			if (EditorPrefs.HasKey ("UTR-runOnRecompilation"))
			{
				runOnRecompilation = EditorPrefs.GetBool ("UTR-runOnRecompilation");
				runTestOnANewScene = EditorPrefs.GetBool ("UTR-runTestOnANewScene");
				autoSaveSceneBeforeRun = EditorPrefs.GetBool ("UTR-autoSaveSceneBeforeRun");
				horizontalSplit = EditorPrefs.GetBool ("UTR-horizontalSplit");
				filtersFoldout = EditorPrefs.GetBool ("UTR-filtersFoldout");
				showFailed = EditorPrefs.GetBool ("UTR-showFailed");
				showIgnored = EditorPrefs.GetBool ("UTR-showIgnored");
				showNotRun = EditorPrefs.GetBool ("UTR-showNotRun");
				showSucceeded = EditorPrefs.GetBool ("UTR-showSucceeded");
			}
		}

		private static void InstantiateUnitTestEngines()
		{
			var type = typeof(IUnitTestEngine);
			var types =
				AppDomain.CurrentDomain.GetAssemblies()
						 .SelectMany(a => a.GetTypes())
						 .Where(type.IsAssignableFrom)
						 .Where(t => !t.IsInterface);
			IEnumerable<IUnitTestEngine> instances = types.Select(t => Activator.CreateInstance(t)).Cast<IUnitTestEngine>();
			testEngines = new List<IUnitTestEngine> ();
			testEngines.AddRange(instances);
		}

		public void SaveOptions()
		{
			EditorPrefs.SetBool("UTR-runOnRecompilation", runOnRecompilation);
			EditorPrefs.SetBool("UTR-runTestOnANewScene", runTestOnANewScene);
			EditorPrefs.SetBool("UTR-autoSaveSceneBeforeRun", autoSaveSceneBeforeRun);
			EditorPrefs.SetBool("UTR-horizontalSplit", horizontalSplit);
			EditorPrefs.SetBool("UTR-filtersFoldout", filtersFoldout);
			EditorPrefs.SetBool("UTR-showFailed", showFailed);
			EditorPrefs.SetBool("UTR-showIgnored", showIgnored);
			EditorPrefs.SetBool("UTR-showNotRun", showNotRun);
			EditorPrefs.SetBool("UTR-showSucceeded", showSucceeded);
		}

		public void OnEnable ()
		{
			RefreshTests ();
			shouldUpdateTestList = true;
		}

		public void Awake ()
		{
			GetRenderer ().Reset ();
			testList.Clear ();
			RefreshTests ();
			shouldUpdateTestList = true;
		}
		
		public void OnGUI ()
		{
			GUILayout.Space (10);
			EditorGUILayout.BeginVertical ();

			EditorGUILayout.BeginHorizontal ();

			var layoutOptions = new[] {
								GUILayout.Width(32),
								GUILayout.Height(24)
								};
			if (GUILayout.Button (guiRunAllTestsIcon, Styles.buttonLeft, layoutOptions))
			{
				RunTests (GetAllVisibleTests ());
			}
			if (GUILayout.Button (guiRunSelectedTestsIcon, Styles.buttonMid, layoutOptions))
			{
				RunTests(GetAllSelectedTests());
			}
			if (GUILayout.Button (guiRerunFailedTestsIcon, Styles.buttonRight, layoutOptions))
			{
				RunTests(GetAllFailedTests());
			}

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button (guiOptionButton, GUILayout.Height(24), GUILayout.Width(80)))
			{
				optionsFoldout = !optionsFoldout;
			}
			EditorGUILayout.EndHorizontal ();
			
			if (optionsFoldout) DrawOptions();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField("Filter:", GUILayout.Width(33));
			testFilter = EditorGUILayout.TextField(testFilter, EditorStyles.textField);
			if (GUILayout.Button(filtersFoldout ? "Hide" : "Advanced", GUILayout.Width(80)))
				filtersFoldout = !filtersFoldout;
			EditorGUILayout.EndHorizontal ();
			
			if (filtersFoldout)
				DrawFilters ();
			
			GUILayout.Box ("", new [] {GUILayout.ExpandWidth (true), GUILayout.Height (1)});

			GetRenderer ().RenderOptions ();

			if (horizontalSplit)
				EditorGUILayout.BeginVertical ();
			else
				EditorGUILayout.BeginHorizontal ();

			RenderToolbar ();
			RenderTestList ();

			if (horizontalSplit)
				GUILayout.Box ("", new[] {GUILayout.ExpandWidth (true), GUILayout.Height (1)});
			else
				GUILayout.Box ("", new[] {GUILayout.ExpandHeight (true), GUILayout.Width (1)});

			RenderTestInfo ();

			if (horizontalSplit)
				EditorGUILayout.EndVertical ();
			else
				EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();
		}

		private string[] GetAllVisibleTests ()
		{
			return FilterResults (testList).Select (result => result.Test.FullName).ToArray ();
		}

		private string[] GetAllSelectedTests ()
		{
			return GetRenderer ().GetSelectedTests ();
		}

		private string[] GetAllFailedTests ()
		{
			return FilterResults (testList).Where (result => result.IsError || result.IsFailure).Select (result => result.Test.FullName).ToArray ();
		}

		private void RenderToolbar ()
		{
			if (rendererList.Count > 1)
			{
				toolbarScroll = EditorGUILayout.BeginScrollView(toolbarScroll, GUILayout.ExpandHeight(false));

				EditorGUILayout.BeginHorizontal ();
				var toolbarList = rendererList.Select(hierarchyRenderer =>
				{
					var label = hierarchyRenderer.filterString;
					if (string.IsNullOrEmpty (label)) label = "All tests";
					return new GUIContent (label, label);
				}).ToArray();

				if (toolbarRect.Contains (Event.current.mousePosition) 
					&& Event.current.type == EventType.MouseDown 
					&& Event.current.button == 1)
				{
					var tabWidth = toolbarRect.width / rendererList.Count;
					var tabNo = (int)(Event.current.mousePosition.x / tabWidth);
					if(tabNo != 0)
					{
						var menu = new GenericMenu ();
						menu.AddItem (new GUIContent ("Remove"), false, ()=>RemoveSelectedTab(tabNo));
						menu.ShowAsContext ();
					}
					Event.current.Use ();
				}
				
				selectedRenderer = GUILayout.Toolbar(selectedRenderer, toolbarList);
				if (Event.current.type == EventType.Repaint)
					toolbarRect = GUILayoutUtility.GetLastRect ();

				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.EndScrollView();
			}
		}

		private void RemoveSelectedTab (int idx)
		{
			rendererList.RemoveAt (idx);
			selectedRenderer--;
		}

		private GroupedByHierarchyRenderer GetRenderer ()
		{
			var r = rendererList.ElementAtOrDefault (selectedRenderer);
			if (r == null)
			{
				selectedRenderer = 0;
				r = rendererList[selectedRenderer];
			}
			return r;
		}

		private void RenderTestList ()
		{
			testListScroll = EditorGUILayout.BeginScrollView (testListScroll,
																GUILayout.ExpandHeight (true),
																GUILayout.ExpandWidth (true),
																horizontalSplit ? GUILayout.MinHeight (0) : GUILayout.MaxWidth (500),
																horizontalSplit ? GUILayout.MinWidth (0) : GUILayout.MinWidth (200));

			var filteredResults = FilterResults (testList);
			if (rendererList.ElementAtOrDefault(selectedRenderer) == null)
				selectedRenderer = 0;
			shouldUpdateTestList = rendererList[selectedRenderer].RenderTests (filteredResults, RunTests);
			EditorGUILayout.EndScrollView ();
		}

		private void RenderTestInfo ()
		{
			testInfoScroll = EditorGUILayout.BeginScrollView (testInfoScroll,
																GUILayout.ExpandHeight (true),
																GUILayout.ExpandWidth (true),
																horizontalSplit ? GUILayout.MaxHeight (200) : GUILayout.MinWidth (0));

			GetRenderer().RenderInfo();
			EditorGUILayout.EndScrollView ();
		}

		private void DrawFilters ()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();
			showSucceeded = EditorGUILayout.Toggle("Show succeeded", showSucceeded, GUILayout.MinWidth (120));
			showFailed = EditorGUILayout.Toggle("Show failed", showFailed, GUILayout.MinWidth (120));
			EditorGUILayout.EndVertical ();
			EditorGUILayout.BeginVertical ();
			showIgnored = EditorGUILayout.Toggle("Show ignored", showIgnored);
			showNotRun = EditorGUILayout.Toggle("Show not runned", showNotRun);
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndHorizontal ();
			if (EditorGUI.EndChangeCheck())
				SaveOptions();
		}

		private void DrawOptions ()
		{
			EditorGUI.BeginChangeCheck ();
			runOnRecompilation = EditorGUILayout.Toggle (guiRunOnRecompile, runOnRecompilation);
			runTestOnANewScene = EditorGUILayout.Toggle (guiRunTestsOnNewScene, runTestOnANewScene);
			EditorGUI.BeginDisabledGroup (!runTestOnANewScene);
			autoSaveSceneBeforeRun = EditorGUILayout.Toggle (guiAutoSaveSceneBeforeRun, autoSaveSceneBeforeRun);
			EditorGUI.EndDisabledGroup ();
			horizontalSplit = EditorGUILayout.Toggle (guiShowDetailsBelowTests, horizontalSplit);
			if (EditorGUI.EndChangeCheck())
				SaveOptions();
			EditorGUILayout.Space ();
		}
		
		private IEnumerable<UnitTestResult> FilterResults (IEnumerable<UnitTestResult> mTestResults)
		{
			var results = mTestResults;

			if (!string.IsNullOrEmpty(GetRenderer ().filterString))
				results = results.Where(result => result.Test.FullClassName == GetRenderer().filterString);
			
			results = results.Where(r => r.Test.FullName.ToLower().Contains(testFilter.ToLower()));

			if (!showIgnored)
				results = results.Where (r => !r.IsIgnored);
			if (!showFailed)
				results = results.Where (r => !(r.IsFailure || r.IsError || r.IsInconclusive));
			if (!showNotRun)
				results = results.Where (r => r.Executed);
			if (!showSucceeded)
				results = results.Where (r => !r.IsSuccess);

			return results;
		}

		private void RefreshTests ()
		{
			var newTestResults = new List<UnitTestResult> ();
			var allTests = new List<UnitTestResult> ();
			foreach (var unitTestEngine in TestEngines)
			{
				allTests.AddRange (unitTestEngine.GetTests ());
			}

			foreach (var result in testList)
			{
				var test = allTests.SingleOrDefault (testResult => testResult.Test.Equals (result.Test));
				if (test != null)
				{
					newTestResults.Add (result);
					allTests.Remove(test);
				}
			}
			newTestResults.AddRange(allTests);
			testList = newTestResults;
		}

		
	}
}
