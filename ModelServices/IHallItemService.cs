using CanvasRoomDesign.ModelCanvas;
using CanvasRoomDesign.ModelGeneral;

namespace CanvasRoomDesign.ModelServices
{
    public interface IHallItemService
    {
        public Task<StatusMessage<DesignItem>> AddHallItem(DesignItem designItem); 

        public Task<StatusMessage<DesignItem>> GetHallItemById(Guid id);

        public Task<StatusMessage<List<DesignItem>>> ListHallItemsBySectionId(Guid id);

        public Task<StatusMessage<DesignItem>> UpdateHallItem(DesignItem designItem);

        public Task<StatusMessage> DeleteHallItem(Guid id);
    }
}
