using FluentAssertions;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace UnitTests.LibraryCart;

public sealed class LibraryCartAggregateTests
{
    [Fact]
    public void LibraryMemberCanLoanBook()
    {
        LibraryMembership.Slimmed.Domain.LibraryCart.LibraryCartAggregate cartAggregate = new (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<BookLoan>());
        
        BookLoan loan = cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-0");
        
        loan.Should().NotBeNull();
        cartAggregate.ActiveBookLoans.Count.Should().Be(1);
    }
    
    [Fact]
    public void LibraryMemberCannotLoanMoreThanFiveBooks()
    {
        LibraryMembership.Slimmed.Domain.LibraryCart.LibraryCartAggregate cartAggregate = new (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<BookLoan>());
        
        cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-0");
        cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-1");
        cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-2");
        cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-3");
        cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-4");
        
        Action act = () => cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-5");

        act.Should().Throw<DomainException>()
            .WithMessage("Loan limit exceeded");
    }
    
    [Fact]
    public void LibraryMemberCannotLoanSameBookAtTheSameTime()
    {
        LibraryMembership.Slimmed.Domain.LibraryCart.LibraryCartAggregate cartAggregate = new (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<BookLoan>());
        
        cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-0");
        
        Action act = () => cartAggregate.Loan(Guid.NewGuid(), "978-3-16-148410-0");

        act.Should().Throw<DomainException>()
            .WithMessage("Cannot loan same book twice");
    }
}