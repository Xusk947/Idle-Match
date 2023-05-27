using UnityEngine;

namespace IdleMatch.Input
{
    /// <summary>
    /// Represents an input handler for game input.
    /// </summary>
    public interface InputHandler
    {
        /// <summary>
        /// Indicates if the input is currently being pressed.
        /// </summary>
        bool IsPressed { get; }

        /// <summary>
        /// The direction of the input.
        /// </summary>
        Vector2Int Direction { get; }

        /// <summary>
        /// The velocity of the input for display purposes.
        /// </summary>
        Vector2 DisplayMoveVelocity { get; }
    }
}
