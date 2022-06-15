// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]

public class FogOfWarMode : MonoBehaviour {

	public bool isDynamic;
	private Camera _camera;

	void Start () 
	{
		_camera = GetComponent<Camera>();
		_camera.clearFlags = CameraClearFlags.Color;
	}

	void OnPostRender () 
	{
		if(!isDynamic)
		{
			_camera.clearFlags = CameraClearFlags.Depth;
		}
	}
}
