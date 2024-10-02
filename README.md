# RetoTecnicoBack

Se crearon 4 Controladores,
Categorías, Productos, Roles y Usuarios

Estos Controladres están siendo alimentados de 4 tablas en la base de datos, las cuales guardan las siguientes relaciones: 
![image](https://github.com/user-attachments/assets/6b6767c0-4255-430f-90fc-69ac00e2bdbf)
 
La tabla Usuarios tiene una relación con Roles
La tabla Productos tiene una relación con Usuarios y Categorías

Se realiza una validación por el ID de Roles con los Claims, este mismo haciendo referencia al ID - 1 de la tabla Roles que es ADMIN, solo permite el uso de ciertos ENDPOINTS a los usuarios que tengan este ROL asignado

Asimismo se intregó un JWT token para los Enpoints, el cual es creado al momento de realizar el login.
Se realizaron las siguientes validaciones con éxito
	Emails únicos para los usuarios.
	Nombre y precio requeridos para los productos.

Por un tema de tiempo, no logré integrar una tabla y su crud extra más de Pedidos, el cual pensaba sobre ella como una tabla cabecera y Detalle, teniendo como detalle una lista de productos asignados a un pedido. 
