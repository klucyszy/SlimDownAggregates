using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Shared.Infrastructure.Persistence;

public static class ListUpdater
{
    public static void Update<TEntity, TAggregate>(
        this List<TEntity> entityList,
        List<TAggregate> aggregateList,
        Func<TEntity, TAggregate, bool> matches,
        Func<TAggregate, TEntity> onAdd,
        DbContext context
    )
    {
        var toUpdate = aggregateList
            .Where(agrItem => entityList.Any(entItem => matches(entItem, agrItem)))
            .ToList();
        var toAdd = aggregateList
            .Except(toUpdate)
            .ToList();
        var toRemove = entityList
            .Where(entItem => aggregateList.All(agrItem => !matches(entItem, agrItem)))
            .ToList();

        foreach (var updated in toUpdate)
        {
            var current = entityList.Single(entItem => matches(entItem, updated));
            //onUpdate(current, updated);
        }

        foreach (var added in toAdd)
        {
            var trackAdded = onAdd(added);
            entityList.Add(trackAdded);
            context.Add(trackAdded);
        }

        foreach (var removed in toRemove)
        {
            entityList.Remove(removed);
            context.Remove(removed);
        }
    }
}