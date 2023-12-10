# MCIO.OutputEnvelop

Um `envelope de resposta` que seja `leve`, de `alto desempenho` e que forneça uma `API de alto nível` para ser usado em aplicações *LOB (line of business)*.

## :label: Labels

| Categoria | Descrição | Labels |
|-|-|-|
| Licença | MIT | [![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/) |
| Nuget | MarceloCastelo.IO.OutputEnvelop | ![Nuget](https://img.shields.io/nuget/v/MarceloCastelo.IO.OutputEnvelop) |
| Segurança | Vulnerabilidades | [![CodeQL](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/github-code-scanning/codeql) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Visão Geral | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Cobertura de testes | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=coverage)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Teste de mutação | [![Mutation Test](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/mutation-test.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/mutation-test.yml) |
| Qualidade | Manutenabilidade | [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Confiabilidade | [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Bugs | [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=bugs)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Dívidas técnicas | [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Linhas duplicadas (%) | [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Melhorias de código | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Pipeline | Compilação e Testes | [![Build and Test](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/build-and-test.yml) |
| Pipeline | Publicação | [![Publish](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/publish.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/publish.yml) |

## :page_facing_up: Introdução

> [!IMPORTANT]
> Existem vários outros pontos que são abordados na documentação do projeto e vale a pena dar uma olhada para que possa compreender os racionais das decisões tomadas nesse projeto lendo o arquivo [decisões de design](docs/DESIGN-DECISIONS-PT.md).

Esse projeto nasceu de uma necessidade pessoal. Quando estamos desenvolvendo um sistema temos que tomar diversas decisões com relação ao design do código e manter o equilíbrio entre legibilidade, manutenabilidade e performance é sempre um desafio pois, no fim das contas, nós escrevemos código para outra pessoa entender e não para o computador.

Como assim? Existem diferentes tipos de sistemas, mas o mais comum são os que chamamos de sistemas *LOB (line of business)* que são aplicações que tem como o objetivo automatizar processos de negócios em corporações como processos de venda, compras, pedidos, chamados, suportes, processos financeiros etc. Esses sistemas possuem algumas características que costumam se repetir:
- Autenticar o usuário que está tentando realizar a operação.
- Autoprizar o usuário autenticado para a operação que está querendo realizar.
- Receber inputs dos usuários.
- Validar os inputs dos usuários.
- Validar os estados dos objetos de negócio.
- Realizar algum processo de negõcio que modifique o estado dos objetos de negócio.
- Persistir essas informações.
- Retornar o resultado da solicitação para o usuário.
- Exibir diversos relatórios a partir das informações armazenadas.

Além disso, nesses tipos de sistemas, os retornos dos métodos nesses tipos de sistemas vão além de um único objeto e é comum querermos saber mais informações do que somente se deu erro ou não, por exemplo:
- Queremos saber as notificações que ocorreram durante a execução dos métodos.
- Essas notificações são mais do que simples mensagens de erro, podem ser mensagens de warning (por exemplo: quando um pedido de compra ultrapassa determinado valor), podem ser mensagens informativas (por exemplo: informar a integração com os parceiros foi realizada com sucesso durante o processamento da requisição) etc.
- O processamento nem sempre se resume a sucess ou falha. Em importações em lote por exemplo, o resultado da operação pode ser parcial onde partes dos itens do lote são processados e outra parte não.

## :book: Conteúdo
- [MCIO.OutputEnvelop](#mciooutputenvelop)
  - [:label: Labels](#label-labels)
  - [:page\_facing\_up: Introdução](#page_facing_up-introdução)
  - [:book: Conteúdo](#book-conteúdo)
  - [:computer: Tecnologias](#computer-tecnologias)
  - [:star: Funcionalidades-chave](#star-funcionalidades-chave)
  - [:books: Exemplos](#books-exemplos)
  - [:people\_holding\_hands: Contribuindo](#people_holding_hands-contribuindo)
  - [:people\_holding\_hands: Autores](#people_holding_hands-autores)

## :computer: Tecnologias

[voltar ao topo](#book-conteúdo)

Esse projeto utiliza as seguintes tecnologias:
- `C#` como linguagem de programação.
- `.NET Standard 2.0` para o pacote nuget.
- `.NET 8` para para os projetos de teste de unidade, benchmark e exemplos.
- `xUnit` como framework de testes de unidade.
- `FluentAssertions` para escrita dos Asserts dos testes de unidade de forma fluída,
- `SonarQube` para ferramenta de análise estática de código (SAST - *Static Application Security Testing*),
- `Stryker.NET` como framework para testes de mutação.
- `BenchmarkDotNet` como framework para realização dos benchmarks.
- `Github Actions` para as pipelines.
- `Nuget.org` como repositório de pacotes.

## :star: Funcionalidades-chave

[voltar ao topo](#book-conteúdo)

Esse projeto tem como objetivo fornecer um `envelope de resposta` para os métodos da aplicação que sigam os seguintes princípios de design: 

- :white_check_mark: Seja otimizado para alocação na stack ao invés da heap para `evitar pressão no garbage collector`.
- :white_check_mark: Tenha a característica da `imutabilidade` para ter a garantia que uma vez criado, não será modificado e que, alterações implicarão na criação de um novo objeto.
- :white_check_mark: Ter uma `API de alto nível` para que seja flexível para ser utilizado em diversos cenários com pouca necessidade de customização do código.
- :white_check_mark: Seja otimizado para `não realizar box e unboxing` e `evitar criação de closures` nos encapsulamentos para não gerar alocações na heap.
- :white_check_mark: Evitar o `uso desnecessário` e `incorreto de lançamento de exceções` ocasionando problemas de desempenho.
- :white_check_mark: Ser `Thread-safe`.

## :books: Exemplos

Para que possam ver, na prática, a utilização do envelope de resposta nos diferentes cenários para o qual ele foi projetado, criei uma API de exemplo localizando dentro do diretório `samples/SampleApi`.

## :people_holding_hands: Contribuindo

[voltar ao topo](#book-conteúdo)

Você está mais que convidado para constribuir. Caso tenha interesse e queira participar do projeto, não deixe de ver nosso [manual de contribuição](docs/CONTRIBUTING-PT.md). 

## :people_holding_hands: Autores

[voltar ao topo](#book-conteúdo)

- Marcelo Castelo Branco - [@MarceloCas](https://www.linkedin.com/in/marcelocastelobranco/)
