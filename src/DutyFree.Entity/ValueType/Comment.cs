namespace DutyFree.Entity.ValueType
{
    using Platform.Enum;
    using System;

    public class Comment
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public Star Star { get; set; }
        public DateTime Created { get; set; }
    }
}
