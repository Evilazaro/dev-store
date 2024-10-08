using DevStore.Billing.API.Models;
using DevStore.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevStore.Billing.API.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BillingContext _context;

        public PaymentRepository(BillingContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public async Task<Payment> GetPaymentByOrderId(Guid orderId)
        {
            return await _context.Payments.AsNoTracking()
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId)
        {
            return await _context.Transactions.AsNoTracking()
                .Where(t => t.Payment.OrderId == orderId).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}