using QMSPOC.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using QMSPOC.ItemMeasuremetnDetails;

using Volo.Abp;

namespace QMSPOC.ItemMessurements
{
    public class ItemMessurement : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string Code { get; set; }

        [CanBeNull]
        public virtual string? Version { get; set; }
        public Guid ItemId { get; set; }
        public ICollection<ItemMeasuremetnDetail> ItemMeasuremetnDetails { get; private set; }

        protected ItemMessurement()
        {

        }

        public ItemMessurement(Guid id, Guid itemId, string code, string? version = null)
        {

            Id = id;
            Check.NotNull(code, nameof(code));
            Code = code;
            Version = version;
            ItemId = itemId;
            ItemMeasuremetnDetails = new Collection<ItemMeasuremetnDetail>();
        }

    }
}