# MCIO.OutputEnvelop

Um **envelope de resposta** que seja **leve**, de **alto desempenho** e que forneça uma **API de alto nível** para ser usado em aplicações *LOB (line of business)*.

## Status

| Categoria | Descrição | Labels |
|-|-|-|
| Nuget | MarceloCastelo.IO.OutputEnvelop | ![Nuget](https://img.shields.io/nuget/v/MarceloCastelo.IO.OutputEnvelop) |
| Segurança | Vulnerabilidades | [![CodeQL](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/github-code-scanning/codeql) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Visão Geral | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Cobertura de testes | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=coverage)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Teste de mutação | |
| Qualidade | Manutenabilidade | [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Confiabilidade | [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Bugs | [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=bugs)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Dívidas técnicas | [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Linhas duplicadas (%) | [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Qualidade | Melhorias de código | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=marcelocasteloio_MCIO.OutputEnvelop&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=marcelocasteloio_MCIO.OutputEnvelop) |
| Pipeline | Compilação e Testes | [![Build and Test](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/build-and-test.yml) |
| Pipeline | Publicação | [![Publish](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/publish.yml/badge.svg)](https://github.com/marcelocasteloio/MCIO.OutputEnvelop/actions/workflows/publish.yml) |



## Introdução

Esse projeto nasceu de uma necessidade pessoal. Quando estamos desenvolvimento um sistema temos que tomar diversas decisões com relação ao design do código e manter o equilíbrio entre legibilidade, manutenabilidade e performance é sempre um desafio pois, no fim das contas, nós escrevemos código para outra pessoa entender e não para o computador.

Como assim? Existem diferentes tipos de sistemas, mas o mais comum são os que chamamos de sistemas *LOB (line of business)* que são aplicações que tem como o objetivo automatizar processos de negócios em corporações como processos de venda, compras, pedidos, chamados, suportes, processos financeiros etc. Esses sistemas possuem algumas características que costumam se repetir:
- Autenticar o usuário que está tentando realizar a operação
- Autoprizar o usuário autenticado para a operação que está querendo realizar
- Receber inputs dos usuários
- Validar os inputs dos usuários
- Validar os estados dos objetos de negócio
- Realizar algum processo de negõcio que modifique o estado dos objetos de negócio
- Persistir essas informações
- Retornar o resultado da solicitação para o usuário
- Exibir diversos relatórios a partir das informações armazenadas

## Funcionalidades-chave

Esse projeto tem como objetivo fornecer um **envelope de resposta** para os métodos da aplicação que sigam os seguintes princípios de design: 

- Seja otimizado para alocação na stack ao invés da heap para **evitar pressão no garbage collector**

- Tenha a característica da **imutabilidade** para ter a garantia que uma vez criado, não será modificado e que, alterações implicarão na criação de um novo objeto

- Ter uma **API de alto nível** para que seja flexível para ser utilizado em diversos cenários com pouca necessidade de customização do código

- Seja otimizado para **não realizar box e unboxing** e **evitar criação de closures** nos encapsulamentos para não gerar alocações na heap

- Ser **Thread-safe**

