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
        public GemElement Element { get; private set; }

        /// <summary>
        /// The chunk that the cell belongs to.
        /// </summary>
        public Chunk Chunk;

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

        /// <summary>
        /// Spawns an element on the cell.
        /// </summary>
        public void SpawnElement()
        {
            GameObject gameObject = new GameObject("Element");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = transform.position;

            Element = gameObject.AddComponent<GemElement>();

            int value = Random.Range(0, Gameboard.Instance.Sprites.Count);
            Element.SpriteRenderer.sprite = Gameboard.Instance.Sprites[value];
            Element.value = value;
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

        private void Awake()
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }
}
