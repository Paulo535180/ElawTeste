using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
  public class LogProxyServer
  {
    [Key]
    public int Id { get; set; }
    public DateTime? HorarioCriação { get; set; }
    public DateTime? HorarioTermino { get; set; }
    public int NumeroPaginas { get; set; }
    public int QtdLinhas { get; set; }
    public byte[] ArquivoJson { get; set; }
  }
}
