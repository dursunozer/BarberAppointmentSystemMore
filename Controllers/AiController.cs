using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace BarberAppointmentSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AiController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _env;

        private static Dictionary<string, string> hairType = new Dictionary<string, string>
        {
            { "101", "Bangs" },
            { "201", "Long hair" },
            { "301", "Bangs with long hair" },
            { "401", "Medium hair increase" },
            { "402", "Light hair increase" },
            { "403", "Heavy hair increase" },
            { "502", "Light curling" },
            { "503", "Heavy curling" },
            { "603", "Short hair" },
            { "801", "Blonde" },
            { "901", "Straight hair" },
            { "1001", "Oil-free hair" },
            { "1101", "Hairline fill" },
            { "1201", "Smooth hair" },
            { "1301", "Fill hair gap" }
        };

        public AiController(IHttpClientFactory clientFactory, IWebHostEnvironment env)
        {
            _clientFactory = clientFactory;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Form sayfası
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile uploadedImage)
        {
            // Kullanıcı resim seçmemişse veya dosya boşsa
            if (uploadedImage == null || uploadedImage.Length == 0)
            {
                ViewBag.Error = "Lütfen bir resim dosyası seçin.";
                return View();
            }

            // Resim boyut kontrolü (5 MB)
            if (uploadedImage.Length > 5 * 1024 * 1024)
            {
                ViewBag.Error = "Resim boyutu 5 MB'dan büyük olamaz.";
                return View();
            }

            // Uzantı kontrolü
            var extension = Path.GetExtension(uploadedImage.FileName).ToLowerInvariant();
            if (extension != ".jpeg" && extension != ".jpg" && extension != ".png" && extension != ".bmp")
            {
                ViewBag.Error = "Resim formatı JPEG, JPG, PNG veya BMP olmalıdır.";
                return View();
            }

            // Geçici dosyaya kaydet
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await uploadedImage.CopyToAsync(stream);
            }

            // Rastgele 1 saç tipi seç
            var random = new Random();
            var allHairTypes = hairType.Keys.ToList(); // "101", "201", ...
            int idx = random.Next(allHairTypes.Count);
            string selectedHairTypeKey = allHairTypes[idx];

            // HTTP Client
            var client = _clientFactory.CreateClient("HairstyleClient");

            // Tek API çağrısı
            string resultBase64 = await SendApiRequestAndGetBase64(client, tempFilePath, uploadedImage.FileName, selectedHairTypeKey);

            // ViewBag'e ekle
            ViewBag.SelectedHairTypeCode = selectedHairTypeKey;
            ViewBag.SelectedHairTypeName = hairType[selectedHairTypeKey];
            ViewBag.ResultImageBase64 = resultBase64;

            // Geçici dosyayı sil
            System.IO.File.Delete(tempFilePath);

            // Aynı sayfada sonucu göster
            return View();
        }

        [HttpGet]
        public IActionResult DownloadImage()
        {
            // Örnek: TempData'dan base64 okuyoruz
            if (TempData["AiResultImage"] is string base64)
            {
                // Gerekirse yeniden koy (çoklu kullanım için)
                // TempData["AiResultImage"] = base64; 

                byte[] fileBytes = Convert.FromBase64String(base64);
                // Dosya adı ve content type ayarla
                return File(fileBytes, "image/png", "AiHairstyle.png");
            }
            else
            {
                return NotFound("İndirilecek resim bulunamadı.");
            }
        }

        private async Task<string> SendApiRequestAndGetBase64(HttpClient client, string filePath, string fileName, string hairType)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(System.IO.File.OpenRead(filePath)), "image_target", fileName);
                    content.Add(new StringContent(hairType), "hair_type");

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        // Dokümantasyonunuza göre gerçek endpoint'i düzenleyin:
                        RequestUri = new Uri("huoshan/facebody/hairstyle", UriKind.Relative),
                        Content = content
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<dynamic>(body);
                        return apiResponse.data.image;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API isteği hatası: {ex.Message}");
                return null;
            }
        }
    }
}
