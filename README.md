# GeekShopping - E-commerce com Microsserviços 🛒

Este repositório contém um sistema de e-commerce desenvolvido em .NET 8 e C# usando arquitetura de microsserviços. O projeto foi criado durante um curso de microsserviços para .NET e inclui diversas APIs que gerenciam diferentes partes do sistema, como carrinho, pagamento, produtos, e muito mais. O Ocelot é utilizado para o gerenciamento do gateway de API, facilitando a comunicação entre os serviços.

## 🛠️ Tecnologias Utilizadas

- **.NET 8**: Framework principal para o desenvolvimento de todos os microsserviços.
- **Ocelot**: Biblioteca de gateway API para .NET, utilizada para roteamento e agregação de APIs.
- **Entity Framework Core**: ORM para manipulação de dados nas APIs.
- **IdentityServer**: Gerenciamento de autenticação e autorização.
- **RabbitMQ** (ou outro message bus): Mensageria para comunicação entre serviços.
- **Docker**: Contêineres para isolar e gerenciar cada microsserviço.

## 📂 Estrutura de Pastas

A estrutura de pastas do projeto está organizada da seguinte forma:

- **GeekShopping.APIGateway**: Configuração do gateway usando Ocelot para gerenciamento de requisições entre os serviços.
- **GeekShopping.CartAPI**: API de gerenciamento do carrinho de compras.
- **GeekShopping.CouponAPI**: API de cupons de desconto.
- **GeekShopping.Email**: Serviço para envio de e-mails.
- **GeekShopping.Gateway**: Outra camada de gateway ou configuração de balanceamento.
- **GeekShopping.IdentityServer**: Gerenciamento de autenticação e autorização dos usuários.
- **GeekShopping.MessageBus**: Mensageria para comunicação assíncrona entre os microsserviços.
- **GeekShopping.OrderApi**: API de processamento de pedidos.
- **GeekShopping.PaymentProcessor**: Lógica para processamento de pagamentos.
- **GeekShopping.PaymentAPI**: API de pagamentos, responsável por interagir com sistemas de pagamento.
- **GeekShopping.ProductApi**: API para gerenciamento de produtos.
- **GeekShopping.Web**: Frontend do e-commerce.

## 📌 Funcionalidades

- **Autenticação e Autorização**: Implementado com o IdentityServer para proteger os endpoints e gerenciar o acesso.
- **Carrinho de Compras**: Gestão do carrinho de compras com funcionalidades de adição, remoção e atualização de produtos.
- **Pedidos**: Criação e gerenciamento de pedidos, incluindo status de processamento.
- **Pagamentos**: Integração com processadores de pagamento para finalizar as compras.
- **Cupons**: Aplicação de cupons de desconto no carrinho.
- **Notificações por E-mail**: Envio de e-mails para notificação de ações específicas.
- **APIGateway com Ocelot**: Roteamento e balanceamento de carga entre as APIs.

## 🚀 Executando o Projeto

### Pré-requisitos

- .NET SDK 8.0
- Docker (para subir os contêineres de cada serviço)
- RabbitMQ (ou outro message bus, caso aplicável)
- Banco de dados SQL (pode ser configurado em cada API)

### Passo a Passo

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/SeuUsuario/GeekShopping.git
   cd GeekShopping
   ```

2. **Configuração de Variáveis de Ambiente**:
   - Cada serviço pode precisar de variáveis de ambiente específicas, como conexões de banco de dados e configurações de RabbitMQ. Edite os arquivos `appsettings.json` em cada projeto ou configure as variáveis de ambiente.

3. **Execute os Contêineres**:
   - Utilize Docker Compose para subir os contêineres de cada serviço:
     ```bash
     docker-compose up -d
     ```

4. **Iniciar o Gateway e APIs**:
   - Inicie o Ocelot Gateway (`GeekShopping.APIGateway`) e as APIs principais.

5. **Acessar o Frontend**:
   - Com todos os serviços rodando, o frontend estará acessível em uma URL como `http://localhost:5000`.

## 🖥️ Principais Endpoints

- **API Gateway (Ocelot)**: `http://localhost:5000`
- **CartAPI**: `http://localhost:5001/api/cart`
- **CouponAPI**: `http://localhost:5002/api/coupon`
- **ProductAPI**: `http://localhost:5003/api/product`
- **OrderAPI**: `http://localhost:5004/api/order`
- **PaymentAPI**: `http://localhost:5005/api/payment`

## 📝 Aprendizados e Desafios

Este projeto abordou os principais conceitos de microsserviços, como comunicação entre serviços, segurança com autenticação e autorização, persistência de dados com EF Core, e escalabilidade com uso de contêineres Docker. Além disso, o uso do Ocelot para roteamento entre APIs trouxe um aprendizado prático sobre como estruturar gateways em projetos modernos.

## 👤 Contato

Desenvolvido por [Davi Jedrzejczyk](https://www.linkedin.com/in/davi-jedrzejczyk-03b22a245/). Conecte-se comigo no LinkedIn para discutir mais sobre desenvolvimento em .NET e microsserviços!

---

Este projeto de e-commerce em microsserviços é uma aplicação prática para estudar integração, escalabilidade e organização de código usando a arquitetura moderna de microsserviços.
