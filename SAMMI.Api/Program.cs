using Microsoft.IdentityModel.Tokens;
using SAMMI.CrossCutting;
using SAMMI.Data.Context;
using SAMMI.Domain.DTOs.Pontuacao.Request;
using SAMMI.Domain.DTOs.Pontuacao.Response;
using SAMMI.Domain.DTOs.Usuario.Request;
using SAMMI.Domain.DTOs.Usuario.Response;
using SAMMI.Domain.Entities;
using SAMMI.Domain.Enumerators;
using SAMMI.Domain.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
Guid usuarioLogadoId;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

builder.Services.AddDbContext<SAMMIContext>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});    

builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthenticateSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

#region Endpoints de Usuários

app.MapGet("/usuario", (SAMMIContext context) =>
{
    var usuarios = context.UsuariosSet.Select(usuario => new UsuarioListarResponse
    {
        Id = usuario.Id,
        Nome = usuario.Nome
    });

    return Results.Ok(usuarios);
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para obter todos os usuários cadastrados";
        operation.Summary = "Listar todos os Usuários";
        return operation;
    })
    .WithTags("Usuários")
    .RequireAuthorization();

app.MapGet("/usuario/{usuarioId}", (SAMMIContext context, Guid usuarioId) =>
{
    var usuario = context.UsuariosSet.Find(usuarioId);
    if (usuario is null)
        return Results.BadRequest("Usuário não Localizado.");

    var usuarioDto = new UsuarioObterResponse
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        EmailLogin = usuario.EmailLogin
    };

    return Results.Ok(usuarioDto);
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para obter um usuário com base no ID informado";
        operation.Summary = "Obter um Usuário";
        operation.Parameters[0].Description = "Id do Usuário";
        return operation;
    })
    .WithTags("Usuários")
    .RequireAuthorization();

app.MapPost("/usuario", (SAMMIContext context, UsuarioAdicionarRequest usuarioAdicionarRequest) =>
{
    try
    {
        if (usuarioAdicionarRequest.EmailLogin != usuarioAdicionarRequest.EmailLoginConfirmacao)
            return Results.BadRequest("Email de Login não Confere.");

        if (usuarioAdicionarRequest.Senha != usuarioAdicionarRequest.SenhaConfirmacao)
            return Results.BadRequest("Senha não Confere.");

        if (context.UsuariosSet.Any(p => p.EmailLogin == usuarioAdicionarRequest.EmailLogin))
            return Results.BadRequest("Email já utilizado para Login em outro Usuário.");

        var usuario = new Usuario(
            usuarioAdicionarRequest.Nome,
            usuarioAdicionarRequest.EmailLogin,
            usuarioAdicionarRequest.Senha.EncryptPassword());

        context.UsuariosSet.Add(usuario);
        context.SaveChanges();

        return Results.Created("Created", $"Usuário Registrado com Sucesso. {usuario.Id}");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Cadastrar um Usuário";
        operation.Summary = "Novo Usuário";
        return operation;
    })
    .WithTags("Usuários");
//.RequireAuthorization();

app.MapPut("/usuario/alterar-senha", (SAMMIContext context, UsuarioAtualizarRequest usuarioAtualizarRequest) =>
{
    try
    {
        var usuario = context.UsuariosSet.Find(usuarioAtualizarRequest.Id);
        if (usuario is null)
            return Results.BadRequest("Usuário não Localizado");

        if (usuarioAtualizarRequest.SenhaAtual.EncryptPassword() == usuario.Senha)
        {
            usuario.AlterarSenha(usuarioAtualizarRequest.SenhaNova.EncryptPassword());
            context.UsuariosSet.Update(usuario);
            context.SaveChanges();

            return Results.Ok("Senha Alterada com Sucesso.");
        }

        return Results.BadRequest("Ocorreu um Problema ao Alterar a Senha do Usuário.");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Alterar a Senha do Usuário";
        operation.Summary = "Alterar Senha";
        return operation;
    })
    .WithTags("Usuários")
    .RequireAuthorization();

#endregion

#region Endpoints de Pontuacao
app.MapPost("/pontuacao", (SAMMIContext context, ClaimsPrincipal user, CriarPontuacaoRequest request) =>
{
    try
    {
        SetarDadosToken(user);

        //TODO: pegar o id do usuario do token e remover do request
        var pontuacao = new Pontuacao(usuarioLogadoId, request.Nivel, request.TipoJogo, request.QuantidadePontuacao);

        context.PontuacaoSet.Add(pontuacao);
        context.SaveChanges();

        return Results.Created("Created", $"Jogo Registrado com Sucesso. {pontuacao.Id}");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
}
).WithOpenApi(operation =>
{
    operation.Description = "Endpoint para Iniciar um Jogo";
    operation.Summary = "Novo Jogo";
    return operation;
}
).WithTags("Pontuação")
.RequireAuthorization();

app.MapGet("/Pontuacao", (SAMMIContext context, ClaimsPrincipal user) =>
{
    SetarDadosToken(user);

    var pontuacoes = context.PontuacaoSet.Where(p => p.UsuarioId == usuarioLogadoId).Select(p => new ListarPontuacaoResponse
    {
        UsuarioId = p.UsuarioId,
        UsuarioNome = p.Usuario.Nome,
        Data = p.Data,
        Nivel = p.Nivel == SAMMI.Domain.Enumerators.Nivel.SeisAnos ? "Seis Anos" : "Sete Anos",
        TipoJogo = p.TipoJogo == TipoJogo.Adicao ? "Matemática Adição" :
                   p.TipoJogo == TipoJogo.Subtracao ? "Matemática Subtração" :
                   p.TipoJogo == TipoJogo.Multiplicacao ? "Matemática Multiplicação" :
                   p.TipoJogo == TipoJogo.Divisao ? "Matemática Divisão" : "Portugues Sílabas",
        QuantidadePontuacao = p.QuantidadePontos
    }).OrderBy(p => p.Data).ThenBy(p => p.QuantidadePontuacao);

    return Results.Ok(pontuacoes);
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para listar a pontuação do usuário logado";
        operation.Summary = "Listar Pontuações do Usuário";
        return operation;
    })
    .WithTags("Pontuação")
    .RequireAuthorization();

#endregion

#region Autenticação

app.MapPost("/autenticar", (SAMMIContext context, UsuarioAutenticarRequest usuarioAutenticarRequest) =>
{
    var usuario = context.UsuariosSet.FirstOrDefault(p => p.EmailLogin == usuarioAutenticarRequest.EmailLogin && p.Senha == usuarioAutenticarRequest.Senha.EncryptPassword());
    if (usuario is null)
        return Results.BadRequest("Não foi Possível Efetuar o Login.");

    var claims = new[]
    {
            new Claim("UsuarioId", usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

    //Recebe uma instância da Classe SymmetricSecurityKey
    //armazenando a chave de criptografia usada na criação do Token
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{ccdc511d-23f0-4a30-995e-ebc3658e901d}"));

    //Recebe um objeto do tipo SigninCredentials contendo a chave de
    //criptografia e o algoritimo de seguran�a empregados na geração
    //de assinaturas digitais para tokens
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "etec.pwIII",
        audience: "etec.pwIII",
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: creds
    );

    return Results.Ok(new
    {
        UsuarioId = usuario.Id,
        UsuarioNome = usuario.Nome,
        AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
    });
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Autenticar um Usuário na API";
        operation.Summary = "Autenticar Usuário";
        return operation;
    })
    .WithTags("Segurança");

#endregion

app.Run();

#region Métodos de Apoio

void SetarDadosToken(ClaimsPrincipal user)
{
    usuarioLogadoId = Guid.Parse(user.Identities.First().Claims.ToList()[0].Value);
}

#endregion