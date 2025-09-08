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
            ["Moradia"] = new[] { "aluguel", "condominio", "iptu", "imobiliaria", "reforma", "faxina" },
            ["Alimentacao"] = new[] { "mercado", "supermercado", "padaria", "marmita", "ifood", "restaurante", "lanche", "acougue", "hortifruti", "pizza", "comida", "bebida", "almoco", "janta", "outback" },
            ["Consumo"] = new[]{"luz", "energia", "agua", "gas", "cemig", "internet", "celular", "telefone", "tv", "claro", "vivo", "tim", "oi"},
            ["Transporte"] = new[] { "uber", "99", "onibus", "metro", "combustivel", "gasolina", "etanol", "ipva", "oficina", "manutencao", "carro", "moto", "pneu", "oleo", "lavagem", "estacionamento", "multa" },
            ["Educacao"] = new[] { "escola", "faculdade", "curso", "apostila", "material", "livro", "mensalidade", "creche", "universidade", "mochila", "rematricula" },
            ["Saude"] = new[] { "remedio", "farmacia", "dentista", "consulta", "plano", "hospital", "vacina", "exame", "laboratorio", "oculos", "medicamento", "clinica" },
            ["Lazer"] = new[] { "cinema", "viagem", "passeio", "churrasco", "balada", "show", "evento", "academia", "jogo", "estetica", "salao", "beleza", "esporte", "parque", "cerveja", "sorvete", "bebida", "pelada", "pizza", "doce", "presente", "restaurante","assinatura", "netflix", "prime", "spotify", },
            ["Investimentos"] = new[] { "poupei", "guardei", "investi", "reservei", "investimento" },
            ["Outros"] = new[] { "diverso", "imprevisto", "emergencia", "despesa", "servico", "presente", "roupa", "cabelo", "doacao" }
        };
    }
}