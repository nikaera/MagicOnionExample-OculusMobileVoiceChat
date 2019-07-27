using System;
using System.Threading.Tasks;
using Grpc.Core;
using UnityEngine;

namespace Nikaera.OculusMobileVoiceChat
{
    public class VRRoomManager : MonoBehaviour
    {
        [SerializeField]
        string m_UserName = "TestUser";

        [SerializeField]
        string m_RoomName = "TestRoom";

        [SerializeField]
        string m_MagicOnionHost = "0.0.0.0";

        [SerializeField]
        GameObject m_AvatarGameObject;

        [SerializeField]
        Transform m_BodyTransform;

        [SerializeField]
        Transform m_HeadTransform;

        [SerializeField]
        Transform m_LeftHandTransform;

        [SerializeField]
        Transform m_RightHandTransform;

        private Channel channel;

        private GamingHubClient client;
        private Part[] playerParts = new Part[Enum.GetNames(typeof(PartType)).Length];

        MicrophoneEncoder encoder;
        OpusData voiceData;

        bool isRotateY = false;

        void Awake()
        {
            channel = new Channel(m_MagicOnionHost, 12345, ChannelCredentials.Insecure);
            client = new GamingHubClient(m_AvatarGameObject);

            for (int i = 0; i < playerParts.Length; i++) {
                playerParts[i] = new Part { Position = Vector3.zero, Rotation = Quaternion.identity };
            }
        }

        async Task Start() {
            await this.client.ConnectAsync(channel, this.m_RoomName, this.m_UserName);
        }

        void OnEnable()
        {
            encoder = GetComponent<MicrophoneEncoder>();
            encoder.OnEncoded += OnEncoded;
        }

        void OnDisable()
        {
            encoder.OnEncoded -= OnEncoded;
        }

        void OnEncoded(byte[] data, int length)
        {
            voiceData = new OpusData
            {
                Bytes = data,
                EncodedLength = length
            };
            client.PlayerDataAsync(playerParts, voiceData);
        }

        void Update()
        {
            if (OVRInput.GetDown(OVRInput.RawButton.Back) ||
                OVRInput.GetDown(OVRInput.RawButton.X))
            {
                Application.Quit();
            }



            if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Quest)
            {
                Debug.Log("OculusQuest");
                Vector2 RStickVec = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                Vector2 LStickVec = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

                m_BodyTransform.position += m_BodyTransform.rotation * new Vector3(LStickVec.x, 0f, LStickVec.y) * 0.025f;

                if (!isRotateY && Math.Abs(RStickVec.x) > 0.7)
                {
                    isRotateY = true;
                    float angle = RStickVec.x > 0 ? 23.5f : -23.5f;
                    m_BodyTransform.localEulerAngles += new Vector3(0f, angle, 0f);
                }
                else if (Math.Abs(RStickVec.x) < 0.7)
                {
                    isRotateY = false;
                }
            }
            else if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Go)
            {
                Debug.Log("OculusGo");
                if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
                {
                    Vector2 touchPadPt = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

                    bool isInputLeft = touchPadPt.x < -0.5 && -0.5 < touchPadPt.y && touchPadPt.y < 0.5;
                    if (!isRotateY && isInputLeft)
                    {
                        isRotateY = true;
                        m_BodyTransform.localEulerAngles += new Vector3(0f, -23.5f, 0f);
                    }

                    bool isInputRight = touchPadPt.x > 0.5 && -0.5 < touchPadPt.y && touchPadPt.y < 0.5;
                    if (!isRotateY && isInputRight)
                    {
                        isRotateY = true;
                        m_BodyTransform.localEulerAngles += new Vector3(0f, 23.5f, 0f);
                    }

                    if (touchPadPt.y > 0.5 && -0.5 < touchPadPt.x && touchPadPt.x < 0.5)
                    {
                        m_BodyTransform.position += m_BodyTransform.rotation * new Vector3(0f, 0f, 1f) * 0.025f;
                    }
                    if (touchPadPt.y < -0.5 && -0.5 < touchPadPt.x && touchPadPt.x < 0.5)
                    {
                        m_BodyTransform.position += m_BodyTransform.rotation * new Vector3(0f, 0f, -1f) * 0.025f;
                    }
                }
                else
                {
                    isRotateY = false;
                }
            }

            SetPlayerParts(PartType.Body, m_BodyTransform);
            SetPlayerParts(PartType.Head, m_HeadTransform);
            SetPlayerParts(PartType.LeftHand, m_LeftHandTransform);
            SetPlayerParts(PartType.RightHand, m_RightHandTransform);
        }

        void SetPlayerParts(PartType type, Transform _transform)
        {
            playerParts[(int)type].Position = _transform.position;
            playerParts[(int)type].Rotation = _transform.rotation;
        }

        async Task OnApplicationQuit() {
            await this.client.LeaveAsync();
            await this.client.DisposeAsync();
            await this.channel.ShutdownAsync();
        }
    }
}