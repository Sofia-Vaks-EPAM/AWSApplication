namespace AWSApplication.MessageQueues.Contracts
{
    public class BookQueueRequest
    {
        public string ISBN { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OperationType OperationType { get; set; }
    }

    public enum OperationType
    {
        Add,
        Update,
        Delete
    }
}
