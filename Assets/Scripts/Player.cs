using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Vector3 spawnPoint;
    Vector3 movement;
    Vector2 rotation;
    public float movespeed,rotationSpeed;
    // Start is called before the first frame update
    private void Awake() {
        levelManager = FindObjectOfType<LevelManager>();
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        weapons = GetComponentsInChildren<Weapon>(true);
        currentweapon = weapons[levelManager.saveData.currentweapon];
        currentweapon.currentammo = levelManager.saveData.currentammo;
        currentweapon.maxammo = levelManager.saveData.maxammo;
        currentweapon.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        hitpoints = levelManager.saveData.hitpoints;
        shieldpoints = levelManager.saveData.shieldpoints;
        spawnPoint = levelManager.saveData.spawnPoint;
        transform.position = spawnPoint;
        //hitpoints = maxhitpoints;
        //shieldpoints = maxshieldpoints;
        dead = false;
        //levelManager = FindObjectOfType<LevelManager>();
        cam = FindObjectOfType<Camera>();
        rotation = Vector2.zero;
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
        if (FindObjectOfType<PauseScreen>() && FindObjectOfType<PauseScreen>().pausescreen.activeSelf) return;
        if (hitpoints <= 0 && !dead) Death();
        if (dead) return;
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Switch")) { // b button
            SwitchWeapon();
            //print("gyatt");
        }
        if (Input.GetButtonDown("Reload")) Reload();
        Vector2 angularVel = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * rotationSpeed;
        rotation += angularVel * Time.deltaTime;
        transform.localEulerAngles = new Vector3(-Mathf.Clamp(rotation.y,-90f,90f), rotation.x, 0);
    }
    private void FixedUpdate() {
        rb.MovePosition(rb.position + movespeed * Time.deltaTime * movement);
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
        if (FindObjectOfType<PauseScreen>().pausescreen.activeSelf) yield break;
        AudioManager.instance.PlaySFX(hitsound);
        Destroy(Instantiate(hurtvfx, cam.transform.position, Quaternion.identity),.5f);
        yield return null;
    }
    void Death() {
        dead = true;
        levelManager.gameoverscreen.SetActive(true);
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
        if (currentweapon.maxammo == currentweapon.currentammo || (currentweapon.maxammo - currentweapon.ammoInClip <= 0)) return;
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
