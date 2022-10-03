using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamPos : MonoBehaviour {

	[SerializeField]
	MeshRenderer meshRenderer;
	[SerializeField]
	Transform camPos;
	void Update ()
	{
		meshRenderer.material.SetVector("_CamPosition", new Vector4(camPos.position.x, camPos.position.y, camPos.position.z, 0));
	}
}
