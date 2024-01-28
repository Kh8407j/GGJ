// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class KillzoneBarrier : MonoBehaviour
    {
        [SerializeField] Vector2 moveDir;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = moveDir;
        }
    }
}