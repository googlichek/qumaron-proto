using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class CircleMovementController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [Space]

        [SerializeField] [Range(0, 1000)] private float _maxSpeed = 500;
        [SerializeField] [Range(0, 1000)] private float _minSpeed = 500;

        [Space]

        [SerializeField] [Range(0, 1)] private float _speedChangeTime = 0.25f;

        [Space]

        [SerializeField] [Range(0, 1000)] private int _speedChangeDistanceMax = 150;

        private readonly Queue<Vector2> _positions = new Queue<Vector2>();

        private Vector2 _position;
        private Vector2 _targetPosition;


        private float _speed;
        private float _smoothVelocity;

        private bool _hasTarget = false;

        void Update()
        {
            HandleMovement();
            UpdateSpeed();
        }

        public void AddMovementPosition(Vector2 position)
        {
            _positions.Enqueue(position);
        }

        private void HandleMovement()
        {
            if (_positions.Count == 0 && !_hasTarget)
                return;

            var hasReachedTarget = _position.IsEqual(_targetPosition);
            if (!_hasTarget && hasReachedTarget)
            {
                _targetPosition = _positions.Dequeue();

                _hasTarget = true;
            }
            else if (_hasTarget && hasReachedTarget)
            {
                _hasTarget = false;
            }
            else
            {
                _position = Vector2.MoveTowards(_position, _targetPosition, Time.deltaTime * _speed);
            }

            transform.localPosition = _position;
        }

        private void UpdateSpeed()
        {
            switch (_positions.Count)
            {
                case 0 when !_hasTarget:
                    _speed = 0;
                    break;
                case 0:
                    _speed =
                        Vector2.Distance(_position, _targetPosition) <= _speedChangeDistanceMax
                            ? Mathf.SmoothDamp(_speed, _minSpeed, ref _smoothVelocity, _speedChangeTime)
                            : Mathf.SmoothDamp(_speed, _maxSpeed, ref _smoothVelocity, _speedChangeTime);
                    break;
                default:
                    _speed = Mathf.SmoothDamp(_speed, _maxSpeed, ref _smoothVelocity, _speedChangeTime);
                    break;
            }

            _speed = Mathf.Clamp(_speed, 0, _maxSpeed);
        }
    }
}
