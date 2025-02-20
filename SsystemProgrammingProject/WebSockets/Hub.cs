using System.Net.WebSockets;
using System.Text;

public class Hub
{
    private Dictionary<Guid, List<WebSocket>> Rooms {  get; set; }
    private byte[] Buffer { get; set; }
    public Hub()
	{
		Rooms = [];
		Buffer = new byte[1024 * 4];
	}

	public void AddSocket(Guid roomId, WebSocket socket)
	{
        if (Rooms.ContainsKey(roomId))
            Rooms[roomId].Add(socket);
        else
            Rooms[roomId] = [socket];
    }

	public void RemoveSocket(Guid roomId, WebSocket socket)
	{
        Rooms[roomId].Remove(socket);
        if (Rooms[roomId].Count == 0)
            Rooms.Remove(roomId);
    }

	public async Task SendAllAsync(Guid roomId, WebSocketReceiveResult message, byte[] bytes)
	{
        foreach (var client in Rooms[roomId])
            await client.SendAsync(new(bytes), message.MessageType, message.EndOfMessage, CancellationToken.None);
    }

	public async Task<WebSocketReceiveResult> ReceiveResultAsync(WebSocket socket)
    {
		return await socket.ReceiveAsync(new(Buffer), CancellationToken.None);
    }

	public string EncodingMessage(WebSocketReceiveResult message)
	{  
		return Encoding.UTF8.GetString(Buffer[..message.Count]); 
	}
}