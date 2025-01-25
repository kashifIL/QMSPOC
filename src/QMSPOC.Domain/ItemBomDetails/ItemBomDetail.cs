using QMSPOC.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace QMSPOC.ItemBomDetails
{
    public class ItemBomDetail : Entity<Guid>, IMultiTenant
    {
        public virtual Guid ItemBomId { get; set; }

        public virtual Guid? TenantId { get; set; }

        public virtual decimal Qty { get; set; }

        [CanBeNull]
        public virtual string? Uom { get; set; }
        public Guid ItemId { get; set; }

        protected ItemBomDetail()
        {

        }

        public ItemBomDetail(Guid id, Guid itemBomId, Guid itemId, decimal qty, string? uom = null)
        {

            Id = id;
            ItemBomId = itemBomId;
            Qty = qty;
            Uom = uom;
            ItemId = itemId;
        }

    }
}