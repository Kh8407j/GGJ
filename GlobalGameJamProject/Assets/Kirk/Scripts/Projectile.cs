// KHOGDEN 001115381
using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float damage = 5f;
        private Vector2 moveDirection;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField][Range(0.001f, 10f)] float lifeTime = 2f;

        private Rigidbody2D rb;

        // KH - Used to read what direction the projectile is facing.
        public enum Direction { north, east, south, west };
        [SerializeField] Direction facingDirection;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = moveDirection * moveSpeed * Time.deltaTime;
        }

        // KH - Called when a collider a trigger collider.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Motor m = collision.gameObject.GetComponent<Motor>();
            Health h = collision.gameObject.GetComponent<Health>();
            if (m != null)
            {
                // KH - Damage the human and push them.
                h.ChangeDamage(damage);
                float dmg = h.GetDamage() / 5f;

                if (facingDirection == Direction.north)
                    m.Push(Vector3.up, dmg);
                else if (facingDirection == Direction.east)
                    m.Push(Vector3.right, dmg);
                else if (facingDirection == Direction.south)
                    m.Push(Vector3.down, dmg);
                else if (facingDirection == Direction.west)
                    m.Push(Vector3.left, dmg);

                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("SoftWall"))
                Destroy(collision.gameObject);
        }

        public void SetMoveDirection(Vector2 dir)
        {
            moveDirection = dir;
        }

        public void SetFacingDirection(Direction dir)
        {
            facingDirection = dir;
        }
    }
}