using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using controleFilmes.Data;
using controleFilmes.Models;

namespace controleFilmes.Controllers



{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly FilmesContext _context;

        public FilmesController(FilmesContext context)
        {
            _context = context;
        }

        // Adicionar novo filme
        [HttpPost]
        public async Task<ActionResult<Filme>> AdicionarFilme(Filme filme)
        {
            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(BuscarFilmePorId), new { id = filme.Id }, filme);
        }

        // Buscar todos os filmes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filme>>> BuscarTodosFilmes()
        {
            return await _context.Filmes.ToListAsync();
        }

        // Buscar filme por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Filme>> BuscarFilmePorId(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }
            return filme;
        }

        // Atualizar informações de um filme
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarFilme(int id, Filme filme)
        {
            if (id != filme.Id)
            {
                return BadRequest();
            }

            _context.Entry(filme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Filmes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Excluir um filme
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }

            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Buscar filme por nome
        [HttpGet("buscarPorNome/{nome}")]
        public async Task<ActionResult<IEnumerable<Filme>>> BuscarFilmePorNome(string nome)
        {
            var filmes = await _context.Filmes.Where(f => f.Titulo.Contains(nome)).ToListAsync();
            if (!filmes.Any())
            {
                return NotFound();
            }
            return filmes;
        }

        // Buscar filmes por gênero
        [HttpGet("buscarPorGenero/{genero}")]
        public async Task<ActionResult<IEnumerable<Filme>>> BuscarFilmesPorGenero(string genero)
        {
            var filmes = await _context.Filmes.Where(f => f.Genero == genero).ToListAsync();
            if (!filmes.Any())
            {
                return NotFound();
            }
            return filmes;
        }
    }
}
