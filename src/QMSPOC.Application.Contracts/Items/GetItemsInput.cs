using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.Items
{
    public class GetItemsInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Description { get; set; }
        public Guid? ItemCategoryId { get; set; }

        public GetItemsInput()
        {

        }
    }
}