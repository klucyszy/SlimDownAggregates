using System;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public sealed class BookLoan
{
    public Guid Id { get; set; }
    public Guid LoanedById { get; set; }
    public Guid BookId { get; set; }
    public string BookIsbn { get; set; }
    public int ProlongedTimes { get; set; }
    public bool Returned { get; set; }
    public DateTimeOffset ReturnDate { get; set; }
}