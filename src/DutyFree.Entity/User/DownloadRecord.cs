namespace DutyFree.Entity.User
{
    using System;
    using System.Collections.Generic;

    public class DownloadRecord : BaseEntity
    {
        //Id为推荐用户Id

        //Ip地址和时间
        public Dictionary<string, DateTime> Ips { get; set; }

        //设备Id
        public HashSet<string> DeviceIds { get; set; }

        public DownloadRecord(string userId, string ip)
        {
            this.Id = userId;
            this.Ips = new Dictionary<string, DateTime>();
            this.Ips.Add(ip, DateTime.Now);
            this.DeviceIds = new HashSet<string>();
        }

        public void AddRecord(string ip)
        {
            if (this.Ips.ContainsKey(ip))
            {
                this.Ips[ip] = DateTime.Now;
            }
            else
            {
                this.Ips.Add(ip, DateTime.Now);
            }
        }

        public void AddDeviceId(string deviceId)
        {
            this.DeviceIds.Add(deviceId);
        }
    }
}
