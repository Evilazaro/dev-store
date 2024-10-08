using DevStore.Core.Data;
using DevStore.Orders.Domain.Vouchers;
using DevStore.Orders.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevStore.Orders.Infra.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly OrdersContext _context;

        public VoucherRepository(OrdersContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Voucher> GetVoucherByCode(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Code == code);
        }

        public void Update(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}