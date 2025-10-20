using backend.Domain.DTO;
using backend.Domain.Entities;
using backend.Domain.Exceptions;
using backend.Domain.Helpers;
using backend.Domain.Interfaces;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Domain.Services
{
    public class PessoaServices : IPessoaServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PessoaServices> _logger;

        public PessoaServices(ApplicationDbContext context, ILogger<PessoaServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Pessoa> Create(Pessoa pessoa)
        {
            try
            {
                if (pessoa.Cpf != null)
                {
                    var cpfNumerico = new string(pessoa.Cpf.Where(char.IsDigit).ToArray());

                    if (!ValidadorDeCpf.Validar(cpfNumerico))
                        throw new BadRequestException("CPF inválido.");
                }

                if (!string.IsNullOrEmpty(pessoa.Email) && !ValidadorDeEmail.Validar(pessoa.Email))
                    throw new BadRequestException("Email inválido");

                _context.Pessoas.Add(pessoa);
                await _context.SaveChangesAsync();
                return pessoa;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Erro de concorrência ao criar pessoa: {@Pessoa}", pessoa);
                throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("violat", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message?.Contains("unique", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message?.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
                {
                    throw new BadRequestException("Já existe uma pessoa com esses dados.");
                }

                _logger.LogError(ex, "Erro de atualização no banco ao criar pessoa: {@Pessoa}", pessoa);
                throw new BadRequestException("Erro ao salvar os dados no banco.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar pessoa: {@Pessoa}", pessoa);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var pessoa = await _context.Pessoas.FindAsync(id);

                if (pessoa == null)
                {
                    throw new NotFoundException($"Pessoa com ID {id} não encontrada.");
                }

                _context.Pessoas.Remove(pessoa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de atualização no banco ao deletar pessoa com ID {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar pessoa com ID {Id}", id);
                throw;
            }
        }

        public async Task<PagedResultsDto<Pessoa>> GetAll(int page, string? nome = null, string? cpf = null, string? email = null)
        {
            const int pageSize = 10;

            try
            {
                IQueryable<Pessoa> query = _context.Pessoas.AsQueryable();

                if (!string.IsNullOrWhiteSpace(nome))
                    query = query.Where(p => p.Nome.Contains(nome));

                if (!string.IsNullOrWhiteSpace(cpf))
                    query = query.Where(p => p.Cpf == cpf);

                if (!string.IsNullOrWhiteSpace(email))
                    query = query.Where(p => p.Email != null && p.Email.Contains(email));

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Valida página
                if (page < 1)
                    throw new BadRequestException("O número da página deve ser maior que 0.");

                if (page > totalPages && totalPages > 0)
                    throw new BadRequestException($"Página {page} não existe. Total de páginas: {totalPages}.");

                var items = await query
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResultsDto<Pessoa>
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    Items = items
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao consultar pessoas (página {Page})", page);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro de operação inválida ao consultar pessoas (página {Page})", page);
                throw;
            }
        }

        public async Task<Pessoa> GetByCpf(string cpf)
        {
            try
            {
                var cpfNumerico = new string(cpf.Where(char.IsDigit).ToArray());

                if (!ValidadorDeCpf.Validar(cpfNumerico))
                    throw new BadRequestException("CPF inválido.");

                var pessoa = await _context.Pessoas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Cpf == cpf);

                if (pessoa == null)
                    throw new NotFoundException($"Pessoa com CPF {cpf} não encontrada.");

                return pessoa;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar pessoa por CPF {Cpf}", cpf);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro de operação inválida ao buscar pessoa por CPF {Cpf}", cpf);
                throw;
            }
        }

        public async Task<Pessoa> GetByEmail(string email)
        {
            try
            {
                var pessoa = await _context.Pessoas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Email == email);

                if (pessoa == null)
                    throw new NotFoundException($"Pessoa com email {email} não encontrada.");

                return pessoa;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de atualização ao buscar pessoa por email {email}", email);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro de operação inválida ao buscar pessoa por email {email}", email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar pessoa por email {email}", email);
                throw;
            }
        }

        public async Task<Pessoa> GetById(int id)
        {
            try
            {
                var pessoa = await _context.Pessoas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (pessoa == null)
                    throw new NotFoundException($"Pessoa com id {id} não encontrada.");

                return pessoa;

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de atualização ao buscar pessoa por CPF {Cpf}", id);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro de operação inválida ao buscar pessoa por CPF {Cpf}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar pessoa por CPF {Cpf}", id);
                throw;
            }
        }

        public async Task<Pessoa> Update(Pessoa pessoa)
        {
            try
            {
                var existingPessoa = await _context.Pessoas.FindAsync(pessoa.Id);
                
                if (existingPessoa == null)
                {
                    _logger.LogWarning("Tentativa de atualizar pessoa inexistente com Id {Id}", pessoa.Id);
                    throw new NotFoundException($"Pessoa com id {pessoa.Id} não encontrada."); ;
                }

                var cpfNumerico = new string(pessoa.Cpf.Where(char.IsDigit).ToArray());

                if (!ValidadorDeCpf.Validar(cpfNumerico))
                    throw new BadRequestException("CPF inválido.");

                if (!string.IsNullOrEmpty(pessoa.Email) && !ValidadorDeEmail.Validar(pessoa.Email))
                    throw new BadRequestException("Email inválido");

                existingPessoa.Nome = pessoa.Nome;
                existingPessoa.Cpf = pessoa.Cpf;
                existingPessoa.DataNascimento = pessoa.DataNascimento;
                
                if ( pessoa.Sexo != null)
                {
                    existingPessoa.Sexo = pessoa.Sexo;
                }
                
                if (pessoa.Email != null)
                {
                    existingPessoa.Email = pessoa.Email;
                }

                await _context.SaveChangesAsync();

                return existingPessoa;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Erro de concorrência ao atualizar pessoa com Id {Id}", pessoa.Id);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de atualização no banco ao atualizar pessoa com Id {Id}", pessoa.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar pessoa com Id {Id}", pessoa.Id);
                throw;
            }
        }

    }
}
