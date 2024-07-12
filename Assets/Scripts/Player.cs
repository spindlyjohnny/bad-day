using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : Unit
{
    LevelManager levelManager;
    Camera cam;
    public GameObject hurtvfx;
    public float shieldpoints;
    public float maxshieldpoints;
    public Weapon currentweapon;
    public Weapon[] weapons;
    public AudioClip pickupsound,reloadSound;
    // Start is called before the first frame update
    void Start()
    {
        //select.action.started += SwitchWeapon;
        weapons = GetComponentsInChildren<Weapon>(true);
        currentweapon = weapons[0];
        currentweapon.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        hitpoints = maxhitpoints;
        shieldpoints = maxshieldpoints;
        dead = false;
        levelManager = FindObjectOfType<LevelManager>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    public override void TakeHit(float damage) {
        if(shieldpoints > 0) {
            shieldpoints -= damage;
        } 
        else {
            hitpoints -= damage;
        }
    }
    void Update() {
        if (hitpoints <= 0 && !dead) Death();
        if (dead) return;
        //int index1 = currentweapon == weapons[0] ? 0 : 1;
        //int index2 = currentweapon == weapons[0] ? 1 : 0;
        ////print(currentweapon);
        if (Input.GetButtonDown("Switch")) { // b button
            SwitchWeapon();
            //print("gyatt");
        }
        if (Input.GetButtonDown("Reload")) Reload();
    }
    public override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (other.GetComponent<Pickup>()) {
            Pickup pickup = other.GetComponent<Pickup>();
            if (pickup.gameObject.CompareTag("Health")) {
                hitpoints = Use(hitpoints, maxhitpoints,2);
            } 
            else if (pickup.gameObject.CompareTag("Shield")) {
                shieldpoints = Use(shieldpoints, maxshieldpoints,3);
            } 
            else if (pickup.gameObject.CompareTag("Ammo")) {
                if (currentweapon.GetComponent<BaseWeapon>()) {
                    currentweapon.maxammo = Use(currentweapon.maxammo, currentweapon.originalmaxammo,15);
                } 
                else {
                    currentweapon.maxammo = Use(currentweapon.maxammo, currentweapon.originalmaxammo, 1);
                }
            }
            other.gameObject.SetActive(false);
            AudioManager.instance.PlaySFX(pickupsound);
        }
    }
    public override IEnumerator Hit() {
        AudioManager.instance.PlaySFX(hitsound);
        Destroy(Instantiate(hurtvfx, cam.transform.position, Quaternion.identity),.5f);
        yield return null;
    }
    void Death() {
        dead = true;
        levelManager.gameoverscreen.SetActive(true);
        //canMove = false;
        PlayDeathSound();
    }
    public void SwitchWeapon() {
        if (weapons[0].gameObject.activeSelf) {
            weapons[0].gameObject.SetActive(false);
            weapons[1].gameObject.SetActive(true);
            currentweapon = weapons[1];
        } 
        else {
            weapons[1].gameObject.SetActive(false);
            weapons[0].gameObject.SetActive(true);
            currentweapon = weapons[0];
        }
        //switchedweapon.SetActive(false);
        //weapon.SetActive(true);
        //currentweapon = weapon.GetComponent<Weapon>();
    }
    public void Reload(/*int ammo*/) {
        if (currentweapon.maxammo == currentweapon.currentammo) return;
        int ammo = currentweapon.ammoInClip - currentweapon.currentammo;
        currentweapon.currentammo += ammo;//(currentweapon.ammoInClip - currentweapon.currentammo);
        currentweapon.maxammo -= ammo;//(currentweapon.ammoInClip - currentweapon.currentammo);
        //if (ammo > 0) {
        //    maxammo -= ammo;
        //    ammo = ammoInClip;
        //} else {
        //    ammo = ammoInClip;
        //    maxammo -= ammoInClip;
        //}
        AudioManager.instance.PlaySFX(reloadSound);
    }
    public float Use(float resource, float resourceMax,float value) {
        resource += value;
        if (resource > resourceMax) resource = resourceMax;
        //gameObject.SetActive(false);
        //Destroy(gameObject, .3f);
        return resource;
    }
    public int Use(int resource, int resourceMax,int value) {
        resource += value;
        if (resource > resourceMax) resource = resourceMax;
        //gameObject.SetActive(false);
        //Destroy(gameObject, .3f);
        return resource;
    }
}
