using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyDemo.ObjectControl
{
    public class ObjectAtivityControl : MonoBehaviour
    {
        [SerializeField] private InputAction _inputAction;
        [Space]
        [SerializeField] private GameObject _gameObject;
        [Space]
        [SerializeField] private TextMeshProUGUI _message;

        private CancellationTokenSource _cancellationToken;
        private bool _isTriggered;

        public void Start()
        {
            _cancellationToken = new CancellationTokenSource();

            _inputAction.Enable();
            _inputAction.performed += ChangeObjActivity;
        }

        private void OnTriggerEnter(Collider other)
        {
            _isTriggered = true;

            _message.gameObject.SetActive(true);
            _message.text = "Press E";
        }

        private void OnTriggerExit(Collider other)
        {
            _isTriggered = false;

            _message.gameObject.SetActive(false);
        }

        private void ChangeObjActivity(InputAction.CallbackContext context)
        {
            if (_isTriggered)
                ChangeObjActivityAsync();
        }

        private async UniTaskVoid ChangeObjActivityAsync()
        {
            var nextAcivityState = !_gameObject.activeSelf;

            await UniTask.Delay(TimeSpan.FromSeconds(5), false, PlayerLoopTiming.Update, _cancellationToken.Token);

            _gameObject.SetActive(nextAcivityState);
        }

        private void OnDisable()
        {
            _cancellationToken.Cancel();
        }
    }
}
