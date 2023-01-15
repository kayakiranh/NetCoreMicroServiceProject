using MP.Core.Application.DataTransferObjects;

namespace MP.Core.Application.Repositories
{
    public interface IPostgreSqlRepository
    {
        int Insert(CreditCardApplicationDto model);
    }
}