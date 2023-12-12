# Decisões de design

> [!IMPORTANT]
> DOCUMENTO NÃO FINALIZADO! EM CONSTRUÇÃO

## :book: Conteúdo
- [Decisões de design](#decisões-de-design)
  - [:book: Conteúdo](#book-conteúdo)
  - [:information\_source: Necessidade](#information_source-necessidade)
  - [:thinking: Analisando as possibilidades](#thinking-analisando-as-possibilidades)
    - [:pushpin: O que é uma notificação?](#pushpin-o-que-é-uma-notificação)
    - [:pushpin: Tipos de notificação](#pushpin-tipos-de-notificação)
    - [:pushpin: Lançando notificações durante a execução de métodos](#pushpin-lançando-notificações-durante-a-execução-de-métodos)
    - [:pushpin: Arrays vazios ou referências nulas para array de mensagens?](#pushpin-arrays-vazios-ou-referências-nulas-para-array-de-mensagens)

## :information_source: Necessidade

Durante o processamento de um método (vou tomar a liberdade de me abster de algumas formalidades como ficar se referindo a procedimentos quando não existe retorno ou função como tem retorno, chamarei tudo de método) em sistemas *LOB (Line of business)* nós, com frequência, queremos mais do que um único objeto de retorno. Isso ocorre não porque estamos projetando métodos com múltiplas responsabilidades ferindo o primeiro princípio do SOLID, mas sim, porque tem informações complementares que são importantes para esse tipo de sistema.

<br/>

> [!IMPORTANT]
> Existem vários tipos de sistemas com diferentes propósitos como sistemas para IoT, sistemas de missão crítica, sistemas para baixo nível etc. Esse pacote e toda análise foi pensado em sistemas *LOB (Line of business)*

<br/>

Esses sistemas possuem algumas características comuns. Algumas delas são:
- Autenticar o usuário que está tentando realizar a operação.
- Autorizar o usuário autenticado para a operação que está querendo realizar.
- Receber inputs dos usuários.
- Validar os inputs dos usuários.
- Validar os estados dos objetos de negócio.
- Realizar algum processo de negõcio que modifique o estado dos objetos de negócio.
- Persistir essas informações.
- Retornar o resultado da solicitação para o usuário.
- Exibir diversos relatórios a partir das informações armazenadas.

Ao atender esses cenários, algumas necessidades surgem e são comuns em todo desenvolvimento, independentemente da regra de negócio aplicada. Por exemplo:
- Queremos saber as notificações que ocorreram durante a execução dos métodos.
- Essas notificações são mais do que simples mensagens de erro, podem ser mensagens de warning (por exemplo: quando um pedido de compra ultrapassa determinado valor), podem ser mensagens informativas (por exemplo: informar a integração com os parceiros foi realizada com sucesso durante o processamento da requisição) etc.
- O processamento nem sempre se resume a sucesso ou falha. Em processamento em lote por exemplo, o resultado da operação pode ser parcial onde parte dos itens do lote são processados e outra parte não.

E a partir disso, vamos começar a construir um raciocínio que vai nos guiar e nos levará as decisões que foram tomadas para esse projeto.

## :thinking: Analisando as possibilidades

[voltar ao topo](#book-conteúdo)

Vamos começar agora a construir uma linha de raciocínio para abordar cada um desses cenários e evoluir mais em alguns outros que irão surgir de acordo com a evolução do raciocínio. O primeiro ponto que vamos conversar é sobre a necessidade dos métodos retornarem mensagens conforme coisas vão ocorrendo na sua execução.

### :pushpin: O que é uma notificação?

Parece algo muito trivial e simples de responder, mas acredite em mim, a maioria ignora os detalhes. Quando analisamos o assunto temos a tendência de ignorar os detalhes e pensamos em notificação somente como uma simples mensagem composta por um texto, mas isso é superficial demais e existem mais detalhes que devemos analisar.

<br/>

> [!TIP]
> Uma notificação vai além do que ser uma simples mensagem de texto

<br/>

Existem notificações que vão além do log. É comum as aplicações terem logs que são escritos a medida que um processamento é realizado. Processos em background que ocorrem a partir de algum scheduler ou reagindo a algum evento não costumam retornar informações para a aplicação cliente que realizou o disparodo processamento, afinal de contas, tanto o disparo por meio de um scheduler ou reagindo a algum evento, são processados de forma assíncrona e não tem uma sessão de alguma aplicação cliente aguardando uma resposta, então o que faz mais sentido é realmente registar essas notificações por meio de logs, PORÉM, quando o processamento é feito a partir de uma chamada síncrona de uma aplicação cliente, é comum termos que retornar essas notificações para a aplicação cliente para que ela possa tomar alguma decisão em cima disso.

<br/>

> [!TIP]
> Quando temos uma requisição síncrona do nosso método, o chamador pode querer ler as notificações geradas para tomar alguam decisão a partir das notificações. 

> [!TIP]
> Não deixe de considerar que sua aplicação pode ter mais de uma aplicação cliente

<br/>

Imagine uma Web API que expõe um endpoint onde possibilita a abertura de pedidos de compra. Essa API é consumida por duas aplicações (uma aplicação web e outra mobile) conforme diagrama a seguir:

<br/>
<div align="center">
  <img src="diagrams/diagram1.svg">
</div>
<br/>

Esse fluxo de negócio possui uma regra hipotética que, quando o pedido de compra passar de 50 mil reais, o pedido deve ser aprovado pelo gestor da área. Com base nessa regra de negócio hipotética, os sistemas, tanto web quanto mobile, devem exibir uma notificação informando que o pedido de compra foi aceito mas está pendente de aprovação do gestor da área. Essa notificação é importante pois, por mais que o sistema já possua um fluxo de aprovação, o solicitante precisa estar ciente disso de imediato para que o processo não caia no esquecimento ou até mesmo para ele possa agilizar esse processo por entrar em contato diretamente com o gestor pedindo agilidade na aprovação dependendo da criticidade da demanda.

<br/>

> [!IMPORTANT]
> Nem toda notificação é um erro

<br/>

Até o momento, nada fora do comum em sistemas *LOB (Line of business)*, porém, imagine o seguinte: Os programadores tiveram o raciocínio de que, por se tratar de uma notificação para o usuário que está utilizando o sistema, a responsabilidade de saber os critérios e quando exibir essa notificação é responsabilidade das aplicações de front-end, afinal, exibir algo é do front-end e não do back-end.

<br/>

> [!IMPORTANT]
> Nem toda notificação para o usuário é gerada no front-end

<br/>

Mas esse raciocínio não está correto. Caso isso ocorra, tanto o front-end web quanto o mobile precisariam codificar a mesma regra. Ambos teriam que saber que precisam validar o valor do pedido para notificar, ambos teriam qeu saber qual valor é esse e ambos teriam que saber qual é a mensagem que deveriam exibir e `esse cenário é desastroso`. Por que? Isso `causaria duplicidade na implementação` da regra de negócio pois web e mobile precisam saber e implementar a regra, além disso, poderíamos ter `inconsistências` perante essas implementações que gerariam `bugs` e `comportamentos diferentes para o mesmo recurso` entre as aplicação web e mobile. Quando o valor de referência para a notificação mudasse (o exemplo foi de 50 mil reais, imagine que mudou para 30 mil reais), teríamos que gerar uma nova versão das aplicações web e mobile (que é um problema pois os `tempo de deploy` e `disponibilização` de uma aplicação web e em uma loja de aplicativos mobile não são as mesmas) ou até criar um endpoint só para buscar esse valor `aumentando mais ainda a complexidade`.

<br/>

> [!WARNING]
> Devemos evitar duplicidade de implementação. Siga o princípio DRY (Don't repeat yourself)

> [!WARNING]
> Os deploys possuem ciclos de vida e tempo de disponibilização diferentes

<br/>

Existem até cenários pouco explorados em que esse cenário causa problemas que é quando o sistema possuí suporte a múltiplos idiomas. Se o front-end é responsável por gerar essa mensagem de notificação que é a partir de uma regra do back-end, ambas as aplicações de front-end (web e mobile) vão ter que ter a tradução correta da mensagem e, além da duplicidade e chance de maior de bugs e erros, os ciclos de deploy das aplicações são diferentes como explicado anteriormente fazendo com que a aplicação web tenha a tradução mais atualizada e a mobile não pois a loja de aplicativos demorou para atualizar ou o usuário não quis atualizar o app ainda podendo trazer até riscos legais! 
`Todos esses cenários descritos seriam desastrosos`!

<br/>

> [!TIP]
> Não deixe de considerar aspectos de globalização. Valide as chances da sua apliação precisar dar suporte a múltiplas culturas e idiomas

<br/>

Então qual seria o cenário mais adequado? Notificações que são geradas a partir de regras do back-end devem ser gerados no back-end, regras que são geradas a partir de regras exclusivas do front-end (como highligth de campos obrigatórios, tool tips etc.) devem ser geradas no front-end. Assim, se o banck-end gerar a notificação do exemplo de que pedidos acima de X reais devam ser aviasados que entraram em um fluxo de aprovação, o back-end que tem que gerar a notificação e retornar as aplicações clientes, assim não teríamos a duplicidade e a atualização da mensagem e dos crtiérios teriam efeito imediato nas aplicações web e mobile. As aplicações front-end seriam responsável somente por renderizar as notificações geradas pelo back-end e não por gerá-las.

<br/>

> [!IMPORTANT]
> Notificações que são geradas a partir de regras do back-end devem ser gerados no back-end e somente renderizadas no front-end

<br/>

Mas isso quer dizer que o back-end, ao invés de registrar as notificações somente em logs, agora o back-end precisa retornar essas notificações para a aplicação que o chamou, ou seja, chegamos a nossa primeira conclusão: `métodos precisam ter a capacidade de retornar as notificações que eles geraram`.

<br/>

> [!IMPORTANT]
> Métodos precisam ter a capacidade de retornar as notificações que eles geraram

<br/>

### :pushpin: Tipos de notificação

[voltar ao topo](#book-conteúdo)

Notificações também possuem um `tipo`. Nós temos a tendência de achar que uma notificação é somente quando algo da errado, mas nós podemos querer notificar mais que isso. Vejamos alguns exemplos:

- Uma `notificação informativa` de que os dados da declaracão do imposto de renda foram transmitidos para a receita federal.
- Uma `notificação de sucesso` de que os dados foram salvos com sucesso.
- Uma `notificação de cuidado` informando que a venda foi realizada mas que o produto está chegando perto da quantidade mínima no estoque 
- Uma `notificação de erro` informando que não foi possível processar a solicitação

<br/>

> [!IMPORTANT]
> Notificação possuem tipos: `informação`, `sucesso`, `aviso` e `erro`

<br/>

Acabou por aqui? Ainda não!

Algo que não pode ficar de fora é que em sistemas cliente/servidor, em sistemas com múltiplas aplicações clientes e em processos de atendimento de suprote, precisamos identificar rapidamente os comportamentos que o sistema apresenta, por isso, é de suma importância que cada notificação do sistema tenha um `identificador único` pois a representação daquela mensagem pode mudar dependendo da aplicação cliente e dependendo do público para o qual ela se destina.

<br/>

> [!IMPORTANT]
> A notificação precisa possuir um identificador único

<br/>

Vamos ver um `exemplo prático`:

Imagine que existe um sistema de gestão de clínica médica de um convênio XPTO que exibe uma mensagem de que a pessoa que vai em busca de atendimento, ao ser registrada no sistema, precisa de um nome. Quando essa pessoa chega na recepção, ela ainda não está em atendimento, por isso, essa pessoa não foi identificada ainda se é uma pessoa segurada pelo convênio ou não, então no sistema da recepção a mensagem seria `"a pessoa precisa possuir um nome"`. 

Agora imagine que essa pessoa foi para a triagem e lá, a pessoa enfermeira precise localizar a pessoa pelo nome dentro do sistema, porém, como essa pessoa chegou até a triagem, então ela já é uma pessoa segurada, portanto, caso o nome não foi informado, a mensagem seria `"a pessoa segurada precisa possuir um nome"`.

Agora imagine que essa pessoa, após a triagem, foi para a consulta médica e o médico atende essa pessoa e registra tudo no sistema em um computador na sua sala de atendimento. O médico localiza o a pessoa segurada pelo nome e, se o médico não informar o nome, como a pessoa segurada já está em atendimento médico, ela passa a ser um paciente e a mensagem seria `"o paciente precisa possuir um nome"`.

Esse exemplo foi bem simples e com certeza tem brechar nas regras (por exemplo: poderiam ser bounded contexts diferentes etc, mas estou assumindo um monolito sem bounded contexts), mas é um cenário que acontece com muita frequência. Note que a natureza da mensagem é a mesma e internamente é o mesmo recurso, porém, dependendo do contexto de negócio, o texto da notificação muda mas, na essência, elas são as mesmas!

Então essa mensagem poderia ser do `tipo erro`, ter o `código Person.Name.Should.Required` e ter `diferentes descrições` de acordo com o contexto e até poder possuir diferentes traduções nesses contextos. Quando a mensagem for exibida, elas poderiam ser exibidas juntamente com o código, que é padronizado, fazendo com que o sistema consiga se comunicar adequadamente para cada contexto mas permitindo que o suporte (tanto nível 1 quanto nível 2 e nível 3) encontrem a solução adequada para a solicitação da forma mais rápida possível já que se orientariam pelo código da mensagem.

<br/>

> [!TIP]
> Separar o identificador único da notificação da sua descrição vai facilitar a sua vida

<br/>

Com isso podemos concluir o que é uma notificação e quais características essa notificação precisa ter. No contexto desse projeto, essa notificação se chama OutputMessage e pode ser vista no arquivo [OutputMessage.cs](../src/OutputEnvelop/Models/OutputMessage.cs).

Uma `OutputMessage` possui a seguinte estrutura:

| Propriedade | Tipo | Valores |
| - | - | - |
| Type | OutputMessageType | `1 - Information`, `2 - Success`, `3 - Warning`, `4 - Error` |
| Code | string | - |
| Description | string | - |

### :pushpin: Lançando notificações durante a execução de métodos

[voltar ao topo](#book-conteúdo)

Lançar notificações durante a execução de um método por sí só não é um grande desafio, afinal de contas, os sistemas sempre fizeram isso por escrever essas notificações em logs, mas a questão que, inicialmente parece simpels, vai muito além disso pois agora, além do método fazer o que ele deveria fazer, ele deve retornar as notificações lançadas para o método chamador. Vamos analisar com mais detalhes.

A `primeira opção` seria a mais óbvia que é fazer o método retornar a lista de mensagens. Na linguagem C#, nós temos algumas opções para fazer isso. Vamos ver algumas quando o método não teria um retorno (ou seja, void).

Vamos ver algumas quando o método não teria um retorno (ou seja, void).

> PS: Eu sei que void é um tipo de retorno, mas para facilitar o entendimento, vou assumir a convenção padrão de que métodos que retornam void não tem retorno.

- Utilizando variáveis de saída:
```csharp
// Output retornando nullable para evitar alocação na heap mas piorando a utilização por retornar nulo
public void DoSomething(string name, out OutputMessage[]? messages)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        messages = new[] { OutputMessage.Create(type: OutputMessageType.Error, code: "...", description: "...") };
        return;
    }

    Name = name;

    messages = null;
}

// Output retornando um array vazio para facilitar a utilização mas gerando alocação na heap
public void DoSomething(string name, out OutputMessage[] messages)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        messages = new[] { OutputMessage.Create(type: OutputMessageType.Error, code: "...", description: "...") };
        return;
    }

    Name = name;

    messages = Array.Empty<OutputMessage>();
}
```

- Utilizando o retorno do método:
```csharp
// Retornando nullable para evitar alocação na heap mas piorando a utilização por retornar nulo
public OutputMessage[]? DoSomething(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return new[] { OutputMessage.Create(type: OutputMessageType.Error, code: "...", description: "...") };

    Name = name;

    return null;
}

// Retornando um array vazio para facilitar a utilização mas gerando alocação na heap
public OutputMessage[] DoSomething(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return new[] { OutputMessage.Create(type: OutputMessageType.Error, code: "...", description: "...") };

    Name = name;

    return Array.Empty<OutputMessage>();
}
```

<br/> 

Agora vamos analisar esses casos com `métodos que teriam um retorno`:

<br/> 

- Utilizando variáveis de saída:
```csharp
// Output retornando nullable para evitar alocação na heap mas piorando a utilização por retornar nulo
public bool DoSomething(string name, out OutputMessage[]? messages)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        messages = new[] { OutputMessage.Create(type: OutputMessageType.Error, code: "...", description: "...") };
        return false;
    }

    Name = name;

    messages = null;

    return true;
}

// Output retornando um array vazio para facilitar a utilização mas gerando alocação na heap
public bool DoSomething(string name, out OutputMessage[] messages)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        messages = new[] { OutputMessage.Create(type: OutputMessageType.Error, code: "...", description: "...") };
        return false;
    }

    Name = name;

    messages = Array.Empty<OutputMessage>();

    return true;
}
```

- Utilizando o retorno do método:
```csharp
// Retornando nullable para evitar alocação na heap mas piorando a utilização por retornar nulo. Por ter mais de um valor, o retorno precisa ser uma tupla
public (bool Success, OutputMessage[]? OutputMessageCollection) DoSomething(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return (Success: false,  OutputMessageCollection: new[] { new OutputMessage(type: OutputMessageType.Error, code: "...", description: "...") });

    Name = name;

    return (Success: true, OutputMessageCollection: null);
}

// Retornando um array vazio para facilitar a utilização mas gerando alocação na heap. Por ter mais de um valor, o retorno precisa ser uma tupla
public (bool Success, OutputMessage[] OutputMessageCollection) DoSomething(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return (Success: false,  OutputMessageCollection: new[] { new OutputMessage(type: OutputMessageType.Error, code: "...", description: "...") });

    Name = name;

    return (Success: true, OutputMessageCollection: Array.Empty<OutputMessage>());
}
```

### :pushpin: Arrays vazios ou referências nulas para array de mensagens?

[voltar ao topo](#book-conteúdo)

Vamos analisar essas duas opções primeiro que acabamos de ver. Vamos começar pelo retorno de um array vazio ou um retorno nulo quando não houverem mensagens. Nós vamos analisar pelo viés da usabilidade e do desempenho.

Ao olhar pelo viés da usabilidade, é mais interessante ter um array vazio do que um valor nulo pois evita possíveis exceções de referências nulas. Utilizar um array vazio vindo de uma constante do .NET como um Array.Empty também não causará pressão no Garbage Collector pois não haverá instanciações de novos arrays. Para reproduzir e validar esse cenário qeu acabei de afirmar, vamos executar dois benchamrks (detalhes sobre os benchmarks podem ser encontrados na [documentação sobre benchmark](BENCHMARKS-PT.md)).

A seguir temos parte do código do objeto [OutputEnvelop.cs](../src/OutputEnvelop/OutputEnvelop.cs). No construtor do objeto recebemos alguns parâmetros e definimos algumas propriedades que são readonly.

Como a biblioteca é feita para o .NET Standard 2.0, não existe suporte a deixar explícito o nullable para arrays, porém, é possível mesmo assim passar `null` no array. Sendo assim, as parâmetros `OutputMessage[] outputMessageCollection` e `Exception[] exceptionCollection` do construtor podem vir nulos.

Note que esses parâmetros do construtor mencionados anteriormente alimentam duas propriedades com o modificador de acesso `internal`. Nós falaremos disso posteriormente.

O ponto de agora é que o código atual permite que as propriedades `internal` tenham valores nulos, conforme demontrado a seguir:

```csharp
// Versão que aceita nulo
public readonly struct OutputEnvelop<TOutput>
{
    // Properties
    internal OutputMessage[] OutputMessageCollectionInternal { get; }
    internal Exception[] ExceptionCollectionInternal { get; }

    public TOutput Output { get; }
    public OutputEnvelopType Type { get; }

    // Constructors
    private OutputEnvelop(
        TOutput output,
        OutputEnvelopType type,
        OutputMessage[] outputMessageCollection,
        Exception[] exceptionCollection
    )
    {
        Output = output;
        Type = type;
        OutputMessageCollectionInternal = outputMessageCollection;
        ExceptionCollectionInternal = exceptionCollection;
    }
}
```

Uma alternativa para que esse objeto passe um Array vazio ao invés de permitir nulo seria checar se o parâmetro do construtor é nulo e substituir por uma constante de um Array vazio conforme a seguir:

```csharp
// Versão que aceita nulo
public readonly struct OutputEnvelop<TOutput>
{
    // Properties
    internal OutputMessage[] OutputMessageCollectionInternal { get; }
    internal Exception[] ExceptionCollectionInternal { get; }

    public TOutput Output { get; }
    public OutputEnvelopType Type { get; }

    // Constructors
    private OutputEnvelop(
        TOutput output,
        OutputEnvelopType type,
        OutputMessage[] outputMessageCollection,
        Exception[] exceptionCollection
    )
    {
        Output = output;
        Type = type;
        OutputMessageCollectionInternal = outputMessageCollection ?? Array.Empty<OutputMessage>();
        ExceptionCollectionInternal = exceptionCollection ?? Array.Empty<Exception>();
    }
}
```

Para medir o impacto dessas duas versões, vamos executar o benchmark do arquivo [CreateOutputEnvelopBenchmark.cs](../benchs/Benchmarks/OutputEnvelopBenchs/CreateOutputEnvelopBenchmark.cs) e analisar o teste `CreateOutputEnvelopWithoutMessageAndException` que vai passar uma coleção de mensagens e exceptions nulas para o cenário que queremos testar. Para cada uma das variações acima vamos executar o mesmo teste e vamos analisar o retorno. Primeiro, vamos aos resultados brutos dos benchmarks:

| Type             | Method                                        | Mean (ns) | Error (ns) | StdDev (ns) | CacheMisses/Op | TotalIssues/Op | TotalCycles/Op | BranchInstructions/Op | BranchMispredictions/Op | Gen0 | Allocated (B) |
|------------------|-----------------------------------------------|-----------|------------|-------------|----------------|----------------|----------------|-----------------------|-------------------------|------|---------------|
| With null        | CreateOutputEnvelopWithoutMessageAndException | 8,603     | 0,0223     | 0,0186      | 0              | 47             | 16             | 11                    | 0                       | 0    | 0             |
| With empty array | CreateOutputEnvelopWithoutMessageAndException | 117,26    | 1,964      | 2,879       | 0              | 518            | 257            | 123                   | 0                       | 0    | 0             |

A primeira coluna (`Type`) refere-se aos nossos dois cenários passando o valor nulo ou o array vazio. Agora vamos analisar esses dados com mais detalhes.

Na coluna `Mean (ns)` temos o tempo médio de execução e já conseguimos notar algo já bem impactante. A versão com o `valor nulo` fez em `8,6 nanosegundos`, já a `versão com o Array.Empty` foi de `117,26`. Isso quer dizer que o código passando o `Array.Empty foi cerca de 13,6 vezes pior`.

Na coluna `Erros (ns)` temos o tempo total gasto com erros durante a execução e, ao comparar os dados, conseguimos ver que o código com o `Array.Empty foi 88 vezes pior`.

Na coluna `StdDev (ns)` conseguirmos ver o desvio padrão das execuções. Quanto menor o desvio padrão, mais estável o código é e menores serão os picos para baixo ou para cima. No arquivo sobre [benchmarks](BENCHMARKS-PT.md) tem mais detalhes sobre o que é o desvio padrão. A versão com `Array.Empty foi bem mais instável`.

Quando analisamos `TotalIssues/Op`, `TotalCycles/Op` e `BranchInstructions/Op` também vemos a clara diferença onde com `Array.Empty` apresentou muito `mais erros e quantidade de instruções`.

Agora, quando analisanmos a alocação de memória, ambos não geraram alocação.

O que podemos concluir disso?
- :white_check_mark: Conclusão 1 - A versão com o valor nulo é mais rápida.
- :white_check_mark: Conclusão 2 - A versão com o valor nulo é mais estável.
- :white_check_mark: Conclusão 3 - A versão com o valor nulo tem uma usabilidade menor pois joga a responsabilidade de tratar o valor nulo para o método chamador.

