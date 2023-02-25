## Getting Stated

### Prerequisites 

```
dotnet tool install --global dotnet-ef  
```

### Run migrations

```
dotnet ef database update InitMigration
```

### Configure Debezium

```
curl -i -X POST -H "Accept:application/json" -H "Content-Type:application/json" localhost:8083/connectors/ -d @debezium/connector-config/todo-connector.json
```

### Create topic

```
kafka-topics.sh --create --topic todo.public.Todos --bootstrap-server kafka:9092
```

### Connect to Kafka

```
docker compose exec kafka bash
```

```
kafka-console-consumer.sh --bootstrap-server kafka:9092 --topic todo.public.Todos --from-beginning
```

### View Debezium state

```
curl -i -H "Accept:application/json" localhost:8083/
```

### List the connectors 

```
curl -i -H "Accept:application/json" localhost:8083/connectors/
```

### List the Kafka topics 

```
kafka-topics.sh --list --zookeeper zookeeper:2181
```