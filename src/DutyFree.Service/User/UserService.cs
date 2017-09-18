namespace DutyFree.Service.User
{
    using Context;
    using Entity.User;
    using Entity.ValueType;
    using Platform.Context;
    using Platform.Enum;
    using Platform.Extension;
    using Result;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : BaseService<User>
    {
        private const string INVALID_USERNAME = "User name is invalid, please check it.";
        private const string INVALID_PWD = "Password is invalid, please check it.";
        private const string USER_ALREADY_EXIST = "User is already exist.";

        public UserService(IContextRepository contextRepository) : base(contextRepository) { }

        public override string Create(User entity)
        {
            var preference = new Preference(entity.Id);
            ServiceFactory.Instance.GetService<PreferenceService>().Create(preference);
            return base.Create(entity);
        }

        public override List<string> Create(List<User> entities)
        {
            var result = new List<string>();

            foreach(var entity in entities)
            {
                result.Add(Create(entity));
            }

            return result;
        }

        public ServiceResult LogIn(string id, string pwd)
        {
            ServiceResult result = new ServiceResult();

            User user = Get(id);

            if (user == null)
            {
                result.IsSuccessful = false;
                result.Msg = INVALID_USERNAME;
            }
            else
            {
                if (user.Password.Equals(pwd))
                {
                    result.IsSuccessful = true;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Msg = INVALID_PWD;
                }
            }

            return result;
        }

        public ServiceResult LogInAdmin(string id, string pwd)
        {
            ServiceResult result = new ServiceResult();

            User user = Get(id);

            if (user == null || user.UserType == UserType.Partner || user.UserType == UserType.User)
            {
                result.IsSuccessful = false;
                result.Msg = INVALID_USERNAME;
            }
            else
            {
                if (user.Password.Equals(pwd))
                {
                    result.IsSuccessful = true;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Msg = INVALID_PWD;
                }
            }

            return result;
        }

        public ServiceResult SignUp(User user)
        {
            if(user == null || string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.Password))
            {
                return new ServiceResult(false, "用户名或者密码不能为空");
            }

            ServiceResult result = new ServiceResult();

            User dbUser = Get(user.Id);

            if (dbUser == null)
            {
                Create(user);
                result.IsSuccessful = true;

                //奖励推荐人
                //if (!string.IsNullOrEmpty(user.RecommendUserId))
                //{
                //    var recommendUser = Get(user.RecommendUserId);
                    
                //    if(recommendUser != null)
                //    {
                //        var preferenceService = ServiceFactory.Instance.GetService<PreferenceService>(ContextUtil.SERVICE_CONTEXT);
                //        preferenceService.AddReward(user.RecommendUserId, RewardType.NewUser, 5);
                //    }
                //}
            }
            else
            {
                result.IsSuccessful = false;
                result.Msg = USER_ALREADY_EXIST;
            }

            return result;
        }

        public bool IsUserExist(string id)
        {
            return Get(id) != null;
        }

        public void SwitchUserType(string userId, UserType userType)
        {
            var user = this.Get(userId);

            if(user != null && user.UserType != userType)
            {
                Update(userId, "UserType", userType);
            }
        }

        public void AddAddress(string userId, bool isDefault, Address address)
        {
            var user = Get(userId);

            if (user != null)
            {
                user.AddAddress(address, isDefault);
                base.Update(user);
            }
        }

        public void SaveAddress(string userId, int index, Address address)
        {
            var user = Get(userId);

            if(user != null && address != null)
            {
                user.SaveAddress(index, address, false);
                base.Update(user);
            }
        }

        public List<Address> GetAddresses(string userId)
        {
            var user = Get(userId);

            if (user != null && !user.Addresses.IsEmpty())
            {
                return user.Addresses;
            }

            return new List<Address>();
        }

        public Address GetDefaultAddress(string userId)
        {
            var user = Get(userId);

            if (user != null && !user.Addresses.IsEmpty())
            {
                return user.Addresses.First(o => o.IsDefault);
            }

            return new Address();
        }

        public void SetDefaultAddress(string userId, int index)
        {
            var user = Get(userId);

            if(user != null)
            {
                user.SetDefaultAddress(index);
                base.Update(user);
            }
        }

        public void RemoveAddress(string userId, int index)
        {
            var user = Get(userId);

            if(user != null)
            {
                user.RemoveAddress(index);
                base.Update(user);
            }
        }
    }
}
