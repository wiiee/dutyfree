namespace DutyFree.Mobile.Api
{
    using Entity.User;
    using Entity.ValueType;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Constant;
    using Platform.Extension;
    using Platform.Util;
    using Service.Result;
    using Service.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;
    using WebCore.Extension;

    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private DownloadRecordService _downloadRecordService;
        private UserService _userService;
        private PreferenceService _preferenceService;

        public UserController(DownloadRecordService downloadRecordService, UserService userService, PreferenceService preferenceService)
        {
            this._downloadRecordService = downloadRecordService;
            this._userService = userService;
            this._preferenceService = preferenceService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("LogIn")]
        [HttpPost]
        public ServiceResult LogIn([FromBody]User user)
        {
            var result = _userService.LogIn(user.Id, user.Password);
            if (result.IsSuccessful)
            {
                this.LogInCookie(user);
            }

            return result;
        }

        [Route("SignUp")]
        [HttpPost]
        public ServiceResult SignUp([FromBody]User user, string deviceId)
        {
            var result = _userService.SignUp(user);
            if (result.IsSuccessful)
            {
                this.LogInCookie(user);
                if (!string.IsNullOrEmpty(deviceId))
                {
                    var record = this._downloadRecordService.Get(o => o.DeviceIds.Contains(deviceId)).FirstOrDefault();

                    if (record != null)
                    {
                        var preference = _preferenceService.Get(record.Id);
                        preference.AddMessage(new Message(user.Id + "刚刚注册成功", false, InternalInfo.SYSTEM_ID, DateTime.Now));
                    }
                }
            }

            return result;
        }

        [Authorize]
        [Route("SetDefaultAddress")]
        [HttpGet]
        public void SetDefaultAddress(int index)
        {
            var userId = this.GetUserId();
            this._userService.SetDefaultAddress(userId, index);
        }

        [Authorize]
        [Route("GetDefaultAddress")]
        [HttpGet]
        public Address GetDefaultAddress()
        {
            var userId = this.GetUserId();
            return this._userService.GetDefaultAddress(userId);
        }

        [Authorize]
        [Route("GetAddresses")]
        [HttpGet]
        public List<Address> GetAddresses()
        {
            var userId = this.GetUserId();
            return this._userService.GetAddresses(userId);
        }

        [Authorize]
        [Route("SaveAddress")]
        [HttpPost]
        public void SaveAddress(int index, [FromBody]Address address)
        {
            var userId = this.GetUserId();
            this._userService.SaveAddress(userId, index, address);
        }

        [Authorize]
        [Route("AddAddress")]
        [HttpPost]
        public void AddAddress(bool isDefault, [FromBody]Address address)
        {
            var userId = this.GetUserId();
            this._userService.AddAddress(userId, isDefault, address);
        }

        [Authorize]
        [Route("RemoveAddress")]
        [HttpGet]
        public void RemoveAddress(int index)
        {
            var userId = this.GetUserId();
            this._userService.RemoveAddress(userId, index);
        }

        [Route("InitDevice")]
        [HttpGet]
        public void InitDevice(string deviceId)
        {
            if (!string.IsNullOrEmpty(deviceId))
            {
                //该设备已经扫描过，不再记录
                if (!_downloadRecordService.Get(o => o.DeviceIds.Contains(deviceId)).IsEmpty())
                {
                    return;
                }

                var ip = HttpContext.GetRemoteIp();
                var records = _downloadRecordService.Get(o => o.Ips.ContainsKey(ip));

                if (!records.IsEmpty())
                {
                    var now = DateTime.Now;
                    var record = records.OrderBy(o => now - o.Ips[ip]).First();
                    record.AddDeviceId(deviceId);
                    _downloadRecordService.Update(record);
                }
            }
        }

        [Authorize]
        [Route("GetQr")]
        [HttpGet]
        public FileContentResult GetQr()
        {
            var userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            byte[] bytes = QrCodeUtil.CreateQr("http://www.yimeiyiyou.com/home/download?userId=" + userId);

            //transform the picture's data from string to an array of bytes

            //return array of bytes as the image's data to action's response. We set the image's content mime type to image/jpeg
            return new FileContentResult(bytes, "image/jpeg");
        }
    }
}
