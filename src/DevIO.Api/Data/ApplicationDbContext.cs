using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //Separa os bancos de dados, esse no caso é armazenado dados de autenticação
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}