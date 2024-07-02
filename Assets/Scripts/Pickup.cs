using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float value;
    Player player;
    bool grabbed;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        grabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Use") && grabbed && !player.dead) {
            if (gameObject.CompareTag("Health")) {
                player.hitpoints = Use(player.hitpoints, player.maxhitpoints);
            } 
            else if (gameObject.CompareTag("Shield")) {
                player.shieldpoints = Use(player.shieldpoints, player.maxshieldpoints);
            } 
            else if (gameObject.CompareTag("Ammo")) {
                player.currentweapon.maxammo = Use(player.currentweapon.maxammo, player.currentweapon.originalmaxammo);
            }
        }
    }
    public float Use(float resource,float resourceMax) {
        resource += value;
        if (resource > resourceMax) resource = resourceMax;
        gameObject.SetActive(false);
        return resource;
        //Destroy(gameObject, .3f);
    }
    public int Use(int resource, int resourceMax) {
        resource += (int)value;
        if (resource > resourceMax) resource = resourceMax;
        gameObject.SetActive(false);
        return resource;
        //Destroy(gameObject, .3f);
    }
    public void Grabbed(bool isGrabbed) {
        grabbed = isGrabbed;
    }
}
