using UnityEngine;
using System.Collections;

public class PresetControllr : MonoBehaviour {

	Color playerColor;
	public int playerIndex =4;
	public int instrumentNum;
	float currentXAxis, currentYAxis;
	
	private UniMoveController controller;
	// Use this for initialization
	void Start () {
		controller = gameObject.AddComponent<UniMoveController>();
		
		if(!controller.Init(playerIndex))
		{
			Debug.Log ("Could not initialize controller "+playerIndex);
		}
		
		controller.InitOrientation();
		controller.ResetOrientation();
		controller.SetLED(playerColor);
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
