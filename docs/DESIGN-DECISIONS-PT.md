# Decisões de design

> [!CAUTION]
> A leitura desse documento pode causar mal estar e atacar as crenças de alguns desenvolvedores pois, ao invés de tomar decisões na base de achismo, iremos fundamentar nossas decisões em medições e experimentos que podem causar danos aos egos de alguns, por isso, tome cuidado! (**isso foi só uma brincadeira com um tom de ironia, não fique bravo** :sweat_smile:)

## :book: Conteúdo
- [Decisões de design](#decisões-de-design)
  - [:book: Conteúdo](#book-conteúdo)
  - [:confused: Necessidade](#confused-necessidade)

## :confused: Necessidade

Durante o processamento de um método em sistemas *LOB (Line of business)* (vou tomar a liberdade de me abster de algumas formalidades como ficar se referindo a procedimentos quando não existe retorno ou função como tem retorno, chamarei tudo de método) nós, com frequência, queremos mais do que um único objeto de retorno. Isso ocorre não porque estamos projetando métodos com múltiplas responsabilidades ferindo o primeiro princípio do SOLID, mas sim, porque tem informações complementares que são importantes para esse tipo de sistema.

> [!IMPORTANT]
> Existem vários tipos de sistemas com diferentes propósitos como sistemas para IoT, sistemas de missão crítica, sistemas para baixo nível etc. Esse pacote e toda análise foi pensado em sistemas *LOB (Line of business)*


