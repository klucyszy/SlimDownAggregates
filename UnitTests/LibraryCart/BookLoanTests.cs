using FluentAssertions;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace UnitTests.LibraryCart;

public sealed class BookLoanTests
{
    [Fact]
    public void CanReturnBook()
    {
        BookLoanAggregate bookLoan = new BookLoanAggregate(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.Now.AddDays(14));
        
        bookLoan.Return();
        
        bookLoan.Returned.Should().BeTrue();
    }
}