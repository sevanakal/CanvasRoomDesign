using CanvasRoomDesign.ModelCanvas;

namespace CanvasRoomDesign.DTOs
{
    public class CanvasDesignDto
    {
        public Guid HallId { get; set; }

        // Eklenecek veya Güncellenecekler Listesi
        public List<SectionDto> SectionToSave { get; set; } = new List<SectionDto>();

        public List<GroupDto> GroupToSave { get; set; } = new List<GroupDto>();

        public List<DesignItem> DesignItemToSave { get; set; } = new List<DesignItem>();

        //Silinecekler Listesi
        public List<Guid> DeleteSectionIds { get; set; } = new List<Guid>();

        public List<Guid> DeleteGroupIds { get; set; } = new List<Guid>();

        public List<Guid> DeleteDesignItemIds { get; set; } = new List<Guid>();
    }
}
