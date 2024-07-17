using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
public class LevelManager : SceneLoader
{
    public GameObject canvas;
    public Transform canvasPos;
    public GameObject gameoverscreen;
    public float canvasSpeed;
    public TMP_Text ammocount;
    public Image weaponimg, healthbar, shieldbar;
    public Sprite[] weaponSprites;
    Player player;
    public float canvasOffset;
    public SaveData saveData;
    string json;
    [SerializeField] GameObject[] activerooms,initactiverooms;
    [SerializeField] GameObject endwall,endroom;
    public AudioClip wallOpenSound,endSound;
    public GameObject saveText;
    // Start is called before the first frame update
    private void Awake() {
        saveData = Load();
    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        activerooms = saveData.activerooms;
        StartCoroutine(AudioManager.instance.SwitchMusic(AudioManager.instance.levelmusic));
    }

    // Update is called once per frame
    void Update()
    {
        SetCanvasPosition();
        activerooms = GameObject.FindGameObjectsWithTag("Room");
        healthbar.fillAmount = player.hitpoints / player.maxhitpoints;
        shieldbar.fillAmount = player.shieldpoints / player.maxshieldpoints;
        ammocount.text = player.currentweapon.currentammo +"/"+ Mathf.Clamp(player.currentweapon.maxammo - player.currentweapon.ammoInClip,0, player.currentweapon.maxammo - player.currentweapon.ammoInClip);
        weaponimg.sprite = weaponSprites[Array.IndexOf(player.weapons,player.currentweapon)];
        //int enemieskilled = 0;
        //foreach(var i in endroom.GetComponentsInChildren<EnemySpawner>()) {
        //    foreach(var x in i.spawnedEnemies) {
        //        if (!x.activeSelf) enemieskilled++;
        //    }
        //}
        //if(enemieskilled == 15) {
        //    endwall.SetActive(false);
        //    foreach (var i in FindObjectsOfType<Enemy>()) i.gameObject.SetActive(false);
        //    AudioManager.instance.PlaySFX(wallOpenSound);
        //    AudioManager.instance.StopMusic();
        //}
        if (gameoverscreen.activeSelf) {
            AudioManager.instance.StopMusic();
        }
    }
    void SetCanvasPosition() {
        //float angle = Mathf.Atan2(player.transform.position.z,canvas.transform.position.z) * Mathf.Rad2Deg;
        canvas.transform.LookAt(canvasPos, Vector3.up);
        canvas.transform.Rotate(0, 180f, 0);
        //canvas.transform.rotation = Quaternion.Euler(canvas.transform.rotation.x, canvasPos.rotation.y, canvas.transform.rotation.z);
        //Vector3 targetposition = new Vector3(canvasPos.position.x, canvas.transform.position.y, canvasPos.position.z  + canvasOffset);
        canvas.transform.position = canvasPos.position + canvasPos.forward * canvasOffset;
        //Vector3.Lerp(canvas.transform.position, targetposition, canvasSpeed * Time.deltaTime);
        canvas.transform.rotation = new Quaternion(0, canvas.transform.rotation.y, 0, canvas.transform.rotation.w);
        //canvas.transform.rotation = Quaternion.identity;
        //canvas.transform.SetPositionAndRotation(Vector3.Lerp(canvas.transform.position, targetposition, canvasSpeed * Time.deltaTime), canvasPos.rotation);
    }
    public void Save() {
        saveData.shieldpoints = player.shieldpoints;
        saveData.hitpoints = player.hitpoints;
        saveData.currentweapon = Array.IndexOf(player.weapons,player.currentweapon);
        saveData.currentammo = player.currentweapon.currentammo;
        saveData.maxammo = player.currentweapon.maxammo;
        saveData.activerooms = activerooms;
        saveData.spawnPoint = player.spawnPoint;
        json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }
    public SaveData InitialData() {
        SaveData save = new SaveData();
        save.shieldpoints = 10;
        save.hitpoints = 10;
        save.currentweapon = 0;
        save.currentammo = 15;
        save.maxammo = 60;
        save.activerooms = initactiverooms;
        save.spawnPoint = new Vector3(0,0,-10);
        json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
        return save;
    }
    public SaveData Load() {
        if(File.Exists(Application.dataPath + "/save.txt")) {
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.dataPath + "/save.txt"));
        } 
        else {
            return InitialData();
        }
    }
    public class SaveData {
        public float shieldpoints;
        public float hitpoints;
        public GameObject[] activerooms;
        public int currentweapon;
        public int currentammo;
        public int maxammo;
        public Vector3 spawnPoint;
    }
}
