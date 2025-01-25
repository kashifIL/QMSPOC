using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMSPOC.ItemBomDetails;

namespace QMSPOC.Web.Pages.ItemBomDetails
{
    public class CreateModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid ItemBomId { get; set; }

        [BindProperty]
        public ItemBomDetailCreateViewModel ItemBomDetail { get; set; }

        public List<SelectListItem> ItemLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemBomDetailsAppService _itemBomDetailsAppService;

        public CreateModalModel(IItemBomDetailsAppService itemBomDetailsAppService)
        {
            _itemBomDetailsAppService = itemBomDetailsAppService;

            ItemBomDetail = new();
        }

        public virtual async Task OnGetAsync()
        {
            ItemBomDetail = new ItemBomDetailCreateViewModel();
            ItemLookupListRequired.AddRange((
                                    await _itemBomDetailsAppService.GetItemLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            ItemBomDetail.ItemBomId = ItemBomId;
            await _itemBomDetailsAppService.CreateAsync(ObjectMapper.Map<ItemBomDetailCreateViewModel, ItemBomDetailCreateDto>(ItemBomDetail));
            return NoContent();
        }
    }

    public class ItemBomDetailCreateViewModel : ItemBomDetailCreateDto
    {
    }
}