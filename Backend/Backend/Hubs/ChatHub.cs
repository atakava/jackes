using Backend.DataBase.Entity;
using Microsoft.AspNetCore.SignalR;
using Backend.DataBase.Entity.Model.Chat;
using Microsoft.EntityFrameworkCore;

namespace Backend.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task OnMessage(string text, string user)
    {
        var message = new Message
        {
            Text = text,
            User = user,
            CreateAt = DateTime.UtcNow
        };

        _context.Message.Add(message);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("OnMessage", new
        {
            Message = message
        });
    }

    public override async Task OnConnectedAsync()
    {
        var items = await _context.Message.Take(100).ToListAsync();

        await Clients.Caller.SendAsync("OnConnected", new
        {
            Items = items
        });
    }
}