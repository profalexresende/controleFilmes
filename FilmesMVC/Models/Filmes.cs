
using System.ComponentModel.DataAnnotations;

namespace FilmesMVC.Models
{
    public class Filme
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public int AnoLancamento { get; set; }

        public string Genero { get; set; }

        public string Diretor { get; set; }

        public int Duracao { get; set; }
    }
}
