using FluentAssertions;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace UnitTests.LibraryCart;

public sealed class BookLoanTests
{
    [Fact]
    public void CanReturnBook()
    {
        BookLoan bookLoan = new BookLoan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "1234567890",
            DateTimeOffset.Now.AddDays(14));
        
        bookLoan.Return();
        
        bookLoan.Returned.Should().BeTrue();
    }
    
    [Fact]
    public void CanProlongBook()
    {
        BookLoan bookLoan = new BookLoan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "1234567890",
            DateTimeOffset.Now.AddDays(14));
        
        bookLoan.Prolong();
        
        bookLoan.ProlongedTimes.Should().Be(1);
    }
    
    [Fact]
    public void CannotProlongBookIfAnyOtherIsHeldAfterTheReturnDate()
    {
        
    }
    
    [Fact]
    public void CannotProlongBookIfWasAlreadyProlonged()
    {
        BookLoan bookLoan = new BookLoan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "1234567890",
            DateTimeOffset.Now.AddDays(14));
        
        bookLoan.Prolong();
        
        Action act = () => bookLoan.Prolong();
        
        act.Should().Throw<DomainException>()
            .WithMessage("Book was already prolonged");
    }
}