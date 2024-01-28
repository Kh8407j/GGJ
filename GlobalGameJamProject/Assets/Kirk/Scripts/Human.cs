// KHOGDEN 001115381
using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Human : MonoBehaviour
    {
        private Health health;
        private Motor motor;

        // KH - Projectiles.
        [SerializeField] GameObject punchObj;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            health = GetComponent<Health>();
            motor = GetComponent<Motor>();
        }

        // Update is called once per frame
        void Update()
        {
            // KH - Stop the human from being able to move around once they're dead.
            if(health.Dead)
                motor.enabled = false;

            Motor.InputAbility punch = motor.GetInputAbility("Punch");
            if (punch.HoldingInput && !punch.OnCooldown())
            {
                punch.StartCooldown();
                Instantiate(punchObj, motor.GetFacingFirepoint().position, Quaternion.identity);
                Projectile p = punchObj.GetComponent<Projectile>();

                // KH - Set the facing/moving direction of the projectile.
                if (motor.GetFacingDirection() == Motor.Direction.north)
                {
                    p.SetFacingDirection(Projectile.Direction.north);
                    p.SetMoveDirection(Vector3.up);
                }
                else if (motor.GetFacingDirection() == Motor.Direction.east)
                {
                    p.SetFacingDirection(Projectile.Direction.east);
                    p.SetMoveDirection(Vector3.right);
                }
                else if (motor.GetFacingDirection() == Motor.Direction.south)
                {
                    p.SetFacingDirection(Projectile.Direction.south);
                    p.SetMoveDirection(Vector3.down);
                }
                else if (motor.GetFacingDirection() == Motor.Direction.west)
                {
                    p.SetFacingDirection(Projectile.Direction.west);
                    p.SetMoveDirection(Vector3.left);
                }
            }

            Motor.InputAbility fire = motor.GetInputAbility("Fire");
            if (fire.HoldingInput && !fire.OnCooldown())
            {

            }

            if (health.Dead)
                Destroy(gameObject);
        }

        // KH - Called upon collision with a trigger collider.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // KH - Kill the human if they go into a kill zone.
            if(collision.gameObject.CompareTag("Killzone"))
                health.Dead = true;
        }
    }
}