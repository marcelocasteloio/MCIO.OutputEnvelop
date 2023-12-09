# Decisões de design

> [!CAUTION]
> A leitura desse documento pode causar mal estar e atacar as crenças de alguns desenvolvedores pois, ao invés de tomar decisões na base de achismo, iremos fundamentar nossas decisões em medições e experimentos que podem causar danos aos egos de alguns, por isso, tome cuidado! (**isso foi só uma brincadeira com um tom de ironia, não fique bravo** :sweat_smile:)

## :book: Conteúdo
- [Decisões de design](#decisões-de-design)
  - [:book: Conteúdo](#book-conteúdo)
  - [:confused: Necessidade](#confused-necessidade)
  - [:thinking: Analisando as possibilidades](#thinking-analisando-as-possibilidades)

## :confused: Necessidade

Durante o processamento de um método em sistemas *LOB (Line of business)* (vou tomar a liberdade de me abster de algumas formalidades como ficar se referindo a procedimentos quando não existe retorno ou função como tem retorno, chamarei tudo de método) nós, com frequência, queremos mais do que um único objeto de retorno. Isso ocorre não porque estamos projetando métodos com múltiplas responsabilidades ferindo o primeiro princípio do SOLID, mas sim, porque tem informações complementares que são importantes para esse tipo de sistema.

> [!IMPORTANT]
> Existem vários tipos de sistemas com diferentes propósitos como sistemas para IoT, sistemas de missão crítica, sistemas para baixo nível etc. Esse pacote e toda análise foi pensado em sistemas *LOB (Line of business)*

Esses sistemas possuem algumas características comuns. Algumas delas são:
- Autenticar o usuário que está tentando realizar a operação.
- Autoprizar o usuário autenticado para a operação que está querendo realizar.
- Receber inputs dos usuários.
- Validar os inputs dos usuários.
- Validar os estados dos objetos de negócio.
- Realizar algum processo de negõcio que modifique o estado dos objetos de negócio.
- Persistir essas informações.
- Retornar o resultado da solicitação para o usuário.
- Exibir diversos relatórios a partir das informações armazenadas.

Ao atender esses cenários, algumas necessidades surgem e são comuns em todo desenvolvimento, independentemente da regra de negócio aplicada.

## :thinking: Analisando as possibilidades