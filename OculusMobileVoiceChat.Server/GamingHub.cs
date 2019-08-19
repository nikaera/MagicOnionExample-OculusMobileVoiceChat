using System.Threading.Tasks;
using System.Linq;

using MagicOnion.Server.Hubs;
using System;

public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
{
    IGroup room;
    Player self;
    IInMemoryStorage<Player> storage;

    public async Task<Player[]> JoinAsync(string roomName, string playerId, string playerName)
    {
        self = new Player(playerId, playerName);

        (room, storage) = await Group.AddAsync(roomName, self);
        BroadcastExceptSelf(room).OnJoin(self);

        return storage.AllValues.ToArray();
    }

    public async Task LeaveAsync()
    {
        await room.RemoveAsync(this.Context);
        Broadcast(room).OnLeave(self);
    }

    public async Task PlayerDataAsync(Part[] parts, OpusData voiceData)
    {
        self.Parts = parts;
        self.VoiceData = voiceData;
        BroadcastExceptSelf(room).OnPlayerData(self);
    }

    protected override async ValueTask OnDisconnected()
    {
        await LeaveAsync();
    }
}
