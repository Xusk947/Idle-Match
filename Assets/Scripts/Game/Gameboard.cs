﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IdleMatch.Game
{
    /// <summary>
    /// Manages the game board and its elements.
    /// </summary>
    public class Gameboard : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the Gameboard.
        /// </summary>
        public static Gameboard Instance;

        /// <summary>
        /// Prefab for the game board elements.
        /// </summary>
        public GameObject ElementPrefab;

        [SerializeField]
        private int chunk_size = 3;

        /// <summary>
        /// Sprites for the game board elements.
        /// </summary>
        public List<Sprite> Sprites;

        private Chunk[,] _chunks;
        private List<Cell> _spawnerCells;

        private bool _needSpawnCellsElement;
        private float _spawnDelay = 5f;
        private float _spawnDelayTimer;

        private void Awake()
        {
            Instance = this;
            _chunks = new Chunk[chunk_size, chunk_size];
            _spawnerCells = new List<Cell>();
            Events.OnCellUnlocked += OnCellUnlock;
        }

        private void Start()
        {
            FillChunks();
            UnlockCentreCells();
            Camera.main.transform.position = new Vector3(chunk_size * Chunk.CHUNK_SIZE / 2f, chunk_size * Chunk.CHUNK_SIZE / 2f, -10f);
        }

        private void Update()
        {
            _spawnDelayTimer -= Time.deltaTime * 60f;
            if (_spawnDelayTimer < 0)
            {
                print(_spawnerCells.Count);
                _spawnDelayTimer = _spawnDelay;
                for (int i =0; i < _spawnerCells.Count; i++) 
                {
                    Cell activeCell = _spawnerCells[i];
                    if (activeCell.Element == null)
                    {
                        _needSpawnCellsElement = true;
                    }
                }
            }

            if (_needSpawnCellsElement)
            {
                StartCoroutine(TryToFillBoard());
            }
        }

        private void OnCellUnlock(object sender, Cell cell)
        {
            cell.AbleToSpawnElement = true;
            Cell cellBelow = cell.GetCellNeighbor(0, -1);
            print(cell + " : " + cellBelow + " : " + cellBelow.Unlocked + " :"  + _spawnerCells.Contains(cellBelow));
            if (cellBelow != null && cellBelow.Unlocked && _spawnerCells.Contains(cellBelow))
            {
                _spawnerCells.Remove(cellBelow);
            } 
            _spawnerCells.Add(cell);
            _spawnDelayTimer = _spawnDelay;
        }

        /// <summary>
        /// Attempts to fill the game board with elements in empty cells.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator TryToFillBoard()
        {
            do
            {
                for (int i = 0; i < _spawnerCells.Count; i++)
                {
                    Cell cell = _spawnerCells[i];
                    if (cell.Element == null && cell.AbleToSpawnElement)
                    {
                        cell.SpawnElement();
                        yield return 0;
                    }
                }
                _needSpawnCellsElement = false;
                yield return 0;
            } while (_needSpawnCellsElement);
            yield return 0;
        }


        /// <summary>
        /// Retrieves the cell at the specified coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <returns>The Cell object at the specified coordinates, or null if out of bounds.</returns>
        public Cell GetCell(int x, int y)
        {
            int chunkX = x / Chunk.CHUNK_SIZE;
            int chunkY = y / Chunk.CHUNK_SIZE;
            if (chunkX < 0 || chunkX >= chunk_size || chunkY < 0 || chunkY >= chunk_size)
            {
                return null;
            }

            int tileX = x % Chunk.CHUNK_SIZE;
            int tileY = y % Chunk.CHUNK_SIZE;
            Chunk chunk = _chunks[chunkX, chunkY];
            if (chunk == null)
            {
                return null;
            }

            Cell cell = chunk.GetCell(tileX, tileY);
            return cell;
        }

        private void UnlockCentreCells()
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    int centrePosition = chunk_size * Chunk.CHUNK_SIZE / 2;
                    int cx = x + centrePosition - 3;
                    int cy = y + centrePosition - 3;
                    GetCell(cx, cy).Unlock();
                }
            }
        }

        private void FillChunks()
        {
            for (int x = 0; x < chunk_size; x++)
            {
                for (int y = 0; y < chunk_size; y++)
                {
                    Chunk chunk = new GameObject($"Chunk({x}:{y})").AddComponent<Chunk>();
                    chunk.x = x;
                    chunk.y = y;
                    chunk.transform.SetParent(transform);
                    chunk.transform.position = new Vector2(x * Chunk.CHUNK_SIZE, y * Chunk.CHUNK_SIZE);
                    chunk.FillCells();

                    _chunks[x, y] = chunk;
                }
            }
        }
        /// <summary>
        /// Converts world position to tile position.
        /// </summary>
        /// <param name="x">X coordinate of the world position.</param>
        /// <param name="y">Y coordinate of the world position.</param>
        /// <returns>The tile position as a Vector2Int.</returns>
        public static Vector2Int WorldToTilePosition(float x, float y)
        {
            return new Vector2Int(Mathf.CeilToInt(x + .5f), Mathf.CeilToInt(y + .5f));
        }

        public static Vector2Int WorldToTilePosition(Vector2 position)
        {
            return new Vector2Int(Mathf.CeilToInt(position.x + .5f), Mathf.CeilToInt(position.y + .5f));
        }
    }
}
