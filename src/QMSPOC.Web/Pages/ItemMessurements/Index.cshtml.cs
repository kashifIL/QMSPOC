using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using QMSPOC.ItemMessurements;
using QMSPOC.Shared;

namespace QMSPOC.Web.Pages.ItemMessurements
{
    public class IndexModel : AbpPageModel
    {
        public string? CodeFilter { get; set; }
        public string? VersionFilter { get; set; }
        [SelectItems(nameof(ItemLookupList))]
        public Guid ItemIdFilter { get; set; }
        public List<SelectListItem> ItemLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        protected IItemMessurementsAppService _itemMessurementsAppService;

        public IndexModel(IItemMessurementsAppService itemMessurementsAppService)
        {
            _itemMessurementsAppService = itemMessurementsAppService;
        }

        public virtual async Task OnGetAsync()
        {
            ItemLookupList.AddRange((
                    await _itemMessurementsAppService.GetItemLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            await Task.CompletedTask;
        }
    }
}