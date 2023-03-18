using Post.Common.Events;

namespace Post.Query.Infrastructure.Handlers;

public interface IEventHandler
{
    /*
     The purpose of the EventHandler is to retrieve all events for a given aggregate from the Event Store and to invoke the ReplayEvents method on the AggregateRoot to recreate the latest state of the aggregate?
         - No. Is the EventSourcingHandler, and not the EventHandler, that is responsible for retrieving all events for a given aggregate from the Even Store and to invoke the ReplayEvents mehotd on the AgregateRoot to recreate the latest state of the aggregate.
     
     The EventHandler is responsible to update the read database via the relevant repository interface once a new event was consumed from kafka.
        - Yes. Once the EventConsumer consumes an event, it will invoke the relevant handler. (O(n)) method which will use the event message to build or alter the PostEntity or CommentEntity, and update the related record in the read database.
     */


    Task On(PostCreatedEvent @event);
    Task On(MessageUpdatedEvent @event);
    Task On(PostLikedEvent @event);
    Task On(CommentdAddedEvent @event);
    Task On(CommentUpdatedEvent @event);
    Task On(CommentRemovedEvent @event);
    Task On(PostRemovedEvent @event);
}
