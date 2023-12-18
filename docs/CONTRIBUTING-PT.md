# Manual de Contribuição

> [!IMPORTANT]
> DOCUMENTO NÃO FINALIZADO! EM CONSTRUÇÃO

Se você tem vontade de contribuir com o projeto, sinta-se a vontade, toda contribuição é bem-vinda e incentivada.

## :book: Conteúdo
- [Manual de Contribuição](#manual-de-contribuição)
  - [:book: Conteúdo](#book-conteúdo)
  - [:pushpin: Código de conduta](#pushpin-código-de-conduta)
    - [:pushpin: Orientações](#pushpin-orientações)
  - [:pushpin: Decisões de Design](#pushpin-decisões-de-design)
  - [:pushpin: Issues](#pushpin-issues)
  - [:pushpin: Pull Requests](#pushpin-pull-requests)

## :pushpin: Código de conduta

Em um mundo onde as pessoas estão cada vez com menos empatia para com o outro e a internet, devido ao anonimato, encoraja com que as pessoas expressem sua opinião com menos filtro e cuidado com relação a outra pessoa, é importante que tomemos alguns cuidados nos tratos com as outras pessoas para que possamos ter uma comunidade saudável livre das discussões e pré-conceitos existentes internet a fora.

Todos são livres para se expressarem e são incentivados a isso. O que se é pedido é que tenhamos cuidado com relação a assuntos sensíveis que possam causar desconforto a outros. Devido a isso, instituí algumas orientações que peço que observem com cuidado e sigam as orientações por favor.

Eu quero que todos sejam muito bem-vindos, acolhidos e respeitados! 

### :pushpin: Orientações

- Conteúdo erótico ou sensual que tenha algum cunho sexual de qualquer espécie não são permitidos ainda mais pelo fato de que podem existir menores de idade conosco.
- Ofensas contra qualquer pessoa, seja por palavras, imagens ou qualquer outro meio não são permitidos. É importante que esse seja um ambiente livre para que possamos ter total liberdade de nos expressarmos sem o medo do julgamento e xingamentos.
- Qualquer discriminação ou injúria com relação a qualquer aspecto de qualquer pessoa não são permitidos.
- Discussões políticas, religiosas ou qualquer outro assunto sensível que possa causar desconforto, injúria ou discriminação com qualquer pessoa não são permitidos. 
- Qualquer discriminação com relação as dúvidas postadas pelos integrantes dessa comunidade.

## :pushpin: Decisões de Design

Contribuições com funcionalidades novas são bem-vindas e incentivadas. Peço que analise o documento sobre as [decisões de design](DESIGN-DECISIONS-PT.md) para que sua contribuição esteja de acordo com elas e esteja alinhada com o objetivo dessa biblioteca mantida nesse repositório. Caso queira criar variações dessa biblioteca com outras decisões de design, incentivo a fazer um fork do projeto para criar essa nova vertente e vai ser um prazer ajudar e incluir no [README](../README.md) sua variação.

## :pushpin: Issues

Caso tenha alguma dúvida sobre o projeto ou encontrou algum bug, peço que abra uma issue no projeto atribuindo as respectivas labels da sua solicitação.

## :pushpin: Pull Requests

Caso queira submeter alguma alteração para o projeto, será necessário abrir uma pull request. Essa pull request irá executar alguns processos e todos os checks da pipeline precisam ter passado para que a pull request seja aceita. As etapas que a pull request executam são:

- Build
- Testes de unidade
- SAST com SonarQube
- Teste de mutação
- Teste de segurança com CodeQL

Além dos requisitos da pipeline, analisarei os seguintes aspectos:

- O SAST está com o requisito default de 80% de cobertura, porém, aceitarei somente pull requests com:
  -  100% de cobertura de testes de unidade
  -  0 Bugs
  -  0 Vulberabilities
  -  0 Code smells
  -  0 Security Hotspots
  -  0% Duplications
- Benchmarks criados dentro do [projeto de benchmarks](../benchs/Benchmarks) seguindo as decisões de design evidenciando o impacto da modificação proposta.
- Descrição da pull request descrevendo os motivadores e as decisões tomadas para a implementação. 