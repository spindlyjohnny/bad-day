using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //public InputHelpers.Button button;
    //public XRController right;
    //public GameObject projectile;
    public Transform firept;
    //XRGrabInteractable weapon;
    public float recoilforce;
    //protected Rigidbody rb;
    public float damage;
    public GameObject muzzleflash;
    public int maxammo, currentammo,ammoInClip,originalmaxammo;
    public AudioClip shootSound;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //weapon = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract void Fire();
    //protected void ApplyRecoil() {
    //    rb.AddRelativeForce(Vector3.back * recoilforce, ForceMode.Impulse);
    //}
}
