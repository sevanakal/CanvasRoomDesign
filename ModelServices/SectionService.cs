using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.Mapper;
using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelGeneral;
using Microsoft.EntityFrameworkCore;

namespace CanvasRoomDesign.ModelServices
{
    public class SectionService : ISectionService
    {
        private readonly CanvasDbcontext _context;

        public SectionService(CanvasDbcontext context)
        {
            _context = context;
        }

        public async Task<StatusMessage<SectionDto>> AddSection(SectionDto section)
        {
            StatusMessage<SectionDto> statusMessage = new StatusMessage<SectionDto>();
            var existingSection = await _context.Sections.Where(s => s.HallId == section.HallId && s.Name == section.Name.Trim()).AnyAsync();
            if (existingSection)
            {
                statusMessage.State = false;
                statusMessage.Message = "Section with the same name already exists in the hall.";
                return statusMessage;
            }
            _context.Sections.Add(section.toSectionEntity());
            await _context.SaveChangesAsync();
            statusMessage.State = true;
            statusMessage.Message = "Section added successfully.";
            statusMessage.Data = section;
            return statusMessage;
            
        }

        public async Task<StatusMessage> DeleteSection(Guid id)
        {
            StatusMessage statusMessage = new StatusMessage();
            var section = await _context.Sections.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (section != null)
            {
                section.IsDeleted = true;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();
                statusMessage.State = true;
                statusMessage.Message = "Section deleted successfully.";
                return statusMessage;
            }
            else
            {
                statusMessage.State = false;
                statusMessage.Message = "Section not found.";
                return statusMessage;
            }
        }

        public async Task<StatusMessage<List<SectionDto>>> GetSectionsByHallId(Guid id)
        {
            StatusMessage<List<SectionDto>> statusMessage = new StatusMessage<List<SectionDto>>();
            List<Section> sections = await _context.Sections.Where(s => s.HallId == id).ToListAsync();
            statusMessage.State = true;
            statusMessage.Message = "Sections retrieved successfully.";
            statusMessage.Data = sections.Select(s => s.toSectionDto()).ToList();
            return statusMessage;
        }

        public async Task<StatusMessage<SectionDto>> GetSectionById(Guid id)
        {
            StatusMessage<SectionDto> statusMessage= new StatusMessage<SectionDto>();
            SectionDto section = await _context.Sections.Where(s=>s.Id==id).Select(s=>s.toSectionDto()).FirstOrDefaultAsync();
            if (section != null) 
            { 
                statusMessage.State = true;
                statusMessage.Message = "Section retrieved successfully.";
                statusMessage.Data = section;
            }else
            {
                statusMessage.State = false;
                statusMessage.Message = "Section not found.";
            }
            return statusMessage;
        }

        public async Task<StatusMessage<SectionDto>> UpdateSection(SectionDto section)
        {
            StatusMessage<SectionDto> statusMessage = new StatusMessage<SectionDto>();
            var _section = await _context.Sections.Where(s => s.Id == section.Id).FirstOrDefaultAsync();
            if(_section != null)
            {
                var existingSection = await _context.Sections.Where(s => s.HallId == section.HallId && s.Name == section.Name.Trim() && s.Id != section.Id).AnyAsync();
                if (existingSection)
                {
                    statusMessage.State = false;
                    statusMessage.Message = "Section with the same name already exists in the hall.";
                    return statusMessage;
                }
                else
                {
                    _section.Name = section.Name;
                    await _context.SaveChangesAsync();
                    statusMessage.State = true;
                    statusMessage.Message = "Section updated successfully.";
                    statusMessage.Data = section;
                }
            }
            else
            {
                statusMessage.State = false;
                statusMessage.Message = "Section not found.";
            }
            return statusMessage;

        }

        
    }
}
