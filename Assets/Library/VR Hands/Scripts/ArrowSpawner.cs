using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour {
	public GameObject arrow;
	public GameObject notch;

	private XRGrabInteractable bow;
	private bool arrowNotched = false;
	private GameObject currentArrow = null;

	void Start() {
		bow = GetComponent<XRGrabInteractable>();
		PullInteraction.PullActionReleased += NotchEmpty;
	}

	private void OnDestroy() {
		PullInteraction.PullActionReleased -= NotchEmpty;
	}

	void Update() {
		if(bow.isSelected && arrowNotched == false) {
			arrowNotched = true;
			StartCoroutine("DelayedSpawn");
		}

		if(!bow.isSelected && currentArrow != null) {
			Destroy(currentArrow);
			NotchEmpty(1f);
        }
	}

	private void NotchEmpty(float value) {
		arrowNotched = false;
		currentArrow = null;
    }

	IEnumerator DelayedSpawn() {
		yield return new WaitForSeconds(1f);
		currentArrow = Instantiate(arrow, notch.transform);
    }
}