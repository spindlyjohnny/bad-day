using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class LevelManager : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCanvasPosition();
        healthbar.fillAmount = player.hitpoints / player.maxhitpoints;
        shieldbar.fillAmount = player.shieldpoints / player.maxshieldpoints;
        ammocount.text = player.currentweapon.currentammo +"/"+ (player.currentweapon.maxammo - player.currentweapon.ammoInClip);
        weaponimg.sprite = weaponSprites[Array.IndexOf(player.weapons,player.currentweapon)];
    }
    void SetCanvasPosition() {
        //float angle = Mathf.Atan2(player.transform.position.z,canvas.transform.position.z) * Mathf.Rad2Deg;
        canvas.transform.LookAt(canvasPos, Vector3.up);
        canvas.transform.Rotate(0, 180f, 0);
        //canvas.transform.rotation = Quaternion.Euler(canvas.transform.rotation.x, canvasPos.rotation.y, canvas.transform.rotation.z);
        //Vector3 targetposition = new Vector3(canvasPos.position.x, canvas.transform.position.y, canvasPos.position.z  + canvasOffset);
        canvas.transform.position = canvasPos.position + canvasPos.forward * canvasOffset;//Vector3.Lerp(canvas.transform.position, targetposition, canvasSpeed * Time.deltaTime);
        canvas.transform.rotation = new Quaternion(0, canvas.transform.rotation.y, 0, canvas.transform.rotation.w);
        //canvas.transform.rotation = Quaternion.identity;
        //canvas.transform.SetPositionAndRotation(Vector3.Lerp(canvas.transform.position, targetposition, canvasSpeed * Time.deltaTime), canvasPos.rotation);
    }

}
