// KHOGDEN 001115381
using System;
using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public enum InputDevice { none, keyboard, gamepad1, gamepad2, gamepad3, gamepad4 };
        [SerializeField] InputDevice inputDevice;

        // KH - Calculated input variables.
        private float hor;
        private float ver;
        private bool holdingAtk1;
        private bool holdingAtk2;
        private bool holdingAtk3;
        private bool holdingAtk4;

        private Motor motor;

        // KH - Called before 'void Start()'.
        void Awake()
        {
            motor = GetComponent<Motor>();
        }

        // KH - Called upon the first frame.
        void Start()
        {
            motor.RegisterInputAbility("Punch", 0.15f);
            motor.RegisterInputAbility("Cream Pie", 0.75f);
            motor.RegisterInputAbility("Balloon", 0.25f);
            motor.RegisterInputAbility("Banana", 0.5f);
        }

        // KH - Called on a constant timeline.
        void Update()
        {
            CalculateInput();
            Output();
        }

        // KH - Feed input information into the necessary output variables for reading.
        void CalculateInput()
        {
            string gamepadNum = "";
            if (inputDevice == InputDevice.gamepad1)
                gamepadNum = 1.ToString();
            if (inputDevice == InputDevice.gamepad2)
                gamepadNum = 2.ToString();
            if (inputDevice == InputDevice.gamepad3)
                gamepadNum = 3.ToString();
            if (inputDevice == InputDevice.gamepad4)
                gamepadNum = 4.ToString();

            hor = Input.GetAxisRaw("Horizontal" + gamepadNum);
            ver = Input.GetAxisRaw("Vertical" + gamepadNum);
            holdingAtk1 = Input.GetButton("AttackOne" + gamepadNum);
            holdingAtk2 = Input.GetButton("AttackTwo" + gamepadNum);
            holdingAtk3 = Input.GetButton("AttackThree" + gamepadNum);
            holdingAtk4 = Input.GetButton("AttackFour" + gamepadNum);
        }

        // KH - Output calculated input data into the motor.
        void Output()
        {
            motor.Horizontal = hor;
            motor.Vertical = ver;
            motor.GetInputAbility("Punch").HoldingInput = holdingAtk1;
            motor.GetInputAbility("Cream Pie").HoldingInput = holdingAtk2;
            motor.GetInputAbility("Balloon").HoldingInput = holdingAtk3;
            motor.GetInputAbility("Banana").HoldingInput = holdingAtk4;
        }
    }
}