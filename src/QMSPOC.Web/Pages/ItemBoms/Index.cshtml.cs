using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using QMSPOC.ItemBoms;
using QMSPOC.Shared;

namespace QMSPOC.Web.Pages.ItemBoms
{
    public class IndexModel : AbpPageModel
    {
        public string? CodeFilter { get; set; }
        public int? VersionFilterMin { get; set; }

        public int? VersionFilterMax { get; set; }
        public string? DescriptionFilter { get; set; }
        [SelectItems(nameof(ItemLookupList))]
        public Guid ItemIdFilter { get; set; }
        public List<SelectListItem> ItemLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        protected IItemBomsAppService _itemBomsAppService;

        public IndexModel(IItemBomsAppService itemBomsAppService)
        {
            _itemBomsAppService = itemBomsAppService;
        }

        public virtual async Task OnGetAsync()
        {
            ItemLookupList.AddRange((
                    await _itemBomsAppService.GetItemLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            await Task.CompletedTask;
        }
    }
}