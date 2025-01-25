using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace QMSPOC.Web.Controllers.ItemMessurements;

[Route("[controller]/[action]")]
public class ItemMessurementsController : AbpController
{
    [HttpGet]
    public virtual async Task<PartialViewResult> ChildDataGrid(Guid itemMessurementId)
    {
        return PartialView("~/Pages/Shared/ItemMessurements/_ChildDataGrids.cshtml", itemMessurementId);
    }
}