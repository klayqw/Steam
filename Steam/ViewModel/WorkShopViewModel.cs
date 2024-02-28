using Steam.Models;

namespace Steam.ViewModel;

public class WorkShopViewModel
{
    public WorkShop item { get; set; }
    public IEnumerable<WorkShop> workShopUserItems { get; set; }
}
