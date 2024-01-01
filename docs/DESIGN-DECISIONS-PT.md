# Decisões de design

> [!IMPORTANT]
> :construction: DOCUMENTO NÃO FINALIZADO! EM CONSTRUÇÃO

## :book: Conteúdo
- [Decisões de design](#decisões-de-design)
  - [:book: Conteúdo](#book-conteúdo)
  - [:information\_source: Necessidade](#information_source-necessidade)
  - [:thinking: Analisando as possibilidades](#thinking-analisando-as-possibilidades)
    - [:pushpin: O que é uma notificação?](#pushpin-o-que-é-uma-notificação)
    - [:pushpin: Tipos de notificação](#pushpin-tipos-de-notificação)
    - [:pushpin: Lançando notificações durante a execução de métodos](#pushpin-lançando-notificações-durante-a-execução-de-métodos)
    - [:pushpin: O que devemos retornar nos métodos?](#pushpin-o-que-devemos-retornar-nos-métodos)
      - [:pushpin: Exceções: uma visão semântica sobre o assunto](#pushpin-exceções-uma-visão-semântica-sobre-o-assunto)
      - [:pushpin: Exceções: uma visão sobre desempenho](#pushpin-exceções-uma-visão-sobre-desempenho)
      - [:white\_check\_mark: Decisão de design 1: A biblioteca deve proporcionar mecanismos de controle que evite o lançamento de exceções](#white_check_mark-decisão-de-design-1-a-biblioteca-deve-proporcionar-mecanismos-de-controle-que-evite-o-lançamento-de-exceções)
      - [:pushpin: Como padronizar o retorno?](#pushpin-como-padronizar-o-retorno)
      - [:white\_check\_mark: Decisão de design 2: Precisamos criar um envelope de resposta para padronizar o retorno dos métodos](#white_check_mark-decisão-de-design-2-precisamos-criar-um-envelope-de-resposta-para-padronizar-o-retorno-dos-métodos)
    - [:pushpin: Arrays vazios ou referências nulas para array de mensagens?](#pushpin-arrays-vazios-ou-referências-nulas-para-array-de-mensagens)
    - [:white\_check\_mark: Decisão de design 3: O tratamento para evitar null reference será feito na leitura da propriedade ao invés de ser feito na criação do objeto](#white_check_mark-decisão-de-design-3-o-tratamento-para-evitar-null-reference-será-feito-na-leitura-da-propriedade-ao-invés-de-ser-feito-na-criação-do-objeto)

## :information_source: Necessidade

Durante a execução de um método (vou me abster de algumas formalidades, como referências a procedimentos quando não há retorno ou função específica, chamando tudo de método) em sistemas *LOB (Line of Business)*, frequentemente desejamos mais do que um único objeto de retorno. Isso não ocorre porque estamos projetando métodos com múltiplas responsabilidades, infringindo o primeiro princípio do SOLID, mas sim porque existem informações complementares essenciais para esse tipo de sistema.

<br/>

> [!IMPORTANT]
> Existem diversos tipos de sistemas com propósitos distintos, como sistemas para IoT, sistemas de missão crítica, sistemas de baixo nível, entre outros. Este pacote e toda a análise foram concebidos para sistemas *LOB (Line of Business)*.

<br/>

Esses sistemas possuem algumas características comuns. Algumas delas são:
- Autenticar o usuário que está tentando realizar a operação.
- Autorizar o usuário autenticado para a operação que está querendo realizar.
- Receber inputs dos usuários.
- Validar os inputs dos usuários.
- Validar os estados dos objetos de negócio.
- Realizar algum processo de negócio que modifique o estado dos objetos de negócio.
- Persistir essas informações.
- Retornar o resultado da solicitação para o usuário.
- Exibir diversos relatórios a partir das informações armazenadas.

Ao atender esses cenários, algumas necessidades surgem e são comuns em todo desenvolvimento, independentemente da regra de negócio aplicada. Por exemplo:
- Queremos saber as notificações que ocorreram durante a execução dos métodos.
- Essas notificações são mais do que simples mensagens de erro, podem ser mensagens de warning (por exemplo: quando um pedido de compra ultrapassa determinado valor), podem ser mensagens informativas (por exemplo: informar a integração com os parceiros foi realizada com sucesso durante o processamento da requisição) etc.
- O processamento nem sempre se resume a sucesso ou falha. Em processamento em lote, por exemplo, o resultado da operação pode ser parcial, onde parte dos itens do lote são processados e outra parte não.

E a partir disso, vamos começar a construir um raciocínio que vai nos guiar e nos levará às decisões que foram tomadas para esse projeto.


## :thinking: Analisando as possibilidades

[voltar ao topo](#book-conteúdo)

Vamos começar agora a construir uma linha de raciocínio para abordar cada um desses cenários e evoluir em alguns outros que irão surgir de acordo com a evolução do raciocínio. O primeiro ponto que vamos abordar é sobre a necessidade dos métodos retornarem mensagens conforme as coisas vão ocorrendo durante a sua execução.

### :pushpin: O que é uma notificação?

[voltar ao topo](#book-conteúdo)

Parece algo muito trivial e simples de responder, mas acredite em mim, a maioria ignora os detalhes. Ao analisarmos o assunto, temos a tendência de negligenciar os detalhes e pensar em notificação apenas como uma mensagem simples composta por um texto. No entanto, isso é superficial demais, e existem mais detalhes que devemos considerar.

<br/>

> [!TIP]
> Uma notificação vai além de ser uma simples mensagem de texto.


<br/>

Existem notificações que vão além do log. É comum as aplicações terem logs que são escritos à medida que um processamento é realizado. Processos em segundo plano que ocorrem a partir de algum agendador ou em resposta a algum evento geralmente não retornam informações para a aplicação cliente que iniciou o processamento. Afinal, tanto o acionamento por meio de um agendador quanto a reação a algum evento são processados de forma assíncrona e não têm uma sessão de alguma aplicação cliente aguardando uma resposta. Portanto, o que faz mais sentido é registrar essas notificações por meio de logs. No entanto, quando o processamento é feito a partir de uma chamada síncrona de uma aplicação cliente, é comum ter que retornar essas notificações para a aplicação cliente, para que ela possa tomar alguma decisão com base nelas.

<br/>

> [!TIP]
> Quando temos uma requisição síncrona do nosso método, o chamador pode querer ler as notificações geradas para tomar alguma decisão com base nelas.

> [!TIP]
> Não deixe de considerar que sua aplicação pode ter mais de um cliente.

<br/>

Imagine uma Web API que expõe um endpoint permitindo a abertura de pedidos de compra. Essa API é consumida por duas aplicações, uma aplicação web e outra mobile, conforme o diagrama a seguir:

<br/>
<div align="center">
  <img src="diagrams/diagram1.svg">
</div>
<br/>

Esse fluxo de negócio possui uma regra hipotética que determina que, quando o pedido de compra ultrapassar 50 mil reais, o pedido deve ser aprovado pelo gestor da área. Com base nessa regra de negócio hipotética, os sistemas, tanto web quanto mobile, devem exibir uma notificação informando que o pedido de compra foi aceito, mas está pendente de aprovação do gestor da área. Essa notificação é importante, pois, mesmo que o sistema já tenha um fluxo de aprovação, o solicitante precisa estar ciente imediatamente para evitar esquecimentos no processo ou, até mesmo, para possibilitar que ele agilize o processo entrando em contato diretamente com o gestor e solicitando celeridade na aprovação, dependendo da criticidade da demanda.

<br/>

> [!IMPORTANT]
> Nem toda notificação é um erro.

<br/>

Até o momento, nada fora do comum em sistemas *LOB (Line of Business)*. No entanto, imagine o seguinte: os programadores tiveram o raciocínio de que, por se tratar de uma notificação para o usuário que está utilizando o sistema, a responsabilidade de conhecer os critérios e decidir quando exibir essa notificação é atribuída às aplicações de front-end. Afinal, exibir algo é uma responsabilidade do front-end e não do back-end.

<br/>

> [!IMPORTANT]
> Nem toda notificação para o usuário é gerada no front-end.

<br/>

Mas esse raciocínio não está correto. Caso isso ocorra, tanto o front-end web quanto o mobile precisariam codificar a mesma regra. Ambos teriam que saber que precisam validar o valor do pedido para notificar, ambos teriam que saber qual valor é esse e ambos teriam que saber qual é a mensagem que deveriam exibir e `esse cenário é desastroso`. Por que? Isso `causaria duplicidade na implementação` da regra de negócio, pois web e mobile precisam saber e implementar a regra. Além disso, poderíamos ter `inconsistências` perante essas implementações que gerariam `bugs` e `comportamentos diferentes para o mesmo recurso` entre as aplicações web e mobile. Quando o valor de referência para a notificação mudasse (o exemplo foi de 50 mil reais, imagine que mudou para 30 mil reais), teríamos que gerar uma nova versão das aplicações web e mobile (o que é um problema, pois os `tempos de deploy` e `disponibilização` de uma aplicação web e em uma loja de aplicativos mobile não são os mesmos) ou até criar um endpoint só para buscar esse valor, `aumentando mais ainda a complexidade`.

<br/>

> [!WARNING]
> Devemos evitar duplicidade de implementação. Siga o princípio DRY (Don't repeat yourself).

> [!WARNING]
> Os deploys possuem ciclos de vida e tempo de disponibilização diferentes.


<br/>

Existem cenários pouco explorados nos quais essa situação pode causar problemas, especialmente quando o sistema possui suporte a múltiplos idiomas. Se o front-end é encarregado de gerar a mensagem de notificação com base em uma regra do back-end, ambas as aplicações de front-end (web e mobile) precisarão ter a tradução correta da mensagem. Além da duplicidade e do risco aumentado de bugs e erros, os ciclos de deploy das aplicações são distintos, como explicado anteriormente. Isso significa que a aplicação web pode ter a tradução mais atualizada, enquanto a versão mobile pode não ter, seja porque a loja de aplicativos demorou para atualizar ou porque o usuário ainda não atualizou o aplicativo, o que pode acarretar até mesmo em riscos legais! `Todos esses cenários descritos seriam desastrosos!`

<br/>

> [!TIP]
> Não deixe de considerar aspectos de globalização. Valide as chances da sua apliação precisar dar suporte a múltiplas culturas e idiomas.

<br/>

Qual seria o cenário mais adequado? Notificações geradas a partir de regras do back-end devem ser geradas no back-end. Regras provenientes exclusivamente do front-end, como destaque de campos obrigatórios, tooltips etc., devem ser geradas no front-end. Dessa forma, se o back-end gerar a notificação, por exemplo, de que pedidos acima de X reais devem ser avisados que entraram em um fluxo de aprovação, cabe ao back-end gerar a notificação e retorná-la às aplicações clientes. Assim, evitaríamos duplicidade, e a atualização da mensagem e dos critérios teria efeito imediato nas aplicações web e mobile. As aplicações front-end seriam responsáveis apenas por renderizar as notificações geradas pelo back-end, não por gerá-las.

<br/>

> [!IMPORTANT]
> Notificações geradas a partir de regras do back-end devem ser geradas no back-end e apenas renderizadas no front-end.

<br/>

Isso implica que, em vez de apenas registrar as notificações em logs, o back-end agora deve retornar essas notificações para a aplicação que o chamou. Portanto, chegamos à nossa primeira conclusão: `os métodos precisam ter a capacidade de retornar as notificações que geraram`.

<br/>

> [!IMPORTANT]
> Métodos precisam ter a capacidade de retornar as notificações que eles geraram

<br/>

### :pushpin: Tipos de notificação

[voltar ao topo](#book-conteúdo)

Notificações também possuem um tipo. Costumamos associar notificações apenas a situações problemáticas, mas podemos desejar notificar sobre diversos eventos. Vamos analisar alguns exemplos:

- Uma notificação informativa de que os dados da declaração do imposto de renda foram transmitidos à Receita Federal.
- Uma notificação de sucesso indicando que os dados foram salvos com êxito.
- Uma notificação de cuidado alertando que a venda foi concluída, mas o produto está se aproximando da quantidade mínima em estoque.
- Uma notificação de erro informando que não foi possível processar a solicitação.

<br/>

> [!IMPORTANT]
> Notificações possuem diferentes tipos: `informação`, `sucesso`, `aviso` e `erro`.

<br/>

Ainda não acabou por aqui!

Em sistemas cliente/servidor, especialmente em ambientes com múltiplas aplicações clientes e processos de suporte, é crucial identificar rapidamente os comportamentos do sistema. Portanto, é de suma importância que cada notificação do sistema possua um `identificador único`, uma vez que a representação da mensagem pode variar conforme a aplicação cliente e o público-alvo para o qual ela se destina.

<br/>

> [!IMPORTANT]
> A notificação precisa possuir um identificador único.

<br/>

Vamos observar um `exemplo prático`:

Considere um sistema de gestão de clínica médica. Ao registrar uma pessoa no sistema durante o processo de atendimento, é necessário que ela tenha um nome. A mensagem exibida varia conforme o contexto:

1. Na recepção: `"A pessoa precisa possuir um nome"` - Ainda não identificada como segurada.
2. Na triagem: `"A pessoa segurada precisa possuir um nome"` - Identificada como segurada.
3. Na consulta médica: `"O paciente precisa possuir um nome"` - Já em atendimento médico.

Essa mensagem, do `tipo erro` com o `código Person.Name.Should.Required`, apresenta diferentes descrições dependendo do contexto, facilitando a comunicação em sistemas complexos. A inclusão de códigos padronizados permite que a equipe de suporte localize rapidamente a solução adequada, orientando-se pelo código da mensagem.

<br/>

> [!TIP]
> Separar o identificador único da notificação de sua descrição facilitará a sua vida.

<br/>

Com isso, podemos concluir o que é uma notificação e quais características ela precisa ter. No contexto deste projeto, essa notificação é chamada de `OutputMessage` e pode ser visualizada no arquivo [OutputMessage.cs](../src/OutputEnvelop/Models/OutputMessage.cs).

Uma `OutputMessage` possui a seguinte estrutura:


| Propriedade | Tipo | Valores |
| - | - | - |
| Type | OutputMessageType | `1 - Information`, `2 - Success`, `3 - Warning`, `4 - Error` |
| Code | string | - |
| Description | string | - |

### :pushpin: Lançando notificações durante a execução de métodos

[voltar ao topo](#book-conteúdo)

Lançar notificações durante a execução de um método, por si só, não é um grande desafio. Afinal de contas, os sistemas sempre fizeram isso ao escrever essas notificações em logs. No entanto, a questão, que inicialmente parece simples, vai muito além, pois agora, além do método realizar sua função principal, ele deve retornar as notificações lançadas para o método chamador. Vamos analisar com mais detalhes.

A `primeira opção` seria a mais óbvia, que é fazer o método retornar a lista de mensagens. Na linguagem C#, temos algumas opções para fazer isso. Vamos explorar algumas, especialmente quando o método não teria um retorno (ou seja, void).

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

### :pushpin: O que devemos retornar nos métodos?

[voltar ao topo](#book-conteúdo)

Nos exemplos acima, retornamos uma coleção de `OutputMessage` quando o método não possuía um retorno esperado (`void`). Também retornamos uma `tupla` quando, além da coleção de `OutputMessage`, temos algum valor esperado para retornar. No entanto, quero aprofundar mais nesse assunto e analisar alguns pontos que considero importantes.

Quando chamamos um método, esperamos saber algumas coisas sobre ele após a sua execução. Essas informações incluem:

- Ocorreu algum erro inesperado durante a execução?
- Em caso de erro, qual foi o tipo específico do problema?
- O método executou com sucesso, realizando todas as tarefas previstas?
- É possível determinar se o processo foi executado de forma parcial (por exemplo, importação de itens de um lote, onde parte foi importada e parte não)?
- É possível obter todas as notificações que ocorreram durante a execução desse método?
- É fácil identificar o retorno do método?
- É claro e compreensível entender a intenção do retorno desse método?

Essas são perguntas que geralmente não fazemos, mas são importantes. Vamos abordar cada uma delas.

Uma das perguntas é: "Ocorreu algum erro inesperado durante a execução?" Essa pergunta é crucial, pois o raciocínio natural de qualquer programador iniciante é pensar que "se o método não lançou nenhuma exceção, é porque não ocorreu nenhum erro inesperado". Embora seja um pensamento natural, devemos ter cuidado com o uso incorreto ou desnecessário de exceções para controlar o fluxo de execução.

É confortável para nós, programadores, utilizar exceções para controlar o fluxo da aplicação, afinal, basta usar um bloco `try/catch` e está tudo certo. O controle de fluxo está feito, e fica fácil identificar se ocorreu um erro e como tratar esse fluxo. No entanto, é crucial considerar alguns pontos importantes em relação ao uso de exceções.

<br/>

> [!WARNING]
> Embora exceções sejam amigáveis para o programador, facilitando o controle do fluxo de execução do código, é crucial analisar pontos importantes ao lançar exceções.

<br/>

#### :pushpin: Exceções: uma visão semântica sobre o assunto

[voltar ao topo](#book-conteúdo)

Em programação, quando nos referimos à semântica, estamos falando do sentido e significado de determinada coisa. No contexto de exceções, analisar o significado do termo pode proporcionar ideias interessantes.

Como o próprio nome sugere, uma exceção é algo que ocorre a critério extraordinário, ou seja, é algo que acontece fora da regra estabelecida. Vamos usar um exemplo do cotidiano para ilustrar melhor: temos regras de trânsito, e uma delas proíbe ultrapassar um semáforo vermelho. Isso é uma regra padrão. No entanto, em situações de emergência médica, podemos abrir uma exceção para essa infração, pois a situação foge à regra padrão devido à urgência envolvendo vidas. A regra foi concebida para situações gerais, e, portanto, essa situação de emergência médica é uma exceção à regra.

No contexto do nosso programa, quando falamos de exceção, estamos nos referindo a algo que ocorre de maneira inesperada, algo que o processamento `não previu`. Em outras palavras, é algo `INESPERADO`.

É crucial prestar atenção nesse conceito, pois se o nosso método prevê algum cenário e trata esse cenário, `aquele cenário não é uma exceção`, mas sim `algo esperado que o próprio método conhece e está tratando`. Em vez de ser uma exceção, faz parte do processamento do método porque é algo conhecido e tratável.

<br/>

> [!TIP]
> Semanticamente, uma exceção é algo que foge à regra. Em programação, uma exceção é algo que ocorreu de forma inesperada. Se houver uma tratativa consciente no método para um determinado cenário, esse cenário não é uma exceção e passa a fazer parte da regra, pois é tratável.


<br/>

Vamos ver um exemplo. Veja o código a seguir:

```csharp
public class Customer
{
    public string Name { get; private set; }

    public void ChangeName(string name)
    {
        if(string.IsNullOrEmpty(name))
            throw new ArgumentNullException(name);
        else if(name.Length == 0 || name.Length > 50)
            throw new ArgumentOutOfRangeException(name);

        Name = name;
    }
}
public class CustomerService
{
    public void ChangeCustomerName(Guid customerId, string newName)
    {
        var customer = _customerRepository.GetById(customerId);

        // Eu não preciso tratar se o ChangeName deu certo, pois, caso um erro
        // ocorra, a chamada do método _customerRepository.Update
        // não ocorrerá pois o fluxo de execução será interrompido pela exceção
        // e não precisaremos nos preocupar com a tratativa do fluxo do código
        customer.ChangeName(newName);

        _customerRepository.Update(customer);
    }
}
```

No código acima, temos a classe `Customer` com o método `void ChangeName(string name)`. Este método valida o parâmetro `name` e, caso esteja nulo ou tenha um tamanho inválido, uma exceção é lançada para cada cenário inválido. É importante observar que esses cenários do parâmetro `name` ser nulo ou ter um tamanho inválido são conhecidos e tratados no código. Portanto, `semanticamente, não é uma exceção, mas sim, parte da regra`.

Na classe `CustomerService`, não é necessário realizar tratativas caso o método `ChangeName` apresente algum erro, pois ele lançará uma exceção e o fluxo de execução do método será interrompido ali mesmo. Isso é muito `confortável` para nós, programadores.

Vamos analisar este código considerando a semântica correta e evitando o uso de exceções nesse cenário:


```csharp
public class Customer
{
    public string Name { get; private set; }

    public void ChangeName(string name, out bool success)
    {
        if(string.IsNullOrEmpty(name) || (name.Length == 0 || name.Length > 50))
        {
            success = false;
            return;
        }

        Name = name;

        success = true;
    }
}
public class CustomerService
{
    public void ChangeCustomerName(Guid customerId, string newName)
    {
        var customer = _customerRepository.GetById(customerId);

        // Agora tivemos que analisar o retorno do método ChangeName
        // pois o controle de fluxo não será feito automaticamente
        // pelo lançamento da exceção. Caso não tratarmos a variável
        // de saída success, podemos mandar atualizar no banco de dados
        // mesmo se a execução não foi realizada com sucesso
        customer.ChangeName(newName, out bool success);
        if(!success)
            return;

        _customerRepository.Update(customer);
    }
}
```

Como observado no código acima, tratar as situações com a semântica correta e não lançar exceção para as regras conhecidas acaba gerando mais complicação para a manutenibilidade da aplicação do que solução. Isso ocorre porque, se o programador esquecer de tratar o retorno do método, comportamentos indesejados podem surgir. Isso ressalta a importância da `programação defensiva` nesse cenário.


<br/>

> [!CAUTION]
> Abster-se de lançar exceções, mesmo ao utilizar a semântica correta, aumenta a susceptibilidade a erros devido a falhas humanas. Em tais cenários, é necessário ter cuidado, pois o código demandará maior atenção em revisões de código, testes e práticas de programação defensiva.


<br/>

Ao analisar esses pontos, podemos concluir que devemos lançar exceções mesmo em cenários onde a regra é tratável, mesmo que isso não seja semanticamente correto, devido às facilidades que proporcionam. No entanto, infelizmente, as coisas não são tão simples assim (embora eu gostaria que fossem, já que também aprecio o uso de exceções pela facilidade que oferecem).

#### :pushpin: Exceções: uma visão sobre desempenho

Lançar exceções no nosso código traz uma consequência que, em determinados cenários, pode ser desastrosa. Estou me referindo à `degradação do desempenho da aplicação`.

Quando lançamos uma exceção no .NET, várias ações ocorrem, incluindo:

- A captura do tipo da exceção
- A criação de uma mensagem contendo todos os detalhes da exceção
- A obtenção do stack trace completo
- O preenchimento de dados adicionais da exceção
- A captura do objeto de origem da exceção
- A obtenção do método que originou a exceção
- A captura da InnerException para identificar se uma exceção foi lançada a partir de outra

Para que tudo isso ocorra, a `thread que está lançando a exceção é bloqueada`, e é realizado `processamento para coletar as informações`, resultando em `mais alocação de objetos no Garbage Collector`.

> [!CAUTION]
> Ao lidar com aplicações de alta volumetria, lançar exceções pode prejudicar significativamente o desempenho e fazer com que a aplicação exija recursos muito além do necessário.

<br/>

#### :white_check_mark: Decisão de design 1: A biblioteca deve proporcionar mecanismos de controle que evite o lançamento de exceções

[voltar ao topo](#book-conteúdo)

Falei sobre o problema de utilizar exceções em aplicações de alta volumetria com base na teoria. Agora, vamos analisar os resultados de um benchmark para ver isso na prática. O benchmark executado está no arquivo [ThrowExceptionBenchmark](../benchs/Benchmarks/ExceptionBenchs/ThrowExceptionBenchmark.cs). O resultado obtido foi:


| Method        | Mean           | Error       | StdDev      | Ratio     | RatioSD  | BranchInstructions/Op | TotalIssues/Op | TotalCycles/Op | Timer/Op | BranchMispredictions/Op | CacheMisses/Op | Allocated | Alloc Ratio |
|-------------- |---------------:|------------:|------------:|----------:|---------:|----------------------:|---------------:|---------------:|---------:|------------------------:|---------------:|----------:|------------:|
| NoException   |      0.5812 ns |   0.0680 ns |   0.0907 ns |      1.00 |     0.00 |                     0 |              3 |              1 |        0 |                       0 |             -0 |         - |          NA |
| WithException | 23,385.8588 ns | 306.3089 ns | 271.5349 ns | 41,163.98 | 7,019.46 |                17,480 |         78,557 |         54,265 |      236 |                     136 |            385 |     232 B |          NA |

Vamos às conclusões:

- Como podemos observar na coluna `Ratio`, o método que lançou a exceção foi `41 MIL vezes mais lento`.
- Na coluna `RatioSD`, percebemos que o método que lançou a exceção teve um desvio padrão `7 MIL vezes maior`, ou seja, é muito mais instável.
- Ao analisar as instruções e ciclos por operação, a versão com lançamento de exceção realizou `milhares de vezes mais operações`.
- O código que lança exceção `gerou alocação na heap`, enquanto o que não lança exceção não gerou alocação na heap.

Dado que o objetivo dessa biblioteca é oferecer suporte a processamentos de alta volumetria com alto desempenho, chegamos à nossa primeira decisão de design!

> [!IMPORTANT]
> Devemos avaliar os requisitos específicos dos nossos projetos para determinar se o uso de exceções causará um impacto real na aplicação.

<br/>

#### :pushpin: Como padronizar o retorno?

[voltar ao topo](#book-conteúdo)

Como vimos anteriormente, devemos evitar o uso de exceções. Quando um método possui um retorno esperado além da coleção de `OutputMessage`, podemos optar por código que retorne tuplas ou utilize variáveis de output.

Embora a linguagem permita o uso desses recursos, é sempre importante manter nosso código o mais coeso e simples de entender possível.

> [!TIP]
> O código precisa ser simples e coeso. Se é necessário ser um sênior para realizar qualquer ação no sistema, temos um problema.


<br/>

Então, vamos analisar o uso de tuplas como retorno ou de parâmetros de saída e os impactos disso no nosso código. Vamos começar pelo retorno usando tuplas. Observe o código a seguir:

```csharp
public enum ResultType
{
    Success = 1,
    Partial = 2,
    Error = 3
}

public (OutputMessage[]? OutputMessageCollection, ResultType ResultType, Customer? RegisteredCustomer) RegisterNewCustomer(string email)
{
    if(string.IsNullOrWhiteSpace(email))
        return (
            OutputMessageCollection: new [] {
                OutputMessage.CreateError(code: "Customer.Email.Should.Required")
            },
            ResultType: ResultType.Error, 
            RegisteredCustomer: null
        );
    
    // Seu código de processamento

    return (OutputMessageCollection: null, ResultType: ResultType.Success, RegisteredCustomer: customer);
}

public (OutputMessage[]? OutputMessageCollection, ResultType ResultType, Customer? RemovedCustomer) RemoveCustomer(string email)
{
    if(string.IsNullOrWhiteSpace(email))
        return (
            OutputMessageCollection: new [] {
                OutputMessage.CreateError(code: "Customer.Email.Should.Required")
            },
            ResultType: ResultType.Error, 
            RemovedCustomer: null
        );
    
    // Seu código de processamento

    return (OutputMessageCollection: null, ResultType: ResultType.Success, RemovedCustomer: customer);
}
```

O que podemos concluir desse código:

- Utilizar tupla é algo simples na linguagem C#, porém não é trivial. Programadores iniciantes podem ter dificuldade de lidar com a sintaxe, além do fato de que a tupla é um value type.
- Dependendo da quantidade de informações adicionais que você deseja saber sobre a execução dos métodos, a tupla terá vários parâmetros, tornando o código difícil de ler.
- Caso queiramos incluir uma nova informação no retorno dos métodos, teríamos que alterar todas as tuplas em todos os métodos e modificar todas as atribuições do retorno desses métodos para se adequarem à nova estrutura da tupla. Isso seria impraticável!

<br/> 

> [!CAUTION]
> Embora as tuplas sejam um recurso da linguagem, dependendo de como são utilizadas, podem gerar diversos problemas de design de código, causando dificuldades de leitura, compreensão e manutenção.

<br/> 

`Por esses motivos, não utilizaremos tuplas no retorno dos métodos`!

Agora vamos analisar a utilização de parâmetros de saída (output) nos métodos. Vamos examinar o mesmo código, porém, com variáveis de saída:


```csharp
public enum ResultType
{
    Success = 1,
    Partial = 2,
    Error = 3
}

public Customer? RegisterNewCustomer(string email, out OutputMessage[]? OutputMessageCollection, out ResultType ResultType)
{
    if(string.IsNullOrWhiteSpace(email))
    {
        OutputMessageCollection = new [] {
            OutputMessage.CreateError(code: "Customer.Email.Should.Required")
        };

        ResultType = ResultType.Error;

        return null;
    }
    
    // Seu código de processamento

    OutputMessageCollection = null;
    ResultType = ResultType.Success;

    return customer;
}

public Customer? RemoveCustomer(string email, out OutputMessage[]? OutputMessageCollection, out ResultType ResultType)
{
    if(string.IsNullOrWhiteSpace(email))
    {
        OutputMessageCollection = new [] {
            OutputMessage.CreateError(code: "Customer.Email.Should.Required")
        };

        ResultType = ResultType.Error;

        return null;
    }
    
    // Seu código de processamento

    OutputMessageCollection = null;
    ResultType = ResultType.Success;

    return customer;
}

// Exemplo do consumo
public bool Register(string email)
{
    RegisterNewCustomer(email, out OutputMessage[]? outputMessageCollection, out ResultType resultType);

    if(resultType != ResultType.Success)
        return false;

    // ...

    return true;
}
```

O que podemos concluir desse código:

- Todos os problemas apontados na utilização das tuplas ainda se aplicam.
- Obriga o código consumidor a declarar as variáveis de saída ou utilizar o operador de descarte, o que significa que uma alteração na assinatura resultaria também na modificação de todos os chamadores desses métodos.

#### :white_check_mark: Decisão de design 2: Precisamos criar um envelope de resposta para padronizar o retorno dos métodos

[voltar ao topo](#book-conteúdo)

Para evitar quebras de código durante a remoção ou inclusão de novas propriedades que queremos analisar sobre a execução do método e para padronizar toda a comunicação entre os métodos, é importante que criemos um envelope de resposta. O que seria isso?

Imagine uma carta em um envelope. Nós temos a carta, que é o nosso objeto de interesse, mas temos um envelope que tem informações adicionais sobre a carta, como o emissor, destinatário, selo postal etc. Note que o objeto de interesse é a carta, mas temos informações adicionais que vão além da carta e que também são importantes. Então, em vez de adicionarmos essas informações na própria carta, dificultando o trabalho da agência de correios, é melhor criarmos um envelope padronizado para facilitar a análise e deixar a carta dentro desse envelope, ou seja, encapsulamos a carta.

O raciocínio aqui é o mesmo: `vamos pegar todas aquelas informações extras que queremos da execução de um método em um envelope que vai encapsular a resposta do método`. Assim, conseguimos padronizar os retornos dentro do sistema e evitar os problemas mencionados anteriormente!

<br/>

> [!TIP]
> Criar encapsulamentos nos permite padronizar os objetos, melhorando a manutenibilidade e compreensão da aplicação!

<br/>

### :pushpin: Arrays vazios ou referências nulas para array de mensagens?

[voltar ao topo](#book-conteúdo)

Vamos analisar essas duas opções primeiro que acabamos de ver. Vamos começar pelo retorno de um array vazio ou um retorno nulo quando não houverem mensagens. Nós vamos analisar pelo viés da usabilidade e do desempenho.

Ao olhar pelo viés da usabilidade, é mais interessante ter um array vazio do que um valor nulo, pois evita possíveis exceções de referências nulas. Utilizar um array vazio vindo de uma constante do .NET como um `Array.Empty` também não causará pressão no Garbage Collector, pois não haverá instanciações de novos arrays. Para reproduzir e validar esse cenário que acabei de afirmar, vamos executar dois benchmarks (detalhes sobre os benchmarks podem ser encontrados na [documentação sobre benchmark](BENCHMARKS-PT.md)).

A seguir, temos parte do código do objeto [OutputEnvelop.cs](../src/OutputEnvelop/OutputEnvelop.cs). No construtor do objeto, recebemos alguns parâmetros e definimos algumas propriedades que são readonly.

Como a biblioteca é feita para o .NET Standard 2.0, não existe suporte para deixar explícito o nullable para arrays. No entanto, mesmo assim, é possível passar `null` no array. Sendo assim, os parâmetros `OutputMessage[] outputMessageCollection` e `Exception[] exceptionCollection` do construtor podem vir nulos.

Note que esses parâmetros do construtor mencionados anteriormente alimentam duas propriedades com o modificador de acesso `internal`. Nós falaremos sobre isso posteriormente.

O ponto agora é que o código atual permite que as propriedades `internal` tenham valores nulos, conforme demonstrado a seguir:


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

Uma alternativa para fazer com que esse objeto retorne um Array vazio em vez de permitir nulo seria verificar se o parâmetro do construtor é nulo e substituí-lo por uma constante de um Array vazio, conforme a seguir:


```csharp
// Versão que não aceita nulo
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

Para medir o impacto dessas duas versões, vamos executar o benchmark do arquivo [CreateOutputEnvelopBenchmark.cs](../benchs/Benchmarks/OutputEnvelopBenchs/CreateOutputEnvelopBenchmark.cs) e analisar o teste `CreateOutputEnvelopWithoutMessageAndException`, que passará uma coleção de mensagens e exceções nulas para o cenário que queremos testar. Para cada uma das variações acima, vamos executar o mesmo teste e analisar o retorno. Primeiro, vamos aos resultados brutos dos benchmarks:


| Type             | Method                                        | Mean (ns) | Error (ns) | StdDev (ns) | CacheMisses/Op | TotalIssues/Op | TotalCycles/Op | BranchInstructions/Op | BranchMispredictions/Op | Gen0 | Allocated (B) |
|------------------|-----------------------------------------------|-----------|------------|-------------|----------------|----------------|----------------|-----------------------|-------------------------|------|---------------|
| With null        | CreateOutputEnvelopWithoutMessageAndException | 8,603     | 0,0223     | 0,0186      | 0              | 47             | 16             | 11                    | 0                       | 0    | 0             |
| With empty array | CreateOutputEnvelopWithoutMessageAndException | 117,26    | 1,964      | 2,879       | 0              | 518            | 257            | 123                   | 0                       | 0    | 0             |

A primeira coluna (`Type`) refere-se aos nossos dois cenários passando o valor nulo ou o array vazio. Agora vamos analisar esses dados com mais detalhes.

Na coluna `Mean (ns)`, temos o tempo médio de execução, e já conseguimos notar algo bem impactante. A versão com o `valor nulo` executou em `8,6 nanosegundos`, enquanto a `versão com o Array.Empty` foi de `117,26 nanosegundos`. Isso quer dizer que o código passando o `Array.Empty` foi cerca de 13,6 vezes pior.

Na coluna `Erros (ns)`, temos o tempo total gasto com erros durante a execução, e ao comparar os dados, conseguimos ver que o código com o `Array.Empty` foi 88 vezes pior.

Na coluna `StdDev (ns)`, conseguimos ver o desvio padrão das execuções. Quanto menor o desvio padrão, mais estável o código é e menores serão os picos para baixo ou para cima. No arquivo sobre [benchmarks](BENCHMARKS-PT.md), há mais detalhes sobre o que é o desvio padrão. A versão com `Array.Empty` foi bem mais instável.

Quando analisamos `TotalIssues/Op`, `TotalCycles/Op` e `BranchInstructions/Op`, também vemos a clara diferença, onde com `Array.Empty` apresentou muito `mais erros e quantidade de instruções`.

Agora, quando analisamos a alocação de memória, ambos os cenários não geraram alocação.

O que podemos concluir disso?
- :white_check_mark: Conclusão 1 - A versão com o valor nulo é mais rápida.
- :white_check_mark: Conclusão 2 - A versão com o valor nulo é mais estável.
- :white_check_mark: Conclusão 3 - A versão com o valor nulo tem uma usabilidade pior, pois joga a responsabilidade de tratar o valor nulo para o método chamador.

Então, temos duas conclusões a favor de usar a opção com o valor nulo (todas relacionadas ao desempenho) e uma conclusão a favor de utilizar o Array.Empty (que está relacionada à usabilidade do código). Então, qual das duas abordagens escolher? Na verdade, não precisamos escolher uma ou outra; há uma alternativa que podemos utilizar para nos beneficiarmos das duas abordagens.

Para entender o ponto, vamos analisar o diagrama abaixo, que representa uma Web API com seus componentes internos (o objetivo desse diagrama não é apresentar um modelo de componentes de referência e nem dizer se a divisão é boa ou ruim; ele serve somente para o nosso exemplo).


<br/>
<div align="center">
  <img src="diagrams/diagram2.svg">
</div>
<br/>

Nesse diagrama, temos a representação de um Web App que solicita para uma Web API a importação de um pedido. Durante essa importação, tanto o pedido quanto os produtos do pedido são importados. Para ajudar a compreender esse fluxo, repare no diagrama de sequência a seguir:


<br/>
<div align="center">
  <img src="diagrams/diagram3.svg">
</div>
<br/>

Cada execução de cada método de cada componente retornaria um envelope de resposta. Nesse fluxo acima, teríamos os seguintes envelopes de resposta:

- Envelope de resposta da execução do método do componente ProductRepository.
- Envelope de resposta da execução do método do componente OrderRepository.
- Envelope de resposta da execução do método do componente ProductDomainService.
- Envelope de resposta da execução do método do componente OrderDomainService.
- Envelope de resposta da execução do método do componente ImportOrderUseCase.
- Envelope de resposta da execução do método do componente OrdersController.

De todos os seis envelopes de respostas que seriam criados, somente em um momento a leitura das notificações seria feita, marcado em laranja na OrdersControllers. Isso quer dizer que, no exemplo acima, `enquanto seis criações de envelopes de resposta são feitas, somente uma leitura das notificações é realizada`. Ou seja, nesse cenário de aplicações *LOB (Line of Business)*, iremos realizar muito mais criações de envelopes de respostas do que a leitura das notificações.

<br/>

> [!TIP]
> Devemos compreender o perfil de utilização de cada objeto.

<br/>

### :white_check_mark: Decisão de design 3: O tratamento para evitar null reference será feito na leitura da propriedade ao invés de ser feito na criação do objeto

[voltar ao topo](#book-conteúdo)

E onde isso ajuda a decidir se vamos usar referência nula ou array vazio?

Como vimos que criamos muito mais notificações do que lemos, podemos fazer algo simples, mas que vai resolver nosso problema e permitir usar o melhor dos dois cenários. `Podemos remover o código que resolveria um problema de leitura no momento da criação`!

Note novamente o código que usamos para tratar o Array.Empty com os devidos comentários no código:


```csharp
// Versão que não aceita nulo
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
        // Estamos resolvendo um problema da leitura no momento da instanciação
        // sendo que na maioria das vezes nós não vamos realizar essa leitura.
        // Com isso, estamos usando um processamento 100% das vezes para algo 
        // que não será usado o tempo todo
        OutputMessageCollectionInternal = outputMessageCollection ?? Array.Empty<OutputMessage>();
        ExceptionCollectionInternal = exceptionCollection ?? Array.Empty<Exception>();
    }
}
```

Agora, vamos remover essa tratativa de nulo do construtor e encapsular em uma propriedade. Note que, como a classe tem os arrays como propriedades `internal`, precisamos exportar esses valores publicamente para outras classes acessarem. A implementação da solução ficaria assim:


```csharp
public readonly struct OutputEnvelop<TOutput>
{
    // Properties
    internal OutputMessage[] OutputMessageCollectionInternal { get; }
    internal Exception[] ExceptionCollectionInternal { get; }

    // Incluimos uma propriedade para leitura que encapsula o array internal
    public IEnumerable<OutputMessage> OutputMessageCollection
    {
        get
        {
            // Tratamos o valor nulo no momento da leitura e evitamos a referência nula
            // somente no momento em que a leitura for solicitada
            if (OutputMessageCollectionInternal is null)
                yield break;

            for (int i = 0; i < OutputMessageCollectionInternal.Length; i++)
                yield return OutputMessageCollectionInternal[i];
        }
    }
    // Incluimos uma propriedade para leitura que encapsula o array internal
    public IEnumerable<Exception> ExceptionCollection
    {
        get
        {
            // Tratamos o valor nulo no momento da leitura e evitamos a referência nula
            // somente no momento em que a leitura for solicitada
            if (ExceptionCollectionInternal is null)
                yield break;

            for (int i = 0; i < ExceptionCollectionInternal.Length; i++)
                yield return ExceptionCollectionInternal[i];
        }
    }

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
        // Nós permitimos a atribuição do valor nulo pois
        // vamos tratar isso somente na leitura
        OutputMessageCollectionInternal = outputMessageCollection;
        ExceptionCollectionInternal = exceptionCollection;
    }
}
```

Com isso, conseguimos obter o desempenho da criação com valores nulos, mas mantemos a usabilidade de permitir uma leitura que evitará instâncias nulas! Vamos ver com mais detalhes a solução aplicada.


