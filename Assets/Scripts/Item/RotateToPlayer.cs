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
            if (target == null) target = FindFirstObjectByType<XRPlayerMovement>().gameObject;
        }

        private void Update()
        {
            if(target is null) return;

            transform.rotation = Quaternion.LookRotation(target.transform.forward, target.transform.up);
        }
    }
}
