using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleMatch.Game
{
    public static class Match
    {
        private static int[] _signs = new int[]{-1, 1};
        public static bool TryMatchElements(Cell cell)
        {
            List<Cell> matchedCells = MatchRow(cell);

            if (matchedCells.Count > 2)
            {
                foreach(Cell matchedCell in matchedCells) {
                    matchedCell.MatchDestroy();
                }
                return true;
            }

            return false;
        }

        private static List<Cell> MatchRow(Cell baseCell)
        {
            List<Cell> matchedCells = MatchHorizontal(baseCell);
            if (matchedCells.Count < 3)
            {
                matchedCells = MatchVertical(baseCell);
            }
            return matchedCells;
        }

        private static List<Cell> MatchHorizontal(Cell baseCell)
        {
            List<Cell> matchedCells = new List<Cell>() { baseCell };

            for (int i = 0; i < _signs.Count(); i++)
            {
                int sign = _signs[i];
                int count = 1;
                while (true)
                {
                    Cell neighborCell = baseCell.GetCellNeighbor(sign * count, 0);
                    if (neighborCell == null) break;
                    if (!neighborCell.Unlocked) break;
                    if (neighborCell.Element == null) break;
                    if (neighborCell.Element.value != baseCell.Element.value) break;
                    count++;
                    matchedCells.Add(neighborCell);
                }
            }

            return matchedCells;
        }

        private static List<Cell> MatchVertical(Cell baseCell)
        {
            List<Cell> matchedCells = new List<Cell>() { baseCell };

            for (int i = 0; i < _signs.Count(); i++)
            {
                int sign = _signs[i];
                int count = 1;
                while (true)
                {
                    Cell neighborCell = baseCell.GetCellNeighbor(0, sign * count);
                    if (neighborCell == null) break;
                    if (!neighborCell.Unlocked) break;
                    if (neighborCell.Element == null) break;
                    if (neighborCell.Element.value != baseCell.Element.value) break;
                    count++;
                    matchedCells.Add(neighborCell);
                }
            }

            return matchedCells;
        }
    }
}