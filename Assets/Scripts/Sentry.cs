using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    [SerializeField] GameObject deathvfx;
    // Start is called before the first frame update
    // Update is called once per frame
    protected override void Start() {
        //rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        hitpoints = maxhitpoints;
        player = FindObjectOfType<Player>();
        //levelManager = FindObjectOfType<LevelManager>();
        dead = false;
    }
    protected override void Update() {
        if (FindObjectOfType<PauseScreen>().pausescreen.activeSelf) return;
        anim.SetBool("Player", !player.dead && hit.collider != null && hit.collider.GetComponentInParent<Player>());
        Physics.Raycast(rayposition.position, rayposition.forward, out hit, 10);
        //Physics.BoxCast(rayposition.position, rayposition.localScale * .5f, rayposition.transform.forward, out hit, Quaternion.identity, 10);
        //print(hitpoints);
        if (hitpoints <= 0 && !dead)StartCoroutine(Death());
        if (!spawner.enabled) gameObject.SetActive(false);
    }
    protected override IEnumerator Death() {
        dead = true;
        PlayDeathSound();
        Destroy(Instantiate(deathvfx, transform.position, transform.rotation),.3f);
        yield return new WaitForSeconds(.3f);
        gameObject.SetActive(false);
        Drop();
    }
    protected override void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayposition.position, rayposition.position + 1 * rayposition.forward);
    }
}
