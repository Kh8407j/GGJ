// KHOGDEN 001115381
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class PlayerWindow : MonoBehaviour
    {
        [SerializeField][Range(1, 4)] int playerIndex = 1;

        [SerializeField] Text playerIndicator;
        [SerializeField] Image characterImage;
        [SerializeField] Text characterName;
        [SerializeField] Text characterDesc;
        [SerializeField] Color32 noPlayerColour;
        private Image panel;

        // KH - Output values.
        private bool holdingToggle;
        private bool toggled;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            panel = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            int i = playerIndex - 1;
            int chr = GameManager.instance.GetPlayer(i).SelectedCharacter;

            if (GameManager.instance.GetPlayer(i).InputDevice == Controllers.PlayerController.InputDevice.none)
            {
                playerIndicator.text = "PRESS ESC/START";
                characterName.text = "";
                characterDesc.text = "";
                panel.color = new Color32(noPlayerColour.r, noPlayerColour.g, noPlayerColour.b, 10);
            }
            else
            {
                playerIndicator.text = "PLAYER " + playerIndex;
                characterName.text = GameManager.instance.GetCharacter(chr).GetName();
                characterImage.sprite = GameManager.instance.GetCharacter(chr).GetUiImage();
                characterDesc.text = GameManager.instance.GetCharacter(chr).GetDescription();
                Color32 c = GameManager.instance.GetPlayer(i).GetUiColour();
                panel.color = new Color32(c.r, c.g, c.b, 10);

                // KH - Calculate input if there is a player for this window.
                CalculateInput();

                // KH - Toggle to the next character upon player input.
                if (holdingToggle && !toggled)
                {
                    toggled = true;
                    ToggleCharacter();
                }
                else if (!holdingToggle)
                    toggled = false;
            }
        }

        // KH - Feed input information into the necessary output variables for reading.
        void CalculateInput()
        {
            int i = playerIndex - 1;

            string gamepadNum = "";
            if (GameManager.instance.GetPlayer(i).InputDevice == Controllers.PlayerController.InputDevice.gamepad1)
                gamepadNum = 1.ToString();
            if (GameManager.instance.GetPlayer(i).InputDevice == Controllers.PlayerController.InputDevice.gamepad2)
                gamepadNum = 2.ToString();
            if (GameManager.instance.GetPlayer(i).InputDevice == Controllers.PlayerController.InputDevice.gamepad3)
                gamepadNum = 3.ToString();
            if (GameManager.instance.GetPlayer(i).InputDevice == Controllers.PlayerController.InputDevice.gamepad4)
                gamepadNum = 4.ToString();

            holdingToggle = Input.GetButton("AttackOne" + gamepadNum);
        }

        // KH - Toggle to the next selectable character on the list.
        void ToggleCharacter()
        {
            int i = playerIndex - 1;
            if (GameManager.instance.GetPlayer(i).SelectedCharacter < GameManager.instance.GetCharacterListCount() - 1)
                GameManager.instance.GetPlayer(i).SelectedCharacter++;
            else
                GameManager.instance.GetPlayer(i).SelectedCharacter = 0;
        }
    }
}