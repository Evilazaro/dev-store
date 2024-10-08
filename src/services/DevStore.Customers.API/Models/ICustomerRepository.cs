using DevStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevStore.Customers.API.Models
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Add(Customer customer);

        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetBySocialNumber(string ssn);

        void AddAddress(Address address);
        Task<Address> GetAddressById(Guid id);
    }
}