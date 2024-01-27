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