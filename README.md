# MCIO.OutputEnvelop

- Seja otimizado para alocação na stack ao invés da heap evitando pressão no garbage collector

- Tenha a característica da imutabilidade para ter a garantia que uma vez criado, não será modificado e que, alterações implicarão na criação de um novo objeto

- Seja flexível para ser utilizado em diversos cenários com pouca necessidade de customização do código

- Seja otimizado para não realizar box e unboxing evitando criação de closures nos encapsulamentos

- Thread-safe