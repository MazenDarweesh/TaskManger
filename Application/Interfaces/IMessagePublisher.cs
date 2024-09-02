namespace Application.Interfaces
{
    public interface IMessagePublisher
    {
        void SendMessage<T>(T message);
    }
}
