using QMSPOC.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using QMSPOC.ItemCategories;

namespace QMSPOC.Web.Pages.ItemCategories
{
    public class EditModalModel : QMSPOCPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ItemCategoryUpdateViewModel ItemCategory { get; set; }

        protected IItemCategoriesAppService _itemCategoriesAppService;

        public EditModalModel(IItemCategoriesAppService itemCategoriesAppService)
        {
            _itemCategoriesAppService = itemCategoriesAppService;

            ItemCategory = new();
        }

        public virtual async Task OnGetAsync()
        {
            var itemCategory = await _itemCategoriesAppService.GetAsync(Id);
            ItemCategory = ObjectMapper.Map<ItemCategoryDto, ItemCategoryUpdateViewModel>(itemCategory);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _itemCategoriesAppService.UpdateAsync(Id, ObjectMapper.Map<ItemCategoryUpdateViewModel, ItemCategoryUpdateDto>(ItemCategory));
            return NoContent();
        }
    }

    public class ItemCategoryUpdateViewModel : ItemCategoryUpdateDto
    {
    }
}