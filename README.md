# Objetivo de repositorio
Este proyecto se encarga de manejar la persistencia de datos para la gestion de una aplicación de inventario

## Requisitos
1. SQL Server
  * Usuario con rol **dbmanager** para pode crear base de datos
3. Visual Studio **2017 o superior** [_En caso de correr aplicación en modo debug_]
4. [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) (**SDK** Modo debug o **Runtime** en mode deploy)

## Instrucciones de uso
Para poder hacer uso del servicio **REST** es muy importante que en la configuracion del proyecto _appsettings.json_ se cambie la **cadena de conexión** con el nombre de usuario y contraseña para poder conectarse al sevicio a una base de datos
 ```json
 "ConnectionStrings": {
    "ExamElipgo": "Server=(URLServer);Initial Catalog=ExamenJulio;Persist Security Info=False;User ID=(usuario);Password=(contraseña)"
  }
 ```
#### Advertencias
>Si el usuario de base de datos no tiene permisos de creación de base de datos simplemente la aplicación no responsera aun que mostrara la definición de los **EndPoints**
