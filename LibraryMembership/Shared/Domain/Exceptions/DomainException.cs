using System;

namespace LibraryMembership.Shared.Domain.Exceptions;

public sealed class DomainException(string message) : Exception(message);