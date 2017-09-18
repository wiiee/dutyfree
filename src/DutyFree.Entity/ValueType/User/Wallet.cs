namespace DutyFree.Entity.ValueType.User
{
    using Platform.Extension;
    using System.Collections.Generic;
    using System.Linq;

    public class Wallet
    {
        //钱包余额
        public double GetTotal()
        {
            double inNumber = 0;
            double outNumber = 0;

            if (!InItems.IsEmpty())
            {
                inNumber = InItems.Sum(o => o.Number);
            }

            if (!OutItems.IsEmpty())
            {
                outNumber = OutItems.Sum(o => o.Number);
            }

            return inNumber - outNumber;
        }

        //获取记录
        public List<WalletItem> InItems { get; set; }

        //使用记录
        public List<WalletItem> OutItems { get; set; }

        public Wallet()
        {
            this.InItems = new List<WalletItem>();
            this.OutItems = new List<WalletItem>();
        }
    }
}
