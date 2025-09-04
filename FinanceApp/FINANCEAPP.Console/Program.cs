// Importa os nossos modelos e serviços do projeto Core
using FINANCEAPP.Core.Models;
using FINANCEAPP.Core.Services;
using System.IO; // Adicione esta linha para usar Path e Directory

// --- 1. Configuração ---
string caminhoArquivoEntrada = string.Empty;
string caminhoArquivoSaida = string.Empty;

Console.WriteLine("--- Iniciando Processador de Finanças ---");

// --- 2. Instância dos Serviços ---
var leitorService = new LeitorDeChatService();
var interpretadorService = new InterpretadorDeTransacaoService();
var escritorService = new EscritorDeRelatorioService();

// --- 3. Execução (Orquestração) ---

// ETAPA 1: LER O ARQUIVO DE CHAT
// (A sua lógica para ler o arquivo de entrada está boa, vamos mantê-la por enquanto)
do
{
    Console.WriteLine("\nInforme o caminho completo do arquivo de chat (.txt):");
    caminhoArquivoEntrada = Console.ReadLine()?.Trim('"');

    if (!File.Exists(caminhoArquivoEntrada))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Erro: O arquivo não foi encontrado no caminho especificado.");
        Console.ResetColor();
        caminhoArquivoEntrada = null; // Força o loop a continuar
    }

} while (string.IsNullOrEmpty(caminhoArquivoEntrada));

var resultadoLeitura = leitorService.LerLinhas(caminhoArquivoEntrada);

if (!resultadoLeitura.Sucesso)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERRO: {resultadoLeitura.Mensagem}");
    Console.ResetColor();
    return;
}

// ETAPA 2: INTERPRETAR CADA LINHA E CRIAR A LISTA DE TRANSAÇÕES
Console.WriteLine("Arquivo lido com sucesso. Interpretando transações...");
var transacoes = new List<Transacao>();
var linhas = resultadoLeitura.Dados!;

foreach (var linha in linhas)
{
    var resultadoInterpretacao = interpretadorService.Interpretar(linha);
    if (resultadoInterpretacao.Sucesso)
    {
        transacoes.Add(resultadoInterpretacao.Dados!);
    }
}

Console.WriteLine($"{transacoes.Count} transações válidas encontradas.");

// ETAPA 3: ESCREVER O RELATÓRIO FINAL EM CSV
if (transacoes.Count == 0)
{
    Console.WriteLine("Nenhuma transação para exportar. Encerrando.");
    return;
}

// --- LOOP CORRIGIDO PARA O CAMINHO DE SAÍDA ---
string diretorioSaida;
do
{
    Console.WriteLine("\nInforme a PASTA onde você quer salvar o relatório (ex: C:\\Users\\SeuNome\\Desktop):");
    diretorioSaida = Console.ReadLine()?.Trim('"');

    // Valida se o diretório (pasta) informado realmente existe.
    if (!Directory.Exists(diretorioSaida))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Erro: A pasta informada não existe. Por favor, tente novamente.");
        Console.ResetColor();
        diretorioSaida = null; // Limpa a variável para o loop continuar
    }

} while (string.IsNullOrEmpty(diretorioSaida));

// AQUI ESTÁ A CORREÇÃO: Juntamos a pasta com o nome do arquivo
string nomeArquivo = "relatorio_financeiro.csv";
caminhoArquivoSaida = Path.Combine(diretorioSaida, nomeArquivo);

Console.WriteLine($"O relatório será salvo em: {caminhoArquivoSaida}");

var resultadoEscrita = escritorService.EscreverCsv(transacoes, caminhoArquivoSaida);

if (!resultadoEscrita.Sucesso)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERRO: {resultadoEscrita.Mensagem}");
    Console.ResetColor();
    return;
}

// FIM: SUCESSO!
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Relatório gerado com sucesso!");
Console.ResetColor();