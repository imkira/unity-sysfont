using System;
using NUnit.Core;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityTest
{
	[Serializable]
	public class UnitTestInfo
	{
		public string ParamName { get; private set; }
		public string MethodName { get; private set; }
		public string FullMethodName { get; private set; }
		public string ClassName { get; private set; }
		public string FullClassName { get; private set; }
		public string Namespace { get; private set; }
		public string FullName { get; private set; }

		public UnitTestInfo ( TestMethod testMethod )
		{
			if (testMethod == null)
				throw new ArgumentException();

			MethodName = testMethod.MethodName;
			FullMethodName = testMethod.Method.ToString ();
			ClassName = testMethod.FixtureType.Name;
			FullClassName = testMethod.ClassName;
			Namespace = testMethod.Method.ReflectedType.Namespace;
			FullName = testMethod.TestName.FullName;

			ParamName = ExtractMethodCallParametersString (FullName);
		}

		public UnitTestInfo (string testName)
		{
			FullName = testName;
		}

		public override bool Equals ( System.Object obj )
		{
			if (!(obj is UnitTestInfo)) return false;

			var testInfo = (UnitTestInfo) obj;
			return FullName == testInfo.FullName;
		}

		public override int GetHashCode ()
		{
			return FullName.GetHashCode ();
		}

		static string ExtractMethodCallParametersString (string methodFullName)
		{
			var match = Regex.Match (methodFullName, @"\((.*)\)");
			string result = "";
			if (match.Groups [1].Success) {
				result = match.Groups [1].Captures [0].Value;
			}
			return result;
		}
	}
}
