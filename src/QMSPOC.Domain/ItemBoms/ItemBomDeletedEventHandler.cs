using QMSPOC.ItemBomDetails;

using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace QMSPOC.ItemBoms;

public class ItemBomDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<ItemBom>>, ITransientDependency
{
    private readonly IItemBomDetailRepository _itemBomDetailRepository;

    public ItemBomDeletedEventHandler(IItemBomDetailRepository itemBomDetailRepository)
    {
        _itemBomDetailRepository = itemBomDetailRepository;

    }

    public async Task HandleEventAsync(EntityDeletedEventData<ItemBom> eventData)
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
            await _itemBomDetailRepository.DeleteManyAsync(await _itemBomDetailRepository.GetListByItemBomIdAsync(eventData.Entity.Id));

        }
        catch
        {
            //...
        }
    }
}