using DevStore.Core.Messages;
using DevStore.Customers.API.Application.Events;
using DevStore.Customers.API.Models;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DevStore.Customers.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler,
        IRequestHandler<NewCustomerCommand, ValidationResult>,
        IRequestHandler<AddAddressCommand, ValidationResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ValidationResult> Handle(NewCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var customer = new Customer(message.Id, message.Name, message.Email, message.SocialNumber);

            var customerExist = await _customerRepository.GetBySocialNumber(customer.SocialNumber);

            if (customerExist != null)
            {
                AddError("Already has this social number.");
                return ValidationResult;
            }

            _customerRepository.Add(customer);

            customer.AddEvent(new NewCustomerAddedEvent(message.Id, message.Name, message.Email, message.SocialNumber));

            return await PersistData(_customerRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AddAddressCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var endereco = new Address(message.StreetAddress, message.BuildingNumber, message.SecondaryAddress, message.Neighborhood, message.ZipCode, message.City, message.State, message.CustomerId);
            _customerRepository.AddAddress(endereco);

            return await PersistData(_customerRepository.UnitOfWork);
        }
    }
}