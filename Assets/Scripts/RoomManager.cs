using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomsToLoad, roomsToUnload;
    LevelManager levelManager;
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
            if (gameObject.CompareTag("Save")) {
                other.GetComponentInParent<Player>().spawnPoint = transform.position;
                levelManager.Save();
            }
            if(roomsToLoad.Length > 0) {
                foreach (var i in roomsToLoad) i.SetActive(true);
            }
            if(roomsToUnload.Length > 0) {
                foreach (var i in roomsToUnload) i.SetActive(false);
            }
        }
    }
}
