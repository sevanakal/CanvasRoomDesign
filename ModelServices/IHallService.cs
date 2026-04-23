using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.ModelGeneral;
namespace CanvasRoomDesign.ModelServices
{
    public interface IHallService
    {
        public Task<StatusMessage<HallDto>> AddHall(HallDto hall); 
        public Task<StatusMessage<HallDto>> GetHallById(Guid id); 
        public Task<StatusMessage<List<HallDto>>> GetHallsWithSection(); 

        public Task<StatusMessage<List<HallDto>>> GetHallWithAll();
        public Task<StatusMessage<HallDto>> UpdateHall(HallDto hall);
        public Task<StatusMessage>DeleteHall(Guid id);

    }
}
