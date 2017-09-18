namespace DutyFree.Mobile.Api
{
    using Entity.User;
    using Entity.ValueType;
    using Entity.ValueType.User;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Service.Model;
    using Service.Product;
    using Service.User;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;
    using WebCore.Extension;

    [Authorize]
    [Route("api/[controller]")]
    public class PreferenceController : BaseController
    {
        private PreferenceService _preferenceService;
        private ProductService _productService;

        public PreferenceController(PreferenceService preferenceService, ProductService productService)
        {
            this._preferenceService = preferenceService;
            this._productService = productService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Preference Get(string id)
        {
            return this._preferenceService.Get(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Preference preference)
        {
            this._preferenceService.Update(preference);
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

        [Route("GetCarts")]
        [HttpGet]
        public object GetCarts()
        {
            var userId = this.GetUserId();
            var preference = _preferenceService.Get(userId);

            if (preference == null || preference.Carts.IsEmpty())
            {
                return new List<string>();
            }

            var result = preference.Carts;

            var products = this._productService.GetByIds(new List<string>(result.Keys));

            return result.Select(o =>
            {
                var product = products.Find(p => p.Id == o.Key);
                return new
                {
                    ProductId = o.Key,
                    Quantity = o.Value.Quantity,
                    Logo = product.Logo,
                    Name = product.Name,
                    MarketPrice = product.MarketPrice,
                    Price = product.Price,
                    ReferencePrice = product.ReferencePrice,
                    MinPrice = product.MinPrice,
                    MaxPrice = product.MaxPrice
                };
            }).ToList();
        }

        [Route("UpdateCarts")]
        [HttpPost]
        public void UpdateCarts([FromBody]List<CartModel> model)
        {
            var userId = this.GetUserId();

            if (model.IsEmpty())
            {
                return;
            }

            var preference = _preferenceService.Get(userId);

            if (preference == null)
            {
                return;
            }

            preference.Carts = new Dictionary<string, CartItem>();
            model.ForEach(o => preference.Carts.Add(o.ProductId, new CartItem(o.Quantity, o.IsSelected)));

            _preferenceService.Update(preference);
        }

        [Route("AddCarts")]
        [HttpPost]
        public void AddCarts([FromBody]List<CartModel> model)
        {
            var userId = this.GetUserId();

            if (model.IsEmpty())
            {
                return;
            }

            var preference = _preferenceService.Get(userId);

            if (preference == null)
            {
                return;
            }

            model.ForEach(o =>
            {
                if (preference.Carts.ContainsKey(o.ProductId))
                {
                    preference.Carts[o.ProductId].Quantity += o.Quantity;
                    preference.Carts[o.ProductId].IsSelected = o.IsSelected;
                }
                else
                {
                    preference.Carts.Add(o.ProductId, new CartItem(o.Quantity, o.IsSelected));
                }
            });

            _preferenceService.Update(preference);
        }

        [Route("AddProduct")]
        [HttpPost]
        public void AddProduct(string productId)
        {
            var userId = this.GetUserId();
            this._preferenceService.AddProduct(userId, productId);
        }

        [Route("GetCartCount")]
        [HttpPost]
        public int GetCartCount()
        {
            var userId = this.GetUserId();
            return this._preferenceService.GetCartCount(userId);
        }

        [Route("getMessages")]
        [HttpGet]
        public Dictionary<long, Message> getMessages()
        {
            var userId = this.GetUserId();

            var preference = this._preferenceService.Get(userId);

            if(preference != null && !preference.Messages.IsEmpty())
            {
                return preference.Messages;
            }

            return new Dictionary<long, Message>();
        }

        [Route("GetWallet")]
        [HttpGet]
        public Wallet getWallet()
        {
            var userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                userId = this.GetUserId();
            }

            var preference = this._preferenceService.Get(userId);

            if (preference != null && preference.Wallet != null)
            {
                return preference.Wallet;
            }

            return new Wallet();
        }
    }
}
