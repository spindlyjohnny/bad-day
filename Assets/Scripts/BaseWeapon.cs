using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : Weapon
{
    public float firerate;
    public AudioClip reloadSound,noAmmoSound;
    Player player;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        currentammo = ammoInClip;
        maxammo = originalmaxammo;
        player = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) Fire();
        if (Input.GetButtonUp("Fire1")) StopAllCoroutines();
    }
    public override void Fire() {
        if (player.dead) return;
        StartCoroutine(Shoot());
        //AudioManager.instance.PlaySFX(shootSound);
    }
    public IEnumerator Shoot() {
        while (true) {
            RaycastHit hit;
            Physics.Raycast(firept.position, firept.forward, out hit);
            if (currentammo > 0) {
                currentammo--;
            } 
            else if (currentammo <= 0) {
                currentammo = 0;
                AudioManager.instance.PlaySFX(noAmmoSound);
                break;
            }
            Destroy(Instantiate(muzzleflash, firept.position, Quaternion.identity), .3f);
            AudioManager.instance.PlaySFX(shootSound);
            if (hit.collider) {
                if (hit.collider.GetComponent<Enemy>()) {
                    hit.collider.GetComponent<Enemy>().Damaged(damage);
                }
                else if (hit.collider.GetComponentInParent<Enemy>()) {
                    hit.collider.GetComponentInParent<Enemy>().Damaged(damage);
                }
            }
            ApplyRecoil();
            yield return new WaitForSeconds(1 / firerate);
        }
    }
    public void Reload(int ammo) {
        if ((maxammo == currentammo) || (maxammo - ammoInClip <= 0)) return;
        currentammo += ammo;
        maxammo -= ammo;
        //if (ammo > 0) {
        //    maxammo -= ammo;
        //    ammo = ammoInClip;
        //} else {
        //    ammo = ammoInClip;
        //    maxammo -= ammoInClip;
        //}
        AudioManager.instance.PlaySFX(reloadSound);
    }
}
