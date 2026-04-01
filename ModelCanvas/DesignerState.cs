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
        public void SelectItem(DesignItem item, bool notify = true)
        {
            item.IsSelected = true;
            if (notify) NotifyStateChanged();
        }

        public void DeSelectItem(DesignItem item, bool notify = true)
        {
            item.IsSelected = false;
            if (notify) NotifyStateChanged();

        }

        public void RemoveItem(DesignItem item, bool notify = true)
        {
            Items.Remove(item);
            if (notify) NotifyStateChanged();
        }

        public void AddItemToGroup(GroupItem group)
        {
            foreach (var item in SelectedItems)
            {
                item.IsAddedToGroup = true;
                item.GroupId = group.Id;
                item.GroupName = group.Name;
                item.GroupColor = group.Color;
            }
        }

        public DesignItem? GetLastItem() => Items.LastOrDefault();
        
    }
}
