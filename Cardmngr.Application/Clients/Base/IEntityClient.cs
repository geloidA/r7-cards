﻿namespace Cardmngr.Application.Clients.Base
{
    public interface IEntityClient<TEntity, TUpdateData>
    {
        Task<TEntity> CreateAsync(int projectId, TUpdateData updateData);
        Task<TEntity> UpdateAsync(int entityId, TUpdateData updateData);
        Task<TEntity> RemoveAsync(int entityId);
    }
}