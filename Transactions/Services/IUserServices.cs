using Transactions.Models;

namespace Transactions.Services
{
    public interface IUserServices
    {
        public List<Users> GetAll();
        public bool TransferFund(TransferFundDto transferFundDto);
    }
}
