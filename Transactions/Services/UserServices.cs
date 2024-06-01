using Microsoft.Data.SqlClient;
using Transactions.Models;

namespace Transactions.Services
{
    public class UserServices : IUserServices
    {
        private readonly IConfiguration config;
        private string connectionstring;
        public UserServices(IConfiguration config)
        {
            this.config = config;
            this.connectionstring = config["ConnectionStrings:DefaultConnection"];
        }

        public List<Users> GetAll()
        {
            using (SqlConnection con = new SqlConnection(this.connectionstring))
            {
                SqlCommand cmd = new SqlCommand("select * from userdetails", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Users> userlist = new List<Users>();
                while (reader.Read())
                {
                    Users user = new Users();
                    user.id = Convert.ToInt32(reader["id"]);
                    user.username = reader["username"].ToString();
                    user.account_balance = Convert.ToInt32(reader["account_balance"]);
                    userlist.Add(user);
                }
                return userlist;

            }
        }

        public bool TransferFund (TransferFundDto transferFundDto)
        {
            using (SqlConnection con = new SqlConnection (this.connectionstring))
            {
                con.Open();
                SqlTransaction transaction = null;
                try
                {
                    transaction = con.BeginTransaction();
                    SqlCommand deductAmt = new SqlCommand("update userdetails set account_balance = account_balance - @amount where id = @senderId", con, transaction);
                    deductAmt.Parameters.AddWithValue("@amount", transferFundDto.amount);
                    deductAmt.Parameters.AddWithValue("@senderId", transferFundDto.senderId);
                    int affectedrows = deductAmt.ExecuteNonQuery();
                    if(affectedrows == 0)
                    {
                        throw new Exception("sending failed");
                        return false;
                    }

                    SqlCommand addAmt = new SqlCommand("update userdetails set account_balance = account_balance + @amount where id = @recieverId", con, transaction);
                    addAmt.Parameters.AddWithValue("@amount", transferFundDto.amount);
                    addAmt.Parameters.AddWithValue("@recieverId", transferFundDto.receiverId);
                    int rowaffected = addAmt.ExecuteNonQuery();
                    if(rowaffected == 0)
                    {
                        throw new Exception("recieving failed");
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    Console.WriteLine(ex.Message);
                    return false;
                    
                }
            }
        }

    }
}
