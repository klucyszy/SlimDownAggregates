using FluentAssertions;
using LibraryMembership.Slimmed;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;

namespace UnitTests.LibraryMembrship;

public class LibraryMembershipMapperTests
{
    [Fact]
    public void ToAggregate_Creates_ActiveAggregateState()
    {
        // Arrange
        LibraryMembershipEntity entity = new()
        {
            BookLoans = [],
            BookReservations = [],
            Fines = [],
            MembershipExpiry = DateTimeOffset.UtcNow.AddYears(1)
        };
        
        // Act
        LibraryMembershipAggregate aggregate = entity
            .ToAggregate(DateTimeOffset.UtcNow, null);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Active>();
    }
    
    [Fact]
    public void ToAggregate_Creates_ExpiredAggregateState()
    {
        // Arrange
        LibraryMembershipEntity entity = new()
        {
            BookLoans = [],
            BookReservations = [],
            Fines = [],
            MembershipExpiry = DateTimeOffset.UtcNow.AddDays(-1)
        };
        
        // Act
        LibraryMembershipAggregate aggregate = entity
            .ToAggregate(DateTimeOffset.UtcNow, null);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Expired>();
    }
    
    [Fact]
    public void ToAggregate_Creates_SuspendedAggregateState_WhenFineIsUnpaid()
    {
        // Arrange
        LibraryMembershipEntity entity = new()
        {
            BookLoans = [],
            BookReservations = [],
            Fines = [
                new FineEntity(Guid.NewGuid(), 10)
            ],
            MembershipExpiry = DateTimeOffset.UtcNow.AddYears(1)
        };
        
        // Act
        LibraryMembershipAggregate aggregate = entity
            .ToAggregate(DateTimeOffset.UtcNow, null);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Suspended>();
    }
    
    [Fact]
    public void ToAggregate_Creates_SuspendedAggregateState_WhenBookLoanIsOverdue()
    {
        // Arrange
        LibraryMembershipEntity entity = new()
        {
            BookLoans = [
                new BookLoanEntity(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(-1))
            ],
            BookReservations = [],
            Fines = [],
            MembershipExpiry = DateTimeOffset.UtcNow.AddYears(1),
            Status = LibraryMembershipEntity.MembershipStatus.Active
        };
        
        // Act
        LibraryMembershipAggregate aggregate = entity
            .ToAggregate(DateTimeOffset.UtcNow, null);
        
        // Assert
        aggregate.Should().BeOfType<LibraryMembershipAggregate.Suspended>();
    }
    
}