using System;
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
        public float health = 100;
        public GameObject coinPrefab;

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
                Bullet bulletComponent = collision.gameObject.GetComponent<Bullet>();
                if (bulletComponent != null)
                {
                    
                    // Pass the bullet power to the enemy
                    TakeDamage(bulletComponent.GetDamage());
                }
                // Move the enemy back by knockbackDistance
                Vector3 knockbackDirection = transform.position - collision.transform.position;
                knockbackDirection.z = 0f;
                knockbackDirection.y = 0f;
                knockbackDirection.Normalize();
                transform.position += knockbackDirection * knockbackDistancebybullet;

                // Destroy the bullet
                Destroy(collision.gameObject);
            }
        }

        private void TakeDamage(float bulletPower)
        {
            health -= bulletPower;

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject); // destroy the enemy game object
            Instantiate(coinPrefab, transform.position, Quaternion.identity); // instantiate the coin prefab
        }
    }
}
