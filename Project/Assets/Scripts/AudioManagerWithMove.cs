using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManagerWithMove : MonoBehaviourExtended {
	
	// We save a list of Move controllers.
	private List<UniMoveController> moves = new List<UniMoveController>();

	void Awake() {
		
		int count = UniMoveController.GetNumConnected();
		for (int i = 0; i < count; i++)
		{
			UniMoveController move = gameObject.AddComponent<UniMoveController>();	// It's a MonoBehaviour, so we can't just call a constructor


			// Remember to initialize!
			if (!move.Init(i))
			{
				Destroy(move);	// If it failed to initialize, destroy and continue on
				continue;
			}



			// This example program only uses Bluetooth-connected controllers
			PSMoveConnectionType conn = move.ConnectionType;
			if (conn == PSMoveConnectionType.Unknown || conn == PSMoveConnectionType.USB)
			{
				Destroy(move);
			}
			else
			{
				moves.Add(move);

				

				move.InitOrientation();
				move.ResetOrientation();

				// Start all controllers with a white LED
				move.SetLED(Color.white);
				
			}
			
		}
		PureData.OpenPatch("main");
	}
	
	void Update() {
		
		foreach(UniMoveController move in moves)
		{
			if (move.Disconnected) continue;
			
			if (move.GetButtonDown(PSMoveButton.Circle))		{move.SetLED(Color.cyan);}
			else if(move.GetButtonDown(PSMoveButton.Cross)) 	{move.SetLED(Color.red);}
			else if(move.GetButtonDown(PSMoveButton.Square)) 	{move.SetLED(Color.yellow);}
			else if(move.GetButtonDown(PSMoveButton.Triangle)) 	{move.SetLED(Color.magenta);}

			// On pressing the move button we reset the orientation as well.
			// Remember to keep the controller leveled and pointing at the screen
			// Reset once in a while because of drifting
			else if(move.GetButtonDown(PSMoveButton.Move)) {
				move.ResetOrientation();

				move.SetLED(Color.black);
			}
		}
		float xAxis = Input.GetAxisRaw("Horizontal");
		float yAxis = Input.GetAxisRaw("Vertical");
		bool trigger = Input.GetButtonDown("Fire1");
		PureData.Send("x_axis", xAxis);
		PureData.Send("y_axis", yAxis);
		PureData.Send("trigger", trigger);
	}
}	