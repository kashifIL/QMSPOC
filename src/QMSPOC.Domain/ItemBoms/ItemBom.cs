using QMSPOC.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using QMSPOC.ItemBomDetails;

using Volo.Abp;

namespace QMSPOC.ItemBoms
{
    public class ItemBom : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string Code { get; set; }

        public virtual int Version { get; set; }

        [CanBeNull]
        public virtual string? Description { get; set; }
        public Guid ItemId { get; set; }
        public ICollection<ItemBomDetail> ItemBomDetails { get; private set; }

        protected ItemBom()
        {

        }

        public ItemBom(Guid id, Guid itemId, string code, int version, string? description = null)
        {

            Id = id;
            Check.NotNull(code, nameof(code));
            Code = code;
            Version = version;
            Description = description;
            ItemId = itemId;
            ItemBomDetails = new Collection<ItemBomDetail>();
        }

    }
}