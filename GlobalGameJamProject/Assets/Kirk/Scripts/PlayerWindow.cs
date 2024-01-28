// KHOGDEN 001115381
using Managers;
using System.Collections;
using System.Collections.Generic;
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

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            panel = GetComponent<Image>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            int i = playerIndex - 1;
            int chr = GameManager.instance.GetPlayer(i).SelectedCharacter;

            if(GameManager.instance.GetPlayer(i).InputDevice == Controllers.PlayerController.InputDevice.none)
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
                characterDesc.text = GameManager.instance.GetCharacter(chr).GetDescription();
                Color32 c = GameManager.instance.GetPlayer(i).GetUiColour();
                panel.color = new Color32(c.r, c.g, c.b, 10);
            }
        }
    }
}