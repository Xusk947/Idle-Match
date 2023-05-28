using IdleMatch.Input;
using UnityEngine;

namespace IdleMatch.Game
{
    public class BoardController : MonoBehaviour
    {
        public static BoardController Instance { get; private set; }

        public bool CanSwap = true;

        private Camera _camera;
        private InputHandler _input;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _input = InputHandler.Instance;
        }

        private void Update()
        {

            UpdateMovement();
            UpdateElementsInteraction();
        }

        private void UpdateElementsInteraction()
        {
            if (CanSwap && _input.TileOn != Vector2Int.zero && _input.Direction != Vector2Int.zero)
            {
                Cell cell = Gameboard.Instance.GetCell(_input.TileOn.x, _input.TileOn.y);
                if (cell == null || cell.Element == null) return;
                Cell neighbor = cell.GetCellNeighbor(_input.Direction);
                if (neighbor == null) return;
                cell.SwapElement(neighbor);
                CanSwap = false;
                _input.Reset();
            }
        }

        private void UpdateMovement()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            if (_input.IsPressed)
            {
                Vector2 velocity = _input.DisplayMoveVelocity * Time.deltaTime * 60f;

                _camera.transform.position += new Vector3(velocity.x, velocity.y, 0);
            }
        }

        public void Toggle(bool value)
        {
            gameObject.SetActive(value);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector2 mouse = UnityEngine.Input.mousePosition;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mouse);
            Vector2Int tilePos = Gameboard.WorldToTilePosition(mousePosition.x, mousePosition.y) - new Vector2Int(1, 1);

            Gizmos.DrawWireCube(new Vector3(tilePos.x, tilePos.y, 0), new Vector3(1, 1, 1));
        }
    }
}