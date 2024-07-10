<img src="/images/malogo.png" width="300" align="right" /><br><br><br>

# Introduction 
The Key Generator Service generates unique Data Format identifiers used as part of the Stream API. It is used by the MA Data Format Management service which is also part of the Stream API, but may also be used by third parties wishing to allocate data format IDs that are unique. The service will never return the same ID more than once, which ensures that multiple requests, even from different clients, will never result in the same ID being given to different data formats. The key generator service is exposed via a gRPC server interface.

# Getting Started
## Server
### Docker
A Docker Image is provided to allow multiple applications to use the same Key Generator Service.
The image is provided in [DockerHub](https://hub.docker.com/r/mclarenapplied/keygenerator-proto-server).
#### Docker Compose
Use the following Docker Compose to use the Key Generator Image:
```
services:  
  zookeeper:
    image: confluentinc/cp-zookeeper:latest    
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000
    ports:
      - 12181:2181
    
  kafka:
    image: confluentinc/cp-kafka:latest
    depends_on:
      - zookeeper 
    ports:
      - 9092:9092    
      - 9094:9094    
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:9093,PLAINTEXT_HOST://kafka:9092,PLAINTEXT_LOCAL_HOST://localhost:9094
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT,PLAINTEXT_LOCAL_HOST:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1

  key-generator-service:
    image: mclarenapplied/keygenerator-proto-server:latest
    ports:    
      - 15379:15379
    environment:
      PORT: 15379

  stream-api-server:
    image: mclarenapplied/streaming-proto-server-host:latest
    ports:
      - 13579:13579
      - 10010:10010

    depends_on:
      - kafka
      - key-generator-service
    restart: always
    environment:     
      CONFIG_PATH: /Configs/AppConfig.json
      AUTO_START: true
    volumes:
      - ./Configs:/app/Configs
```
