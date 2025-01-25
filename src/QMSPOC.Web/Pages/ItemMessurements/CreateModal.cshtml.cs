using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMSPOC.ItemMessurements;

namespace QMSPOC.Web.Pages.ItemMessurements
{
    public class CreateModalModel : QMSPOCPageModel
    {

        [BindProperty]
        public ItemMessurementCreateViewModel ItemMessurement { get; set; }

        public List<SelectListItem> ItemLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemMessurementsAppService _itemMessurementsAppService;

        public CreateModalModel(IItemMessurementsAppService itemMessurementsAppService)
        {
            _itemMessurementsAppService = itemMessurementsAppService;

            ItemMessurement = new();
        }

        public virtual async Task OnGetAsync()
        {
            ItemMessurement = new ItemMessurementCreateViewModel();
            ItemLookupListRequired.AddRange((
                                    await _itemMessurementsAppService.GetItemLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _itemMessurementsAppService.CreateAsync(ObjectMapper.Map<ItemMessurementCreateViewModel, ItemMessurementCreateDto>(ItemMessurement));
            return NoContent();
        }
    }

    public class ItemMessurementCreateViewModel : ItemMessurementCreateDto
    {
    }
}