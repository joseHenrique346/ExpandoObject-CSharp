## C# - ExpandoObject

### Definição
Em C#, um ExpandoObject é uma classe do namespace System.Dynamic que permite a criação e manipulação de objetos 
cujas propriedades e métodos podem ser adicionados ou removidos em tempo de execução, sem a necessidade de definir uma classe previamente. 
Ele funciona como um dicionário dinâmico, implementando a interface IDictionary<string, object>, o que oferece grande flexibilidade para trabalhar com estruturas de dados que não são conhecidas antecipadamente.

### Objetos Dinâmicos
Em C#, um objeto dinâmico é um objeto cuja estrutura (tipos de membros e seus tipos) não é conhecida até a execução, permitindo que membros sejam adicionados ou acessados sem validação em tempo de compilação.
Esta característica é útil para interagir com linguagens dinâmicas, objetos COM e dados de formatos flexíveis como JSON, embora seja recomendável usá-la com cautela devido ao impacto no desempenho e à perda da segurança de tipo em tempo de compilação. 

## Objetivo do projeto
Este projeto procedural foi desenvolvido para:
- Testar e explorar o funcionamento do ExpandoObject.
- Manipular propriedades em tempo de execução.
- Exercitar o uso de recursividade junto a estruturas dinâmicas.
- Aprimorar a prática em flexibilidade de dados dentro do C#.

## Exemplo de uso
```
using System;
using System.Dynamic;

class Program
{
    static void Main()
    {
        dynamic pessoa = new ExpandoObject();
        pessoa.Nome = "João";
        pessoa.Idade = 25;

        Console.WriteLine($"{pessoa.Nome} tem {pessoa.Idade} anos.");

        // Adicionando método em tempo de execução
        pessoa.DizerOla = (Action)(() => Console.WriteLine($"Olá, meu nome é {pessoa.Nome}!"));
        pessoa.DizerOla();
    }
}
```
