using daleel.Entities;

namespace daleel.Services
{
    public interface ISourceService
    {
        Task<IEnumerable<Source>> GetAllSourcesAsync();
        Task<Source> GetSourceAsync(Guid id);
        Task<Source> CreateSourceAsync(Source source);
        Task UpdateSourceAsync(Source source);
        Task DeleteSourceAsync(Guid id);
    }
}
