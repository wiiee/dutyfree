namespace DutyFree.Service.User
{
    using Entity.User;
    using Entity.ValueType;
    using Platform.Context;
    using Platform.Enum;
    using Platform.Extension;
    using System.Collections.Generic;
    using System.Linq;

    public class PreferenceService : BaseService<Preference>
    {
        public PreferenceService(IContextRepository contextRepository) : base(contextRepository) { }

        override
        public void Update(Preference preference)
        {
            if(preference != null)
            {
                if (preference.ViewedProductIds.IsEmpty())
                {
                    preference.ViewedProductIds = new HashSet<string>();
                }

                foreach(var item in preference.Carts)
                {
                    preference.ViewedProductIds.Add(item.Key);
                }
            }

            base.Update(preference);
        }

        public void AddProduct(string userId, string productId)
        {
            var preference = this.Get(userId);
            
            if(preference == null)
            {
                return;
            }

            preference.AddProduct(productId);
            base.Update(preference);
        }

        public int GetCartCount(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return 0;
            }

            var result = 0;

            var preference = Get(userId);

            if (preference != null && !preference.Carts.IsEmpty())
            {
                result = preference.Carts.Sum(o => o.Value.Quantity);
            }

            return result;
        }

        public void AddReward(string userId, RewardType type, string orderInfoId, double number)
        {
            var preference = this.Get(userId);

            if(preference != null)
            {
                preference.AddReward(type, orderInfoId, number);
                base.Update(preference);
            }
        }

        public void AddMessage(string userId, Message message)
        {
            var preference = this.Get(userId);

            if (preference != null)
            {
                preference.AddMessage(message);
                base.Update(preference);
            }
        }
    }
}
