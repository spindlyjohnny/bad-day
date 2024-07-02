using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction : XRBaseInteractable {
	public static event Action<float> PullActionReleased;

	public Transform start, end;
	public GameObject notch;
	public float pullAmount { get; private set; } = 0.0f;

	private LineRenderer lineRenderer;
	private IXRSelectInteractor pullingInteractor = null;

	private AudioSource audioSource;

	protected override void Awake() {
		base.Awake();
		lineRenderer = GetComponent<LineRenderer>();
		audioSource = GetComponent<AudioSource>();
	}

	public void SetPullInteractor(SelectEnterEventArgs args) {
		pullingInteractor = args.interactorObject;
	}

	public void Release() {
		PullActionReleased?.Invoke(pullAmount);
		pullingInteractor = null;
		pullAmount = 0f;
		notch.transform.localPosition = new Vector3(notch.transform.localPosition.z, notch.transform.localPosition.y, 0);
		UpdateString();

		PlayReleaseSound();
	}

	public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
		base.ProcessInteractable(updatePhase);

		if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic) {
			if(isSelected) {
				Vector3 pullPosition = pullingInteractor.transform.position;
				pullAmount = CalculatePull(pullPosition);

				UpdateString();

				HapticFeedback();
			}
		}
	}

	private float CalculatePull(Vector3 pullPosition) {
		Vector3 pullDirection = pullPosition - start.position;
		Vector3 targetDirection = end.position - start.position;
		float maxLength = targetDirection.magnitude;

		targetDirection.Normalize();
		float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
		return Mathf.Clamp(pullValue, 0, 1);
	}

	private void UpdateString() {
		Vector3 linePosition = Vector3.right * Mathf.Lerp(start.transform.localPosition.x, end.transform.localPosition.x, pullAmount);
		notch.transform.localPosition = new Vector3(notch.transform.localPosition.z, notch.transform.localPosition.y, linePosition.x);
		lineRenderer.SetPosition(1, linePosition);
	}

	private void HapticFeedback() {
		if(pullingInteractor != null) {
			ActionBasedController currentController = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
			currentController.SendHapticImpulse(pullAmount, 0.1f);
        }
    }

	private void PlayReleaseSound() {
		audioSource.Play();
	}
}