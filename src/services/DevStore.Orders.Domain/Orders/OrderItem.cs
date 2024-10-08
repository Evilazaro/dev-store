using DevStore.Core.DomainObjects;
using System;

namespace DevStore.Orders.Domain.Orders
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string ProductImage { get; set; }

        // EF Rel.
        public Order Order { get; set; }

        public OrderItem(Guid productId, string productName, int quantity,
            decimal price, string productImage = null)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            ProductImage = productImage;
        }

        // EF ctor
        protected OrderItem() { }

        internal decimal CalculateAmount()
        {
            return Quantity * Price;
        }
    }
}