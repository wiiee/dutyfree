namespace DutyFree.Entity.ValueType
{
    using System;

    public class Message
    {
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public string SenderId { get; set; }
        public DateTime Created { get; set; }

        public Message(string content, bool isRead, string senderId, DateTime created)
        {
            this.Content = content;
            this.IsRead = isRead;
            this.SenderId = senderId;
            this.Created = created;
        }
    }
}