using UnityEngine;

namespace Common.Effects
{
    public class FireLight : MonoBehaviour
    {
        private bool m_Burning = true;
        private UnityEngine.Light m_Light;

        private void Start()
        {
            m_Light = GetComponent<UnityEngine.Light>();
        }


        private void Update()
        {
            if (m_Burning)
            {
                m_Light.intensity = 6 * ((Mathf.PerlinNoise(Time.time, 0) + 1) / 2);
            }
        }

        public void Extinguish()
        {
            m_Burning = false;
            m_Light.enabled = false;
        }
    }
}
