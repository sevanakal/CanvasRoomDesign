using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelGeneral;
namespace CanvasRoomDesign.ModelServices
{
    public interface IHallService
    {
        public Task<StatusMessage> AddHall(Hall hall);
        public Task<Hall> GetHallById(Guid id);
        public Task<List<Hall>> GetHallsWithSection();

        public Task<List<Hall>> GetHallWithAll();
        public Task<StatusMessage> UpdateHall(Hall hall);
        public Task<StatusMessage> DeleteHall(Guid id);

    }
}
