using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Steam.Data;
using Steam.Models;
using Steam.Services;

namespace UnitTest;

public class GameServiceTest
{
    [Fact]
    public void GetById_SendNegativeId_ThrowArgumentOutOfRangeException()
    {
        var sqldbcontextmock = new Mock<SteamDBContext>();        
        var gameservice = new GameService(null);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await gameservice.GetById(-1);
        });
    }
    [Fact]
    public void DeleteFromLibary_SendNegativeId_ThrowArgumentOutOfRangeException()
    {
        var sqldbcontextmock = new Mock<SteamDBContext>();
        var gameservice = new GameService(null);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await gameservice.DeleteFromLibary(-1,"test");
        });
    }

    [Fact]
    public void Update_SendNegativeId_ThrowArgumentOutOfRangeException()
    {
        var sqldbcontextmock = new Mock<SteamDBContext>();
        var gameservice = new GameService(null);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await gameservice.Update(-1, null);
        });
    }

}