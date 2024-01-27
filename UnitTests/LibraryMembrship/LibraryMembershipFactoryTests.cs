using FluentAssertions;
using LibraryMembership.Slimmed;

namespace UnitTests;

public class LibraryMembershipFactoryTests
{
    [Fact]
    public void ToAggregate_Creates_ActiveAggregateState()
    {
        // Arrange
        LibraryMembershipModel model = new()
        {
            BookLoans = [],
            BookReservations = [],
            Fines = [],
            MembershipExpiry = DateTimeOffset.UtcNow.AddYears(1)
        };
        
        // Act
        LibraryMembershipAggregate aggregate = model
            .ToAggregate(DateTimeOffset.UtcNow);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Active>();
    }
    
    [Fact]
    public void ToAggregate_Creates_ExpiredAggregateState()
    {
        // Arrange
        LibraryMembershipModel model = new()
        {
            BookLoans = [],
            BookReservations = [],
            Fines = [],
            MembershipExpiry = DateTimeOffset.UtcNow.AddDays(-1)
        };
        
        // Act
        LibraryMembershipAggregate aggregate = model
            .ToAggregate(DateTimeOffset.UtcNow);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Expired>();
    }
    
    [Fact]
    public void ToAggregate_Creates_SuspendedAggregateState_WhenFineIsUnpaid()
    {
        // Arrange
        LibraryMembershipModel model = new()
        {
            BookLoans = [],
            BookReservations = [],
            Fines = [
                new FineModel(Guid.NewGuid(), 10)
            ],
            MembershipExpiry = DateTimeOffset.UtcNow.AddYears(1)
        };
        
        // Act
        LibraryMembershipAggregate aggregate = model
            .ToAggregate(DateTimeOffset.UtcNow);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Suspended>();
    }
    
    [Fact]
    public void ToAggregate_Creates_SuspendedAggregateState_WhenBookLoanIsOverdue()
    {
        // Arrange
        LibraryMembershipModel model = new()
        {
            BookLoans = [
                new BookLoanModel(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(-1))
            ],
            BookReservations = [],
            Fines = [],
            MembershipExpiry = DateTimeOffset.UtcNow.AddYears(1),
            Status = MembershipStatus.Active
        };
        
        // Act
        LibraryMembershipAggregate aggregate = model
            .ToAggregate(DateTimeOffset.UtcNow);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Suspended>();
    }
    
}