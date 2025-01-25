using QMSPOC.ItemMeasuremetnDetails;
using QMSPOC.ItemMessurements;
using QMSPOC.ItemBomDetails;
using QMSPOC.ItemBoms;
using QMSPOC.Items;
using System;
using QMSPOC.Shared;
using Volo.Abp.AutoMapper;
using QMSPOC.ItemCategories;
using AutoMapper;

namespace QMSPOC;

public class QMSPOCApplicationAutoMapperProfile : Profile
{
    public QMSPOCApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<ItemCategory, ItemCategoryDto>();
        CreateMap<ItemCategory, ItemCategoryExcelDto>();

        CreateMap<Item, ItemDto>();
        CreateMap<Item, ItemExcelDto>();
        CreateMap<ItemWithNavigationProperties, ItemWithNavigationPropertiesDto>();
        CreateMap<ItemCategory, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Code));

        CreateMap<ItemBom, ItemBomDto>();
        CreateMap<ItemBom, ItemBomExcelDto>();
        CreateMap<ItemBomWithNavigationProperties, ItemBomWithNavigationPropertiesDto>();
        CreateMap<Item, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Code));

        CreateMap<ItemBomDetail, ItemBomDetailDto>();
        CreateMap<ItemBomDetailWithNavigationProperties, ItemBomDetailWithNavigationPropertiesDto>();

        CreateMap<ItemMessurement, ItemMessurementDto>();
        CreateMap<ItemMessurement, ItemMessurementExcelDto>();
        CreateMap<ItemMessurementWithNavigationProperties, ItemMessurementWithNavigationPropertiesDto>();

        CreateMap<ItemMeasuremetnDetail, ItemMeasuremetnDetailDto>();
    }
}