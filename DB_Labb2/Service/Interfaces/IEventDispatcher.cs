namespace Bookstore.Service
{
    public interface IEventDispatcher
    {
        void EntityList<T>(List<T> list);
        void EntityAdded<T>(T entity);
        void EntityRemoved<T>(T entity);
        void EntityUpdated<T>(T entity);
        public event EventHandler<object>? EntityAddedEvent;
        public event EventHandler<object>? EntityUpdatedEvent;
        public event EventHandler<object>? EntityRemovedEvent;
        public event EventHandler<object>? EntityListEvent;
    }
}