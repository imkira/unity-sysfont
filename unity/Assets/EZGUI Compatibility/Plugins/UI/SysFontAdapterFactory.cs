using UnityEngine;

public class SysFontAdapterFactory : ILabelFactory
{
	public ILabelAdapter CreateLabel (ILabelAdapter baseLabel)
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
