using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemBomDetails
{
    public class GetItemBomDetailListInput : PagedAndSortedResultRequestDto
    {
        public Guid ItemBomId { get; set; }
    }
}