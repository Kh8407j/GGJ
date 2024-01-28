// KHOGDEN 001115381
using Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        private bool lockAssigningDevices;

        [System.Serializable]
        public class Player
        {
            [SerializeField] Color32 uiColour;
            [SerializeField] PlayerController.InputDevice inputDevice;
            [SerializeField] int selectedCharacter;

            public Color GetUiColour()
            {
                return uiColour;
            }

            public PlayerController.InputDevice InputDevice
            {
                get { return inputDevice; }
                set { inputDevice = value; }
            }

            public int SelectedCharacter
            {
                get { return selectedCharacter; }
                set { selectedCharacter = value; }
            }
        }
        public List<Player> players = new List<Player>();

        [System.Serializable]
        public class Character
        {
            [SerializeField] string name;
            [SerializeField] string description;
            [SerializeField] Sprite uiImage;
            [SerializeField] GameObject playerBlueprint;

            public string GetName()
            {
                return name;
            }

            public string GetDescription()
            {
                return description;
            }

            public Sprite GetUiImage()
            {
                return uiImage;
            }

            public GameObject GetPlayerBlueprint()
            {
                return playerBlueprint;
            }
        }
        public List<Character> characters = new List<Character>();

        private void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            // KH - Check first if not all players yet have assigned their devices before checking for controller assign input.
            if (players[players.Count - 1].InputDevice == PlayerController.InputDevice.none)
            {
                // KH - Check if users are allowed to assign their input devices before continuing.
                if (!lockAssigningDevices)
                {
                    // KH - Check which player next is yet to assign their device.
                    int playerIndex = 0;
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].InputDevice != PlayerController.InputDevice.none)
                            playerIndex++;
                        else
                            break;
                    }

                    //  KH - Check for input to see if the next player is trying to assign their device.
                    if (Input.GetKeyDown(KeyCode.Escape))
                        AssignDevice(playerIndex, PlayerController.InputDevice.keyboard);
                    else if (Input.GetKeyDown(KeyCode.Joystick1Button7))
                        AssignDevice(playerIndex, PlayerController.InputDevice.gamepad1);
                    else if (Input.GetKeyDown(KeyCode.Joystick2Button7))
                        AssignDevice(playerIndex, PlayerController.InputDevice.gamepad2);
                    else if (Input.GetKeyDown(KeyCode.Joystick3Button7))
                        AssignDevice(playerIndex, PlayerController.InputDevice.gamepad3);
                    else if (Input.GetKeyDown(KeyCode.Joystick4Button7))
                        AssignDevice(playerIndex, PlayerController.InputDevice.gamepad4);
                }
            }

            if (Input.GetKeyDown(KeyCode.F5))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // KH - Assign a player with the device they're trying to use.
        void AssignDevice(int playerIndex, PlayerController.InputDevice device)
        {
            // KH - Check to see if the device the player is trying to use is already taken.
            bool alreadyTaken = false;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].InputDevice == device && i != playerIndex)
                    alreadyTaken = true;
            }

            // KH - Assign the device to the player.
            if (!alreadyTaken)
                players[playerIndex].InputDevice = device;
        }

        public Player GetPlayer(int index)
        {
            return players[index];
        }

        public int GetPlayerListCount()
        {
            return players.Count;
        }

        public Character GetCharacter(int index)
        {
            return characters[index];
        }

        public int GetCharacterListCount()
        {
            return characters.Count;
        }
    }
}