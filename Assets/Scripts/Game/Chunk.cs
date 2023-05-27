using UnityEngine;

namespace IdleMatch.Game
{
    /// <summary>
    /// Represents a chunk of the game board.
    /// </summary>
    public class Chunk : MonoBehaviour
    {
        /// <summary>
        /// The size of each chunk.
        /// </summary>
        public static int CHUNK_SIZE = 16;

        /// <summary>
        /// The size of each tile in the chunk.
        /// </summary>
        public static int TILE_SIZE = 64;

        /// <summary>
        /// The x-coordinate of the chunk.
        /// </summary>
        public int x;

        /// <summary>
        /// The y-coordinate of the chunk.
        /// </summary>
        public int y;

        /// <summary>
        /// Indicates if the chunk is active.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// The size of each tile in the chunk.
        /// </summary>
        public int tileSize = 64;

        private Cell[,] cells;

        private void Awake()
        {
            cells = new Cell[CHUNK_SIZE, CHUNK_SIZE];
        }

        /// <summary>
        /// Activates the chunk.
        /// </summary>
        public void Activate()
        {
            Active = true;
        }

        /// <summary>
        /// Retrieves the cell at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell within the chunk.</param>
        /// <param name="y">The y-coordinate of the cell within the chunk.</param>
        /// <returns>The cell at the specified coordinates.</returns>
        public Cell GetCell(int x, int y)
        {
            return cells[x, y];
        }

        /// <summary>
        /// Fills the chunk with cells.
        /// </summary>
        public void FillCells()
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    Cell cell = new GameObject($"Cell({x}:{y})").AddComponent<Cell>();
                    cell.x = x;
                    cell.y = y;
                    cell.Chunk = this;

                    cell.transform.SetParent(transform);
                    cell.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);

                    cells[x, y] = cell;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + new Vector3(CHUNK_SIZE / 2f, CHUNK_SIZE / 2f, 0), new Vector3(CHUNK_SIZE, CHUNK_SIZE, 0));
        }
    }
}
