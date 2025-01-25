using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using QMSPOC.Items;
using QMSPOC.Shared;

namespace QMSPOC.Web.Pages.Items
{
    public class IndexModel : AbpPageModel
    {
        public string? CodeFilter { get; set; }
        public string? DescriptionFilter { get; set; }
        [SelectItems(nameof(ItemCategoryLookupList))]
        public Guid ItemCategoryIdFilter { get; set; }
        public List<SelectListItem> ItemCategoryLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        protected IItemsAppService _itemsAppService;

        public IndexModel(IItemsAppService itemsAppService)
        {
            _itemsAppService = itemsAppService;
        }

        public virtual async Task OnGetAsync()
        {
            ItemCategoryLookupList.AddRange((
                    await _itemsAppService.GetItemCategoryLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            await Task.CompletedTask;
        }
    }
}