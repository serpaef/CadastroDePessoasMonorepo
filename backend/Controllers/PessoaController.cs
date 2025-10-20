using backend.Domain.Entities;
using backend.Domain.Exceptions;
using backend.Domain.Interfaces;
using backend.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("/v1/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IPessoaServices _pessoaServices;

        public PessoaController(ILogger<LoginController> logger, IPessoaServices pessoaServices)
        {
            _logger = logger;
            _pessoaServices = pessoaServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] string? nome = null,
            [FromQuery] string? cpf = null,
            [FromQuery] string? email = null)
        {
            var result = await _pessoaServices.GetAll(page, nome, cpf, email);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pessoa = await _pessoaServices.GetById(id);
            return Ok(pessoa);
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var pessoa = await _pessoaServices.GetByCpf(cpf);
            return Ok(pessoa);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var pessoa = await _pessoaServices.GetByEmail(email);
            return Ok(pessoa);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pessoaServices.Delete(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pessoa pessoa)
        {
            var createdPessoa = await _pessoaServices.Create(pessoa);
            return CreatedAtAction(nameof(GetById), new { id = createdPessoa.Id }, createdPessoa);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pessoa pessoa)
        {
            if (pessoa.Id == 0)
                pessoa.Id = id;

            if (id != pessoa.Id)
                throw new BadRequestException("O ID da URL não corresponde ao ID da pessoa.");

            var updatedPessoa = await _pessoaServices.Update(pessoa);
            return Ok(updatedPessoa);
        }


    }
}
