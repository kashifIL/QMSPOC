using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemCategories
{
    public class GetItemCategoriesInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }

        public GetItemCategoriesInput()
        {

        }
    }
}