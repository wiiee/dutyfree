namespace DutyFree.Service.Result
{
    public class ServiceResult
    {
        public bool IsSuccessful { get; set; }
        public string Msg { get; set; }

        public ServiceResult(bool isSuccessful, string msg)
        {
            this.IsSuccessful = isSuccessful;
            this.Msg = msg;
        }

        public ServiceResult()
        {

        }
    }
}
