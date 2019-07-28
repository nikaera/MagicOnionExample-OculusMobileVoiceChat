using UnityEngine;

namespace Nikaera.OculusMobileVoiceChat
{
    public class VRPlayer : MonoBehaviour
    {
        [SerializeField]
        Transform m_HeadTransform;

        [SerializeField]
        Transform m_BodyTransform;

        [SerializeField]
        Transform m_LeftHandTransform;

        [SerializeField]
        Transform m_RightHandTransform;

        [SerializeField]
        TextMesh m_NameplateTextMesh;

        [SerializeField]
        OpusPlayer m_OpusPlayer;

        int currentAudioFrameCount = -1;

        public void PlayAudio(OpusData opusData)
        {
            if(currentAudioFrameCount != opusData.FrameCount) {
                currentAudioFrameCount = opusData.FrameCount;
                m_OpusPlayer.PlayAudio(opusData);
            }
        }

        public void SetName(string name) {
            m_NameplateTextMesh.text = name;
        }

        public void SetTransform(PartType partType, Part[] parts)
        {
            Part part = parts[(int)partType];

            switch (partType)
            {
                case PartType.Body:
                    this.m_BodyTransform.SetPositionAndRotation(part.Position, part.Rotation);
                    break;
                case PartType.Head:
                    this.m_HeadTransform.SetPositionAndRotation(part.Position, part.Rotation);
                    break;
                case PartType.LeftHand:
                    this.m_LeftHandTransform.SetPositionAndRotation(part.Position, part.Rotation);
                    break;
                case PartType.RightHand:
                    this.m_RightHandTransform.SetPositionAndRotation(part.Position, part.Rotation);
                    break;
            }
        }
    }
}
