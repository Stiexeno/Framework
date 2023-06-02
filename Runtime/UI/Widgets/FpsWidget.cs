using TMPro;
using UnityEngine;

namespace Framework.Utils
{
    public class FpsWidget : MonoBehaviour
    {
        private TMP_Text text;

        private float hudRefreshRate = 1;
        private float timer;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (Time.unscaledTime > timer)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                text.text = "FPS: " + fps;
                timer = Time.unscaledTime + hudRefreshRate;
            }
        }
        
    }
}
