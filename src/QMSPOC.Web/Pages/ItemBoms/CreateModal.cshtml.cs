using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMSPOC.ItemBoms;

namespace QMSPOC.Web.Pages.ItemBoms
{
    public class CreateModalModel : QMSPOCPageModel
    {

        [BindProperty]
        public ItemBomCreateViewModel ItemBom { get; set; }

        public List<SelectListItem> ItemLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemBomsAppService _itemBomsAppService;

        public CreateModalModel(IItemBomsAppService itemBomsAppService)
        {
            _itemBomsAppService = itemBomsAppService;

            ItemBom = new();
        }

        public virtual async Task OnGetAsync()
        {
            ItemBom = new ItemBomCreateViewModel();
            ItemLookupListRequired.AddRange((
                                    await _itemBomsAppService.GetItemLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _itemBomsAppService.CreateAsync(ObjectMapper.Map<ItemBomCreateViewModel, ItemBomCreateDto>(ItemBom));
            return NoContent();
        }
    }

    public class ItemBomCreateViewModel : ItemBomCreateDto
    {
    }
}