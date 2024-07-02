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
    //public InputActionReference select;
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
        int index1 = currentweapon == weapons[0] ? 0 : 1;
        int index2 = currentweapon == weapons[0] ? 1 : 0;
        print(currentweapon);
        if (Input.GetButtonDown("Switch")) { // b button
            SwitchWeapon(weapons[index2].gameObject, weapons[index1].gameObject);
            print("gyatt");
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
    public void SwitchWeapon(GameObject weapon,GameObject switchedweapon) {
        switchedweapon.SetActive(false);
        weapon.SetActive(true);
        currentweapon = weapon.GetComponent<Weapon>();
    }
}
