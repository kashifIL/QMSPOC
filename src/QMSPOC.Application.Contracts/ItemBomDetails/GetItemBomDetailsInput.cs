using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemBomDetails
{
    public class GetItemBomDetailsInput : PagedAndSortedResultRequestDto
    {
        public Guid? ItemBomId { get; set; }

        public string? FilterText { get; set; }

        public decimal? QtyMin { get; set; }
        public decimal? QtyMax { get; set; }
        public string? Uom { get; set; }
        public Guid? ItemId { get; set; }

        public GetItemBomDetailsInput()
        {

        }
    }
}