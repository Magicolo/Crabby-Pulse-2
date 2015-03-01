using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {

	public Vector3 currentAxis;
	[Range(1, 4)] public int currentController = 1;
	
	void Awake() {
		PureData.OpenPatch("main");
	}
	
	void Update() {
		currentAxis = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetKey(KeyCode.JoystickButton0).GetHashCode());
		
		SendInput(currentController, currentAxis.x, currentAxis.y, 0, currentAxis.z);
	}
	
	public static void SendInput(int controllerIndex, float xAxis, float yAxis, float zAxis, float trigger) {
		PureData.Send("x_axis" + controllerIndex, xAxis);
		PureData.Send("y_axis" + controllerIndex, yAxis);
		PureData.Send("z_axis" + controllerIndex, zAxis);
		PureData.Send("trigger" + controllerIndex, trigger);
	}
}