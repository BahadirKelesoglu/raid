using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        public float moveSpeed = 1f; 
        public LayerMask ground;
        public LayerMask wall;
        public float knockbackDistancebybullet = 1f;

        private Rigidbody2D rigidbody; 
        public Collider2D triggerCollider;
        
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
        }

        void FixedUpdate()
        {
           
            if(!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
            {
                Flip();
            }
        }
        
        private void Flip()
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed *= -1;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the collision was with a bullet
            if (collision.gameObject.CompareTag("bullet"))
            {
                // Move the enemy back by knockbackDistance
                Vector3 knockbackDirection = transform.position + collision.transform.position;
                knockbackDirection.z = 0f;
                knockbackDirection.Normalize();
                transform.position += knockbackDirection * knockbackDistancebybullet;

                // Destroy the bullet
                Destroy(collision.gameObject);
            }
        }
    }
}
