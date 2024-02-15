using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Shared.Infrastructure.Persistence;

public static class ListUpdater
{
    public static void Update<TLTo, TLFrom>(
        this List<TLTo> to,
        List<TLFrom> from,
        Func<TLFrom, Func<TLTo, bool>> matches,
        Func<TLFrom, TLTo> onAdd,
        DbContext context
    )
    {
        var toUpdate = from.Where(f => to.Any(matches(f))).ToList();
        var toAdd = from.Except(toUpdate);
    
        foreach (var updated in toUpdate)
        {
            var current = to.Single(matches(updated));
            //onUpdate(current, updated);
        }

        foreach (var added in toAdd)
        {
            var bla = onAdd(added);
            to.Add(bla);
            context.Add(bla);
        }
    }
}