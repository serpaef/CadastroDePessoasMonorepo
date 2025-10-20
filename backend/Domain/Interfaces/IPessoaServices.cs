using backend.Domain.Entities;
using backend.Domain.DTO;


namespace backend.Domain.Interfaces
{
    public interface IPessoaServices
    {
        Task<PagedResultsDto<Pessoa>> GetAll(int page, string? nome = null, string? cpf = null, string? email = null);
        Task<Pessoa> GetByCpf(string cpf);
        Task<Pessoa> GetByEmail(string email);
        Task<Pessoa> GetById(int id);
        Task<Pessoa> Create(Pessoa entity);
        Task<Pessoa> Update(Pessoa entity);
        Task Delete(int id);
    }
}
