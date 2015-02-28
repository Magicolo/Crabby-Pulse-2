using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {

	[Range(1, 4)] public int currentController = 1;
	
	void Awake() {
		PureData.OpenPatch("main");
	}
	
	void Update() {
		float xAxis = Input.GetAxisRaw("Horizontal");
		float yAxis = Input.GetAxisRaw("Vertical");
		float zAxis = 0;
		float trigger = Input.GetKey(KeyCode.JoystickButton0).GetHashCode();
		
		SendInput(currentController, xAxis, yAxis, zAxis, trigger);
	}
	
	public static void SendInput(int controllerIndex, float xAxis, float yAxis, float zAxis, float trigger) {
		PureData.Send("x_axis" + controllerIndex, xAxis);
		PureData.Send("y_axis" + controllerIndex, yAxis);
		PureData.Send("z_axis" + controllerIndex, zAxis);
		PureData.Send("trigger" + controllerIndex, trigger);
	}
}