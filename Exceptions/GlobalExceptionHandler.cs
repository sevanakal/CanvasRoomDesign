using CanvasRoomDesign.ModelGeneral;
using Microsoft.AspNetCore.Diagnostics;

namespace CanvasRoomDesign.Exceptions
{
    // IExceptionHandler: .NET 8'in yeni nesil hata yakalayıcısı
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Hatayı GİZLİCE Kaydet (Logla). 
            // Yönetici (Sen) arka planda gerçekte neyin patladığını (örn: SQL çöktü) loglardan göreceksin.
            _logger.LogError(exception, "Şantiyede beklenmeyen kaza: {Message}", exception.Message);

            // 2. Kullanıcıya Gidecek KİBAR Mesaj (StatusMessage formatında)
            var errorResponse = new StatusMessage
            {
                State = false,
                // Kullanıcıya "Veritabanı patladı" denmez, kibarca uyarılır.
                Message = "Sistemde geçici bir işlem hatası oluştu. Lütfen yöneticinizle iletişime geçin. "
            };

            // 3. HTTP durum kodunu ayarla (500 Internal Server Error)
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // 4. Müşteriye (UI) StatusMessage paketini gönder
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            // "Hatayı ben devraldım, uygulamayı çökertme" mesajı veriyoruz.
            return true;
        }
    }
}