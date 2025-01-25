using QMSPOC.ItemMeasuremetnDetails;

using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace QMSPOC.ItemMessurements;

public class ItemMessurementDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<ItemMessurement>>, ITransientDependency
{
    private readonly IItemMeasuremetnDetailRepository _itemMeasuremetnDetailRepository;

    public ItemMessurementDeletedEventHandler(IItemMeasuremetnDetailRepository itemMeasuremetnDetailRepository)
    {
        _itemMeasuremetnDetailRepository = itemMeasuremetnDetailRepository;

    }

    public async Task HandleEventAsync(EntityDeletedEventData<ItemMessurement> eventData)
    {
        if (eventData.Entity is not ISoftDelete softDeletedEntity)
        {
            return;
        }

        if (!softDeletedEntity.IsDeleted)
        {
            return;
        }

        try
        {
            await _itemMeasuremetnDetailRepository.DeleteManyAsync(await _itemMeasuremetnDetailRepository.GetListByItemMessurementIdAsync(eventData.Entity.Id));

        }
        catch
        {
            //...
        }
    }
}