namespace DutyFree.Entity.User
{
    using Platform.Enum;
    using Platform.Extension;
    using System.Collections.Generic;
    using ValueType;

    public class User : BaseEntity
    {
        //Id为用户手机号码

        public string NickName { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public List<string> Pics { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        
        //用户端推荐人号码
        public string RecommendUserId { get; set; }

        //供给端推荐人号码
        public string RecommendPartnerId { get; set; }

        //用户地址，第一个为缺省地址
        public List<Address> Addresses { get; set; }

        //用户类型,User和Provider可以自由切换
        public UserType UserType { get; set; }

        public User(string id, string password, string name, string nickName, string mobileNo, Gender gender, List<string> pics, UserType userType)
        {
            this.Id = id;
            this.Password = password;
            this.Name = name;
            this.NickName = nickName;
            this.MobileNo = mobileNo;
            this.Gender = gender;
            this.Pics = pics;
            this.UserType = userType;
        }

        public User()
        {

        }

        public void SetDefaultAddress(int index)
        {
            if(!Addresses.IsEmpty() && index < Addresses.Count && index >= 0)
            {
                Addresses.ForEach(o => o.IsDefault = Addresses.IndexOf(o) == index);
            }
        }

        public void SaveAddress(int index, Address address, bool isDefault = true)
        {
            if (!Addresses.IsEmpty() && index < Addresses.Count && index >= 0)
            {
                Addresses[index] = address;

                if (isDefault)
                {
                    SetDefaultAddress(index);
                }
            }
        }

        public void AddAddress(Address address, bool isDefault = true)
        {
            if(Addresses == null)
            {
                Addresses = new List<Address>();
            }

            Addresses.Add(address);

            if (isDefault)
            {
                SetDefaultAddress(Addresses.Count - 1);
            }
            else
            {
                address.IsDefault = false;
            }
        }

        public void RemoveAddress(int index)
        {
            if (!Addresses.IsEmpty() && index < Addresses.Count && index >= 0)
            {
                if(Addresses[index].IsDefault && Addresses.Count > 1)
                {
                    Addresses[0].IsDefault = true;
                }

                Addresses.RemoveAt(index);
            }
        }
    }
}
