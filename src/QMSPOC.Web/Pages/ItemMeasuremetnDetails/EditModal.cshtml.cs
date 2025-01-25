using QMSPOC.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using QMSPOC.ItemMeasuremetnDetails;

namespace QMSPOC.Web.Pages.ItemMeasuremetnDetails
{
    public class EditModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ItemMeasuremetnDetailUpdateViewModel ItemMeasuremetnDetail { get; set; }

        protected IItemMeasuremetnDetailsAppService _itemMeasuremetnDetailsAppService;

        public EditModalModel(IItemMeasuremetnDetailsAppService itemMeasuremetnDetailsAppService)
        {
            _itemMeasuremetnDetailsAppService = itemMeasuremetnDetailsAppService;

            ItemMeasuremetnDetail = new();
        }

        public virtual async Task OnGetAsync()
        {
            var itemMeasuremetnDetail = await _itemMeasuremetnDetailsAppService.GetAsync(Id);
            ItemMeasuremetnDetail = ObjectMapper.Map<ItemMeasuremetnDetailDto, ItemMeasuremetnDetailUpdateViewModel>(itemMeasuremetnDetail);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _itemMeasuremetnDetailsAppService.UpdateAsync(Id, ObjectMapper.Map<ItemMeasuremetnDetailUpdateViewModel, ItemMeasuremetnDetailUpdateDto>(ItemMeasuremetnDetail));
            return NoContent();
        }
    }

    public class ItemMeasuremetnDetailUpdateViewModel : ItemMeasuremetnDetailUpdateDto
    {
    }
}