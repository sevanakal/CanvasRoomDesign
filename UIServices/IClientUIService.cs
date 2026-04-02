namespace CanvasRoomDesign.UIServices
{
    public interface IClientUIService
    {
        Task<bool> ConfirmDelete(string message);

        Task ShowSuccess(string message);

        Task ShowError(string message);
    }
}
