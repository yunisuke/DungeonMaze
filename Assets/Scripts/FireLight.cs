using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Effects
{
    public class FireLight : MonoBehaviour
    {
        private bool m_Burning = true;
        private Light m_Light;


        private void Start()
        {
            m_Light = GetComponent<Light>();
        }


        private void Update()
        {
            if (m_Burning)
            {
                m_Light.intensity = 4 * ((Mathf.PerlinNoise(Time.time, 0) + 1) / 2);
            }
        }

        public void Extinguish()
        {
            m_Burning = false;
            m_Light.enabled = false;
        }
    }
}
