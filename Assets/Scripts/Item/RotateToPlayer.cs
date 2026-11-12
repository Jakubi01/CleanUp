using Input;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class RotateToPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        private void Awake()
        {
            if (!target) target = FindFirstObjectByType<XRPlayerMovement>().gameObject;
        }

        private void Update()
        {
            if(!target) return;

            transform.rotation = Quaternion.LookRotation(target.transform.forward, target.transform.up);
        }
    }
}
