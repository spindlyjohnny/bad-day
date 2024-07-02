using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FireProjectile : MonoBehaviour {
	public float speed;
	public GameObject projectile;
	public Transform spawnPoint;
 
	public static event Action GunFired;

	public void Fire() {
		//GetComponent<AudioSource>().Play();
		GameObject spawnedBullet = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
		spawnedBullet.GetComponent<Rigidbody>().velocity = speed * spawnPoint.forward;
		Destroy(spawnedBullet, 1.5f);
		GunFired?.Invoke();
	}
}