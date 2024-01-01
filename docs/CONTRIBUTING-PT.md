# Manual de Contribuição

Se você tem vontade de contribuir com o projeto, sinta-se à vontade; toda contribuição é bem-vinda e incentivada.

## :book: Conteúdo
- [Manual de Contribuição](#manual-de-contribuição)
  - [:book: Conteúdo](#book-conteúdo)
  - [:pushpin: Decisões de Design](#pushpin-decisões-de-design)
  - [:pushpin: Issues](#pushpin-issues)
  - [:pushpin: Pull Requests](#pushpin-pull-requests)

:pushpin: Código de Conduta

Num mundo onde a empatia diminui entre as pessoas, e a internet, devido ao anonimato, encoraja a expressão de opiniões sem filtro e cuidado, é crucial agir com consideração para manter uma comunidade saudável, livre de discussões e preconceitos.

Todos são livres para se expressar e são incentivados a fazê-lo. No entanto, pede-se cuidado ao abordar assuntos sensíveis que possam causar desconforto. Para isso, algumas orientações foram estabelecidas, e solicito que as sigam atentamente.

Desejo que todos sejam bem-vindos, acolhidos e respeitados!

:pushpin: Orientações

- Conteúdo erótico ou sensual com cunho sexual não é permitido, especialmente devido à possível presença de menores de idade.
- Ofensas, seja por palavras, imagens ou qualquer outro meio, não são toleradas. Este é um ambiente para expressão livre sem medo de julgamento ou xingamentos.
- Discriminação ou injúria em relação a qualquer aspecto de uma pessoa são proibidas.
- Discussões políticas, religiosas ou de qualquer natureza sensível que possam causar desconforto, injúria ou discriminação são vedadas.
- Qualquer discriminação em relação às dúvidas postadas pelos membros desta comunidade.

## :pushpin: Decisões de Design

Contribuições com novas funcionalidades são bem-vindas e incentivadas. Recomendo analisar o documento sobre as [decisões de design](DESIGN-DECISIONS-PT.md) para garantir que sua contribuição esteja alinhada com os objetivos da biblioteca mantida neste repositório. Se desejar criar variações com outras decisões de design, encorajo a criar um fork do projeto, e ficarei feliz em ajudar e incluir sua variação no [README](../README.md) como sugestão de variação.

## :pushpin: Issues

Em caso de dúvidas sobre o projeto ou se encontrar algum bug, abra uma issue no projeto atribuindo as labels correspondentes à sua solicitação.

## :pushpin: Pull Requests

Se deseja submeter alguma alteração, é necessário abrir uma pull request. Esta passará por diversos processos, sendo que todos os checks da pipeline precisam ser aprovados para que a pull request seja aceita. As etapas incluem:

- Build
- Testes de unidade
- SAST com SonarQube
- Teste de mutação
- Teste de segurança com CodeQL

Além dos requisitos da pipeline, analisarei os seguintes aspectos:

- Aceitarei apenas pull requests com:
  -  100% de cobertura de testes de unidade
  -  0 Bugs
  -  0 Vulberabilities
  -  0 Code smells
  -  0 Security Hotspots
  -  0% Duplications
- Benchmarks criados dentro do [projeto de benchmarks](../benchs/Benchmarks), seguindo as decisões de design, evidenciando o impacto da modificação proposta.
- Descrição da pull request, descrevendo os motivadores e as decisões tomadas para a implementação.