using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class ItemMeasuremetnDetail : Entity<Guid>, IMultiTenant
    {
        public virtual Guid ItemMessurementId { get; set; }

        public virtual Guid? TenantId { get; set; }

        [CanBeNull]
        public virtual string? Type { get; set; }

        public virtual decimal Value { get; set; }

        [CanBeNull]
        public virtual string? Uom { get; set; }

        protected ItemMeasuremetnDetail()
        {

        }

        public ItemMeasuremetnDetail(Guid id, Guid itemMessurementId, decimal value, string? type = null, string? uom = null)
        {

            Id = id;
            ItemMessurementId = itemMessurementId;
            Value = value;
            Type = type;
            Uom = uom;
        }

    }
}