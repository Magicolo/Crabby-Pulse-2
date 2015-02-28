using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {

	void Awake() {
		PureData.OpenPatch("main");
	}
	
	void Update() {
		float xAxis = Input.GetAxisRaw("Horizontal");
		float yAxis = Input.GetAxisRaw("Vertical");
		bool trigger = Input.GetKey(KeyCode.JoystickButton0);
		PureData.Send("x_axis", xAxis);
		PureData.Send("y_axis", yAxis);
		PureData.Send("trigger", trigger);
	}
}