using CanvasRoomDesign.ModelCanvas;

namespace CanvasRoomDesign.DTOs
{
    public class SectionDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid HallId { get; set; }
        public string Name { get; set; } = string.Empty;
        public HallDto? hall { get; set; }

        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public List<DesignItem> HallItems { get; set; } = new List<DesignItem>();

    }
}
