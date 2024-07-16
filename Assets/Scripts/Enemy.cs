using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Enemy : Unit
{
    protected EnemyWeapon weapon;
    public RaycastHit hit; // for detecting player
    [HideInInspector]public Player player;
    public Transform rayposition;
    public GameObject[] drops;
    [SerializeField]protected float rng;
    protected int dropindex;
    NavMeshAgent agent;
    public float minTimeUnderCover, maxTimeUnderCover;
    public int minShotsToTake, maxShotsToTake;
    [Range(0, 100)] public float accuracy;
    public Transform nearestCover;
    bool shooting;
    public int currentShotsTaken,currentMaxShotsToTake;
    //[SerializeField]Transform[] coverSpots;
    public EnemySpawner spawner;
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
        shooting = false;
        agent = GetComponent<NavMeshAgent>();
        Init();
        //canMove = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (FindObjectOfType<PauseScreen>().pausescreen.activeSelf) return;
        anim.SetBool("Death", dead);
        anim.SetBool("Player", shooting && !player.dead /*&& hit.collider != null && hit.collider.GetComponent<Weapon>()*/); // since boxcast cannot detect player(changing player layer does nothing), boxcast checks for player weapon instead.
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        Physics.BoxCast(rayposition.position, rayposition.localScale * .5f, rayposition.transform.forward, out hit, Quaternion.identity, 10);
        if (hit.collider != null && hit.collider.GetComponentInParent<Player>()) { // move to closest cover if player detected
            GetToCover();
        }
        if (hitpoints <= 0)StartCoroutine(Death());
        if(agent.isStopped == false && (transform.position - nearestCover.position).sqrMagnitude < 0.1f) { // if agent is near cover
            agent.isStopped = true;
            StartCoroutine(ShootCo());
        }
        Vector3 dir = player.GetComponentInChildren<Camera>().transform.position - transform.position;
        dir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 180 * Time.deltaTime);
        if (!spawner.enabled) gameObject.SetActive(false);
    }
    protected virtual IEnumerator Death() {
        dead = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
        //yield return null;
        Drop();
    }
    protected void Drop() {
        rng = UnityEngine.Random.Range(0f, 1f);
        if (rng > 0 && rng <= 0.35f) {
            dropindex = 0; // base weapon ammo
        }
        else if(rng > 0.35f && rng <= 0.65f) {
            dropindex = 1; // shield pickup
        }
        else if(rng > 0.65f && rng <=0.95f) {
            dropindex = 2; // health pickup
        }
        else if(rng > 0.95f && rng <= 1){
            dropindex = 3; // GBE ammo
        }
        Instantiate(drops[dropindex], transform.position, transform.rotation);
    }
    void Init() {
        nearestCover = spawner.coverSpots[UnityEngine.Random.Range(0, spawner.coverSpots.Length)];
        //RaycastHit hit;
        //if (Physics.Raycast(rayposition.position, nearestCover.position, out hit)) {
        //    if (hit.collider) if (hit.collider.GetComponent<Enemy>()) nearestCover = coverSpots[UnityEngine.Random.Range(0, coverSpots.Length)];
        //}
        //foreach (var i in coverSpots) { // finds closest cover spot
        //    float closest = 999;
        //    if (Vector3.Distance(transform.position, i.position) < closest) {
        //        closest = Vector3.Distance(transform.position, i.transform.position);
        //    }
        //    if (Vector3.Distance(transform.position, i.position) == closest) {
        //        nearestCover = i;
        //    }
        //    print(closest);
        //}
        GetToCover();
    }
    public virtual void Shoot() { // animation event.
        weapon.Fire();
    }
    void GetToCover() {
        agent.isStopped = false;
        agent.SetDestination(nearestCover.position);
    }
    public IEnumerator ShootCo() {
        anim.SetTrigger("Crouch");
        shooting = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeUnderCover, maxTimeUnderCover)); // enemy hides under cover for random period
        //anim.SetTrigger("Crouch");
        shooting = true;
        currentMaxShotsToTake = UnityEngine.Random.Range(minShotsToTake, maxShotsToTake); // enemy takes a random amount of shots at the player
        currentShotsTaken = 0;
    }
    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rayposition.position, rayposition.localScale);
        //Gizmos.DrawLine(rayposition.position, rayposition.transform.forward);
    }
}
