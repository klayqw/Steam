using Steam.Dto;
using Steam.Models;

namespace Steam.ViewModel;

public class WorkShopToAddViewModel
{
    public WorkShopDto WorkShopDto { get; set; }
    public IEnumerable<Game> Games { get; set; }
}
