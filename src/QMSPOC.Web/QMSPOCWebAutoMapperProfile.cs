using QMSPOC.Web.Pages.ItemMeasuremetnDetails;
using QMSPOC.ItemMeasuremetnDetails;
using QMSPOC.Web.Pages.ItemMessurements;
using QMSPOC.ItemMessurements;
using QMSPOC.Web.Pages.ItemBomDetails;
using QMSPOC.ItemBomDetails;
using QMSPOC.Web.Pages.ItemBoms;
using QMSPOC.ItemBoms;
using QMSPOC.Web.Pages.Items;
using QMSPOC.Items;
using QMSPOC.Web.Pages.ItemCategories;
using Volo.Abp.AutoMapper;
using QMSPOC.ItemCategories;
using AutoMapper;

namespace QMSPOC.Web;

public class QMSPOCWebAutoMapperProfile : Profile
{
    public QMSPOCWebAutoMapperProfile()
    {
        //Define your object mappings here, for the Web project

        CreateMap<ItemCategoryDto, ItemCategoryUpdateViewModel>();
        CreateMap<ItemCategoryUpdateViewModel, ItemCategoryUpdateDto>();
        CreateMap<ItemCategoryCreateViewModel, ItemCategoryCreateDto>();

        CreateMap<ItemDto, ItemUpdateViewModel>();
        CreateMap<ItemUpdateViewModel, ItemUpdateDto>();
        CreateMap<ItemCreateViewModel, ItemCreateDto>();

        CreateMap<ItemBomDto, ItemBomUpdateViewModel>();
        CreateMap<ItemBomUpdateViewModel, ItemBomUpdateDto>();
        CreateMap<ItemBomCreateViewModel, ItemBomCreateDto>();

        CreateMap<ItemBomDetailDto, ItemBomDetailUpdateViewModel>();
        CreateMap<ItemBomDetailUpdateViewModel, ItemBomDetailUpdateDto>();
        CreateMap<ItemBomDetailCreateViewModel, ItemBomDetailCreateDto>();

        CreateMap<ItemMessurementDto, ItemMessurementUpdateViewModel>();
        CreateMap<ItemMessurementUpdateViewModel, ItemMessurementUpdateDto>();
        CreateMap<ItemMessurementCreateViewModel, ItemMessurementCreateDto>();

        CreateMap<ItemMeasuremetnDetailDto, ItemMeasuremetnDetailUpdateViewModel>();
        CreateMap<ItemMeasuremetnDetailUpdateViewModel, ItemMeasuremetnDetailUpdateDto>();
        CreateMap<ItemMeasuremetnDetailCreateViewModel, ItemMeasuremetnDetailCreateDto>();
    }
}