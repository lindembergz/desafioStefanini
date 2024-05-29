<b><h3>Desafio stefanini</h3></b>


<b>Arquitetura em Camadas:</b>

O projeto está organizado em diferentes camadas, como Domain, Data, App e Api.
A camada Domain contém as entidades de negócio (ProdutoEntity, PedidoEntity, ItemPedidoEntity) e regras de validação (Validation).
A camada Data é responsável pelo mapeamento e acesso aos dados, contendo os repositórios e o contexto do Entity Framework Core.
A camada App abriga os serviços de aplicação, ViewModels e mapeamentos entre os modelos de domínio e ViewModels utilizando AutoMapper.
A camada Api contém os controladores e a configuração da API RESTful.


<b>Alinhamento com Domain-Driven Design (DDD):</b>

As entidades de negócio (ProdutoEntity, PedidoEntity, ItemPedidoEntity) são classes ricas em comportamento e regras de negócio.
Cada entidade possui um método Validar() para garantir a consistência dos dados.
As classes de entidade são agrupadas em namespaces separados por domínio (Estoque e Venda).
As regras de validação são encapsuladas na classe Validation, seguindo o padrão de Serviços de Domínio.


<b>Alinhamento com SOLID:</b>

Single Responsibility Principle (SRP): As classes têm responsabilidades bem definidas, como as entidades de domínio, repositórios, serviços e controladores.
Open/Closed Principle (OCP): As classes estão abertas para extensão, mas fechadas para modificação, por meio da utilização de interfaces e injeção de dependência.
Liskov Substitution Principle (LSP): As classes derivadas (ProdutoEntity, PedidoEntity, ItemPedidoEntity) são substituíveis por suas classes base (Entity<T>).
Interface Segregation Principle (ISP): As interfaces são coesas e específicas para cada caso de uso, como IProdutoRepository, IPedidoRepository, IProdutoService e IPedidoService.
Dependency Inversion Principle (DIP): As dependências são invertidas por meio da injeção de dependência, como a utilização de IMapper e os repositórios injetados nos serviços.


<b>Alinhamento com Clean Code:</b>

O código é organizado em namespaces e camadas lógicas, promovendo a separação de responsabilidades e a manutenibilidade.
As classes e métodos possuem nomes descritivos e expressivos.
O código segue convenções de nomenclatura adequadas.
Algumas classes, como Validation, aplicam o princípio "Don't Repeat Yourself" (DRY), fornecendo métodos utilitários para validação.


<b>API RESTful:</b>

O projeto implementa uma minimal API RESTful  para gerenciar pedidos e produtos.
Os controladores definem as rotas e operações RESTful, como GET, POST, PUT e DELETE.
As operações de negócio são delegadas aos serviços de aplicação (PedidoService e ProdutoService).
As respostas da API seguem as melhores práticas, como o uso de códigos de status HTTP apropriados.


<b>Observações:</b>

O projeto utiliza o Entity Framework Core como ORM para interagir com o banco de dados SQLite.
As camadas são mapeadas usando o AutoMapper, facilitando a conversão entre os modelos de domínio e ViewModels.
O projeto adota a injeção de dependência por meio do contêiner de injeção de dependência do .NET Core.
O projeto está integrado com Swagger para documentar e testar a API RESTful.


<b><h3>Estrutura do Projeto</h3></b>

O projeto é dividido em diversos namespaces, cada um responsável por uma parte específica da aplicação. A seguir, apresentamos uma visão geral dos principais namespaces e suas respectivas responsabilidades.

<b>1. Stefanini.Core.DomainObjects</b>

Contém as classes e funcionalidades básicas utilizadas por outros módulos do projeto.

<b>Classe:</b>

DomainException: Classe para tratamento de exceções de domínio.

Entity<T>: Classe base para entidades do domínio, com uma propriedade Id e um método EhValido para validação da entidade.
Validation: Classe com métodos estáticos para validação de regras de negócios.

<b>2. Stefanini.Estoque.Domain.Entities</b>

Contém as entidades relacionadas ao estoque.

<b>Classe:</b>

<b>ProdutoEntity:</b> Representa um produto no estoque, com propriedades NomeProduto e Valor. Inclui um método Validar para garantir que o nome do produto não seja vazio.

<b>3. Stefanini.Estoque.Domain.Repository.Interfaces</b>

Define a interface para o repositório de produtos.

<b>Interface:</b>

<b>IProdutoRepository:</b> Define métodos para obtenção e inserção de produtos.

<b>4. Stefanini.Estoque.Data.Mappings</b>

Configura mapeamentos de entidades para o Entity Framework.

<b>Classe:</b>

<b>ProdutoMapping:</b> Configura o mapeamento da entidade ProdutoEntity para a tabela "Produto".

<b>5. Stefanini.Estoque.Data.Repositories.Implementation</b>

Implementação da interface IProdutoRepository.

<b>Classe:</b>

<b>ProdutoRepository:</b> Implementa métodos para obter e inserir produtos no banco de dados.

<b>6. Stefanini.Estoque.Data.Context</b>

Define o contexto do Entity Framework para o módulo de estoque.

<b>Classe:</b>

<b>EstoqueContext:</b> Contexto do Entity Framework para gerenciar a conexão com o banco de dados e as entidades relacionadas ao estoque.

<b>7. Stefanini.Estoque.App.AutoMapper</b>

Configurações do AutoMapper para mapeamento entre entidades de domínio e view models.

<b>Classe:</b>

<b>DomainToViewModelProduto:</b> Configura mapeamento de ProdutoEntity para ProdutoViewModel.<br>
<b>ViewModelToDomainProduto:</b> Configura mapeamento de ProdutoViewModel para ProdutoEntity.

<b>8. Stefanini.Estoque.App.Services.Interfaces</b>

Define a interface para o serviço de produtos.

<b>Interface:</b>

<b>IProdutoService:</b> Define métodos para obter e criar produtos.

<b>9. Stefanini.Estoque.App.Services.Implementation</b>

Implementação dos serviços de produtos.

<b>Classe:</b>

<b>ProdutoService:</b> Implementa os métodos definidos na interface IProdutoService.<br>

<b>10. Stefanini.Estoque.App.ViewModel</b>

Define os modelos de visualização para produtos.

<b>Classe:</b>

<b>ProdutoViewModel:</b> Modelo de visualização para ProdutoEntity.<br>

<b>11. Stefanini.Venda.Domain.Entities</b>

Contém as entidades relacionadas a vendas.

<b>Classe:</b>

<b>PedidoEntity:</b> Representa um pedido, com propriedades para o cliente, pagamento e itens do pedido.<br>
<b>ItemPedidoEntity:</b> Representa um item em um pedido, com propriedades para o produto, quantidade e valor unitário.<br>

<b>12. Stefanini.Venda.Domain.Repository.Interfaces</b>

Define a interface para o repositório de pedidos.

<b>Interface:</b>

<b>IPedidoRepository:</b> Define métodos para gerenciar pedidos.<br>

<b>13. Stefanini.Venda.Data.Mappings</b>

Configura mapeamentos de entidades para o Entity Framework.

<b>Classe:</b>

<b>PedidoMapping:</b> Configura o mapeamento da entidade PedidoEntity para a tabela "Pedido".<br>
<b>ItemPedidoMapping:</b> Configura o mapeamento da entidade ItemPedidoEntity para a tabela "ItemPedido".<br>

<b>14. Stefanini.Venda.Data.Repositories.Implementation</b>

Implementação da interface IPedidoRepository.

<b>Classe:</b>

<b>PedidoRepository:</b>  Implementa métodos para gerenciar pedidos no banco de dados.

<b>15. Stefanini.Venda.Data.Context</b>

Define o contexto do Entity Framework para o módulo de vendas.

<b>Classe:</b>

<b>VendaContext:</b>  Contexto do Entity Framework para gerenciar a conexão com o banco de dados e as entidades relacionadas a vendas.<br>

<b>16. Stefanini.Estoque.App.AutoMapper</b>

Configurações do AutoMapper para mapeamento entre entidades de domínio e view models.

<b>Classe:</b>

<b>DomainToViewModelPedido:</b>  Configura mapeamento de PedidoEntity para PedidoViewModel.<br>
<b>ViewModelToDomainPedido:</b>  Configura mapeamento de PedidoViewModel para PedidoEntity.<br>

<b>17. Stefanini.Venda.App.Services.Interfaces</b>

Define a interface para o serviço de pedidos.

<b>Interface:</b>

<b>IPedidoService:</b> Define métodos para gerenciar pedidos.

<b>18. Stefanini.Venda.App.Services.Implementation</b>

Implementação dos serviços de pedidos.

<b>Classe:</b>

<b>PedidoService:</b> Implementa os métodos definidos na interface IPedidoService.

<b>19. Stefanini.Venda.App.ViewModels</b>

Define os modelos de visualização para pedidos e itens de pedidos.

<b>Classe:</b>

<b>PedidoViewModel:</b> Modelo de visualização para PedidoEntity.<br>
<b>ItemPedidoViewModel:</b> Modelo de visualização para ItemPedidoEntity.

<b>20. Stefanini.Api.Setup</b>

Configuração de injeção de dependência e registro de serviços.

<b>Classe:</b>

<b>DependencyInjection:</b>  Configura os serviços para injeção de dependência.

<b>21. DesafioAPI.Api.Pedido.Controllers</b>

Define os endpoints da API para o gerenciamento de pedidos.

<b>Classe:</b>

<b>PedidoController:</b>  Configura os endpoints para criar, atualizar, obter e deletar pedidos.


<b>Configuração do Banco de Dados:</b>

No arquivo appsettings.json, configure a string de conexão com o banco de dados:

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=dbDesafio.db"
  }
}
