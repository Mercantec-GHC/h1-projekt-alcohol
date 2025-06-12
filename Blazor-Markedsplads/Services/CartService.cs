using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor_Markedsplads.Models;

namespace Blazor_Markedsplads.Services
{
    public class CartService
    {
        private readonly List<CartItem> _items = new();
        public event Func<Task>? OnChange;

        public IReadOnlyList<CartItem> GetItems() => _items;
        public decimal GrandTotal => _items.Sum(x => x.TotalPrice);

        public Task AddToCartAsync(ProductModel product, int quantity)
        {
            var existing = _items.FirstOrDefault(x => x.ID == product.ID);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _items.Add(new CartItem
                {
                    ID = product.ID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    
                });

            OnChange?.Invoke();
            return Task.CompletedTask;
        }

        public Task RemoveFromCartAsync(int productId)
        {
            var existing = _items.FirstOrDefault(x => x.ID == productId);
            if (existing != null)
            {
                _items.Remove(existing);
                OnChange?.Invoke();
            }
            return Task.CompletedTask;
        }

        public Task ClearCartAsync()
        {
            _items.Clear();
            OnChange?.Invoke();
            return Task.CompletedTask;
        }
    }
}