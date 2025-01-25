using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace QMSPOC.Items
{
    public class ItemDto : EntityDto<Guid>
    {
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ItemCategoryId { get; set; }

    }
}