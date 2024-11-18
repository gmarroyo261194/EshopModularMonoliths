namespace Catalog.Products.EventHandler;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger) 
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handler: {DomainEvent}", notification.GetType().Name);
        
        return Task.CompletedTask;
    }
}