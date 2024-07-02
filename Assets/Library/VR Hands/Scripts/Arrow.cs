using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour {
	public float speed = 10f;
	public Transform tip;

	private Rigidbody rigidbody;
	private bool inAir = false;
	private Vector3 lastPosition = Vector3.zero;

	private ParticleSystem particleSystem;
	private TrailRenderer trailRenderer;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		particleSystem = GetComponentInChildren<ParticleSystem>();
		trailRenderer = GetComponentInChildren<TrailRenderer>();

		PullInteraction.PullActionReleased += Release;
		Stop();
	}

	private void OnDestroy() {
		PullInteraction.PullActionReleased -= Release;
	}

	private void Release(float value) {
		PullInteraction.PullActionReleased -= Release;
		gameObject.transform.parent = null;
		inAir = true;
		SetPhysics(true);

		Vector3 force = transform.forward * value * speed;
		rigidbody.AddForce(force, ForceMode.Impulse);

		StartCoroutine(RotateWithVelocity());
		lastPosition = tip.position;

		particleSystem.Play();
		trailRenderer.emitting = true;
	}

	private IEnumerator RotateWithVelocity() {
		yield return new WaitForFixedUpdate();

		while(inAir) {
			Quaternion newRotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);
			transform.rotation = newRotation;
			yield return null;
		}
	}

	private void FixedUpdate() {
		if(inAir) {
			CheckCollision();
			lastPosition = tip.position;
		}
	}

	private void CheckCollision() {
		if(Physics.Linecast(lastPosition, tip.position, out RaycastHit hitInfo)) {
			if(hitInfo.transform.gameObject.layer != 8) {
				if(hitInfo.transform.TryGetComponent(out Rigidbody body)) {
					rigidbody.interpolation = RigidbodyInterpolation.None;
					transform.parent = hitInfo.transform;
					body.AddForce(rigidbody.velocity, ForceMode.Impulse);
                }

				Stop();
            }
        }
	}

	private void Stop() {
		inAir = false;
		SetPhysics(false);

		particleSystem.Stop();
		trailRenderer.emitting = false;
    }

	private void SetPhysics(bool usePhysics) {
		rigidbody.useGravity = usePhysics;
		rigidbody.isKinematic = !usePhysics;
    }
}