using UnityEngine;
using NSubstitute;
using NUnit.Framework;

namespace UnityTest
{
	[TestFixture]
	internal class HybridLabelTest 
	{
		private ILabelAdapter ezguiLabel;
		private ILabelAdapter sysFontLabel;
		private HybridLabel label;

		[SetUp]
		public void SetUp ()
		{
			ezguiLabel = Substitute.For<ILabelAdapter> ();
			sysFontLabel = Substitute.For<ILabelAdapter> ();

			label = new HybridLabel (ezguiLabel, sysFontLabel);
		}

		[Test]
		public void UseDefaultLabelByDefault ()
		{
			new HybridLabel (ezguiLabel, sysFontLabel);

			Assert.IsTrue (ezguiLabel.IsVisible);
			Assert.IsFalse (sysFontLabel.IsVisible);
		}

		[Test]
		public void ChangeTextOfActiveLabel ()
		{
			string str = "Lorem ipsum";
			label.Text = str;

			Assert.AreEqual (str, ezguiLabel.Text);
			Assert.AreNotEqual (str, sysFontLabel.Text);
		}

		[Test]
		public void ChangeActiveLabel ()
		{
			label.UseDefaultLabel = false;

			Assert.IsFalse (ezguiLabel.IsVisible);
			Assert.IsTrue (sysFontLabel.IsVisible);
		}

		[Test]
		public void RefreshPropertiesOnActivation ()
		{
			string strA = "Lorem ipsum";
			string strB = "dolor sit amet";

			label.Text = strA;
			label.UseDefaultLabel = false;
			Assert.AreEqual (strA, sysFontLabel.Text);

			label.Text = strB;
			label.UseDefaultLabel = true;
			Assert.AreEqual (strB, ezguiLabel.Text);
		}

		[Test]
		public void SpecialLabelCanBeNull ()
		{
			label = new HybridLabel (ezguiLabel);
			label.UseDefaultLabel = false;

			Assert.IsNotNull (label.EnabledLabel);
		}

		[Test]
		public void CreateSpecialFontOnDemand ()
		{
			var factory = Substitute.For<ILabelFactory> ();
			factory.CreateLabel (Arg.Any<ILabelAdapter> ()).Returns (sysFontLabel);

			label = new HybridLabel (ezguiLabel, factory);
			Assert.AreSame (ezguiLabel, label.EnabledLabel);

			label.UseDefaultLabel = false;
			Assert.AreSame (sysFontLabel, label.EnabledLabel);
		}
	}
}
