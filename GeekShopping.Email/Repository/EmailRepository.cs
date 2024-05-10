using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {

        private readonly DbContextOptions<SqlServerContext> _context;

        public EmailRepository(DbContextOptions<SqlServerContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePayamentResultMessage message)
        {
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderID} has been created successfuly"
            };
            
            await using var _db = new SqlServerContext(_context);

            _db.EmailLogs.Add(email);

            await _db.SaveChangesAsync();
        }
    }
}
