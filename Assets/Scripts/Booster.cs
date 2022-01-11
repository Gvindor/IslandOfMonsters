﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SF
{
    public class Booster : MonoBehaviour
    {
        private const string LightsKey = "Lights";
        private const string PickKey = "Pick";

        [SerializeField] float howerHeight = 0.5f;
        [SerializeField] float rotationSpeed = 30f;
        [SerializeField] float fallSpeed = 1f;

        private bool picked;
        private bool landed;
        private Animator animator;

        public UnityEvent<Booster> OnPicked;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<FighterCharacterController>();

            if (player)
            {
                ApplyBoost(player);
            }
        }

        private void Update()
        {
            if (!Physics.Raycast(transform.position, Vector3.down, howerHeight))
            {
                var y = -fallSpeed * Time.deltaTime;
                transform.Translate(0, y, 0);  
            }
            else
            {
                if (!landed)
                {
                    landed = true;
                    animator.SetTrigger(LightsKey);
                }
            }

            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        private void ApplyBoost(FighterCharacterController player)
        {
            picked = true;

            var manager = FindObjectOfType<BoosterManager>();

            manager?.ApplyBoostToCharacter(player);

            animator.SetTrigger(PickKey);
        }

        private void OnPickAnimationDone()
        {
            OnPicked.Invoke(this);
            Destroy(gameObject);
        }
    }
}