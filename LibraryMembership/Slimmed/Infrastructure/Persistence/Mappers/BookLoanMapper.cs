using System;
using LibraryMembership.Slimmed.Domain.BookLoan;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;

public static class BookLoanMapper
{
    public static BookLoanAggregate ToAggregate(this BookLoan entity)
    {
        return new BookLoanAggregate(
            entity.Id,
            entity.LoanedById,
            entity.BookId,
            entity.ReturnDate);
    }

    public static void MapFrom(this BookLoan entity, BookLoanAggregate aggregate)
    {
        entity.Returned = aggregate.Returned;
        if (aggregate.ReturnDate.HasValue)
        {
            entity.ReturnDate = aggregate.ReturnDate.Value;
        }
    }
}