using UnityEngine;

namespace Nikaera.OculusMobileVoiceChat
{
    public class OpusPlayerVisualizer : MonoBehaviour
    {
        [SerializeField]
        OpusPlayer m_OpusPlayer;

        void Update()
        {
            var y = transform.localScale.y;
            var z = transform.localScale.z;
            transform.localScale = new Vector3(m_OpusPlayer.GetRMS() * 2.0f, y, z);
        }
    }
}
