// KHOGDEN 001115381
using Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class Motor : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] float moveSpeed = 5f;

        [Header("Fire Points")]
        [SerializeField] Transform firepointUp;
        [SerializeField] Transform firepointDown;
        [SerializeField] Transform firepointLeft;
        [SerializeField] Transform firepointRight;

        // KH - Used to read what direction the motor is facing.
        private enum Direction { north, east, south, west };
        [SerializeField] Direction facingDirection;

        // KH - Outputted variables from controller script.
        private float hor;
        private float ver;

        [System.Serializable]
        public class InputAbility
        {
            [SerializeField] string name;
            [SerializeField][Range(0.001f, 10f)] float cooldownTime = 0.5f;
            private float cooldownTimer;
            private bool holdingInput;

            #region Methods
            public string GetName()
            {
                return name;
            }

            // KH - Method to see if this ability is on cooldown.
            public bool OnCooldown()
            {
                return cooldownTimer == 0f;
            }

            public float GetCooldownTime()
            {
                return cooldownTime;
            }

            public float CooldownTimer
            {
                get { return cooldownTimer; }
                set { cooldownTimer = value; }
            }

            public bool HoldingInput
            {
                get { return holdingInput; }
                set { holdingInput = value; }
            }
            #endregion

            #region Constructors
            public InputAbility(string abilityName, float AbilityCooldown)
            {
                name = abilityName;
                cooldownTime = AbilityCooldown;
            }

            #endregion
        }
        [SerializeField] List<InputAbility> inputAbilities = new List<InputAbility>();

        // Class containing data for push mechanics on the motor.
        [System.Serializable]
        public class PushDirection
        {
            private Vector2 direction;
            private float multiplier;

            public Vector2 Direction
            {
                get { return direction; }
                set { direction = value; }
            }

            public float Multiplier
            {
                get { return multiplier; }
                set { multiplier = value; }
            }
        }
        [SerializeField] PushDirection pushDirection = new PushDirection();

        private Rigidbody2D rb;
        private Health health;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
        }

        // KH - Called upon every frame.
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Push(Vector2.down, 5f);

            Move();

            foreach (InputAbility i in inputAbilities)
            {
                // KH - Tick down the timer for each cooldown until it reaches zero.
                if (i.CooldownTimer > 0f)
                    i.CooldownTimer -= Time.deltaTime;
                else if (i.CooldownTimer < 0f)
                    i.CooldownTimer = 0f;
            }

            // KH - decrease the multiplier over time until it's at zero.
            if(pushDirection.Multiplier > 0f)
                pushDirection.Multiplier -= Time.deltaTime;
            else if(pushDirection.Multiplier < 0f)
                pushDirection.Multiplier = 0f;
        }

        // KH - Make the motor move based on controller script inputs.
        private void Move()
        {
            float x = hor;
            float y = ver;

            // KH - This prevents diagonal movement.
            if (hor != 0f)
                y = 0f;

            Vector2 moveDir = new Vector2(x, y);
            transform.Translate(moveDir * (moveSpeed / 2f) * Time.deltaTime + (pushDirection.Direction * pushDirection.Multiplier) * Time.deltaTime);

            // KH - Update facing direction.
            if(hor > 0f)
                facingDirection = Direction.east;
            else if(hor < 0f)
                facingDirection = Direction.west;
            else if (ver > 0f)
                facingDirection = Direction.north;
            else if (ver < 0f)
                facingDirection = Direction.south;
        }

        // KH - Push the motor in a inputted direction.
        public void Push(Vector2 direction, float impact)
        {
            pushDirection.Direction = direction;
            pushDirection.Multiplier = impact;
        }

        // KH - Add a new scriptable ability for the motor.
        public void RegisterInputAbility(string abilityName, float abilityCooldown)
        {
            InputAbility i = new InputAbility(abilityName, abilityCooldown);
            inputAbilities.Add(i);
        }

        #region Methods
        public float Horizontal
        {
            get { return hor; }
            set { hor = value; }
        }

        public float Vertical
        {
            get { return ver; }
            set { ver = value; }
        }

        // KH - Ability to get a scriptable input ability by it's name.
        public InputAbility GetInputAbility(string abilityName)
        {
            foreach (InputAbility i in inputAbilities)
            {
                if (i.GetName() == abilityName)
                    return i;
            }

            Debug.LogWarning("Could not find scriptable input ability name: " + abilityName);
            return null;
        }
        #endregion
    }
}