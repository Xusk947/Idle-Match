using IdleMatch.Input;
using UnityEngine;

namespace IdleMatch
{
    public class GameManager : MonoBehaviour
    {
        
        private void Awake()
        {
            CreateInputSystem();
        }

        private void CreateInputSystem()
        {
            GameObject gameObject = new GameObject("Input Manager");
            if (Application.isMobilePlatform)
            {
                gameObject.AddComponent<MobileInput>();
            } else
            {
                gameObject.AddComponent<DesktopInput>();
            }
        }
    }
}