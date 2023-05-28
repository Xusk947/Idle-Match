using UnityEngine;

namespace IdleMatch.Input
{
    /// <summary>
    /// Represents an input handler for game input.
    /// </summary>
    public interface InputHandler
    {
        public static InputHandler Instance { get; protected set; }
        /// <summary>
        /// Indicates if the input is currently being pressed.
        /// </summary>
        bool IsPressed { get; }

        /// <summary>
        /// The direction of the input.
        /// </summary>
        Vector2Int Direction { get; }
        /// <summary>
        /// Indicates the Tile of the input
        /// </summary>
        Vector2Int TileOn { get; }

        /// <summary>
        /// The velocity of the input for display purposes.
        /// </summary>
        Vector2 DisplayMoveVelocity { get; }
        public void Reset();
    }
}
