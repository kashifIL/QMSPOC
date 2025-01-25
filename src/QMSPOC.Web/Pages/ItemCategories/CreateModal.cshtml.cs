using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMSPOC.ItemCategories;

namespace QMSPOC.Web.Pages.ItemCategories
{
    public class CreateModalModel : QMSPOCPageModel
    {

        [BindProperty]
        public ItemCategoryCreateViewModel ItemCategory { get; set; }

        protected IItemCategoriesAppService _itemCategoriesAppService;

        public CreateModalModel(IItemCategoriesAppService itemCategoriesAppService)
        {
            _itemCategoriesAppService = itemCategoriesAppService;

            ItemCategory = new();
        }

        public virtual async Task OnGetAsync()
        {
            ItemCategory = new ItemCategoryCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _itemCategoriesAppService.CreateAsync(ObjectMapper.Map<ItemCategoryCreateViewModel, ItemCategoryCreateDto>(ItemCategory));
            return NoContent();
        }
    }

    public class ItemCategoryCreateViewModel : ItemCategoryCreateDto
    {
    }
}