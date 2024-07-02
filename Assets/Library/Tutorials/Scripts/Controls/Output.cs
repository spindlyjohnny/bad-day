using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction;

public class Output : MonoBehaviour {
	public TMP_Text leverText;
	public TMP_Text buttonText;
	public TMP_Text joystickText;
	public TMP_Text dialText;
	public TMP_Text sliderText;

	public void LeverOnLatched() {
		leverText.text = "ON";
	}

	public void ButtonOnPress() {
		leverText.text = "ON";
	}

	public void ButtonOnRelease() {
		leverText.text = "OFF";
	}

	//public void Joystick() {
	//	joystickText.text = 
	//}

	//public void DialTurned() {
	//	dialText.text = 
	//}

	//public void SliderShifted() {
	//	sliderText.text = On
	//}
}