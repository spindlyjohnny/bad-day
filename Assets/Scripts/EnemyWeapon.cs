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
        bool hitplayer = Random.Range(0, 100) < enemy.accuracy; // enemy hits player only if rng falls within its accuracy
        if (hitplayer) {
            RaycastHit hit;
            if (Physics.Raycast(firept.position, FindObjectOfType<Camera>().transform.position - firept.position, out hit)) {
                //print(gameObject.name + " Hit: " + hit.collider);
                Player player = hit.collider.GetComponentInParent<Player>();
                if (player) player.Damaged(damage);
            }
            enemy.currentShotsTaken++;
            if(enemy.currentShotsTaken >= enemy.currentMaxShotsToTake) {
                StartCoroutine(enemy.ShootCo());
            }
        }
        //if (enemy.hit.collider && enemy.hit.collider.GetComponent<Weapon>() && hitplayer) {
        //    enemy.player.Damaged(damage);
        //}
        AudioManager.instance.PlaySFX(shootSound,true);
        //if(hit.collider)print(hit.collider.name);
        //if (hit.collider.GetComponent<Enemy>()) {
        //    hit.collider.GetComponent<Enemy>().Damaged(damage);
        //    //StartCoroutine(hit.collider.GetComponent<Enemy>().Hit());
        //}
        //ApplyRecoil();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firept.position, firept.position + FindObjectOfType<Camera>().transform.position - firept.position);
    }
    //IEnumerator Shoot() {
    //    while (true) {
    //        GameObject go = Instantiate(projectile, firept.position, firept.rotation);
    //        yield return wait;
    //    }
    //}
}
