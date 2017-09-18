namespace DutyFree.Service.Product
{
    using Entity.Product;
    using Platform.Context;

    public class ProductService : BaseService<Product>
    {
        public ProductService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}