<img src="/images/malogo.png" width="300" align="right" /><br><br><br>

# Introduction 
The Key Generator Service generates unique identifiers which can be used as part of the Stream API. It creates two type of IDs: a `long` and a `GUID`. This service can be used by any solution that wants a central key generator and avoid ID duplication within a distributed environment. The service will never return the same ID more than once, which ensures that multiple requests, even from different clients, will never result in the same ID being given for different things. The Key Generator Service is exposed via a gRPC server interface.

# Getting Started
## Server
### Docker
A Docker Image is provided to allow multiple applications to use the same Key Generator Service.
The image is provided in [DockerHub](https://hub.docker.com/r/mclarenapplied/keygenerator-proto-server).
#### Docker Compose
Use the following Docker Compose to use the Key Generator Image:
```
services: 
  key-generator-service:
    image: mclarenapplied/keygenerator-proto-server:latest
    ports:   
       - 15379:15379
    environment:
      PORT: 15379
```
