using IdleMatch.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleMatch.Input
{
    public class MobileInput : MonoBehaviour, InputHandler
    {
        public bool IsPressed => _isPressed;

        public Vector2Int Direction => _direction;

        public Vector2 DisplayMoveVelocity => _displayMoveVelocity;

        public Vector2Int TileOn => _tilePosition;

        private bool _isPressed;
        private Vector2 _lastPosition;
        private Vector2Int _direction;
        private Vector2Int _tilePosition;
        private Vector2 _displayMoveVelocity;

        private void Awake()
        {
            InputHandler.Instance = this;    
            _direction = new Vector2Int();
            _displayMoveVelocity = new Vector2();
        }

        private void Update()
        {
            _direction.Set(0, 0);
            _displayMoveVelocity.Set(0, 0);

            List<Touch> touches = UnityEngine.Input.touches.ToList();
            if (touches.Count > 0)
            {
                if (touches.Count > 1) HandleTwoTouches(touches);
                else HandleOneTouch(touches[0]);
            }
        }

        private void HandleOneTouch(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _lastPosition = touch.position;
                Vector2 worldTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(_lastPosition.x, _lastPosition.y, 0));
                _tilePosition = Gameboard.WorldToTilePosition(worldTouchPosition.x, worldTouchPosition.y);
            } else if (touch.phase == TouchPhase.Moved && _lastPosition != Vector2.zero)
            {
                Vector2 currentPosition = touch.position;

                Vector2 direction = (currentPosition - _lastPosition).normalized;

                float distance = Vector2.Distance(currentPosition, _lastPosition);

                if (distance > 1)
                {
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
                    {
                        _direction.x = (int) Mathf.Sign(direction.x);
                        _direction.y = 0;
                    } else
                    {
                        _direction.x = 0;
                        _direction.y = (int) Mathf.Sign(direction.y);
                    }
                    if (_direction.x == -1 || _direction.x == 1)
                    {
                        _direction.y = 0 - _direction.y;
                    }
                }
            } else if (touch.phase == TouchPhase.Ended )
            {
                _tilePosition.x = 0;
                _tilePosition.y = 0;
            }
        }

        private void HandleTwoTouches(List<Touch> touches)
        {
            if (touches[0].phase == TouchPhase.Moved && touches[1].phase == TouchPhase.Moved)
            {
                _displayMoveVelocity.x = touches[0].deltaPosition.x;
                _displayMoveVelocity.y = touches[1].deltaPosition.y;
                _displayMoveVelocity.Normalize();
            }
        }

        public void Reset()
        {
            _tilePosition = Vector2Int.zero;
            _direction = Vector2Int.zero;
            _lastPosition = Vector2.zero;
        }
    }
}