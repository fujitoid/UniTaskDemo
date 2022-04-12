using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyDemo.ObstaclesTest
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private InputAction _controlInput;
        [Space]
        [SerializeField] private SimpleObstacle _obstacle;
        [SerializeField] private Trigger _trigger;
        [Space]
        [SerializeField] private TextMeshProUGUI _timeInfo;
        [SerializeField] private TextMeshProUGUI _message;

        private CancellationTokenSource _tokenSourceToOpen;
        private CancellationTokenSource _tokenSourceToClose;

        private float _currentTimeSource;

        private void Start()
        {
            _currentTimeSource = 0;

            _trigger.Triggered += OnObstacleCalledToClose;

            _tokenSourceToOpen = new CancellationTokenSource();

            _tokenSourceToClose = new CancellationTokenSource();

            _controlInput.Enable();
            _controlInput.performed += OnObstacleCalledToOpen;

            _timeInfo.text = _currentTimeSource.ToString();
        }

        private void OnDestroy()
        {
            _controlInput.performed -= OnObstacleCalledToOpen;
            _controlInput.Disable();

            _tokenSourceToOpen.Cancel();
            _tokenSourceToOpen.Dispose();

            _tokenSourceToClose.Cancel();
            _tokenSourceToClose.Dispose();

            _obstacle.Stop();
        }

        private void OnObstacleCalledToOpen(InputAction.CallbackContext context)
        {
            if (_trigger.IsTriggered)
                OnTriggered();
        }

        private void OnObstacleCalledToClose(bool isTriggered)
        {
            _message.gameObject.SetActive(isTriggered);
            _message.text = "Press Q";

            if (isTriggered)
                return;

            CloseAsync();
        }

        private async UniTaskVoid OnTriggered()
        {
            _tokenSourceToClose.Cancel();
            _tokenSourceToOpen = new CancellationTokenSource();

            if (_currentTimeSource <= 0)
            {
                await _obstacle.MoveUpAsync();
            }

            while (_tokenSourceToOpen.IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), false, PlayerLoopTiming.Update, _tokenSourceToOpen.Token);
                _currentTimeSource++;
                _timeInfo.text = _currentTimeSource.ToString();
            }
        }

        private async UniTaskVoid CloseAsync()
        {
            _tokenSourceToOpen.Cancel();
            _tokenSourceToClose = new CancellationTokenSource();

            while(_currentTimeSource > 0 && _tokenSourceToClose.IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                _currentTimeSource--;
                _timeInfo.text = _currentTimeSource.ToString();
            }

            if (_currentTimeSource <= 0)
                await _obstacle.MoveDownAsync();
        }
    }
}
