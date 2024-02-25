using Steam.Models;

namespace Steam.Dto;

public class UserDto
{
    public User user {  get; set; }
    public IEnumerable<Game> games { get; set; }
    public IEnumerable<Group> groups { get; set; }
}
