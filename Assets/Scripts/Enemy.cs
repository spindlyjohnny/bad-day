using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Unit
{
    protected EnemyWeapon weapon;
    public RaycastHit hit; // for detecting player
    [HideInInspector]public Player player;
    public Transform rayposition;
    public GameObject[] drops;
    [SerializeField]protected float rng;
    protected int dropindex;
    //LevelManager levelManager;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        hitpoints = maxhitpoints;
        player = FindObjectOfType<Player>();
        //levelManager = FindObjectOfType<LevelManager>();
        dead = false;
        //canMove = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        anim.SetBool("Death", dead);
        anim.SetBool("Player", !player.dead && hit.collider != null); // since boxcast cannot detect player(changing player layer does nothing), boxcast checks for player weapon instead.
        Physics.BoxCast(rayposition.position, rayposition.localScale * .5f, rayposition.transform.forward, out hit, Quaternion.identity, 10);
        //Physics.Raycast(rayposition.position,rayposition.transform.forward,out hit,10);
        if (hitpoints <= 0)StartCoroutine(Death());
        //Physics.Raycast(weapon.transform.localPosition, Vector3.forward, out player, Mathf.Infinity);
        //float angle = Mathf.Atan2(player.transform.position.y, transform.position.x) * Mathf.Rad2Deg;
        //Camera cam = FindObjectOfType<Camera>();
        //Quaternion lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 360 * Time.deltaTime);
        
    }
    protected virtual IEnumerator Death() {
        dead = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime -.5f);
        gameObject.SetActive(false);
        Drop();
    }
    protected void Drop() {
        rng = UnityEngine.Random.Range(0f, 1f);
        if (rng > 0 && rng <= 0.5f) {
            dropindex = 0; // base weapon ammo
        }
        else if(rng > 0.5f && rng <= 0.8f) {
            dropindex = 1; // shield pickup
        }
        else if(rng > 0.8f && rng <=0.95f) {
            dropindex = 2; // health pickup
        }
        else if(rng > 0.95f && rng <= 1){
            dropindex = 3; // GBE ammo
        }
        Instantiate(drops[dropindex], transform.position, transform.rotation);
    }
    public virtual void Shoot() { // animation event.
        weapon.Fire();
    }
    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rayposition.position, rayposition.localScale);
        //Gizmos.DrawLine(rayposition.position, rayposition.transform.forward);
    }
}
