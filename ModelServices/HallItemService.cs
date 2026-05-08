using CanvasRoomDesign.Mapper;
using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelCanvas;
using CanvasRoomDesign.ModelGeneral;
using Microsoft.EntityFrameworkCore;

namespace CanvasRoomDesign.ModelServices
{
    public class HallItemService : IHallItemService
    {
        private readonly CanvasDbcontext _context;

        public HallItemService(CanvasDbcontext context)
        {
            _context = context;
        }

        public async Task<StatusMessage<DesignItem>> AddHallItem(DesignItem designItem)
        {
            StatusMessage<DesignItem> statusMessage = new StatusMessage<DesignItem>();
            var isExist = await _context.HallItems.AnyAsync(h => h.SectionId == designItem.SectionId && h.Name == designItem.Name);
            if (isExist)
            {
                statusMessage.State = false;
                statusMessage.Message = $"HallItem with name '{designItem.Name}' already exists in the section.";
                statusMessage.Data = designItem;
            }
            else
            {
                HallItem hallItem = designItem.toHallItemEntity();
                _context.HallItems.Add(hallItem);
                await _context.SaveChangesAsync();
                statusMessage.State = true;
                statusMessage.Message = "HallItem added successfully.";
                statusMessage.Data = hallItem.toDesignItemDto();
            }

            return statusMessage;

        }


        public async Task<StatusMessage<DesignItem>> GetHallItemById(Guid id)
        {
            StatusMessage<DesignItem> statusMessage = new StatusMessage<DesignItem>();
            var hallItem = await _context.HallItems.FindAsync(id);
            if (hallItem == null)
            {
                statusMessage.State = false;
                statusMessage.Message = $"HallItem with ID '{id}' not found.";
                statusMessage.Data = null;
            }
            else
            {
                statusMessage.State = true;
                statusMessage.Message = "HallItem retrieved successfully.";
                statusMessage.Data = hallItem.toDesignItemDto();
            }

            return statusMessage;
        }


        public async Task<StatusMessage<List<DesignItem>>> ListHallItemsBySectionId(Guid id)
        {
            StatusMessage<List<DesignItem>> statusMessage = new StatusMessage<List<DesignItem>>();
            var hallItems = await _context.HallItems.Where(h => h.SectionId == id).Select(h => h.toDesignItemDto()).ToListAsync();
            if (hallItems == null || hallItems.Count == 0)
            {
                statusMessage.State = false;
                statusMessage.Message = $"No HallItems found for Section ID '{id}'.";
                statusMessage.Data = null;
            }
            else
            {
                statusMessage.State = true;
                statusMessage.Message = "HallItems retrieved successfully.";
                statusMessage.Data = hallItems;
            }
            return statusMessage;
        }

        public async Task<StatusMessage<DesignItem>> UpdateHallItem(DesignItem designItem)
        {
            StatusMessage<DesignItem> statusMessage = new StatusMessage<DesignItem>();
            var hallItem = await _context.HallItems.FindAsync(designItem.Id);
            if (hallItem == null)
            {
                statusMessage.State = false;
                statusMessage.Message = $"HallItem with ID '{designItem.Id}' not found.";
                statusMessage.Data = null;
            }
            else
            {
                hallItem.Name = designItem.Name;
                hallItem.X = designItem.X;
                hallItem.Y = designItem.Y;
                hallItem.Width = designItem.Width;
                hallItem.Height = designItem.Height;
                hallItem.Rotation = designItem.Rotation;
                await _context.SaveChangesAsync();

                statusMessage.State = true;
                statusMessage.Message = "HallItem updated successfully.";
                statusMessage.Data = hallItem.toDesignItemDto();
            }

            return statusMessage;

        }

        public async Task<StatusMessage> DeleteHallItem(Guid id)
        {
            StatusMessage statusMessage = new StatusMessage();
            var hallItem = await _context.HallItems.FindAsync(id);
            if (hallItem == null)
            {
                statusMessage.State = false;
                statusMessage.Message = $"HallItem with ID '{id}' not found.";
            }
            else
            {
                hallItem.IsDeleted = true;
                await _context.SaveChangesAsync();
                statusMessage.State = true;
                statusMessage.Message = "HallItem deleted successfully.";
            }
            return statusMessage;
        }
        
    }
}
