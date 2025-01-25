using QMSPOC.ItemCategories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace QMSPOC.Items
{
    public class Item : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string Code { get; set; }

        [NotNull]
        public virtual string Description { get; set; }
        public Guid ItemCategoryId { get; set; }

        protected Item()
        {

        }

        public Item(Guid id, Guid itemCategoryId, string code, string description)
        {

            Id = id;
            Check.NotNull(code, nameof(code));
            Check.NotNull(description, nameof(description));
            Code = code;
            Description = description;
            ItemCategoryId = itemCategoryId;
        }

    }
}