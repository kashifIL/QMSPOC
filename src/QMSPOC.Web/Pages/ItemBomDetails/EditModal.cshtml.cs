using QMSPOC.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using QMSPOC.ItemBomDetails;

namespace QMSPOC.Web.Pages.ItemBomDetails
{
    public class EditModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ItemBomDetailUpdateViewModel ItemBomDetail { get; set; }

        public List<SelectListItem> ItemLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemBomDetailsAppService _itemBomDetailsAppService;

        public EditModalModel(IItemBomDetailsAppService itemBomDetailsAppService)
        {
            _itemBomDetailsAppService = itemBomDetailsAppService;

            ItemBomDetail = new();
        }

        public virtual async Task OnGetAsync()
        {
            var itemBomDetailWithNavigationPropertiesDto = await _itemBomDetailsAppService.GetWithNavigationPropertiesAsync(Id);
            ItemBomDetail = ObjectMapper.Map<ItemBomDetailDto, ItemBomDetailUpdateViewModel>(itemBomDetailWithNavigationPropertiesDto.ItemBomDetail);

            ItemLookupListRequired.AddRange((
                                    await _itemBomDetailsAppService.GetItemLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _itemBomDetailsAppService.UpdateAsync(Id, ObjectMapper.Map<ItemBomDetailUpdateViewModel, ItemBomDetailUpdateDto>(ItemBomDetail));
            return NoContent();
        }
    }

    public class ItemBomDetailUpdateViewModel : ItemBomDetailUpdateDto
    {
    }
}