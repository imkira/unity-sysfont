using UnityEngine;

public interface ILabelFactory
{
	ILabelAdapter CreateLabel (ILabelAdapter baseLabel);
}
