using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace QMSPOC.Web.Controllers.ItemBoms;

[Route("[controller]/[action]")]
public class ItemBomsController : AbpController
{
    [HttpGet]
    public virtual async Task<PartialViewResult> ChildDataGrid(Guid itemBomId)
    {
        return PartialView("~/Pages/Shared/ItemBoms/_ChildDataGrids.cshtml", itemBomId);
    }
}