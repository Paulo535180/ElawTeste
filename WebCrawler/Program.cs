using ControleEstoque.API.Data;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebCrawler.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await RunCrawlerAsync();

        static async Task RunCrawlerAsync()
        {
            //Url do site
            var url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";

            //Horario que iniciou a execução
            DateTime dataInicio = DateTime.Now;

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            //Carrega o documento
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            //Pega a paginação do datatable e seleciona apenas a tag onde existem os links com a numeração das paginas
            var pagination = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='pagination justify-content-end']");
            var pageLinks = pagination.SelectNodes(".//a[@class='page-link']");

            string quantidadePaginas = "";

            //Foreach que percorre as tags <a> e obter o valor delas e quarda na variavel com a quantidade de paginas
            foreach (HtmlNode page in pageLinks)
            {
                string valor = page.InnerText;
                quantidadePaginas = valor;
            }

            //Converte o Número de paginas para Int
            var numPages = int.Parse(quantidadePaginas);
            var tasks = new List<Task>();

            //For enquanto não executar o processamento de todas as paginas
            for (int pageNum = 1; pageNum <= numPages; pageNum++)
            {
                string pageUrl = $"{url}/page/{pageNum}";
                //Cria e chama a task que processa a pagina
                var task = ProcessPageAsync(httpClient, pageUrl, pageNum, dataInicio);
                tasks.Add(task);

                if (tasks.Count >= 3)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("Execução Terminou, verificar o caminho e os arquivos!");
        }

        //Método que processa a pagina web e obtem os valores da table
        static async Task ProcessPageAsync(HttpClient httpClient, string pageUrl, int pageNum, DateTime dataInicio)
        {
            var html = await httpClient.GetStringAsync(pageUrl);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            //Obtem a tabela apenas a tabela
            var proxyTable = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='table table-hover']");
            var rowsDados = htmlDocument.DocumentNode.SelectSingleNode(".//tbody");
            var rows = rowsDados.SelectNodes(".//tr");
            //var rowsAntiga = proxyTable.SelectNodes(".//tr[position() > 1]");


            List<ProxyServer> proxyServers = new List<ProxyServer>();

            foreach (var row in rows)
            {

                var cells = row.SelectNodes(".//td");
                var spanNode = row.SelectSingleNode(".//span[@class='port']");
                var ip = cells[1].InnerText.Trim();
                var port = spanNode.GetAttributeValue("data-port", "");
                var country = cells[3].InnerText.Trim();
                var protocol = cells[6].InnerText.Trim();

                var serverProtocol = new ProxyServer()
                {
                    IPAddress = ip,
                    Port = port,
                    Country = country,
                    Protocol = protocol,
                };
                proxyServers.Add(serverProtocol);
            }
            SaveToJson(proxyServers, pageNum, dataInicio);

        }

        static void SaveToJson(List<ProxyServer> listaProxyServers, int numeroPagina, DateTime dataInicio)
        {
            string json = JsonConvert.SerializeObject(listaProxyServers, Formatting.Indented);
            string nomeArquivo = $"{numeroPagina}";
            string caminho = @"C:\Users\paulo\Documents\ProxysServer\ProxyServerPagina";
            string caminhoFormatado = $"{caminho}{nomeArquivo}.json";
            File.WriteAllText(caminhoFormatado, json);
        }
    }
}