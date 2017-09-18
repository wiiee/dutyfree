namespace DutyFree.Platform.Exception
{
    using System;

    public class DutyFreeException : Exception
    {
        public string AppId { get; set; }

        public DutyFreeException(string appId, string message) : base(message)
        {
            this.AppId = AppId;
        }

        public DutyFreeException(string appId, string message, Exception innerException) : base(message, innerException)
        {
            this.AppId = AppId;
        }
    }
}
