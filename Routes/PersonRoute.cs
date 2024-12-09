using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

namespace Person.Routes
{
    public static class PersonRoute
    {
        // Extensão para registrar rotas relacionadas à entidade "Person"
        public static void PersonRoutes(this WebApplication app)
        {
            // Define o prefixo "person" para as rotas
            var route = app.MapGroup("person");

            // Rota POST: Cria uma nova pessoa no banco de dados
            route.MapPost("", async (PersonRequest req, PersonContext context, CancellationToken ct) =>
            {
                var person = new PersonModel(req.name);
                await context.AddAsync(person); 
                await context.SaveChangesAsync(); 
            });

            // Rota GET: Retorna a lista de todas as pessoas
            route.MapGet("", async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync(); 
                return Results.Ok(people); 
            });

            // Rota PUT: Atualiza o nome de uma pessoa com base no ID fornecido
            route.MapPut("{id:guid}", async (Guid Id, PersonRequest req, PersonContext context, CancellationToken ct) =>
            {
                var person = await context.People.FindAsync(Id); 

                if (person == null) 
                    return Results.NotFound(); 

                person.ChangeName(req.name); 
                await context.SaveChangesAsync(); 

                return Results.Ok(person); 
            });

            // Rota DELETE: Marca uma pessoa como inativa com base no ID fornecido
            route.MapDelete("{id:guid}", async (Guid id, PersonContext context, CancellationToken ct) =>
            {
                var person = await context.People.FindAsync(id); 

                if (person == null) 
                    return Results.NotFound(); 

                person.SetInactive(); 
                await context.SaveChangesAsync(); 
                return Results.Ok(person); 
            });
        }
    }
}
