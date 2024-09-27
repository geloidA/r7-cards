using AutoMapper;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Milestone
{
    /// <summary>
    /// Клиент для работы с вехами.
    /// </summary>
    /// <remarks>
    /// инициализирует экземпляр <see cref="MilestoneClient"/>.
    /// </remarks>
    /// <param name="milestoneRepository">репозиторий для работы с вехами.</param>
    /// <param name="mapper">маппер для преобразования dto в доменные модели.</param>
    public class MilestoneClient(IMilestoneRepository milestoneRepository, IMapper mapper) : IMilestoneClient
    {
        /// <summary>
        /// Создает веху.
        /// </summary>
        /// <param name="projectId">идентификатор проекта, к которому будет добавлена веха.</param>
        /// <param name="updateData">данные для создания вехи.</param>
        /// <returns>созданная веха.</returns>
        public async Task<Domain.Entities.Milestone> CreateAsync(int projectId, MilestoneUpdateData updateData)
        {
            var milestoneDto = await milestoneRepository.CreateAsync(projectId, updateData);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        /// <summary>
        /// Получает веху по ее идентификатору.
        /// </summary>
        /// <param name="entityId">идентификатор вехи.</param>
        /// <returns>веха.</returns>
        public async Task<Domain.Entities.Milestone> GetAsync(int entityId)
        {
            var milestoneDto = await milestoneRepository.GetByIdAsync(entityId);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        /// <summary>
        /// Получает вехи.
        /// </summary>
        /// <param name="filterBuilder">фильтр для поиска вех.</param>
        /// <returns>вехи.</returns>
        public IAsyncEnumerable<Domain.Entities.Milestone> GetEntitiesAsync(FilterBuilder? filterBuilder = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет веху.
        /// </summary>
        /// <param name="milestoneId">идентификатор вехи, которую нужно удалить.</param>
        /// <returns>удаленная веха.</returns>
        public async Task<Domain.Entities.Milestone> RemoveAsync(int milestoneId)
        {
            var milestoneDto = await milestoneRepository.DeleteAsync(milestoneId);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        /// <summary>
        /// Обновляет веху.
        /// </summary>
        /// <param name="milestoneId">идентификатор вехи, которую нужно обновить.</param>
        /// <param name="updateData">данные для обновления вехи.</param>
        /// <returns>обновленная веха.</returns>
        public async Task<Domain.Entities.Milestone> UpdateAsync(int milestoneId, MilestoneUpdateData updateData)
        {
            var milestoneDto = await milestoneRepository.UpdateAsync(milestoneId, updateData);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        /// <summary>
        /// Обновляет статус вехи.
        /// </summary>
        /// <param name="id">идентификатор вехи, статус которой нужно обновить.</param>
        /// <param name="status">новый статус вехи.</param>
        /// <returns>обновленная веха.</returns>
        public async Task<Domain.Entities.Milestone> UpdateStatusAsync(int id, int status)
        {
            if (status < 0 || status > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(status));
            }
            
            var milestoneDto = await milestoneRepository.UpdateStatusAsync(id, status);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }
    }
}
