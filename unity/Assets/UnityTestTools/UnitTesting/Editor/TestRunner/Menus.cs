using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
	public partial class UnitTestView
	{
		[MenuItem ("Unity Test Tools/Unit Tests/Unit Test Runner %#&u")]
		public static void ShowWindow ()
		{
			GetWindow (typeof (UnitTestView)).Show ();
		}

		[MenuItem ("Unity Test Tools/Unit Tests/Run all unit tests")]
		public static void RunAllTestsBatch ()
		{
			var window = GetWindow (typeof (UnitTestView)) as UnitTestView;
			window.RefreshTests ();
			window.RunTests (new string[0]);
		}

		[MenuItem ("Assets/Unity Test Tools/Load tests from this file")]
		private static void LoadTestsFromFile (MenuCommand command)
		{
			if (!ValidateLoadTestsFromFile () && Selection.objects.Any ())
			{
				Debug.Log ("Not all selected files are script files");
			}
			var window = GetWindow (typeof (UnitTestView)) as UnitTestView;
			foreach (var o in Selection.objects)
			{
				var classType = (o as MonoScript).GetClass ();
				if (classType != null)
					window.selectedRenderer = window.AddNewRenderer (classType);
			}
			window.toolbarScroll = new Vector2 (float.MaxValue, 0);
		}

		private int AddNewRenderer (Type classFilter)
		{
			var elem = rendererList.SingleOrDefault (hierarchyRenderer => hierarchyRenderer.filterString == classFilter.FullName);
			if (elem == null)
			{
				elem = new GroupedByHierarchyRenderer (classFilter);
				rendererList.Add (elem);
			}
			return rendererList.IndexOf (elem);
		}

		[MenuItem ("Assets/Unity Test Tools/Load tests from this file", true)]
		private static bool ValidateLoadTestsFromFile ()
		{
			return Selection.objects.All (o => o is MonoScript);
		}
	}
}
