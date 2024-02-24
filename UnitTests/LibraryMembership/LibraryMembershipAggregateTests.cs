using FluentAssertions;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;

namespace UnitTests.LibraryMembership;

public class LibraryMembershipAggregateTests
{
    [Fact]
    public void ActiveUserCanLoanBook()
    {
        // Arrange
        LibraryMembershipAggregate.Active aggregate = CreateFakeActiveMembershipAggregate(
            numberOfLoans: 0);
        BookLoanModel bookLoanModel = CreateFakeBookLoan();
        
        // Act
        aggregate.LoanBook(bookLoanModel);
        
        // Assert
        aggregate.BookLoans.Should().Contain(bookLoanModel);
        aggregate.DomainEvents.Count.Should().Be(1);
        aggregate.DomainEvents.FirstOrDefault()
            .Should().BeOfType<LibraryMembershipEvent.BookLoaned>()
            .Which.BookId.Should().Be(bookLoanModel.BookId);
    }
    
    [Fact]
    public void ActiveUserCannotLoanBookWhenLimitReached()
    {
        // Arrange
        LibraryMembershipAggregate.Active aggregate = CreateFakeActiveMembershipAggregate(
            numberOfLoans: 5);
        BookLoanModel bookLoanModel = new(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
        
        // Act
        Action action = () => aggregate.LoanBook(bookLoanModel);
        
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
        BookLoanModel bookLoanModel = new(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
        
        // Act
        aggregate.LoanBook(bookLoanModel);
        Action action = () => aggregate.LoanBook(bookLoanModel);
        
        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot loan same book twice");
    }
    
    private LibraryMembershipAggregate.Active CreateFakeActiveMembershipAggregate(int numberOfLoans = 0)
    {
        List<BookLoanModel> bookLoans = Enumerable
            .Range(0, numberOfLoans)
            .Select(_ => CreateFakeBookLoan())
            .ToList();
        
        return new LibraryMembershipAggregate.Active(
            Guid.NewGuid(),
            bookLoans,
            new List<BookReservationEntity>(),
            new List<FineEntity>());
    }
    
    private BookLoanModel CreateFakeBookLoan()
    {
        return new BookLoanModel(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now.AddDays(30));
    }
}