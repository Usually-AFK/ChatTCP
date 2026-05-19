using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using WpfChatClient.Core.Models;

namespace WpfChatClient.Infrastructure;

public class MessageCache
{
    private readonly string _connectionString;

    public MessageCache()
    {
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chat.db");
        _connectionString = $"Data Source={dbPath}";
    }

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Messages (
                Id TEXT PRIMARY KEY,
                RoomId TEXT,
                Sender TEXT,
                Content TEXT,
                Timestamp TEXT,
                IsPrivate INTEGER
            )";
        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveMessageAsync(ChatMessageData msg)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR IGNORE INTO Messages (Id, RoomId, Sender, Content, Timestamp, IsPrivate)
                VALUES ($id, $roomId, $sender, $content, $timestamp, 0)";
            command.Parameters.AddWithValue("$id", string.IsNullOrEmpty(msg.MessageId) ? Guid.NewGuid().ToString() : msg.MessageId);
            command.Parameters.AddWithValue("$roomId", string.IsNullOrWhiteSpace(msg.RoomId) ? "General" : msg.RoomId);
            command.Parameters.AddWithValue("$sender", msg.Username);
            command.Parameters.AddWithValue("$content", msg.Content);
            command.Parameters.AddWithValue("$timestamp", string.IsNullOrEmpty(msg.Timestamp) ? DateTime.Now.ToString("o") : msg.Timestamp);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CACHE] Error saving message: {ex.Message}");
        }
    }

    public async Task SavePrivateMessageAsync(PrivateMessageData msg)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR IGNORE INTO Messages (Id, RoomId, Sender, Content, Timestamp, IsPrivate)
                VALUES ($id, $roomId, $sender, $content, $timestamp, 1)";
            command.Parameters.AddWithValue("$id", string.IsNullOrEmpty(msg.MessageId) ? Guid.NewGuid().ToString() : msg.MessageId);
            command.Parameters.AddWithValue("$roomId", msg.Recipient); // Using Recipient as RoomId for DMs
            command.Parameters.AddWithValue("$sender", msg.Sender);
            command.Parameters.AddWithValue("$content", msg.Content);
            command.Parameters.AddWithValue("$timestamp", string.IsNullOrEmpty(msg.Timestamp) ? DateTime.Now.ToString("o") : msg.Timestamp);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CACHE] Error saving private message: {ex.Message}");
        }
    }

    public async Task<List<ChatMessageData>> GetRoomMessagesAsync(string roomId, int limit = 50)
    {
        var messages = new List<ChatMessageData>();
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, RoomId, Sender, Content, Timestamp 
                FROM Messages 
                WHERE RoomId = $roomId AND IsPrivate = 0
                ORDER BY Timestamp DESC 
                LIMIT $limit";
            command.Parameters.AddWithValue("$roomId", roomId);
            command.Parameters.AddWithValue("$limit", limit);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new ChatMessageData
                {
                    MessageId = reader.GetString(0),
                    RoomId = reader.GetString(1),
                    Username = reader.GetString(2),
                    Content = reader.GetString(3),
                    Timestamp = reader.GetString(4)
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CACHE] Error getting messages: {ex.Message}");
        }
        
        messages.Reverse();
        return messages;
    }
}
