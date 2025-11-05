using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Service
{
    public class EventDispatcher : IEventDispatcher
    {
        public event EventHandler<object>? EntityAddedEvent;
        public event EventHandler<object>? EntityUpdatedEvent;
        public event EventHandler<object>? EntityRemovedEvent;
        public event EventHandler<object>? EntityListEvent;

        public void EntityAdded<T>(T entity) => EntityAddedEvent?.Invoke(this, entity!);
        public void EntityUpdated<T>(T entity) => EntityUpdatedEvent?.Invoke(this, entity!);
        public void EntityRemoved<T>(T entity) => EntityRemovedEvent?.Invoke(this, entity!);

        public void EntityList<T>(List<T> list) => EntityListEvent?.Invoke(this, list!);
    }
}
