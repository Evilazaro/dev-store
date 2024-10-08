using DevStore.Orders.API.Application.DTO;
using DevStore.Orders.Domain.Vouchers;
using System.Threading.Tasks;

namespace DevStore.Orders.API.Application.Queries
{
    public interface IVoucherQueries
    {
        Task<VoucherDTO> GetVoucher(string voucher);
    }

    public class VoucherQueries : IVoucherQueries
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> GetVoucher(string voucher)
        {
            var voucherDb = await _voucherRepository.GetVoucherByCode(voucher);

            if (voucherDb == null) return null;

            if (!voucherDb.CanUse()) return null;

            return new VoucherDTO
            {
                Code = voucherDb.Code,
                DiscountType = (int)voucherDb.DiscountType,
                Percentage = voucherDb.Percentage,
                Discount = voucherDb.Discount
            };
        }
    }
}