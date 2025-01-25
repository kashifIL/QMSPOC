using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using QMSPOC.ItemCategories;
using QMSPOC.Shared;

namespace QMSPOC.Web.Pages.ItemCategories
{
    public class IndexModel : AbpPageModel
    {
        public string? CodeFilter { get; set; }
        public string? NameFilter { get; set; }

        protected IItemCategoriesAppService _itemCategoriesAppService;

        public IndexModel(IItemCategoriesAppService itemCategoriesAppService)
        {
            _itemCategoriesAppService = itemCategoriesAppService;
        }

        public virtual async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}