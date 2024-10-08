using Microsoft.AspNetCore.Mvc;

using FilmesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FilmesMVC.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public FilmesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Exibir todos os filmes
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("FilmesAPI");
            var response = await client.GetAsync("filmes");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var filmes = JsonConvert.DeserializeObject<List<Filme>>(conteudo);
                return View(filmes);
            }

            return View(new List<Filme>());
        }

        // Detalhes de um filme específico
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _clientFactory.CreateClient("FilmesAPI");
            var response = await client.GetAsync($"filmes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var filme = JsonConvert.DeserializeObject<Filme>(conteudo);
                return View(filme);
            }

            return NotFound();
        }

        // Formulário para criar um novo filme
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Filme filme)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("FilmesAPI");
                var conteudo = new StringContent(JsonConvert.SerializeObject(filme));
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("filmes", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(filme);
        }

        // Formulário para editar um filme existente
        public async Task<IActionResult> Editar(int id)
        {
            var client = _clientFactory.CreateClient("FilmesAPI");
            var response = await client.GetAsync($"filmes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var filme = JsonConvert.DeserializeObject<Filme>(conteudo);
                return View(filme);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Filme filme)
        {
            if (id != filme.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("FilmesAPI");
                var conteudo = new StringContent(JsonConvert.SerializeObject(filme));
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync($"filmes/{id}", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(filme);
        }

        // Exibir confirmação para exclusão de um filme
        public async Task<IActionResult> Excluir(int id)
        {
            var client = _clientFactory.CreateClient("FilmesAPI");
            var response = await client.GetAsync($"filmes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var filme = JsonConvert.DeserializeObject<Filme>(conteudo);
                return View(filme);
            }

            return NotFound();
        }

        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var client = _clientFactory.CreateClient("FilmesAPI");
            var response = await client.DeleteAsync($"filmes/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
