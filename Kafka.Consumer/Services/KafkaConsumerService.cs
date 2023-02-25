using Confluent.Kafka;

namespace Kafka.Consumer.Services
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly string _topic = "todo.public.Todos";
        private readonly ILogger<KafkaConsumerHandler> _logger;


        public KafkaConsumerHandler(ILogger<KafkaConsumerHandler> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "group1",
                BootstrapServers = "localhost:29092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(_topic);
                CancellationTokenSource cancelToken = new CancellationTokenSource();

                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cancelToken.Cancel();
                };

                try
                {
                    _logger.LogInformation("Listening Kafka topic...");

                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        
                        _logger.LogInformation($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    builder.Close();
                }

                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}