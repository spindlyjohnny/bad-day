using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    Enemy enemy;
    // Start is called before the first frame update
    protected override void Start() {
        enemy = GetComponentInParent<Enemy>();
        //wait = new WaitForSeconds(1 / firerate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Fire() {
        Destroy(Instantiate(muzzleflash, firept.position, Quaternion.identity), .3f);
        if (enemy.hit.collider && enemy.hit.collider.GetComponent<Weapon>()) {
            enemy.player.Damaged(damage);
        }
        AudioManager.instance.PlaySFX(shootSound);
        //if(hit.collider)print(hit.collider.name);
        //if (hit.collider.GetComponent<Enemy>()) {
        //    hit.collider.GetComponent<Enemy>().Damaged(damage);
        //    //StartCoroutine(hit.collider.GetComponent<Enemy>().Hit());
        //}
        //ApplyRecoil();
    }
    //IEnumerator Shoot() {
    //    while (true) {
    //        GameObject go = Instantiate(projectile, firept.position, firept.rotation);
    //        yield return wait;
    //    }
    //}
}
