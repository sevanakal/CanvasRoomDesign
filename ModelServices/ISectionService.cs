using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.ModelGeneral;

namespace CanvasRoomDesign.ModelServices
{
    public interface ISectionService
    {
        public Task<StatusMessage<SectionDto>> AddSection(SectionDto section);
        public Task<StatusMessage<SectionDto>> GetSectionById(Guid id);
        public Task<StatusMessage<List<SectionDto>>> GetSectionsByHallId(Guid id);
        public Task<StatusMessage<SectionDto>> UpdateSection(SectionDto section);
        public Task<StatusMessage> DeleteSection(Guid id);
    }
}
