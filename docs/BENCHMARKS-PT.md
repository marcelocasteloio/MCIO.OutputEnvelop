# Benchmarks

> [!IMPORTANT]
> DOCUMENTO NÃO FINALIZADO! EM CONSTRUÇÃO


## :book: Conteúdo
- [Benchmarks](#benchmarks)
  - [:book: Conteúdo](#book-conteúdo)
  - [:pushpin: O que é um benchmark?](#pushpin-o-que-é-um-benchmark)
  - [:pushpin: O que comparar?](#pushpin-o-que-comparar)
  - [:pushpin: Mitos sobre benchmarks](#pushpin-mitos-sobre-benchmarks)
    - [:pushpin: Mito 1: A única coisa que temos que avaliar é o tempo de execução do código](#pushpin-mito-1-a-única-coisa-que-temos-que-avaliar-é-o-tempo-de-execução-do-código)
    - [:pushpin: Mito 2: O código que eu escrevi é o mesmo que é executado](#pushpin-mito-2-o-código-que-eu-escrevi-é-o-mesmo-que-é-executado)

## :pushpin: O que é um benchmark?

[voltar ao topo](#book-conteúdo)

Um benchmark é uma comparação entre duas ou mais coisas. Quando não estamos comparando com algo, estamos simplesmente fazendo uma medição e não um benchmark. Mas quando estamos falando sobre software, associamos o termo benchmark tanto para medir a execução de algum sistema ou componente quanto para a comparação entre sistemas e/ou componentes.

<br/>

> [!TIP]
> Na prática, usamos o benchmark para medir e comparar o desempenho do sistema e/ou seus componentes.

> 
<br/>

## :pushpin: O que comparar?

[voltar ao topo](#book-conteúdo)

Nós podemos medir e comparar diversas coisas quando estamos falando sobre software. Podemos medir o tempo em que o sistema demora para executar determinada tarefa, podemos medir o quanto de recursos (CPU, RAM, HD, Network, etc.) são utilizados durante determinada ação que o software executa, podemos medir quanto tempo o sistema demora para inicializar e estar completamente pronto para realizar suas tarefas, e mais um monte de coisa.

No nosso caso, estamos falando de uma biblioteca que é executada por outros sistemas. Devido a isso, vamos realizar medições que façam sentido para esse cenário. Ou seja, vamos medir:

- Tempo de execução.
- Alocação de memória.
- Quantidade de ciclos de processamento.
- Erros que ocorreram.
- Estabilidade do código.


<br/>

> [!IMPORTANT]
> Devemos medir o que faz sentido para o cenário no qual nosso projeto foi idealizado.

<br/>

Após a medição, vamos realizar a comparação entre as medições de diferentes variações do código para tirarmos conclusões a partir dos dados obtidos. Mas antes, vamos analisar alguns pontos que acabam se tornando verdadeiros mitos quando falamos de benchmark.

## :pushpin: Mitos sobre benchmarks

[voltar ao topo](#book-conteúdo)

Existem mitos sobre benchmarks, e o objetivo é falar sobre alguns desses mitos para que possamos ter uma compreensão mais clara sobre benchmarks e ter um alinhamento claro sobre alguns pontos.

### :pushpin: Mito 1: A única coisa que temos que avaliar é o tempo de execução do código

[voltar ao topo](#book-conteúdo)

Podemos ter a impressão de que a única coisa que importa é a velocidade do código, ou seja, quanto mais rápido, melhor o código, mas isso não é uma verdade simples e absoluta.

Para entender melhor esse ponto, vamos fazer uma ilustração com duas máquinas industriais hipotéticas. Essas máquinas produzem um determinado produto. Foram produzidas uma amostra de dez produtos (claro que o ideal é produzir muito mais peças para comparar, mas para facilitar o exemplo, usaremos dez peças somente) e, para cada peça construída, anotamos o tempo que levou para cada peça. Vamos analisar os dados:

<div align="center">

<table>

<td>

| Máquina A ||
|:----:|:----:|
| Número peça | Duração (minutos) |
|1|5|
|2|4|
|3|6|
|4|5|
|5|5|
|6|6|
|7|6|
|8|4|
|9|4|
|10|5|
| Total | 50 minutos |
| Média | 5 minutos |
| Desvio Padrão | 0,77 |

</td>

<td>

| Máquina B ||
|:----:|:----:|
| Número peça | Duração (minutos) |
|1|6|
|2|3|
|3|7|
|4|4|
|5|4|
|6|7|
|7|7|
|8|3|
|9|3|
|10|6|
| Total | 50 minutos |
| Média | 5 minutos |
| Desvio Padrão | 1,67 |

</td>

</table>

</div>

Note que tanto a `máquina A` quanto a `máquina B` levaram um total de `50 minutos`. Já que ambas as máquinas demoraram o mesmo tempo total, quer dizer que ambas tiveram o mesmo desempenho? `NÃO!` Vamos analisar melhor.

<br/>

> [!IMPORTANT]
> Códigos com eficiências diferentes podem ter o mesmo tempo de execução.

<br/> 

Vamos expressar essas duas tabelas em dois gráficos:

<br/>
<div align="center">

![MaquinaA](diagrams/diagram4.png)

</div>
<br/>

Incluir uma série (em laranja) que representa a média móvel. Mas por quê isso? Quanto mais próximo o valor (série em azul) estiver da média móvel (série em laranja), menos variação teve, ou seja, mais estável e previsível o valor é.

Note que a `máquina A` ficou com os valores `mais próximos` da média móvel, já a `máquina B` ficou com valores `mais distantes` da média móvel. Isso quer dizer que a máquina B é mais instável. Então o que podemos concluir da análise dessas duas máquinas?

**Conclusões:**
- As máquinas A e B utilizaram o mesmo tempo total para produzir as 10 peças.
- A máquina A foi mais estável e previsível na duração por peça do que a máquina B.
- O pico de tempo mais alto da máquina A foi de 6 minutos, enquanto na máquina B foi de 7 minutos.

Para uma linha de produção de peças, buscamos `previsibilidade`. Portanto, embora as duas máquinas tenham demorado o mesmo tempo total para produzir as 10 peças, a `máquina A` trouxe uma previsibilidade de tempo muito maior, pois a duração da produção das peças ficou mais próxima da média móvel.

Medimos essa distância do valor da média móvel a partir de uma medida estatística chamada `desvio padrão`. Os desvios padrões das máquinas A e B foram:

<div align="center">

| Máquina | Desvio Padrão |
| :-: | :-: |
| Máquina A | 0,77 |
| Máquina B | 1,67 |

</div>

<br/>

> [!IMPORTANT]
> O desvio padrão é o controle de qualidade da média: quanto mais baixo, menos a média varia, ou seja, é mais estável!

<br/> 

Como isso se aplica com benchmarks em programação? Note o resultado de um benchmark que fizemos no documento [decisões de design](DESIGN-DECISIONS-PT.md) quando estávamos analisando sobre Exceptions:

| Type             | Method                                        | Mean (ns) | Error (ns) | StdDev (ns) | CacheMisses/Op | TotalIssues/Op | TotalCycles/Op | BranchInstructions/Op | BranchMispredictions/Op | Gen0 | Allocated (B) |
|------------------|-----------------------------------------------|-----------|------------|-------------|----------------|----------------|----------------|-----------------------|-------------------------|------|---------------|
| With null        | CreateOutputEnvelopWithoutMessageAndException | 8,603     | 0,0223     | 0,0186      | 0              | 47             | 16             | 11                    | 0                       | 0    | 0             |
| With empty array | CreateOutputEnvelopWithoutMessageAndException | 117,26    | 1,964      | 2,879       | 0              | 518            | 257            | 123                   | 0                       | 0    | 0             |

Note que a coluna `StdDev (ns)` tem o menor valor do `desvio padrão` para o teste `With null` em relação ao teste `With empty array`. Isso quer dizer que o teste `With null` foi muito mais estável com relação ao tempo de execução. Na coluna `Mean (ns)` conseguimos comprovar que o teste `With null` foi mais rápido, mas caso utilizassem o mesmo tempo ou tivessem um tempo mais próximo, deveríamos utilizar o valor do desvio padrão na análise também!

Se estivéssemos falando de memória RAM, por exemplo, poderíamos ter um código que demorasse o mesmo tempo, mas, por causa do desvio padrão alto, poderiam ocorrer picos de uso de memória RAM, exigindo um servidor com uma quantidade maior de memória RAM, por exemplo: o código executa com 500 MB de memória RAM, mas como o uso de memória está com um alto desvio padrão, em algum momento esse código atinge um pico de 900 MB de memória. Isso quer dizer que teríamos que ter reservado (em uma máquina, um cluster K8S etc.) um total de 1 GB de memória somente para atender a um possível pico de consumo de memória. Um código mais estável poderia demorar o mesmo tempo final, mas, por ter um desvio padrão menor, não ultrapassaria 750 MB de RAM, por exemplo.

Vamos finalizar esse assunto vendo um gráfico hipotético de uso de memória RAM de dois programas que demoram o mesmo tempo para executar o processo:

<br/>
<div align="center">

![MaquinaA](diagrams/diagram5.png)

</div>
<br/>

Embora tanto o `Programa A` quanto o `Programa B` demorem o `mesmo tempo para executar`, conseguimos ver como o `Programa B varia muito mais no uso de memória RAM`. Isso quer dizer que enquanto o `Programa A` executaria com até `1 GB de memória RAM`, o `Programa B` precisaria de `1,5 GB de memória RAM` devido à sua instabilidade no uso de memória RAM, ou seja, o `Programa A é mais eficiente no uso de memória` embora ambos utilizem o mesmo tempo de execução.


<br/>

> [!IMPORTANT]
> Códigos mais estáveis permitem reservar menos recursos de hardware para a aplicação

<br/> 

### :pushpin: Mito 2: O código que eu escrevi é o mesmo que é executado

[voltar ao topo](#book-conteúdo)

Nós podemos acreditar que o código gerado é o mesmo que executamos, mas isso não é verdade. Independentemente de sua aplicação ser compilada ou interpretada, o código executado não coincide exatamente com o que você escreveu. Se você deseja entender mais sobre esse assunto, recomendo assistir ao vídeo do Fabio Akita, disponível neste [link](https://www.youtube.com/watch?v=SNyh-cubxaU).

O ponto crucial é que, ao escrevermos o código para benchmark, é imperativo levar isso em consideração. Vou explicar brevemente e de forma resumida (apenas o necessário para o exemplo) o que ocorre no .NET quando compilamos nosso código no modo Release. Observe o diagrama a seguir:


<br/>
<div align="center">

![MaquinaA](diagrams/diagram6.svg)

</div>
<br/>

Vamos compreender o que está acontecendo:

- Temos nosso projeto C# (arquivo .csproj) com seus respectivos arquivos de código (arquivos .cs).
- **Passo 1:** Executamos o comando `dotnet build -c Release` para compilar o projeto no modo `Release`.
- **Passo 2:** Durante a compilação no modo Release, ocorre um processo de `otimização` em nosso código realizado pelo `Roslyn`, o compilador do C#.
- **Passo 3:** O resultado desse processo é o nosso assembly.
- **Passo 4:** Utilizamos o comando `dotnet` para executar o código contido no arquivo gerado no passo 3.
- A `CLR (Common Language Runtime)` executa nosso assembly.

No .NET 8, houve a inclusão do suporte ao [AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=net8plus%2Cwindows), mas isso não impactará no ponto abordado em nosso exemplo.

<br/>

> [!IMPORTANT]
> A compilação no modo Release realiza otimizações em nosso código durante o processo de compilação.


<br/> 

Essa otimização realizada tem um impacto direto em nosso benchmark. Vamos analisar alguns exemplos de códigos que escrevemos e como eles se comportarão após a compilação no modo Release:

- Exemplo 1

<center>

<table>

<td>

```csharp
// Código original
public class SampleClass {
    public int SampleMethod() {
        
        var number1 = 10;
        var number2 = 12;
        var sum = number1 + number2;
        
        return sum;
    }
}
```

</td>

<td>

```csharp
// Código compilado em modo release
public class SampleClass
{
    public int SampleMethod()
    {
        int num = 10;
        int num2 = 12;
        return num + num2;
    }
}

```

</td>

</table>

</center>

- Exemplo 2

<center>

<table>

<td>

```csharp
// Código original
public class SampleClass {
    public String SampleMethod(string[] names) {
        
        var sb = new StringBuilder();
        
        foreach(var name in names)
        {
            sb.Append(name);
        }
        
        return sb.ToString();
    }
}



```

</td>

<td>

```csharp
// Código compilado em modo release
public class SampleClass
{
    [NullableContext(1)]
    public string SampleMethod(string[] names)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        while (num < names.Length)
        {
            string value = names[num];
            stringBuilder.Append(value);
            num++;
        }
        return stringBuilder.ToString();
    }
}
```

</td>

</table>

</center>

- Exemplo 3

<center>

<table>

<td>

```csharp
// Código original
public class SampleClass {
    public String SampleMethod1(
        String logradouro, 
        String numero
    )
    {
        return $"{logradouro}, n {numero}";
    }
    public String SampleMethod2(
        String logradouro, 
        String numero, 
        String bairro, 
        String cidade, 
        String estado
    ) 
    {
        return $"{logradouro}, n {numero} - {bairro} - {cidade}/{estado}";
    }
}















```

</td>

<td>

```csharp
// Código compilado em modo release
[NullableContext(1)]
[Nullable(0)]
public class SampleClass
{
    public string SampleMethod1(
      string logradouro, 
      string numero
    )
    {
        return string.Concat(logradouro, ", n ", numero);
    }

    public string SampleMethod2(
      string logradouro, 
      string numero, 
      string bairro, 
      string cidade, 
      string estado
    )
    {
        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = 
          new DefaultInterpolatedStringHandler(11, 5);
        defaultInterpolatedStringHandler.AppendFormatted(logradouro);
        defaultInterpolatedStringHandler.AppendLiteral(", n ");
        defaultInterpolatedStringHandler.AppendFormatted(numero);
        defaultInterpolatedStringHandler.AppendLiteral(" - ");
        defaultInterpolatedStringHandler.AppendFormatted(bairro);
        defaultInterpolatedStringHandler.AppendLiteral(" - ");
        defaultInterpolatedStringHandler.AppendFormatted(cidade);
        defaultInterpolatedStringHandler.AppendLiteral("/");
        defaultInterpolatedStringHandler.AppendFormatted(estado);
        return defaultInterpolatedStringHandler.ToStringAndClear();
    }
}
```

</td>

</table>

</center>

Observe como o código sofreu alterações significativas, principalmente nos loops. Portanto, ao realizar um benchmark, é crucial considerar essas mudanças e escrever um código de benchmark que represente de perto o cenário real. Como demonstrado no exemplo 3, até mesmo os objetos que serão utilizadas no código podem sofrer alterações.


<br/>

> [!IMPORTANT]
> O benchmark deve ser semelhante ao cenário real para evitar medições com situações que não ocorreram em produção.

<br/> 