// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField][Range(1, 4)] int playerIndex = 1;
        [SerializeField] GameObject playerBlueprint;

        public void SpawnPlayer()
        {
            GameObject plr = Instantiate(playerBlueprint, transform.position, Quaternion.identity);
        }
    }
}