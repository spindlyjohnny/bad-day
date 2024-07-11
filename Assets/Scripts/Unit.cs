
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
    public float hitpoints;
    public float maxhitpoints;
    //public Slider healthbar;
    public AudioClip hitsound, deathsound;
    //public GameObject bloodvfx, hiteffect;
    public bool /*canMove, */dead;
    [HideInInspector]public Animator anim;
    [HideInInspector]public Rigidbody rb;
    public Material originalmat,hurtmat;
    //public float yOffset;// offset where hit effect spawns
    // Start is called before the first frame update
    public virtual void TakeHit(float damage) {
        hitpoints -= damage;
    }
    /*public void SetHealth(float health, float maxHealth) {
        healthbar.value = health;
        healthbar.maxValue = maxHealth;
    }*/
    public virtual IEnumerator Hit() {
        foreach(var i in GetComponentsInChildren<Renderer>())i.material = hurtmat;
        AudioManager.instance.PlaySFX(hitsound);
        yield return new WaitForSeconds(0.3f);
        foreach (var i in GetComponentsInChildren<Renderer>()) i.material = originalmat;
    }
    /*protected virtual void OnEnable() {
        isHit = false;
        Physics.IgnoreLayerCollision(3, 6, false);
    }*/
    public virtual void OnTriggerEnter(Collider other) {
        if (dead) return;
        if (other.GetComponent<Projectile>()) {
            Damaged(0,other);
        }
    }
    public virtual void Damaged(float damage = 0, Collider other = null) {
        if (dead) return;

        if (other) TakeHit(other.GetComponentInParent<Projectile>().damage);
        else TakeHit(damage);
        StartCoroutine(Hit());
    }
    public virtual void PlayDeathSound() {
        AudioManager.instance.PlaySFX(deathsound);
    }
}
