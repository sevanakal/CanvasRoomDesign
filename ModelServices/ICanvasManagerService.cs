using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.ModelGeneral;

namespace CanvasRoomDesign.ModelServices
{
    public interface ICanvasManagerService
    {
        public Task<StatusMessage> SaveCanvasAsync(CanvasDesignDto canvasDesignDto);
    }
}
