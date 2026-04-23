namespace CanvasRoomDesign.ModelGeneral
{
    public class StatusMessage
    {
        public bool State { get; set; }
        public string? Message { get; set; }
    }

    public class  StatusMessage<T> : StatusMessage
    {
        public T? Data { get; set; }
    }
}
