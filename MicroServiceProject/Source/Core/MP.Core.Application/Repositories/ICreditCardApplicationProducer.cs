using MP.Core.Application.DataTransferObjects;

namespace MP.Core.Application.Repositories
{
    public interface ICreditCardApplicationProducer
    {
        void SendApplication(CreditCardApplicationDto creditCardApplication);
    }
}