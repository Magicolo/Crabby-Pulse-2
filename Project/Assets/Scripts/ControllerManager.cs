using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour {
	
	
	public Color playerColor;
	public int playerIndex;
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
		
		float hue = Mathf.Clamp01(Mathf.Abs(Mathf.Lerp (currentYAxis,controller.Acceleration.y,Time.deltaTime*5)));
		playerColor = HSVtoRGB(hue,1,1,1);
		controller.SetLED(playerColor);
		currentXAxis =  controller.Acceleration.z;
		currentYAxis =  controller.Acceleration.y;	
		float zAxis = 0;
		float trigger = controller.Trigger;
		SendInput( instrumentNum, currentXAxis, currentYAxis, zAxis, trigger);
		
	}
	

	
		public static void SendInput(int controllerIndex, float xAxis, float yAxis, float zAxis, float trigger) {
		PureData.Send("x_axis" + controllerIndex, xAxis);
		PureData.Send("y_axis" + controllerIndex, yAxis);
		PureData.Send("z_axis" + controllerIndex, zAxis);
		PureData.Send("trigger" + controllerIndex, trigger);
	}
	
	void OnDrawGizmos () {
		playerIndex= Mathf.Clamp (playerIndex, 0, 4);
		instrumentNum =Mathf.Clamp (instrumentNum, 1, 5);
	}
		void OnGUI(){
		GUILayout.Label(currentXAxis + "x" +currentYAxis +"y");
	}
	
	
	 public static Color HSVtoRGB(float hue, float saturation, float value, float alpha)
    {
        while (hue > 1f) { hue -= 1f; }
        while (hue < 0f) { hue += 1f; }
        while (saturation > 1f) { saturation -= 1f; }
        while (saturation < 0f) { saturation += 1f; }
        while (value > 1f) { value -= 1f; }
        while (value < 0f) { value += 1f; }
        if (hue > 0.999f) { hue = 0.999f; }
        if (hue < 0.001f) { hue = 0.001f; }
        if (saturation > 0.999f) { saturation = 0.999f; }
        if (saturation < 0.001f) { return new Color(value * 255f, value * 255f, value * 255f); }
        if (value > 0.999f) { value = 0.999f; }
        if (value < 0.001f) { value = 0.001f; }

        float h6 = hue * 6f;
        if (h6 == 6f) { h6 = 0f; }
        int ihue = (int)(h6);
        float p = value * (1f - saturation);
        float q = value * (1f - (saturation * (h6 - (float)ihue)));
        float t = value * (1f - (saturation * (1f - (h6 - (float)ihue))));
        switch (ihue)
        {
            case 0:
                return new Color(value, t, p, alpha);
            case 1:
                return new Color(q, value, p, alpha);
            case 2:
                return new Color(p, value, t, alpha);
            case 3:
                return new Color(p, q, value, alpha);
            case 4:
                return new Color(t, p, value, alpha);
            default:
                return new Color(value, p, q, alpha);
        }
    }
}
