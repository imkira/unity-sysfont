using UnityEngine;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnityTest
{
	[TestFixture]
	internal class SysFontAdapterFactoryTest 
	{
		private SysFontAdapterFactory factory;
		private List<GameObject> stubGameObj;

		private string 	default_text = "Lorem ipsum";
		private int default_fontSize = 12;
		private Label.Alignment default_alignment = Label.Alignment.Right;
		private Label.Pivot defaul_pivot = Label.Pivot.BottomRight;
		private Color default_color = Color.red;
		private bool default_multiline = true;
		private int default_maxWidth = 1001;

		[SetUp]
		public void SetUp ()
		{
			stubGameObj = new List<GameObject> ();
			factory = new SysFontAdapterFactory ();
		}

		[TearDown]
		public void TearDown ()
		{
			foreach (GameObject obj in stubGameObj)
			{
				GameObject.DestroyImmediate (obj);
			}
		}

		[Test]
		public void CreatesAndCloneLabelFromOther ()
		{
			ILabelAdapter baseLabel = GetDefaultFakeLabel ();
			ILabelAdapter label = factory.CreateLabel (baseLabel);

			Assert.IsNotNull (label);
			AssertDefaultLabelProperties (label);

			RegisterStubForTearDown (label.Transform.gameObject);
		}

		private Transform CreateStubTransform ()
		{
			GameObject obj = new GameObject ("(Test)SysFontAdapterFactory");
			obj.hideFlags = HideFlags.HideAndDontSave;
			RegisterStubForTearDown (obj);

			return obj.transform;

		}

		private void RegisterStubForTearDown (GameObject stub)
		{
			if (!stubGameObj.Contains (stub))
			{
				stubGameObj.Add (stub);
			}
		}

		private ILabelAdapter GetDefaultFakeLabel ()
		{
			var substitute = Substitute.For <ILabelAdapter> ();

			substitute.Text.Returns (default_text);
			substitute.FontSize.Returns (default_fontSize);
			substitute.Alignment.Returns (default_alignment);
			substitute.Pivot.Returns (defaul_pivot);
			substitute.FontColor.Returns (default_color);
			substitute.IsMultiLine.Returns (default_multiline);
			substitute.MaxWidth.Returns (default_maxWidth);

			return substitute;
		}

		private void AssertDefaultLabelProperties (ILabelAdapter label)
		{
			Assert.AreEqual (default_text, label.Text, "Text");
			Assert.AreEqual (default_fontSize, label.FontSize, "Font Size");
			Assert.AreEqual (default_alignment, label.Alignment, "Alignment");
			Assert.AreEqual (defaul_pivot, label.Pivot, "Pivot");
			Assert.AreEqual (default_color, label.FontColor, "Color");
			Assert.AreEqual (default_multiline, label.IsMultiLine, "Multiline");
			Assert.AreEqual (default_maxWidth, label.MaxWidth, "Max Width");
		}
	}
}
