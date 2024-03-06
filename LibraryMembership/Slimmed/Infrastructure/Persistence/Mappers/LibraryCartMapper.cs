using System.Linq;
using LibraryMembership.Shared.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;

public static class LibraryCartMapper
{
    public static LibraryCartAggregate ToAggregate(this LibraryCart entity)
    {
        return new LibraryCartAggregate(
            entity.Id,
            entity.MembershipId,
            entity.BookLoans
                .Select(bl => new LibraryCartAggregate.ActiveBookLoan(
                    bl.BookId,
                    bl.BookIsbn,
                    bl.ReturnDate,
                    bl.Prolongations))
                .ToList()
        );
    }

    public static void MapFrom(this LibraryCart entity, LibraryCartAggregate aggregate, DbContext context)
    {
        entity.BookLoans.Update(
            aggregate.ActiveBookLoans.ToList(),
            (loan, aggregateLoan) => loan.BookId == aggregateLoan.BookId,
            aggregateItem => new BookLoan
            {
                BookId = aggregateItem.BookId,
                BookIsbn = aggregateItem.BookIsbn,
                ReturnDate = aggregateItem.ReturnDate,
                Prolongations = aggregateItem.Prolongations
            },
            context);
    }
}