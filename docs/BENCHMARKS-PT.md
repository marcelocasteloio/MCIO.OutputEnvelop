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

## :pushpin: O que é um benchmark?

[voltar ao topo](#book-conteúdo)

Um benchmark é uma comparação entre duas ou mais coisas. Quando não estamos comparando com algo, estamos simplesmente fazendo uma medição e não um benchmark. Mas quando estamos falando sobre software, nós associamos o termo benchmark tanto para medir a execução de algum sistema ou componente dele quanto a comparação entre sistemas e/ou componentes.

<br/>

> [!TIP]
> Na prática, usamos o benchmark para medir e comparar o desempenho do sistema e/ou seus componentes
> 
<br/>

## :pushpin: O que comparar?

[voltar ao topo](#book-conteúdo)

Nós podemos medir e comparar diversas coisas quando estamos falando sobre software. Podemos medir o tempo em que o sistema demora para executar determinada tarefa, podemos medir o quanto de recursos (CPU, RAM, HD, Network etc) são utilizados durante determinada ação que o software executa, podemos medir quanto tempo o sistema demora para inicializar e estar completamente pronto para realizar suas tarefas e mais um monte de coisa.

No nosso caso, estamos falando de uma biblioteca que é executada por outros sistemas. Devido a isso, nós vamos realizar medições que façam sentido para esse cenário, ou seja, nós vamos medir:

- Tempo de execução.
- Alocação de memória.
- Quantidade ciclos de processamento.
- Erros que ocorreram.
- Estabilidade do código.

<br/>

> [!IMPORTANT]
> Nós devemos medir o que faz sentido para o cenário no qual nosso projeto foi idealizado

<br/>

Após a medição, nós vamos realizar a comparação entre as medições de diferentes variações do código para tirarmos conclusões a partir dos dados obtidos. Mas antes, vamos analisar alguns pontos que acabam se tornando verdeiros mitos quando falamos de benchmark.

## :pushpin: Mitos sobre benchmarks

[voltar ao topo](#book-conteúdo)

Existem mitos sobre benchmarks e o objetivo é falar sobre alguns desses mitos para que possamos ter uma compreensão mais clara sobre benchmarks e ter um alinhamento claro sobre alguns pontos.

### :pushpin: Mito 1: A única coisa que temos que avaliar é o tempo de execução do código

Podemos ter a impressão de que a única coisa que importa é a velocidade do código, ou seja, quanto mais rápido, melhor o código, mas isso não é uma verdade simples e absoluta.

Para entender melhor esse ponto, vamos fazer uma ilustração com duas máquinas industriais hipotéticas. Essas máquinas produzem um determinado produto. Foram produzidas uma amostra de dez produtos (claro que o ideal é produzir muito mais peças para comparar, mas para facilitar o exemplo, usaremos dez peças somente) e, para cada peça construida, anotamos o tempo que levou para cada peça. Vamos analisar os dados:

<center>

<table>

<td>

| Máquina A | Total: 50 minutos |
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

</td>

<td>

| Máquina B | Total: 50 minutos |
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

</td>

</table>

</center>

Note que tanto a `máquina A` quanto a `máquina B`  levaram um total de `50 minutos`. Já que ambas as máquinas demoraram o mesmo tempo total, quer dizer qeu ambas tiveram o mesmo desempenho? `NÃO!` Vamos analisar melhor.

Vamos expressar essas duas tabelas em dois gráficos:

<center>

<table>

<td>

![MaquinaA](images/machine-a.png)

</td>

<td>

![MaquinaA](images/machine-b.png)

</td>

</table>

</center>

