namespace FitnessApp.Models;

using System;
using System.Collections.Generic;
using FitnessApp.Enums; 

public class Message
{
    private int id;
    private string content;
    private DateTime timestamp;
    private int senderId;
    private int tutelageId;

    public int Id { get => id; set => id = value; }
    public string Content { get => content; set => content = value; }
    public DateTime Timestamp { get => timestamp; set => timestamp = value; }
    public int SenderId { get => senderId; set => senderId = value; }
    public int TutelageId { get => tutelageId; set => tutelageId = value; }

    public Message(string content, int senderId, int tutelageId)
    {
        this.content = content;
        this.senderId = senderId;
        this.tutelageId = tutelageId;
        this.timestamp = DateTime.Now;
    }
}