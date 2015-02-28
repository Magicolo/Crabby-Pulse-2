using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {

	void Awake() {
		PureData.OpenPatch("main");
	}
	
	void Update() {
		float xAxis1 = Input.GetAxisRaw("Horizontal");
		float yAxis1 = Input.GetAxisRaw("Vertical");
		float zAxis1 = 0;
		bool trigger = Input.GetKey(KeyCode.JoystickButton0);
		PureData.Send("x_axis1", xAxis1);
		PureData.Send("y_axis1", yAxis1);
		PureData.Send("z_axis1", zAxis1);
		PureData.Send("trigger1", trigger);
	}
}