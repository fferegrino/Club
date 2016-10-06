# El Club
Aplicación para la gestión de sesiones, avisos, miembros ( y sus avances) del  Club de Algoritmia de la Escuela Superior de Cómputo del IPN.

## Tecnologías usadas  
 - Para la creación de la aplicación web se usó **ASP.NET MVC**, en su versión <code>6.0.0-rc1-final</code>
 - Para la capa de interacción con la base de datos se empleó el framework ORM **Entity Framework** en su versión <code>7.0.0-rc1-final</code>
 - El resto de las dependencias de la aplicación web se pueden encontrar en [el archivo project.json](https://github.com/fferegrino/Club/blob/master/src/Club/project.json#L7)
 - Para el almacenamiento de los datos se usó SQL Server, del cual son soportadas las versiones de 2008 en adelante.  
 
## Requisitos para la implementación  
### Servidor de aplicaciones  
Para montar la aplicación web es necesario un servidor de aplicaciones con:

 - Al menos 1 GB de memoria RAM,
 - 1 GB de espacio en disco,
 - procesador de un núcleo con al menos 2.20GHz de velocidad,
 - posibilidad de ejecutar aplicaciones ASP.NET  5, o ASP.NET Core. 

Favor de consultar el [siguiente enlace](https://docs.asp.net/en/latest/publishing/) en donde se detalla el proceso de publicación.

### Servidor de Base de Datos

 - Al menos 1 GB de memoria RAM,
 - 20 GB de espacio en disco,
 - procesador de un núcleo con al menos 2.20GHz de velocidad
 - SQL Server 2008 en adelante

**Recomendado:** publicar a una base de datos de Azure
