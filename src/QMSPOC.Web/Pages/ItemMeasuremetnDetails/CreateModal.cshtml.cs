using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMSPOC.ItemMeasuremetnDetails;

namespace QMSPOC.Web.Pages.ItemMeasuremetnDetails
{
    public class CreateModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid ItemMessurementId { get; set; }

        [BindProperty]
        public ItemMeasuremetnDetailCreateViewModel ItemMeasuremetnDetail { get; set; }

        protected IItemMeasuremetnDetailsAppService _itemMeasuremetnDetailsAppService;

        public CreateModalModel(IItemMeasuremetnDetailsAppService itemMeasuremetnDetailsAppService)
        {
            _itemMeasuremetnDetailsAppService = itemMeasuremetnDetailsAppService;

            ItemMeasuremetnDetail = new();
        }

        public virtual async Task OnGetAsync()
        {
            ItemMeasuremetnDetail = new ItemMeasuremetnDetailCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            ItemMeasuremetnDetail.ItemMessurementId = ItemMessurementId;
            await _itemMeasuremetnDetailsAppService.CreateAsync(ObjectMapper.Map<ItemMeasuremetnDetailCreateViewModel, ItemMeasuremetnDetailCreateDto>(ItemMeasuremetnDetail));
            return NoContent();
        }
    }

    public class ItemMeasuremetnDetailCreateViewModel : ItemMeasuremetnDetailCreateDto
    {
    }
}