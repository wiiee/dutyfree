namespace DutyFree.Entity.ValueType
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public bool IsSelected { get; set; }

        public CartItem(int quantity, bool isSelected)
        {
            this.Quantity = quantity;
            this.IsSelected = isSelected;
        }
    }
}
