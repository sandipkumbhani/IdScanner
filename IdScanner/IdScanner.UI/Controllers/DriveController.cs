using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    [Route("drive")]
    public class DriveController : Controller
    {
        private readonly HttpClient _http;

        public DriveController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> Image(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Missing file id");
            }

            var url = $"https://drive.google.com/uc?export=download&id={id}";
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg";

            return File(bytes, contentType);
        }
    }
}
