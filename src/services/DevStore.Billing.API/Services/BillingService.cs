using DevStore.Billing.API.Facade;
using DevStore.Billing.API.Models;
using DevStore.Core.DomainObjects;
using DevStore.Core.Messages.Integration;
using FluentValidation.Results;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevStore.Billing.API.Services
{
    public class BillingService : IBillingService
    {
        private readonly IPaymentFacade _paymentFacade;
        private readonly IPaymentRepository _paymentRepository;

        public BillingService(IPaymentFacade paymentFacade,
                              IPaymentRepository paymentRepository)
        {
            _paymentFacade = paymentFacade;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseMessage> AuthorizeTransaction(Payment payment)
        {
            var transaction = await _paymentFacade.AuthorizePayment(payment);
            var validationResult = new ValidationResult();

            if (transaction.TransactionStatus != TransactionStatus.Authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                        "Payment refused, please contact your card operator"));

                return new ResponseMessage(validationResult);
            }

            payment.AdicionarTransacao(transaction);
            _paymentRepository.AddPayment(payment);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    "There was an error while making the payment."));

                // Canceling the payment on the service
                await CancelTransaction(payment.OrderId);

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> GetTransaction(Guid orderId)
        {
            var transactions = await _paymentRepository.GetTransactionsByOrderId(orderId);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction == null) throw new DomainException($"Transaction not found for order {orderId}");

            var transaction = await _paymentFacade.CapturePayment(authorizedTransaction);

            if (transaction.TransactionStatus != TransactionStatus.Paid)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"Unable to capture order payment {orderId}"));

                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = authorizedTransaction.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"It was not possible to persist the capture of the payment of the order {orderId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CancelTransaction(Guid orderId)
        {
            var transactions = await _paymentRepository.GetTransactionsByOrderId(orderId);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.TransactionStatus == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction == null) throw new DomainException($"Transaction not found for order {orderId}");

            var transaction = await _paymentFacade.CancelAuthorization(authorizedTransaction);

            if (transaction.TransactionStatus != TransactionStatus.Canceled)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"Unable to cancel order payment {orderId}"));

                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = authorizedTransaction.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"It was not possible to persist the cancellation of the order payment {orderId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}