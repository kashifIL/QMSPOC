using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace QMSPOC.ItemCategories
{
    public class ItemCategoryDto : EntityDto<Guid>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

    }
}