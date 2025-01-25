using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMSPOC.Items;

namespace QMSPOC.Web.Pages.Items
{
    public class CreateModalModel : QMSPOCPageModel
    {

        [BindProperty]
        public ItemCreateViewModel Item { get; set; }

        public List<SelectListItem> ItemCategoryLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemsAppService _itemsAppService;

        public CreateModalModel(IItemsAppService itemsAppService)
        {
            _itemsAppService = itemsAppService;

            Item = new();
        }

        public virtual async Task OnGetAsync()
        {
            Item = new ItemCreateViewModel();
            ItemCategoryLookupListRequired.AddRange((
                                    await _itemsAppService.GetItemCategoryLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _itemsAppService.CreateAsync(ObjectMapper.Map<ItemCreateViewModel, ItemCreateDto>(Item));
            return NoContent();
        }
    }

    public class ItemCreateViewModel : ItemCreateDto
    {
    }
}