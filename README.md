# SistemaVentas (.NET 8)

> Proyecto de referencia para demostrar conocimientos de desarrollo Backend con .NET.

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Backend-239120?logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-6DB33F)
![Docker](https://img.shields.io/badge/Docker-Container-2496ED?logo=docker)
![JWT](https://img.shields.io/badge/JWT-Authentication-000000)
![Serilog](https://img.shields.io/badge/Serilog-Logging-orange)
![FluentValidation](https://img.shields.io/badge/FluentValidation-Validation-blue)

---

# Descripción

**SistemaVentas** es un proyecto desarrollado con **.NET 8** cuyo objetivo es demostrar conocimientos de arquitectura, buenas prácticas y tecnologías utilizadas habitualmente en el desarrollo de APIs Backend.

> **Importante**
>
> Este proyecto fue desarrollado **exclusivamente con fines educativos y de portafolio**.
>
> **No intenta representar una aplicación de negocio completa ni un sistema listo para producción.**
>
> Su propósito es mostrar mi forma de diseñar, organizar e implementar una API utilizando buenas prácticas de desarrollo.
>
> En un proyecto real existirían muchas más reglas de negocio, validaciones, procesos, pruebas, controles de seguridad, monitoreo y funcionalidades específicas según los requerimientos del cliente.

---

# Objetivos del proyecto

Este proyecto busca demostrar experiencia en:

* Arquitectura limpia (Clean Architecture)
* Principios SOLID
* Repository Pattern
* Dependency Injection
* Desarrollo de APIs REST
* Entity Framework Core
* SQL Server
* Autenticación JWT
* Logging
* Caché en memoria
* Background Services
* Versionado de APIs
* Validación de modelos
* Docker
* Testing

La intención es que cualquier desarrollador o líder técnico pueda recorrer el proyecto y comprender cómo suelo estructurar una aplicación Backend.

---

# Tecnologías utilizadas

* .NET 8
* ASP.NET Core Web API
* C#
* SQL Server
* Entity Framework Core
* ADO.NET (implementación alternativa)
* Swagger / OpenAPI
* JWT Authentication
* Serilog
* FluentValidation
* MemoryCache
* Hosted Services (BackgroundService)
* Health Checks
* API Versioning
* Docker
* Docker Compose
* xUnit
* Moq

---

# Arquitectura

El proyecto está organizado siguiendo **Clean Architecture**, manteniendo una clara separación de responsabilidades.

```text
SistemaVentas.Api
│
├── Controllers
├── Middleware
└── Configuración

SistemaVentas.Application
│
├── DTOs
├── Interfaces
├── Services
└── Validators

SistemaVentas.Domain
│
├── Entities
└── Reglas de dominio

SistemaVentas.Infrastructure
│
├── Data
├── Repositories
└── Security

SistemaVentas.Tests
```

Cada capa posee una única responsabilidad y las dependencias siempre apuntan hacia el dominio.

---

# Funcionalidades implementadas

## API REST

* CRUD de Productos
* CRUD de Pedidos
* Paginación
* Filtros

## Persistencia

* SQL Server
* Entity Framework Core
* Implementación alternativa mediante ADO.NET

## Seguridad

* JWT Authentication
* Autorización mediante Bearer Token

## Logging

* Serilog
* Generación automática de archivos de log diarios

## Middleware

* Manejo global de excepciones

## Performance

* MemoryCache
* Invalidación de caché
* BackgroundService para tareas programadas

## Calidad

* xUnit
* Fake Repositories
* Introducción a Moq
* FluentValidation

## Infraestructura

* Docker
* Docker Compose
* SQL Server en contenedor

## API

* Swagger
* API Versioning (v1)

---

# Organización del proyecto

La solución intenta mantener una clara separación entre:

* Presentación
* Aplicación
* Dominio
* Infraestructura

La lógica de negocio reside en **Application**, mientras que el acceso a datos se abstrae mediante interfaces y repositorios.

---

# Ejecución

## Desde Visual Studio

1. Abrir la solución.
2. Establecer **SistemaVentas.Api** como proyecto de inicio.
3. Ejecutar mediante IIS Express o Kestrel.

---

## Mediante Docker

```bash
docker compose up -d --build
```

Swagger:

```text
http://localhost:8080/swagger
```

---

# Testing

El proyecto incluye pruebas unitarias utilizando:

* xUnit
* Fake Repositories
* Moq

El objetivo no es únicamente validar el funcionamiento del código, sino también mostrar distintas estrategias para aislar dependencias y probar la lógica de negocio.

---

# Próxima evolución

Este repositorio corresponde al **Proyecto 1**, implementado como un **Monolito**.

El siguiente paso consiste en desarrollar una segunda versión evolucionando esta misma aplicación hacia una arquitectura de **Microservicios**, incorporando tecnologías como:

* RabbitMQ
* Comunicación entre servicios
* API Gateway
* Docker Compose distribuido
* Kubernetes

De esta manera será posible comparar un mismo dominio implementado primero como monolito y posteriormente como una solución distribuida.

---

# Autor

**Sergio Hernán Rossi**

Desarrollador Backend especializado en:

* C#
* .NET
* SQL Server
* Entity Framework Core
* APIs REST
* Arquitectura de aplicaciones
* Docker
* Clean Architecture

---

> Este proyecto forma parte de mi portafolio profesional y fue desarrollado con el objetivo de demostrar conocimientos técnicos, organización del código y buenas prácticas de desarrollo Backend.
