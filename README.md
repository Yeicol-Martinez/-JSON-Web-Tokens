# Usuarios API REST — JWT + SQL Server (.NET 10)

## Requisitos
- .NET 10 SDK
- SQL Server (local o Express)

## Configurar la conexión

Abre `appsettings.json` y ajusta la cadena de conexión:

```json
"DefaultConnection": "Server=localhost;Database=UsuariosDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

Si usas usuario y contraseña en lugar de Windows Auth:
```json
"DefaultConnection": "Server=localhost;Database=UsuariosDB;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
```

## Ejecutar por primera vez

En la Consola del Administrador de Paquetes de Visual Studio:

```powershell
Add-Migration InitialCreate
Update-Database
```

Luego presiona F5 o ejecuta `dotnet run`.

Swagger estará en: `http://localhost:5000`

## Flujo de prueba

1. `POST /api/auth/register` — crea tu cuenta
2. Copia el `accessToken` de la respuesta
3. Clic en **Authorize** 🔒 en Swagger → pega el token
4. Prueba los endpoints de `/api/usuarios`
5. `POST /api/auth/refresh` — cuando el token expire

## Endpoints

| Método | Ruta | Auth |
|--------|------|------|
| POST | /api/auth/register | ❌ |
| POST | /api/auth/login | ❌ |
| POST | /api/auth/refresh | ❌ |
| POST | /api/auth/logout | ❌ |
| GET | /api/usuarios | ✅ JWT |
| GET | /api/usuarios/{id} | ✅ JWT |
| GET | /api/usuarios/me | ✅ JWT |
| POST | /api/usuarios | ✅ JWT |
| PUT | /api/usuarios/{id} | ✅ JWT |
| DELETE | /api/usuarios/{id} | ✅ JWT |
