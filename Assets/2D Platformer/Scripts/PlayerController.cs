using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;
        public LayerMask wall;
        

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        //For gun 
        public LayerMask enemyLayer;
        public float detectionRadius = 5f;
        public float detectionAngle = 90f;
        public float maxObstacleDistance = 3f;
        public LayerMask obstacleLayer;
        private Collider2D closestEnemy;
        public GameObject bulletPrefab;
        public float bulletSpeed = 20f;
        //public float bulletPower = 1f;
        public float bulletLifetime = 1f;
        public GameObject gunpoint;
        public float timeBetweenShots = 0.5f;
        public Collider2D triggerCollider;
        public float moveSpeed = -3f;
        BulletController bulletController;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            bulletController = GameObject.Find("BulletManager").GetComponent<BulletController>();
        }

        private void FixedUpdate()
        {
            CheckGround();
            if (triggerCollider.IsTouchingLayers(wall))
            {
                Flip();
            }
        }

        void Update()
        {
            //Move with keyboard start
            //----------------------------------------
            /*  if (Input.GetButton("Horizontal"))
              {
                  moveInput = Input.GetAxis("Horizontal");

                  Vector3 direction = transform.right * moveInput;

                  animator.SetInteger("playerState", 1); // Turn on run animation
                  if (moveInput > 0)
                  {
                      transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                      transform.position = Vector3.MoveTowards(transform.position, transform.position - direction, movingSpeed * Time.deltaTime);
                  }
                  else
                  {
                      transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                      transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                  }
              }
              else
              {
                  if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
              }
              if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
              {
                  rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
              }
              if (!isGrounded) animator.SetInteger("playerState", 2); // Turn on jump animation
            //----------------------------------------
            //Move with keyboard end
          
              */

            //Move automaticcally and turn when you hit the walls start
            //-------------------------------------------------------------
            if(Input.GetKey(KeyCode.S))
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            else
                rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
            

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (!isGrounded) animator.SetInteger("playerState", 2); // Turn on jump animation


            //-------------------------------------------------------------
            //Move automaticcally and turn when you hit the walls end

            //For Gun
            // Calculate the raycast direction based on the gun's rotation
            /* Vector3 ahead = transform.TransformDirection(new Vector3(-1f,0f,0f));
             Vector3 rightRay = Quaternion.Euler(0, 100, 0) * ahead;
             Vector3 upRay = Quaternion.Euler(0, -angle / 2f, 0) * ahead;
             Vector3 downRay = Quaternion.Euler(0, -angle / 2f, 0) * ahead;
             Debug.DrawLine(transform.position, transform.position + rightRay * distance, Color.green);*/
            // Calculate the detection direction based on the player's rotation

            Vector3 detectionDirection = -transform.right;
            timeBetweenShots -= Time.deltaTime;

            // Detect enemies within the specified range and angle
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                
                Vector3 targetDirection = (hit.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(detectionDirection, targetDirection);

                if (angle <= detectionAngle / 2f)
                {
                    
                    // Enemy detected within the detection angle
                    Debug.DrawLine(transform.position, hit.transform.position, Color.green);
                    
                    // Check if the detected enemy is closer than the current closest
                    if (closestEnemy == null || Vector2.Distance(transform.position, hit.transform.position) < Vector2.Distance(transform.position, closestEnemy.transform.position))
                    {
                       
                            closestEnemy = hit;
                      
                    }
                }
                else
                {
                    // Enemy is outside of the detection angle
                    Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
                }
            }

            // Check for obstacles only when a closest enemy has been found
            if (closestEnemy != null)
            {
                // Calculate the direction to the closest enemy
                Vector3 targetDirection = (closestEnemy.transform.position - transform.position).normalized;

                // Cast a ray to check for obstacles between the enemy and the player
                RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, targetDirection, (closestEnemy.transform.position.x - transform.position.x), obstacleLayer);

                if (obstacleHit.collider != null)
                {
                    // An obstacle was hit, so the enemy is not visible
                    Debug.DrawLine(transform.position, obstacleHit.point, Color.yellow);
                    closestEnemy = null;
                }
                else
                {
                    // No obstacle was hit, so the enemy is visible
                    Debug.DrawLine(transform.position, closestEnemy.transform.position, Color.blue);
                    
                    if (timeBetweenShots<=0) {
                        // Instantiate a bullet and set its position to the player's position
                        GameObject bullet = Instantiate(bulletPrefab, gunpoint.transform.position, Quaternion.identity);
                        bullet.GetComponent<Bullet>().SetDamage(bulletController.bulletDamage);
                        timeBetweenShots = 2f;
                    
                    // Calculate the velocity vector for the bullet based on the target direction and bullet speed
                    Vector2 bulletVelocity = targetDirection * bulletSpeed;

                    // Set the bullet's velocity
                    bullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity;

                    // Destroy the bullet after a certain time to prevent memory leaks
                    Destroy(bullet, bulletLifetime);
                    }

                    // Reset the closest enemy so that the player can find a new target
                    closestEnemy = null;
                }
            }

        }

        private void Flip()
        {
            if (moveSpeed < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                
            }
            moveSpeed *= -1;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }
    }
}
