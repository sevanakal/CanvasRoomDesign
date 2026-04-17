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

        public async Task<StatusMessage> AddHall(Hall hall)
        {
            StatusMessage statusMessage = new StatusMessage();
            try
            {
                _context.Halls.Add(hall);
                await _context.SaveChangesAsync();
                statusMessage.State = true;
                statusMessage.Message = "Hall added successfully.";
            }
            catch (Exception ex)
            {
                statusMessage.State = false;
                statusMessage.Message = $"System Error (HallService): {ex.Message}";
            }
            return statusMessage;
        }

        public async Task<Hall> GetHallById(Guid id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                return hall;
            }
            else
            {
                return hall!;
            }
        }

        public async Task<List<Hall>> GetHallsWithSection()
        {
            List<Hall> halls = await _context.Halls.Include(h => h.Sections).ToListAsync();
            return halls;
        }

        public async Task<List<Hall>> GetHallWithAll()
        {
            List<Hall> halls = await _context.Halls
        .Include(h => h.Sections)
            .ThenInclude(s => s.Groups)
                .ThenInclude(g => g.HallItems) // 1. Dal: Gruplu nesneler
        .Include(h => h.Sections)
            .ThenInclude(s => s.HallItems)     // 2. Dal: Grupsuz (serbest) nesneler
        .ToListAsync();
            return halls;
        }

        public async Task<StatusMessage> DeleteHall(Guid id)
        {
            var hall = await _context.Halls.Where(h => h.Id == id).FirstOrDefaultAsync();
            StatusMessage statusMessage = new StatusMessage();
            try
            {
                if (hall != null)
                {
                    _context.Halls.Remove(hall);
                    await _context.SaveChangesAsync();
                    statusMessage.State = true;
                    statusMessage.Message = "Hall deleted successfully.";
                }
                else
                {
                    statusMessage.State = false;
                    statusMessage.Message = "Hall not found.";
                }
            }
            catch (Exception ex)
            {
                statusMessage.State = false;
                statusMessage.Message = $"Error deleting hall: {ex.Message}";
            }
            return statusMessage;

        }

        public async Task<StatusMessage> UpdateHall(Hall hall)
        {
            StatusMessage statusMessage = new StatusMessage();
            try
            {
                if (String.IsNullOrEmpty(hall.Name.Trim()))
                {
                    statusMessage.State = false;
                    statusMessage.Message = "Hall name cannot be empty.";
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
                    }
                    else
                    {
                        _context.Halls.Update(hall);
                        await _context.SaveChangesAsync();
                        statusMessage.State = true;
                        statusMessage.Message = "Hall updated successfully.";

                    }
                }
                
            }
            catch (Exception ex)
            {
                statusMessage.State = false;
                statusMessage.Message = $"Error updating (HallService): {ex.Message}";
            }
            return statusMessage;

        }
    }
}
