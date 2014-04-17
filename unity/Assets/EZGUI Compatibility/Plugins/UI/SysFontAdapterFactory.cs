using UnityEngine;

public class SysFontAdapterFactory : ILabelFactory
{
	public ILabel CreateLabel (ILabel baseLabel)
	{
		SysFontAdapter newLabel = InstantiateLabel (baseLabel.Transform);
		newLabel.CopyPropertiesOf (baseLabel);

		return newLabel;
	}

	private SysFontAdapter InstantiateLabel (Transform parent)
	{
		GameObject obj = new GameObject ("SysFontAdapter");
		SysFontAdapter adapter = obj.AddComponent <SysFontAdapter> ();
		adapter.SetParent (parent);
		
		return adapter;
	}
}
