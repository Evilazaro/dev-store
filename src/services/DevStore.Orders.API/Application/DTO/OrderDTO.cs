using DevStore.Orders.Domain.Orders;
using System;
using System.Collections.Generic;

namespace DevStore.Orders.API.Application.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }

        public Guid CustomerId { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public decimal Discount { get; set; }
        public string Voucher { get; set; }
        public bool HasVoucher { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; }
        public AddressDto Address { get; set; }

        public static OrderDTO ToOrderDTO(Order order)
        {
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                Status = (int)order.OrderStatus,
                Date = order.DateAdded,
                Amount = order.Amount,
                Discount = order.Discount,
                HasVoucher = order.HasVoucher,
                OrderItems = new List<OrderItemDTO>(),
                Address = new AddressDto()
            };

            foreach (var item in order.OrderItems)
            {
                orderDTO.OrderItems.Add(new OrderItemDTO
                {
                    Name = item.ProductName,
                    Image = item.ProductImage,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    OrderId = item.OrderId
                });
            }

            orderDTO.Address = new AddressDto
            {
                StreetAddress = order.Address.StreetAddress,
                BuildingNumber = order.Address.BuildingNumber,
                SecondaryAddress = order.Address.SecondaryAddress,
                Neighborhood = order.Address.Neighborhood,
                ZipCode = order.Address.ZipCode,
                City = order.Address.City,
                State = order.Address.State,
            };

            return orderDTO;
        }
    }
}