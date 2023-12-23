using ConsoleApp1.Attribute;
using ConsoleApp1.Models;
using Dapper;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace ConsoleApp1.Controllers;

public class FormController : ControllerBase
{
    private const string ConnectionString = "Server=localhost;Database=stackoverflow;Trusted_Connection=True;";

    [HttpGet("GetAll")]
    public async Task GetAllAsync()
    {
        using var writer = new StreamWriter(HttpContext.Response.OutputStream);
        using var connection = new SqlConnection(ConnectionString);
        var forms = await connection.QueryAsync<Form>("select * from Forms");
        var formjson = JsonSerializer.Serialize(forms);
        await writer.WriteLineAsync(formjson);
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetByIdAsync()
    {
        using var writer = new StreamWriter(HttpContext.Response.OutputStream);
        using var connection = new SqlConnection(ConnectionString);
        var id = HttpContext.Request.QueryString["id"];
        if (id == null || int.TryParse(id, out int productIdToGet) == false)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        var form = await connection.QueryFirstOrDefaultAsync<Form>("select * from Forms where Id = @Id", new { Id = id });
        if (form is null)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        var formjson = JsonSerializer.Serialize(form);
        await writer.WriteLineAsync(formjson);

        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPost("Create")]
    public async Task PostFormAsync()
    {
        using var reader = new StreamReader(HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var formtoadd = JsonSerializer.Deserialize<Form>(json);

        if (formtoadd == null || string.IsNullOrWhiteSpace(formtoadd.Name) || string.IsNullOrWhiteSpace(formtoadd.Description))
        {
             HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var products = await connection.ExecuteAsync(
            @"insert into Forms ([Name], [Description],[Like],[Dislike]) 
            values(@Name, @Description,@Like,@Dislike)",
            param: new
            {
                formtoadd.Name,
                formtoadd.Description,
                formtoadd.Like,
                formtoadd.Dislike,
            });

        HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteFormAsync()
    {
        var IdToDelete = HttpContext.Request.QueryString["id"];

        if (IdToDelete == null || int.TryParse(IdToDelete, out int IdToDeleteForm) == false)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Forms
        where Id = @Id",
            param: new
            {
                Id = IdToDeleteForm,
            });

        if (deletedRowsCount == 0)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutProductAsync()
    {
        var IdToUpdate = HttpContext.Request.QueryString["id"];

        if (IdToUpdate == null || int.TryParse(IdToUpdate, out int formidtoupdate) == false)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var formToupdate = JsonSerializer.Deserialize<Form>(json);

        if (formToupdate == null || string.IsNullOrEmpty(formToupdate.Description) || string.IsNullOrEmpty(formToupdate.Name))
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Forms
        set Name = @Name, Description = @Description, [Like] = @Like,[Dislike] = @Dislike
        where Id = @Id",
            param: new
            {
                formToupdate.Name,
                formToupdate.Description,
                formToupdate.Like,
                formToupdate.Dislike,
                Id = formidtoupdate,
            });

        if (affectedRowsCount == 0)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}
