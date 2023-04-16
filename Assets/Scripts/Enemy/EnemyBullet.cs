using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float bulletSpeed = 7f;
    private float destroyTime = 2f;
    private Rigidbody2D rb;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * bulletSpeed;
        Destroy(gameObject, destroyTime);
    }
    // Destroy gameobject on collision too
}
