using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManager : MonoBehaviour {
	[SerializeField] GameObject mockSimulator;

	void Start() {
		Debug.Log("Is device active: " + XRSettings.isDeviceActive);
		Debug.Log("Device name: " + XRSettings.loadedDeviceName);

		if(!XRSettings.isDeviceActive) {
			Debug.Log("No Headset plugged in");
			mockSimulator.SetActive(true);
		} else if(XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "MockHMD Display") {
			Debug.Log("Using mock HMD");
			mockSimulator.SetActive(true);
		} else {
			Debug.Log("Using headset" + XRSettings.loadedDeviceName);
			mockSimulator.SetActive(false);
		}
	}
	
	// Lock mouse cursor at center of game window and hide it
	// Press Esc key to see cursor
	//Cursor.lockState = CursorLockMode.Locked;
}