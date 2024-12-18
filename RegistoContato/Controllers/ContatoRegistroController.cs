using CoreContato;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RegistoContato.Controllers
{
    [ApiController]
    [Route("/RegistroContato")]
    public class ContatoRegistroController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostRegistroContato([FromBody] Contato contato)
        {
            var contatoInstanciado = new Contato(

                contato.id_contato,
                contato.nome_contato,
                contato.telefone_contato,
                contato.email_contato
            );

            // Validação simples do objeto recebido
            if (contato == null || string.IsNullOrEmpty(contato.nome_contato))
            {
                return BadRequest("Dados do contato inválidos.");
            }

            // Configurando conexão com RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            // Publicando a mensagem no RabbitMQ
            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    // Declarando a fila
                    channel.QueueDeclare(
                        queue: "filaContato",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    // Serializando o objeto contato
                    var message = JsonSerializer.Serialize(contatoInstanciado);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    // Publicando a mensagem
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "filaContato",
                        basicProperties: null,
                        body: body
                    );
                }

                return Ok("Mensagem enviada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}
