using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomsToLoad, roomsToUnload;
    LevelManager levelManager;
    public AudioClip saveSFX;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<Player>()) {
            other.GetComponentInParent<Player>().spawnPoint = transform.position;
            StartCoroutine(ShowSaveText());
            AudioManager.instance.PlaySFX(saveSFX);
            levelManager.Save();
            //if (gameObject.CompareTag("Save")) {
            //    //other.GetComponentInParent<Player>().spawnPoint = transform.position;
            //    //StartCoroutine(ShowSaveText());
            //    //AudioManager.instance.PlaySFX(saveSFX);
            //    //levelManager.Save();
            //}
            if (gameObject.CompareTag("Finish")) {
                AudioManager.instance.PlaySFX(levelManager.endSound);
                levelManager.LoadScene(0);
            }
            if(roomsToLoad.Length > 0) {
                foreach (var i in roomsToLoad) {
                    i.SetActive(true);
                    foreach(var x in i.GetComponentsInChildren<EnemySpawner>()) {
                        x.canSpawn = true;
                    }
                }
            }
            if(roomsToUnload.Length > 0) {
                foreach (var i in roomsToUnload) i.SetActive(false);
            }
        }
    }
    IEnumerator ShowSaveText() {
        levelManager.saveText.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelManager.saveText.SetActive(false);
    }
}
