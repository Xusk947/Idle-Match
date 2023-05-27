using IdleMatch.Game;
using System;
using UnityEngine;

namespace IdleMatch
{
    public static class Events
    {
        public static event EventHandler<Cell> OnCellUnlocked;
        
        public static void UnlockCell(Cell cell)
        {
            OnCellUnlocked?.Invoke(typeof(Events), cell);
        }
    }
}