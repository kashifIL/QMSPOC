using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemMessurements
{
    public class GetItemMessurementsInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Version { get; set; }
        public Guid? ItemId { get; set; }

        public GetItemMessurementsInput()
        {

        }
    }
}