using QMSPOC.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using QMSPOC.Items;

namespace QMSPOC.Web.Pages.Items
{
    public class EditModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ItemUpdateViewModel Item { get; set; }

        public List<SelectListItem> ItemCategoryLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemsAppService _itemsAppService;

        public EditModalModel(IItemsAppService itemsAppService)
        {
            _itemsAppService = itemsAppService;

            Item = new();
        }

        public virtual async Task OnGetAsync()
        {
            var itemWithNavigationPropertiesDto = await _itemsAppService.GetWithNavigationPropertiesAsync(Id);
            Item = ObjectMapper.Map<ItemDto, ItemUpdateViewModel>(itemWithNavigationPropertiesDto.Item);

            ItemCategoryLookupListRequired.AddRange((
                                    await _itemsAppService.GetItemCategoryLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _itemsAppService.UpdateAsync(Id, ObjectMapper.Map<ItemUpdateViewModel, ItemUpdateDto>(Item));
            return NoContent();
        }
    }

    public class ItemUpdateViewModel : ItemUpdateDto
    {
    }
}