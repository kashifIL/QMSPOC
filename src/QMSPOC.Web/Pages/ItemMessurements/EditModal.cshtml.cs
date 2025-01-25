using QMSPOC.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using QMSPOC.ItemMessurements;

namespace QMSPOC.Web.Pages.ItemMessurements
{
    public class EditModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ItemMessurementUpdateViewModel ItemMessurement { get; set; }

        public List<SelectListItem> ItemLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IItemMessurementsAppService _itemMessurementsAppService;

        public EditModalModel(IItemMessurementsAppService itemMessurementsAppService)
        {
            _itemMessurementsAppService = itemMessurementsAppService;

            ItemMessurement = new();
        }

        public virtual async Task OnGetAsync()
        {
            var itemMessurementWithNavigationPropertiesDto = await _itemMessurementsAppService.GetWithNavigationPropertiesAsync(Id);
            ItemMessurement = ObjectMapper.Map<ItemMessurementDto, ItemMessurementUpdateViewModel>(itemMessurementWithNavigationPropertiesDto.ItemMessurement);

            ItemLookupListRequired.AddRange((
                                    await _itemMessurementsAppService.GetItemLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _itemMessurementsAppService.UpdateAsync(Id, ObjectMapper.Map<ItemMessurementUpdateViewModel, ItemMessurementUpdateDto>(ItemMessurement));
            return NoContent();
        }
    }

    public class ItemMessurementUpdateViewModel : ItemMessurementUpdateDto
    {
    }
}