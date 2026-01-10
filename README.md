# Evaluación técnica ValueTech

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

## Documentación

* [Requerimientos del desarrollo](ValueTech.Docs/PruebaDesarrollador.md)

## Stack Tecnológico

La solución ha sido construida utilizando un stack tecnológico moderno y robusto, seleccionado para cumplir con los estándares de rendimiento y escalabilidad de nivel empresarial.

### Backend & Core
*   **Lenguaje**: C# 13 / .NET 10
*   **API Framework**: ASP.NET Core Web API
*   **Arquitectura**: 3-Layer (Data, Service, API) + Repository Pattern
*   **Testing**: xUnit, Moq, Coverage Analysis

### Base de Datos
*   **Motor**: SQL Server 2022 (Ejecutándose en Contenedor Docker)
*   **Integración**: ADO.NET Puro (Sin ORM) para máximo control y rendimiento
*   **Lógica de Datos**: 100% Stored Procedures
*   **Tipos Avanzados**: Manipulación nativa de columnas `XML`
*   **Operaciones**: Uso estricto de `MERGE` para actualizaciones atómicas

### Frontend
*   **Framework**: ASP.NET Core MVC (.NET 10)
*   **Motor de Vistas**: Razor View Engine
*   **Estilos**: Bootstrap 5 + CSS Personalizado
*   **Comunicación**: `HttpClient` fuertemente tipado (ApiClient)

### Infraestructura & Herramientas
*   **Contenerización**: Docker & Docker Compose
*   **IDE Recomendado**: Visual Studio Code / Visual Studio 2022