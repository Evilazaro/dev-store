using DevStore.Core.Data;
using DevStore.Orders.Domain.Orders;
using DevStore.Orders.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DevStore.Orders.Infra.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;

        public OrderRepository(OrdersContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public async Task<Order> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetCustomersById(Guid customerId)
        {
            return await _context.Orders
                .Include(p => p.OrderItems)
                .AsNoTracking()
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }


        public async Task<OrderItem> GetItemById(Guid id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.OrderId == orderId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<Order> GetLastOrder(Guid customerId)
        {
            var fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

            return _context.Orders
                .Include(i => i.OrderItems)
                .Where(o => o.CustomerId == customerId && o.DateAdded > fiveMinutesAgo && o.DateAdded <= DateTime.Now)
                .OrderByDescending(o => o.DateAdded).FirstOrDefaultAsync();
        }

        public Task<Order> GetLastAuthorizedOrder()
        {
            return _context.Orders.Include(i => i.OrderItems)
                .Where(o => o.OrderStatus == OrderStatus.Authorized)
                .OrderBy(o => o.DateAdded).FirstOrDefaultAsync();


        }
    }
}