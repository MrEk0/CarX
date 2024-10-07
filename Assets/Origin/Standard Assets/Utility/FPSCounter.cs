using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class FPSCounter : MonoBehaviour
    {
        private const float FPS_MEASURE_PERIOD = 0.5f;
        private const string DISPLAY = "{0} FPS";
        
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        private Text m_Text;
        
        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + FPS_MEASURE_PERIOD;
            m_Text = GetComponent<Text>();
        }
        
        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (!(Time.realtimeSinceStartup > m_FpsNextPeriod))
                return;
            
            m_CurrentFps = (int)(m_FpsAccumulator / FPS_MEASURE_PERIOD);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += FPS_MEASURE_PERIOD;
            m_Text.text = string.Format(DISPLAY, m_CurrentFps);
        }
    }
}
