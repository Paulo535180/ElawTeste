using Microsoft.EntityFrameworkCore;
using WebCrawler.Models;

namespace ControleEstoque.API.Data
{
  public class ContextoBanco : DbContext
  {
    public ContextoBanco(DbContextOptions<ContextoBanco> options) : base(options)
    {
    }    

    public ContextoBanco CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ContextoBanco>();
      optionsBuilder.UseSqlServer("Data Source=GAMING3I-PAULO;Initial Catalog=LocalDb01;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

      return new ContextoBanco(optionsBuilder.Options);
    }

    public DbSet<ProxyServer> ProxyServer { get; set; }
    public DbSet<LogProxyServer> LogProxyServer { get; set; }

  }
}
