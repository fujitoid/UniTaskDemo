using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

namespace MyDemo.ObstaclesTest
{
    public class SimpleObstacle : MonoBehaviour
    {
        [SerializeField] private float _timeDuration = 2;
        [Space]
        [SerializeField] private Transform _upperPoint;
        [SerializeField] private Transform _buttomPoint;

        private Sequence _sequence;
        private CancellationTokenSource _tokenSource;

        private void Start()
        {
            _tokenSource = new CancellationTokenSource();
            _tokenSource.Token.Register(() =>
            {
                _sequence?.Kill();
            });
        }

        public async UniTask MoveUpAsync()
        {
            _tokenSource = new CancellationTokenSource();

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(this.transform.DOMove(_upperPoint.position, _timeDuration));
            await _sequence.AsyncWaitForCompletion();
        }

        public async UniTask MoveDownAsync()
        {
            _tokenSource = new CancellationTokenSource();

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(this.transform.DOMove(_buttomPoint.position, _timeDuration));
            await _sequence.AsyncWaitForCompletion();
        }

        public void Stop() => _tokenSource.Cancel();

        private void OnDestroy()
        {
            _tokenSource.Cancel();
        }
    } 
}
