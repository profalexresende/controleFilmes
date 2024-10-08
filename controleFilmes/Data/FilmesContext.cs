using Microsoft.EntityFrameworkCore;
using controleFilmes.Models;


namespace controleFilmes.Data

{
    public class FilmesContext : DbContext
    {
        public FilmesContext(DbContextOptions<FilmesContext> options) : base(options) { }

        public DbSet<Filme> Filmes { get; set; }
    }
}
