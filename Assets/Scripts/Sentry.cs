using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    [SerializeField] GameObject deathvfx;
    // Start is called before the first frame update
    // Update is called once per frame
    protected override void Update() {
        anim.SetBool("Player", !player.dead && hit.collider != null); // since raycast cannot detect player(changing player layer does nothing), raycast checks for player weapon instead.
        Physics.Raycast(rayposition.position, rayposition.forward, out hit, 10);
        //Physics.BoxCast(rayposition.position, rayposition.localScale * .5f, rayposition.transform.forward, out hit, Quaternion.identity, 10);
        //print(hitpoints);
        if (hitpoints <= 0 && !dead)StartCoroutine(Death());
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
