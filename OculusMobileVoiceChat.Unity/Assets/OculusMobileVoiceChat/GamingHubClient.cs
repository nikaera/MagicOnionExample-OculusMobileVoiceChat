using Grpc.Core;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using MagicOnion.Client;
using System;

namespace Nikaera.OculusMobileVoiceChat
{
    public class GamingHubClient : IGamingHubReceiver
    {
        Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

        IGamingHub client;

        GameObject avatar;

        string playerId;

        public GamingHubClient(GameObject avatar)
        {
            this.avatar = avatar;
            this.playerId = Guid.NewGuid().ToString("N");
        }

        public async Task<GameObject> ConnectAsync(Channel grpcChannel, string roomName, string playerName)
        {
            client = StreamingHubClient.Connect<IGamingHub, IGamingHubReceiver>(grpcChannel, this);
            var roomPlayers = await client.JoinAsync(roomName, playerId, playerName);

            foreach (var player in roomPlayers)
            {
                if (player.Id != playerId)
                {
                    (this as IGamingHubReceiver).OnJoin(player);
                }
            }

            return players[playerName];
        }

        public Task LeaveAsync()
        {
            return client.LeaveAsync();
        }

        public Task PlayerDataAsync(Part[] parts, OpusData voiceData)
        {
            return client.PlayerDataAsync(parts, voiceData);
        }

        public Task DisposeAsync()
        {
            return client.DisposeAsync();
        }

        public Task WaitForDisconnect()
        {
            return client.WaitForDisconnect();
        }

        void IGamingHubReceiver.OnJoin(Player player)
        {
            var gameObject = UnityEngine.Object.Instantiate(avatar);
            gameObject.name = player.Id;

            var vrPlayer = gameObject.GetComponent<VRPlayer>();
			vrPlayer.SetName(player.Name);
            SetVRPlayerTransform(ref vrPlayer, player.Parts);

            players[player.Id] = gameObject;
        }

        void IGamingHubReceiver.OnLeave(Player player)
        {
            if (players.TryGetValue(player.Id, out var gameObject))
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        void IGamingHubReceiver.OnPlayerData(Player player)
        {
            if (players.TryGetValue(player.Id, out var gameObject))
            {
                var vrPlayer = gameObject.GetComponent<VRPlayer>();
                if(playerId != player.Id)
                {
                    vrPlayer.PlayAudio(player.VoiceData);
                }
                SetVRPlayerTransform(ref vrPlayer, player.Parts);
            }
        }

        void SetVRPlayerTransform(ref VRPlayer vrPlayer, Part[] parts)
        {
            vrPlayer.SetTransform(PartType.Body, parts);
            vrPlayer.SetTransform(PartType.Head, parts);
            vrPlayer.SetTransform(PartType.LeftHand, parts);
            vrPlayer.SetTransform(PartType.RightHand, parts);
        }
    }
}