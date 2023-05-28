using IdleMatch.Game;
using UnityEngine;

namespace IdleMatch.Input
{
    public class DesktopInput : MonoBehaviour, InputHandler
    {
        public bool IsPressed => UnityEngine.Input.anyKey;

        public Vector2Int Direction => _direction;

        public Vector2 DisplayMoveVelocity => _displayMoveVelocity;

        public Vector2Int TileOn => _tilePosition;

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
            HandleKeyboard();
            HandleMouse();
        }

        private void HandleKeyboard()
        {
            _displayMoveVelocity.x = UnityEngine.Input.GetAxis("Horizontal");
            _displayMoveVelocity.y = UnityEngine.Input.GetAxis("Vertical");
        }
        private void HandleMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _lastPosition = UnityEngine.Input.mousePosition;
                Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(_lastPosition.x, _lastPosition.y, 0));
                // mouse offset
                _tilePosition = Gameboard.WorldToTilePosition(worldMousePosition.x, worldMousePosition.y) - new Vector2Int(1, 1);
            } else if (UnityEngine.Input.GetMouseButton(0) && _lastPosition != Vector2.zero)
            {
                Vector2 currentPosition = UnityEngine.Input.mousePosition;

                Vector2 direction = (currentPosition - _lastPosition).normalized;
                Vector2Int dif = Gameboard.WorldToTilePosition(currentPosition) - Gameboard.WorldToTilePosition(_lastPosition);
                //print(new Vector2(dif.x, dif.y).normalized);
                float distance = Vector2.Distance(currentPosition, _lastPosition);

                if (distance > 10f)
                {
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        _direction.x = (int)Mathf.Sign(direction.x);
                        _direction.y = 0;
                    }
                    else
                    {
                        _direction.x = 0;
                        _direction.y = (int)Mathf.Sign(direction.y);
                    }
                    if (_direction.x == -1 || _direction.x == 1)
                    {
                        _direction.y = 0 - _direction.y;
                    }
                }
            } else if (UnityEngine.Input.GetMouseButtonUp(0))
            {

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