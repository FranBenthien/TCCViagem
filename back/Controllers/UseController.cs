using Microsoft.AspNetCore.Mvc;
using dto;

namespace back.Controllers;

using Services;
using Model;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{   
    [HttpPost("login")]   
    public async Task<IActionResult> Login(
        [FromBody]UsuarioDTO user,
        [FromServices]TokenService service
    )
    {
        using WebSiteViagemContext context = new WebSiteViagemContext();
        
        var possibleUser = context.Usuarios
             .FirstOrDefault(
                 u => u.Email == user.Email);
        
        if (possibleUser == null)
            return this.Ok(new {
                    Message = "Nome de usuário inválido",
                    Status = "Validation Error"
                });

        if (possibleUser.Userpass != user.Password)
            return this.Ok(new {
                    Message = "Senha inválida!",
                    Status = "Validation Error"
                });
        
        var token = await service.CreateToken(possibleUser);
        return this.Ok(new {
                    Message = "Login realizado com sucesso",
                    Status = "Success",
                    Content = token.Value
                });
    }

    [HttpPost("register")]
    public IActionResult Register(
        [FromBody]UsuarioDTO user
        )
    {
        using WebSiteViagemContext context = new WebSiteViagemContext();

        List<string> errors = new List<string>();

        if (user.Email == null)
        {
             errors.Add("Email não foi informado");
        }
                     
        if(user.Name.Length < 5)
        {
             errors.Add("O nome do usuário precisa conter ao menos 5 letras.");
        }

        if (context.Usuarios
            .Any(u => u.UserId == user.UserId))
        {
            errors.Add("Seu nome de usuário já está em uso!");
        }

        if (errors.Count > 0)
        {
             return this.BadRequest(errors);
        }

        Usuario usuario = new Usuario();
        usuario.Name = user.Name;
        usuario.Email = user.Email;        
        usuario.UserId = user.UserId;
        usuario.Userpass = user.Password;                  

        context.Add(usuario);
        context.SaveChanges();
        
        return Ok();
    }


    [HttpPost("update")]
    public IActionResult UpdateName()
    {
        throw new NotImplementedException();

    }

    [HttpPost("formulario")]
    public IActionResult Formulario(
        [FromBody]FormularioDTO form
        )
    {
        using WebSiteViagemContext context = new WebSiteViagemContext();

        List<string> errors = new List<string>();

        if (form.ArrivalDate == null)
        {
             errors.Add("Data não informada");
        }

        if (form.DepartureDate == null)
        {
             errors.Add("Data não informada");
        }


        Formulario formulario = new Formulario();   
        formulario.ArrivalDate = form.ArrivalDate.Value;
        formulario.DepartureDate = form.DepartureDate.Value;
        formulario.TypeHosting = form.TypeHosting;
        formulario.Accommodation = form.Accommodation;
        formulario.Link = form.Link;
        formulario.Food = form.Food;
        formulario.FoodAmount = form.FoodAmount;
        formulario.TypeFood = form.TypeFood;
        formulario.TypeAttraction = form.TypeAttraction;
        formulario.AttractionAmount = form.AttractionAmount;
        formulario.TypeTransport = form.TypeTransport;
        formulario.Comments = form.Comments;
   
                     
        context.Add(formulario);
        context.SaveChanges();

        return Ok();
        
    }

}
