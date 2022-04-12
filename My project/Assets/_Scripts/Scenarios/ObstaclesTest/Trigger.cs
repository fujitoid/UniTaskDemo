using System;
using UnityEngine;

namespace MyDemo.ObstaclesTest
{
    public class Trigger : MonoBehaviour
    {
        private bool _isTriggered = false;

        public bool IsTriggered => _isTriggered;
        public event Action<bool> Triggered;

        private void OnTriggerEnter(Collider other)
        {
            _isTriggered = true;
            Triggered.Invoke(_isTriggered);
        }

        private void OnTriggerExit(Collider other)
        {
            _isTriggered = false;
            Triggered.Invoke(_isTriggered);
        }
    } 
}
