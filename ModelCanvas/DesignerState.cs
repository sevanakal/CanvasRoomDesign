namespace CanvasRoomDesign.ModelCanvas
{
    public class DesignerState
    {
        //Sahnede bulununan tüm nesneler

        public List<DesignItem> Items { get; set; } = new List<DesignItem>();

        //Sadece seçili olan nesneler
        public IEnumerable<DesignItem> SelectedItems => Items.Where(i => i.IsSelected);

        public List<GroupItem> Groups { get; set; } = new List<GroupItem>();

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
            if (!HasGroup(group.Name))
            {
                Groups.Add(group);
                foreach (var item in SelectedItems)
                {
                    item.IsAddedToGroup = true;
                    item.GroupId = group.Id;
                    item.GroupName = group.Name;
                    item.GroupColor = group.Color;

                }
            }
            else
            {
                var groupItem = Groups.Where(g => g.Name == group.Name).FirstOrDefault();
                if (groupItem != null)
                {
                    foreach (var item in SelectedItems)
                    {
                        item.IsAddedToGroup = true;
                        item.GroupId = groupItem.Id;
                        item.GroupName = group.Name;
                        item.GroupColor = groupItem.Color;

                    }
                }
            }


            CheckBlankGroup(group.Name);
        }

        private bool HasGroup(string groupname)
        {
            var group = Groups.FirstOrDefault(g => g.Name == groupname);
            if (group != null) { return true; } else { return false; }
        }

        public void CheckBlankGroup(string groupname)
        {
            List<Guid> tempGroup= new List<Guid>();
            foreach (var item in Groups)
            {
                if (Items.Where(i => i.GroupId == item.Id).Count() == 0)
                {
                    tempGroup.Add(item.Id);
                }
            }
            if (tempGroup.Any())
            {
                foreach (var groupId in tempGroup)
                {
                    var group = Groups.FirstOrDefault(g => g.Id == groupId);
                    if (group != null)
                    {
                        Groups.Remove(group);
                    }
                }
                tempGroup = new List<Guid>();
            }
        }
        
        public DesignItem? GetLastItem() => Items.LastOrDefault();
        
    }
}
