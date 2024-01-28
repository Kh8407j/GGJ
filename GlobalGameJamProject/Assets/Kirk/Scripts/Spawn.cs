// KHOGDEN 001115381
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
        }

        public void SetPlayerIndex(int index)
        {
            playerIndex = index;
        }
    }
}