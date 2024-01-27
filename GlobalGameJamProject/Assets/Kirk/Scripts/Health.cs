// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Health : MonoBehaviour
    {
        private float damage = 0f;
        [SerializeField] float maxDamage = 100f;

        private bool dead;

        // KH - Add or remove damage from 'damage' by the inputted value in 'amount'.
        public void ChangeDamage(float amount)
        {
            // KH - Store the value of damage before altering it.
            float prevDamage = damage;

            // KH - Add the inputted amount of damage being given to 'damage'.
            damage += amount;

            // KH - Check that damage went down or up.
            if (prevDamage > damage)
            {
                
            }
            else
            {

            }

            // KH - Prevent damage going over maximum/minimum.
            if (damage > maxDamage)
                damage = maxDamage;
            else if (damage < 0f)
                damage = 0f;
        }

        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetMaxDamage()
        {
            return maxDamage;
        }
    }
}
