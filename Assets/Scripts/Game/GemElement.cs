using UnityEngine;
using UnityEngine.EventSystems;

namespace IdleMatch.Game
{
    /// <summary>
    /// Represents a gem element in the game.
    /// </summary>
    public class GemElement : MonoBehaviour
    {
        /// <summary>
        /// The sprite renderer component of the gem element.
        /// </summary>
        public SpriteRenderer SpriteRenderer { get; private set; }

        /// <summary>
        /// The value of the gem element.
        /// </summary>
        public int value;

        /// <summary>
        /// The position of the gem element.
        /// </summary>
        [HideInInspector]
        public Vector2 pos;

        private void Awake()
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            transform.localScale = new Vector3(.9f, .9f, 1f);
        }

        /// <summary>
        /// Resets the position of the gem element.
        /// </summary>
        public void ResetPosition()
        {
            pos = new Vector2(0, 0);
        }
    }
}
