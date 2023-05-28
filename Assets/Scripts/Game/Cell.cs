using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IdleMatch.Game
{
    /// <summary>
    /// Represents a cell on the game board.
    /// </summary>
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// The SpriteRenderer component attached to the cell.
        /// </summary>
        public SpriteRenderer SpriteRenderer { get; private set; }

        /// <summary>
        /// Indicates if the cell is unlocked.
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        /// The element placed on the cell.
        /// </summary>
        public GemElement Element { get { return _element; }
            private set
            {
                _element = value;
                SetElement(value);
            } 
        }

        private GemElement _element;

        /// <summary>
        /// The chunk that the cell belongs to.
        /// </summary>
        public Chunk Chunk;

        public bool AbleToSpawnElement;

        /// <summary>
        /// The x-coordinate of the cell within the chunk.
        /// </summary>
        public int x, y;

        /// <summary>
        /// The global x-coordinate of the cell in the game board.
        /// </summary>
        public int globalX
        {
            get { return Chunk.CHUNK_SIZE * Chunk.x + x; }
        }

        /// <summary>
        /// The global y-coordinate of the cell in the game board.
        /// </summary>
        public int globalY
        {
            get { return Chunk.CHUNK_SIZE * Chunk.y + y; }
        }

        /// <summary>
        /// The global position of the cell in the game board.
        /// </summary>
        public Vector2Int globalPosition
        {
            get { return new Vector2Int(Chunk.CHUNK_SIZE * Chunk.x + x, Chunk.CHUNK_SIZE * Chunk.y + y); }
        }
        [SerializeField, ReadOnly(true)]

        /// <summary>
        /// Spawns an element on the cell.
        /// </summary>
        public void SpawnElement()
        {
            AbleToSpawnElement = false;

            GameObject gameObject = new GameObject("Element");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = transform.position + new Vector3(0, 1, 0);
            GemElement element = gameObject.AddComponent<GemElement>();


            int value = Random.Range(0, Gameboard.Instance.Sprites.Count);
            element.SpriteRenderer.sprite = Gameboard.Instance.Sprites[value];
            element.value = value;
            element.transform.localScale = new Vector3();
            element.transform.LeanMove(transform.position, .25f).setEase(LeanTweenType.easeOutBack);
            element.transform.LeanScale(new Vector3(.9f, .9f, .9f), .25f).setOnComplete(()=> {
                Element = element;
                AbleToSpawnElement = true;
            });
        }

        public void SwapElement(Cell from)
        {
            if (Element == null)
            {
                BoardController.Instance.CanSwap = true;
                return;
            }

            if (from.Element != null)
            {
                Element.transform.LeanMove(from.transform.position, .5f).setEase(LeanTweenType.easeOutBack);
                from.Element.transform.LeanMove(transform.position, .5f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => {
                    (from.Element, Element) = (Element, from.Element);
                    BoardController.Instance.CanSwap = true;
                });
            }
            else
            {
                Element.transform.LeanMove(from.transform.position, .5f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => {
                    Element.transform.LeanMove(transform.position, .5f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
                    {
                        BoardController.Instance.CanSwap = true;
                    });
                });
            }
        }

        private void SetElement(GemElement element)
        {
            if (element == null) return;
            element.transform.position = transform.position;
            element.transform.SetParent(transform);

            // Push element to the bottom cell when it's empty
            Cell cellBelow = GetCellNeighbor(0, -1);
            if (cellBelow == null || cellBelow.Element != null || !cellBelow.Unlocked) return;
            Element.transform.LeanMove(cellBelow.transform.position, .125f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => {
                (Element, cellBelow.Element) = (null, Element);
            });
        }

        /// <summary>
        /// Unlocks the cell.
        /// </summary>
        public void Unlock()
        {
            Chunk.Activate();
            Unlocked = true;
            Events.UnlockCell(this);
        }

        /// <summary>
        /// Retrieves the neighboring cell at the specified offset.
        /// </summary>
        /// <param name="x">X offset from the current cell.</param>
        /// <param name="y">Y offset from the current cell.</param>
        /// <returns>The neighboring cell, or null if out of bounds.</returns>
        public Cell GetCellNeighbor(int x, int y)
        {
            return Gameboard.Instance.GetCell(globalX + x, globalY + y);
        }
        /// <summary>
        /// Retrieves the neighboring cell at the specified offset.
        /// </summary>
        /// <param name="direction">X/Y offset from the current cell.</param>
        /// <returns>The neighboring cell, or null if out of bounds.</returns>
        public Cell GetCellNeighbor(Vector2Int direction)
        {
            return Gameboard.Instance.GetCell(globalX + direction.x, globalY + direction.y);
        }

        private void Awake()
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }
}
