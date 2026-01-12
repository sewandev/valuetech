# Setup Local de Prueba Técnica

![Windows](https://img.shields.io/badge/OS-Windows_10%2F11-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![Docker](https://img.shields.io/badge/Container-Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![.NET](https://img.shields.io/badge/Framework-.NET_10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server_2022-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

## Prerrequisitos

Para garantizar la correcta ejecución de la solución, debe contar con el siguiente software instalado:

*   **[Docker Desktop para Windows](https://www.docker.com/products/docker-desktop/)**: Configurado con *WSL 2 backend*.
*   **[Git for Windows](https://git-scm.com/download/win)**: Para el control de versiones.
*   **(Opcional) [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)**: Solo si se desea compilar o depurar fuera de los contenedores.

## Pasos de Inicialización

### 1. Clonar el Repositorio

```powershell
git clone https://github.com/sewandev/valuetech.git
cd valuetech
```

### 2. Seleccione su Método de Despliegue

<details>
<summary><strong>Opción 1: Full Docker (Recomendado)</strong></summary>

Ejecute el siguiente comando en PowerShell en la raíz del proyecto:

```powershell
docker compose up --build -d
```
> **Nota**: La primera ejecución puede tomar unos minutos mientras se descargan las imágenes base y se compila el código .NET.

#### Verificación de Servicios
Una vez finalizado el arranque, el sistema expondrá los siguientes servicios locales:

| Componente | URL de Acceso | Descripción |
| :--- | :--- | :--- |
| **Aplicación Web** | `http://localhost:5000` | Frontend MVC para usuarios finales. |
| **API REST** | `http://localhost:5105` | Backend y Swagger UI. |
| **SQL Server** | `localhost:1433` | Base de datos (Puerto estándar). |

#### Acceso Inicial
El sistema aprovisiona automáticamente una base de datos con datos semilla y un usuario administrador por defecto.

*   **Usuario**: `admin`
*   **Contraseña**: `admin123`

</details>

---

<details>
<summary><strong>Opción 2: Híbrido (Propósito General / Debugging)</strong></summary>

Esta opción levanta solamente la base de datos en Docker, permitiendo ejecutar la API y la Web localmente con `dotnet run` para tareas de depuración y desarrollo rápido.

#### Paso 2.1: Levantar infraestructura de datos
Ejecute solo los servicios de soporte (SQL Server y Migrator):

```powershell
docker compose up sqlserver migrator -d
```
Espere a que el contenedor `valuetech_migrator` finalice (use `docker ps` para verificar cuando se detenga).

#### Paso 2.2: Ejecutar Backend (API)
Abra una terminal en `ValueTech.Api` y ejecute:

```powershell
cd ValueTech.Api
dotnet run
```
La API estará disponible en `http://localhost:5105`.
> **Swagger UI**: Puede explorar los endpoints en `http://localhost:5105/swagger`.

#### Paso 2.3: Ejecutar Frontend (Web)
Abra una **nueva** terminal en `ValueTech.Web` y ejecute:

```powershell
cd ValueTech.Web
dotnet run --urls "http://localhost:5000"
```
La Web estará disponible en `http://localhost:5000`.

> **Nota**: Asegúrese de tener el SDK de .NET 10 instalado para esta opción.

</details>

---

<details>
<summary><strong>Solución de Problemas Comunes</strong></summary>

*   **Error de Base de Datos ("Cannot open database")**: Espere unos segundos. El contenedor `valuetech_migrator` debe finalizar la ejecución de los scripts de inicialización antes de que la API pueda conectar.
*   **Puertos Ocupados**: Asegúrese de que los puertos `5000`, `5105` y `1433` no estén en uso por otras aplicaciones (como IIS Express o una instancia local de SQL Server).

</details>

---

<details>
<summary><strong>Ejecución de Pruebas Unitarias</strong></summary>

El proyecto incluye una suite de pruebas unitarias con **xUnit** para validar la lógica de negocio crítica.

Para ejecutar las pruebas, abra una terminal en la raíz del repositorio y ejecute:

```powershell
dotnet test
```

Si desea ver el detalle de cada prueba ejecutada:

```powershell
dotnet test --logger "console;verbosity=detailed"
```

Alternativamente, **si no tiene .NET SDK instalado localmente**, puede ejecutar las pruebas usando un contenedor efímero:

```powershell
docker run --rm -v ${PWD}:/app -w /app mcr.microsoft.com/dotnet/sdk:10.0 dotnet test ValueTech.Tests/ValueTech.Tests.csproj
```

</details>

---

### 3. Accesos y Recursos Dashboard

Una vez desplegado el entorno (Docker o Híbrido), utilice los siguientes accesos:

| Componente | URL | Credenciales / info |
| :--- | :--- | :--- |
| **Frontend Web** | [`http://localhost:5000`](http://localhost:5000) | User: `admin` / `admin123` |
| **API Swagger** | [`http://localhost:5105/swagger`](http://localhost:5105/swagger) | API Key: `ValueTech-Secret-Key-2026` (Botón Authorize) |
| **API Endpoint** | [`http://localhost:5105`](http://localhost:5105) | Base URL para Postman |
| **SQL Server** | `localhost:1433` | User: `sa` / `ValueTech_2024!` |

---