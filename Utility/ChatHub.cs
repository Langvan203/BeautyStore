using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using my_cosmetic_store.Repository;
using Microsoft.EntityFrameworkCore;
using my_cosmetic_store.Models;

namespace my_cosmetic_store.Utility
{
    public class ChatHub : Hub
    {
        private readonly ConnectionRepository _connectionRepository;
        private readonly MessageRepository _messageRepository;

        private static readonly ConcurrentDictionary<string, string> _connections = new();

        public ChatHub(ConnectionRepository connectionRepository, MessageRepository messageRepository)
        {
            _connectionRepository = connectionRepository;
            _messageRepository = messageRepository;
        }

        public override async Task OnConnectedAsync()
        {
            //var userId = Context.UserIdentifier; // Lấy userId từ thông tin xác thực
            //if (userId != null)
            //{
            //    _connections[Context.ConnectionId] = userId;
            //    // Lưu connectionId vào cơ sở dữ liệu (sẽ triển khai ở bước sau)
            //    await SaveConnection(userId, Context.ConnectionId);
            //}
            Console.WriteLine("Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                _connections.TryRemove(Context.ConnectionId, out _);
                // Xóa connectionId khỏi cơ sở dữ liệu
                await RemoveConnection(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string toUserId, string message)
        {
            var senderId = Context.UserIdentifier;
            if (senderId == null) return;

            // Lưu tin nhắn vào cơ sở dữ liệu
            await SaveMessage(senderId, toUserId, message);

            // Gửi tin nhắn đến người nhận
            var receiverConnectionId = _connections.FirstOrDefault(x => x.Value == toUserId).Key;
            if (receiverConnectionId != null)
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message);
            }

            // Gửi lại cho người gửi (để hiển thị trên giao diện của họ)
            await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
        }

        private async Task SaveConnection(string userId, string connectionId)
        {
            var connection = new Connection { UserId = userId, ConnectionId = connectionId };
            await _connectionRepository.AddAsync(connection);
        }
        private async Task RemoveConnection(string connectionId)
        {
            var connection = await _connectionRepository.FindFirstOrDefaultAsync(c => c.ConnectionId == connectionId);
            if (connection != null)
            {
                _connectionRepository.DeleteByEntity(connection);
            }
        }

        private async Task SaveMessage(string senderId, string receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.UtcNow
            };
            await _messageRepository.AddAsync(message);
        }
    }
}
