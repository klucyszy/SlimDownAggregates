using System.Threading.Tasks;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using Microsoft.Extensions.Logging;

namespace LibraryMembership.Slimmed.Presentation.Events.BookLoaned;

public sealed class BookLoanHandler
{
    private readonly ILogger<BookLoanHandler> _logger;

    public BookLoanHandler(ILogger<BookLoanHandler> logger)
    {
        _logger = logger;
    }
    
    public void Handle(LibraryMembershipEvent.BookLoaned message)
    {
        _logger.LogInformation($"Book {message.BookId} loaned by {message.MembershipId} on {message.LoanDate}" );
    }
}