namespace CanvasRoomDesign.ModelCanvas
{
    public class DesignerState
    {
        //Sahnede bulununan tüm nesneler

        public List<DesignItem> Items { get; set; } = new List<DesignItem>();

        //Sadece seçili olan nesneler
        public IEnumerable<DesignItem> SelectedItems => Items.Where(i => i.IsSelected);

        //Kopyalanan nesnelerin hafızaya alınması
        private List<DesignItem> Clipboard { get; set; } = new List<DesignItem>();

        public event Action OnStateChanged;
        public void NotifyStateChanged() => OnStateChanged?.Invoke();

        //Temel işelmeler
        public void AddItem(DesignItem item) 
        { 
            Items.Add(item);
            NotifyStateChanged();
        }

        //Seçimi yapılan nesnelerin sıfırlanması
        public void ClearSelection() 
        { 
            Items.ForEach(i => i.IsSelected = false);
            NotifyStateChanged();
        }
        public void SelectItem(DesignItem item, bool isMultiSelect = false)
        {
            if (!isMultiSelect)
            {
                ClearSelection();
            }
            item.IsSelected = true;
            NotifyStateChanged();
        }

        public DesignItem? GetLastItem() => Items.LastOrDefault();
        
    }
}
