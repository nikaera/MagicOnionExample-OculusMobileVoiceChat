using MagicOnion;
using UnityEngine;
using MessagePack;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IGamingHubReceiver
{
    void OnJoin(Player player);
    void OnLeave(Player player);
    void OnPlayerData(Player player);
}

public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
{
    Task<Player[]> JoinAsync(string roomName, string playerId, string playerName);
    Task LeaveAsync();
    Task PlayerDataAsync(Part[] parts, OpusData voiceData);
}

public enum PartType
{
    Body = 0, Head, LeftHand, RightHand,
}

[MessagePackObject]
public class Part
{
    [Key(0)]
    public Vector3 Position { get; set; }
    [Key(1)]
    public Quaternion Rotation { get; set; }
}

[MessagePackObject]
public class OpusData
{
    [Key(0)]
    public byte[] Bytes { get; set; }
    [Key(1)]
    public int EncodedLength { get; set; }
    [Key(2)]
    public int FrameCount { get; set; }
}

[MessagePackObject]
public class Player
{
    [Key(0)]
    public string Id { get; set; }
    [Key(1)]
    public string Name { get; set; }
    [Key(2)]
    public Part[] Parts { get; set; }
    [Key(3)]
    public OpusData VoiceData { get; set; }

    public Player(string Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
        this.Parts = new Part[Enum.GetNames(typeof(PartType)).Length];
        for (var i = 0; i < this.Parts.Length; i++)
        {
            this.Parts[i] = new Part { Position = new Vector3(), Rotation = new Quaternion() };
        }
    }
}
