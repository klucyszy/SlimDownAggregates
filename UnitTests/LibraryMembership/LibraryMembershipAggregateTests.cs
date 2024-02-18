using FluentAssertions;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

namespace UnitTests.LibraryMembership;

public class LibraryMembershipAggregateTests
{
    [Fact]
    public void ActiveUserCanLoanBook()
    {
        // Arrange
        LibraryMembershipAggregate.Active aggregate = CreateFakeActiveMembershipAggregate(
            numberOfLoans: 0);
        BookLoan bookLoan = CreateFakeBookLoan();
        
        // Act
        aggregate.LoanBook(bookLoan);
        
        // Assert
        aggregate.BookLoans.Should().Contain(bookLoan);
        aggregate.DomainEvents.Count.Should().Be(1);
        aggregate.DomainEvents.FirstOrDefault()
            .Should().BeOfType<LibraryMembershipEvent.BookLoaned>()
            .Which.BookId.Should().Be(bookLoan.BookId);
    }
    
    [Fact]
    public void ActiveUserCannotLoanBookWhenLimitReached()
    {
        // Arrange
        LibraryMembershipAggregate.Active aggregate = CreateFakeActiveMembershipAggregate(
            numberOfLoans: 5);
        BookLoan bookLoan = new(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
        
        // Act
        Action action = () => aggregate.LoanBook(bookLoan);
        
        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Loan limit exceeded");
    }
    
    [Fact]
    public void ActiveUserCannotLoanBookSameBookTwice()
    {
        // Arrange
        LibraryMembershipAggregate.Active aggregate = CreateFakeActiveMembershipAggregate(
            numberOfLoans: 0);
        BookLoan bookLoan = new(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
        
        // Act
        aggregate.LoanBook(bookLoan);
        Action action = () => aggregate.LoanBook(bookLoan);
        
        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot loan same book twice");
    }
    
    private LibraryMembershipAggregate.Active CreateFakeActiveMembershipAggregate(int numberOfLoans = 0)
    {
        List<BookLoan> bookLoans = Enumerable
            .Range(0, numberOfLoans)
            .Select(_ => CreateFakeBookLoan())
            .ToList();
        
        return new LibraryMembershipAggregate.Active(
            Guid.NewGuid(),
            bookLoans,
            new List<BookReservationEntity>(),
            new List<FineEntity>());
    }
    
    private BookLoan CreateFakeBookLoan()
    {
        return new BookLoan(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now.AddDays(30));
    }
}