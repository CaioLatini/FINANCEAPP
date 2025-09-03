namespace FINANCEAPP.Core.Data
{
    public static class Dicionarios
    {
        public static readonly HashSet<string> PalavrasEntrada = new(StringComparer.OrdinalIgnoreCase)
            { "ganhei", "recebi", "+", "entrada", "salario", "ganho", "extra", "reembolso" };

        public static readonly HashSet<string> FormatosPagamento = new(StringComparer.OrdinalIgnoreCase)
            { "pix", "cartao", "dinheiro", "debito", "credito", "vale" };

        public static readonly Dictionary<string, string[]> Categorias = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Moradia"] = new[] { "aluguel", "condominio", "iptu", "luz", "energia", "agua", "gas", "imobiliaria", "reforma", "faxina" },
            ["Alimentacao"] = new[] { "mercado", "supermercado", "padaria", "marmita", "ifood", "restaurante", "lanche", "acougue", "hortifruti", "pizza", "comida", "bebida", "almoco", "janta", "outback" },
            ["Transporte"] = new[] { "uber", "99", "onibus", "metro", "combustivel", "gasolina", "etanol", "ipva", "oficina", "manutencao", "carro", "moto", "pneu", "oleo", "lavagem", "estacionamento", "multa" },
            ["Educacao"] = new[] { "escola", "faculdade", "curso", "apostila", "material", "livro", "mensalidade", "creche", "universidade", "mochila", "rematricula" },
            ["Saude"] = new[] { "remedio", "farmacia", "dentista", "consulta", "plano", "hospital", "vacina", "exame", "laboratorio", "oculos", "medicamento", "clinica" },
            ["Contas"] = new[] { "internet", "celular", "telefone", "tv", "assinatura", "netflix", "prime", "spotify", "claro", "vivo", "tim", "oi", "banco", "boleto", "cemig" },
            ["Lazer"] = new[] { "cinema", "viagem", "passeio", "churrasco", "balada", "show", "evento", "academia", "jogo", "estetica", "salao", "beleza", "esporte", "parque", "cerveja", "sorvete", "bebida", "pelada", "pizza", "doce", "presente", "restaurante" },
            ["Outros"] = new[] { "diverso", "imprevisto", "emergencia", "despesa", "servico", "presente", "roupa", "cabelo", "doacao" }
        };
    }
}