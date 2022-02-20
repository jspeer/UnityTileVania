using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileHorizontalSpeed = 1f;
    [SerializeField] float projectileVerticleSpeed = 0f;
    PlayerMovement player;
    Rigidbody2D myRigidBody;
    BoxCollider2D myCollider;
    float xSpeed;
    float ySpeed;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        xSpeed = player.transform.localScale.x * projectileHorizontalSpeed;
        ySpeed = player.transform.localScale.y * projectileVerticleSpeed;
        myRigidBody.transform.localScale = new Vector2(player.transform.localScale.x, 1f);
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(xSpeed, ySpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Destroy(gameObject);
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            Destroy(gameObject);
        }
    }
}
