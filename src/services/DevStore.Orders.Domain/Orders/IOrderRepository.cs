using DevStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace DevStore.Orders.Domain.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetById(Guid id);
        Task<IEnumerable<Order>> GetCustomersById(Guid customerId);
        void Add(Order order);
        void Update(Order order);
        DbConnection GetConnection();
        Task<Order> GetLastOrder(Guid customerId);
        Task<Order> GetLastAuthorizedOrder();
        /* Order Item */
        Task<OrderItem> GetItemById(Guid id);
        Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId);
    }
}