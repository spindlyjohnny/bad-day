using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBE : Weapon
{
    public GameObject projectile;
    public float maxdamage;
    public float baseDamage;
    public AudioClip chargeSound,reloadSound,noAmmoSound;
    Player player;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentammo = ammoInClip;
        maxammo = originalmaxammo;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Reload")) Reload(ammoInClip - currentammo);
        print("Damage " + damage);
    }
    public override void Fire() {
        if (currentammo > 0) {
            currentammo--;
        } else if (currentammo <= 0) {
            currentammo = 0;
            return;
        }
        Destroy(Instantiate(muzzleflash, firept.position, Quaternion.identity), .3f);
        GameObject go = Instantiate(projectile, firept.position, firept.rotation);
        go.GetComponent<Projectile>().damage = damage;
        AudioManager.instance.PlaySFX(shootSound);
        damage = baseDamage;
    }
    public void Charge() {
        if (currentammo == 0 || player.dead) {
            AudioManager.instance.PlaySFX(noAmmoSound);
            return;
        }
        StartCoroutine(ChargeCo());
    }
    public IEnumerator ChargeCo() {
        while (true) {
            damage += .1f;
            if (damage > maxdamage) {
                damage = maxdamage;
                break;
            }
            AudioManager.instance.PlaySFX(chargeSound);
            yield return new WaitForSeconds(.1f);
        }
    }
    public void Reload(/*int ammo*/) {
        if ((maxammo == currentammo) || (maxammo - ammoInClip <= 0)) return;
        currentammo += (ammoInClip - currentammo);
        AudioManager.instance.PlaySFX(reloadSound);
        maxammo -= (ammoInClip - currentammo);
    }
}
