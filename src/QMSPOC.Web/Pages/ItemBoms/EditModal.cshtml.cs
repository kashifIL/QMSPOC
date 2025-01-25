using QMSPOC.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using QMSPOC.ItemBoms;

namespace QMSPOC.Web.Pages.ItemBoms
{
    public class EditModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ItemBomUpdateViewModel ItemBom { get; set; }

        public List<SelectListItem> ItemLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemBomsAppService _itemBomsAppService;

        public EditModalModel(IItemBomsAppService itemBomsAppService)
        {
            _itemBomsAppService = itemBomsAppService;

            ItemBom = new();
        }

        public virtual async Task OnGetAsync()
        {
            var itemBomWithNavigationPropertiesDto = await _itemBomsAppService.GetWithNavigationPropertiesAsync(Id);
            ItemBom = ObjectMapper.Map<ItemBomDto, ItemBomUpdateViewModel>(itemBomWithNavigationPropertiesDto.ItemBom);

            ItemLookupListRequired.AddRange((
                                    await _itemBomsAppService.GetItemLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _itemBomsAppService.UpdateAsync(Id, ObjectMapper.Map<ItemBomUpdateViewModel, ItemBomUpdateDto>(ItemBom));
            return NoContent();
        }
    }

    public class ItemBomUpdateViewModel : ItemBomUpdateDto
    {
    }
}