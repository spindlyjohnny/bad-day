using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody rb;
    public float speed;
    public float damage;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        Move();
        
    }

    // Update is called once per frame
    protected void Update()
    {
        Destroy(gameObject, 20f);
    }
    protected virtual void Move() {
        rb.velocity = transform.forward * speed;
    }
    private void OnTriggerEnter(Collider other) {
        if (other) print(other.name);
        if (other.GetComponent<Unit>()) Destroy(gameObject);
    }
}
