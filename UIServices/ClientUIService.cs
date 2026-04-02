using CurrieTechnologies.Razor.SweetAlert2;
using System.Runtime.CompilerServices;
namespace CanvasRoomDesign.UIServices
{
    public class ClientUIService : IClientUIService
    {
        private readonly SweetAlertService _sweetAlertService;

        public ClientUIService(SweetAlertService sweetAlertService)
        {
            _sweetAlertService = sweetAlertService;
        }

        public async Task<bool> ConfirmDelete(string message) 
        {
            var result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Deletion",
                Text = message,
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "No, keep it",
                ConfirmButtonColor = "#3085d6",
                CancelButtonColor= "#d33"

            });

            return !string.IsNullOrEmpty(result.Value);
            
        }

        public async Task ShowSuccess(string message) {
            var result = await _sweetAlertService.FireAsync(new SweetAlertOptions { 
                Title= "Success",
                Text = message,
                Icon = SweetAlertIcon.Success,
                Timer = 2000,
                ShowConfirmButton = false
            });
        }

        public async Task ShowError(string message) {
            var result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Error",
                Text = message,
                Icon = SweetAlertIcon.Error,
                Timer = 3000,
                ShowConfirmButton = false
            });
        }
    }
}
