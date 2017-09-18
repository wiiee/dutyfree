namespace DutyFree.Entity.User
{
    using Platform.Enum;
    using Platform.Extension;
    using System.Collections.Generic;
    using System.Linq;
    using ValueType;
    using ValueType.User;

    //用户喜好
    public class Preference : BaseEntity
    {
        //Id为UserId

        //访问的物品
        public HashSet<string> ViewedProductIds { get; set; }

        //购物车产品和数量
        public Dictionary<string, CartItem> Carts { get; set; }

        public Dictionary<long, Message> Messages { get; set; }

        //用户钱包
        public Wallet Wallet { get; set; }

        public Preference(string id, HashSet<string> viewedProductIds, Dictionary<string, CartItem> carts)
        {
            this.Id = id;
            this.ViewedProductIds = viewedProductIds;
            this.Carts = carts;
        }

        public Preference(string id)
        {
            this.Id = id;
            this.ViewedProductIds = new HashSet<string>();
            this.Carts = new Dictionary<string, CartItem>();
        }

        public Preference()
        {

        }

        public void AddProduct(string productId)
        {
            if(Carts == null)
            {
                Carts = new Dictionary<string, CartItem>();
                Carts.Add(productId, new CartItem(1, true));
            }
            else
            {
                if (Carts.ContainsKey(productId))
                {
                    Carts[productId].Quantity++;
                }
                else
                {
                    Carts.Add(productId, new CartItem(1, true));
                }
            }
        }

        public void UnselectProductIds(List<string> productIds)
        {
            if (!Carts.IsEmpty())
            {
                productIds.ForEach(o => {
                    if (Carts.ContainsKey(o)) {
                        Carts[o].IsSelected = false;
                    }
                });
            }
        }

        public int GetPendingOrderCount()
        {
            if (!Carts.IsEmpty())
            {
                return Carts.Where(o => o.Value.IsSelected).Sum(o => o.Value.Quantity);
            }

            return 0;
        }

        public bool IsSelectAll()
        {
            if (!Carts.IsEmpty())
            {
                return Carts.Where(o => !o.Value.IsSelected).Count() == 0;
            }

            return false;
        }

        public List<string> GetPendingOrderProductIds()
        {
            var result = new List<string>();

            if (!Carts.IsEmpty())
            {
                return Carts.Where(o => o.Value.IsSelected).Select(o => o.Key).ToList();
            }

            return result;
        }

        public void AddReward(RewardType type, string orderInfoId, double number)
        {
            if(this.Wallet == null)
            {
                Wallet = new Wallet();
            }

            Wallet.InItems.Add(new WalletItem(type, orderInfoId, number));
        }

        public void UseReward(string orderId, double number)
        {
            if (this.Wallet == null)
            {
                Wallet = new Wallet();
            }

            Wallet.OutItems.Add(new WalletItem(orderId, number));
        }

        public void AddMessage(Message msg)
        {
            if (this.Messages.IsEmpty())
            {
                this.Messages = new Dictionary<long, Message>();
            }

            this.Messages.Add(msg.Created.Ticks, msg);
        }
    }
}
