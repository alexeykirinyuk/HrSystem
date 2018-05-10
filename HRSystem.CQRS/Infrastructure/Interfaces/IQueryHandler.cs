﻿namespace HRSystem.CQRS.Infrastructure.Interfaces
{
    public interface IQueryHandler<in TDefinition, out TResult>
        where TDefinition : IQueryDefinition<TDefinition, TResult>
    {
        TResult Execute(TDefinition parameters);
    }

}