// Importa os nossos modelos e serviços do projeto Core
using FINANCEAPP.Core.Helpers;
using FINANCEAPP.Core.Models;
using FINANCEAPP.Core.Services;

// --- 1. Configuração ---
// Define os caminhos dos arquivos. Mude aqui para os caminhos corretos na sua máquina.
// Dica: Use @ antes da string para não precisar escapar as barras invertidas (ex: @"C:\Users\...")
string caminhoArquivoEntrada = string.Empty;
string caminhoArquivoSaida = string.Empty;

Console.WriteLine("--- Iniciando Processador de Finanças ---");

// --- 2. Instância dos Serviços ---
// Cria os nossos trabalhadores especialistas
var leitorService = new LeitorDeChatService();
var interpretadorService = new InterpretadorDeTransacaoService();
var escritorService = new EscritorDeRelatorioService();

// --- 3. Execução (Orquestração) ---

// ETAPA 1: LER O ARQUIVO DE CHAT
// VALIDA SE O DIRETO É ACESSIVEL E SE O ARQUIVO É VALIDO
do
{
    Console.WriteLine("\nInforme o caminho de diretorio do arquivo txt: ");
    caminhoArquivoEntrada = Console.ReadLine();

} while (ValidacaoArquivoHelper.ValidarDiretorio(caminhoArquivoEntrada).Sucesso &&
        ValidacaoArquivoHelper.ValidarArquivo(caminhoArquivoEntrada).Sucesso);

var resultadoLeitura = leitorService.LerLinhas(caminhoArquivoEntrada);

if (!resultadoLeitura.Sucesso)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERRO: {resultadoLeitura.Mensagem}");
    Console.ResetColor();
    return; // Encerra a aplicação se a leitura falhar
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
        // Se a linha foi interpretada com sucesso, adiciona a transação à lista
        transacoes.Add(resultadoInterpretacao.Dados!);
    }
    // Se não teve sucesso, simplesmente ignora a linha (não é uma transação válida)
}

Console.WriteLine($"{transacoes.Count} transações válidas encontradas.");

// ETAPA 3: ESCREVER O RELATÓRIO FINAL EM CSV
if (transacoes.Count == 0)
{
    Console.WriteLine("Nenhuma transação para exportar. Encerrando.");
    return;
}

Console.WriteLine($"Escrevendo relatório em: {caminhoArquivoSaida}");
var resultadoEscrita = escritorService.EscreverCsv(transacoes, caminhoArquivoSaida);

if (!resultadoEscrita.Sucesso)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"ERRO: {resultadoEscrita.Mensagem}");
    Console.ResetColor();
    return; // Encerra a aplicação se a escrita falhar
}

// FIM: SUCESSO!
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Relatório gerado com sucesso!");
Console.ResetColor();
