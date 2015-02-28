using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {
	
	// We save a list of Move controllers.
	private List<UniMoveController> moves = new List<UniMoveController>();

	void Awake() {
		
		PureData.OpenPatch("main");
	}
	
	void Update() {
		
		
		float xAxis = Input.GetAxisRaw("Horizontal");
		float yAxis = Input.GetAxisRaw("Vertical");
		bool trigger = Input.GetButtonDown("Fire1");
		PureData.Send("y_axis", yAxis);
		PureData.Send("trigger", trigger);
	}
}	