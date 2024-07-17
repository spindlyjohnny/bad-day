
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]int enemiesspawned;
    public List<GameObject> spawnedEnemies;
    public int enemiestospawn;
    // Start is called before the first frame update
    public GameObject[] instprefab;
    public float instrate;
    //LevelManager levelManager;
    Player player;
    public bool canSpawn;
    //float nextinsttime;
    public Transform[] coverSpots;
    //public bool canSpawn;
    //public int currentwave;
    //public int iter = 0;
    void Start() {
        //levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Player>();
        coverSpots = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update() {
        if(canSpawn)StartCoroutine(Spawn());
        //Spawn();
        //if (!levelManager.currentroom.GetComponent<Room>().roomstart && levelManager.wavecomplete) gameObject.SetActive(false);
    }
    //void Spawn() {
    //    if (enemiesspawned <= enemiestospawn - 1 && !player.dead) {
    //        if (Time.time < nextinsttime) return;
    //        GameObject go = Instantiate(instprefab[Random.Range(0, instprefab.Length)], transform.position, transform.rotation);
    //        go.GetComponentInChildren<Enemy>().nearestCover = coverSpots[Random.Range(0, coverSpots.Length)];
    //        go.GetComponentInChildren<Enemy>().GetToCover();
    //        nextinsttime = Time.time + instrate;
    //        enemiesspawned++;
    //    }
    //}
    IEnumerator Spawn() {
        while(enemiesspawned < enemiestospawn) {
            GameObject go = Instantiate(instprefab[Random.Range(0, instprefab.Length)], transform.position, transform.rotation);
            enemiesspawned++;
            if (go.GetComponent<Enemy>()) go.GetComponent<Enemy>().spawner = this;
            else go.GetComponentInChildren<Enemy>().spawner = this;
            spawnedEnemies.Add(go);
            yield return new WaitForSeconds(instrate);
            if (enemiesspawned >= enemiestospawn) break;
        }
    }
    //public void BossSpawn() {
    //    if (enemiesspawned <= enemiestospawn[iter] - 1) {
    //        if (Time.time < nextinsttime) return;
    //        GameObject go = Instantiate(instprefab[Random.Range(0, instprefab.Length)], transform.position, transform.rotation);
    //        //go.GetComponentInChildren<Enemy>().spawned = true;
    //        go.GetComponentInChildren<Enemy>().canMove = true;
    //        go.GetComponentInChildren<Enemy>().spawner = transform;
    //        nextinsttime = Time.time + instrate;
    //        enemiesspawned++;
    //    }
    //}
}
