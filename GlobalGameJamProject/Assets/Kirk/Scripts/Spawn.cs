// KHOGDEN 001115381
using Controllers;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Spawn : MonoBehaviour
    {
        private int playerIndex = 1;
        [SerializeField] GameObject playerBlueprint;

        public void SpawnPlayer()
        {
            GameObject plr = Instantiate(playerBlueprint, transform.position, Quaternion.identity);
            plr.GetComponent<PlayerController>().SetInputDevice(GameManager.instance.GetPlayer(playerIndex - 1).InputDevice);
            plr.GetComponentInChildren<SpriteRenderer>().sprite = GameManager.instance.GetCharacter(GameManager.instance.GetPlayer(playerIndex - 1).SelectedCharacter).GetUiImage();
        }

        public int PlayerIndex
        {
            get { return playerIndex; }
            set { playerIndex = value; }
        }
    }
}