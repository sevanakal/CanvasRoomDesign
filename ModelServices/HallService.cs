using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.Mapper;
using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelGeneral;
using Microsoft.EntityFrameworkCore;
namespace CanvasRoomDesign.ModelServices
{
    public class HallService : IHallService
    {
        private readonly CanvasDbcontext _context;

        public HallService(CanvasDbcontext context)
        {
            _context = context;
        }

        public async Task<StatusMessage<HallDto>> AddHall(HallDto hall)
        {
            StatusMessage<HallDto> statusMessage = new StatusMessage<HallDto>();
            var existingHall = await _context.Halls.Where(h => h.Name == hall.Name).AnyAsync();
            if (existingHall)
            {
                statusMessage.State = false;
                statusMessage.Message = "Hall name already exists.";
                return statusMessage;
            }

            _context.Halls.Add(hall.toHallEntity());
            await _context.SaveChangesAsync();
            statusMessage.Data = hall;
            statusMessage.State = true;
            statusMessage.Message = "Hall added successfully.";

            return statusMessage;
        }

        public async Task<StatusMessage<HallDto>> GetHallById(Guid id)
        {
            var hall = await _context.Halls.FindAsync(id);
            StatusMessage<HallDto> statusMessage = new StatusMessage<HallDto>();
            if (hall != null)
            {
                statusMessage.Data = hall.ToHallDto();
                statusMessage.State = true;
                statusMessage.Message = "Hall found successfully.";
                return statusMessage;
            }
            else
            {
                statusMessage.Data = null;
                statusMessage.State = false;
                statusMessage.Message = "Hall not found.";
                return statusMessage;
            }
        }

        public async Task<StatusMessage<List<HallDto>>> GetHallsWithSection()
        {
            StatusMessage<List<HallDto>> statusMessage = new StatusMessage<List<HallDto>>();

            List<Hall> halls = await _context.Halls.Include(h => h.Sections).ToListAsync();
            List<HallDto> hallDtos = halls.Select(h => h.ToHallDto()).ToList();
            statusMessage.Data = hallDtos;
            statusMessage.State = true;
            statusMessage.Message = "Halls with sections retrieved successfully.";

            return statusMessage;
        }

        public async Task<StatusMessage<List<HallDto>>> GetHallWithAll()
        {
            StatusMessage<List<HallDto>> statusMessage = new StatusMessage<List<HallDto>>();

            List<Hall> halls = await _context.Halls
    .Include(h => h.Sections)
        .ThenInclude(s => s.Groups)
            .ThenInclude(g => g.HallItems) // 1. Dal: Gruplu nesneler
    .Include(h => h.Sections)
        .ThenInclude(s => s.HallItems)     // 2. Dal: Grupsuz (serbest) nesneler
    .ToListAsync();
            List<HallDto> hallDtos = halls.Select(h => h.ToHallDto()).ToList();
            statusMessage.Data = hallDtos;
            statusMessage.State = true;
            statusMessage.Message = "Halls with all details retrieved successfully.";



            return statusMessage;
        }

        public async Task<StatusMessage> DeleteHall(Guid id)
        {
            var hall = await _context.Halls.Where(h => h.Id == id).FirstOrDefaultAsync();
            StatusMessage statusMessage = new StatusMessage();

            if (hall != null)
            {
                hall.IsDeleted = true;
                _context.Halls.Update(hall);
                await _context.SaveChangesAsync();
                statusMessage.State = true;
                statusMessage.Message = "Hall deleted successfully.";
            }
            else
            {
                statusMessage.State = false;
                statusMessage.Message = "Hall not found.";
            }

            return statusMessage;

        }

        public async Task<StatusMessage<HallDto>> UpdateHall(HallDto hall)
        {
            StatusMessage<HallDto> statusMessage = new StatusMessage<HallDto>();

            if (String.IsNullOrEmpty(hall.Name.Trim()))
            {
                statusMessage.State = false;
                statusMessage.Message = "Hall name cannot be empty.";
                return statusMessage;
            }
            else
            {
                // 1. AnyAsync kullandık (Çok hızlı ve Asenkron)
                // 2. h.Id != hall.Id diyerek kendi kendini bulmasını engelledik!
                bool nameExists = await _context.Halls.AnyAsync(h => h.Name == hall.Name && h.Id != hall.Id);
                if (nameExists)
                {
                    statusMessage.State = false;
                    statusMessage.Message = "Hall name already exists.";
                    return statusMessage;
                }
                else
                {
                    var existingHall = await _context.Halls.FindAsync(hall.Id);

                    if (existingHall == null)
                    {
                        statusMessage.State = false;
                        statusMessage.Message = "Hall not found in database.";
                        return statusMessage;
                    }
                    // 3. Güvenli Eşleştirme (Sadece değişmesine izin verdiğimiz alanları güncelliyoruz)
                    existingHall.Name = hall.Name;

                    // EF Core 'existingHall' nesnesini zaten takip ettiği (Tracking) için 
                    // _context.Halls.Update() YAZMIYORUZ! Direkt SaveChanges diyoruz.
                    await _context.SaveChangesAsync();

                    statusMessage.Data = hall;
                    statusMessage.State = true;
                    statusMessage.Message = "Hall updated successfully.";

                }
            }


            return statusMessage;

        }
    }
}
