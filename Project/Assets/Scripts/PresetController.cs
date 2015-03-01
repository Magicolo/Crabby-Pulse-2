using UnityEngine;
using System.Collections;

public class PresetController : MonoBehaviour {

	Color playerColor;
	public int playerIndex =4;
	public int instrumentNum;
	float currentXAxis, currentYAxis;
	public ControllerManager p1,p2,p3,p4;
	
	public Preset[] sets;
	private UniMoveController controller;
	
	[System.Serializable]
	public class Preset {
		public Color led1;
		public Color led2;
		public Color led3;
		public Color led4;
		public int instrument1;
		public int instrument2;
		public int instrument3;
		public int instrument4;
	}
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
		
		if(controller.GetButtonDown(PSMoveButton.Circle))
		{
			preset (0);
		}
			else if(controller.GetButtonDown(PSMoveButton.Cross))
			{
				preset (1);
			}
				else if(controller.GetButtonDown(PSMoveButton.Square))
				{
					preset (2);		
				}
					else if(controller.GetButtonDown(PSMoveButton.Triangle))
					{	
						preset (3);
					}
						else if(controller.GetButtonDown(PSMoveButton.Move))
						{
			                 //disable this controller
						}
	}
	
	void preset(int i)
	{
		p1.playerColor = sets[i].led1;
		p2.playerColor = sets[i].led2;
		p3.playerColor = sets[i].led3;
		p4.playerColor = sets[i].led4;
		
		p1.instrumentNum = sets[i].instrument1;
		p2.instrumentNum = sets[i].instrument2;
		p3.instrumentNum = sets[i].instrument3;
		p4.instrumentNum = sets[i].instrument4;
	}
}
