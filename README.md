# 🛠 metalurgicaMVC

Es un sistema para la gestión de los trabajos realizados por una empresa metalurgica.
Permite administrar via web a los clientes y los trabajo solicitados por estos.
También permite administrar los detalles de estos trabajos y los materiales utilizados en estos.
Tiene un sistema de autenticación de usuarios y restricción de funcionalidades en base al rol de estos.

---

## 📌 Tabla de Evidencias de Requerimientos Mínimos

| Requerimiento Mínimo | Estado | Enlace al Código / Controladores | Evidencia (Captura / GIF / Video) |
| :--- | :---: | :--- | :--- |
| **1. 4 Tablas y Relación 1:N** | ✅ Cumplido | [DataContext.cs](Repositories/DBContext.cs) | [DER](docs/DER.jpg) |
| **2. Login, Roles y Avatar** | ✅ Cumplido | [UsuarioController.cs](Controllers/UsuarioController.cs) | [avatar](docs/prueba_avatar.png) |
| **3. Manejo de Archivos (Extra)** | ✅ Cumplido | [FotoController.cs](Api/FotoController.cs#L19) | [video](https://drive.google.com/file/d/10efCpRS6QVWWgKYH-pf61j42sayZkgx0/view?usp=sharing) |
| **4. ABM en Vue.js + AJAX** | ✅ Cumplido | [VueComponent.js](Views/Cliente/Index.cshtml) | [video](https://drive.google.com/file/d/1nohHgfCJ55zAo3FLl4iPF1cGkxprqtor/view?usp=sharing) |
| **5. Paginado por Servidor** | ✅ Cumplido | [Paginador.cs](Api/ClienteController.cs#L47) | [paginado](docs/prueba_paginado.png) |
| **6. Selección Asíncrona (AJAX)** | ✅ Cumplido | [Create.cshtml](Views/Trabajo/Create.cshtml) | [GIF](docs/prueba_busqueda_asincrona.gif) |
| **7. Web API + JWT** | ✅ Cumplido | [ClienteController.js](Api/ClienteController.cs) | [imagen](docs/prueba_jwt.png) |

---

### 🗄️ Crear e Inicializar la Base de Datos
El archivo [metalurgica.sql](docs/metalurgica.sql) contiene la creación de la BD en MySql y la creación de las tablas necesarias para la aplicación.

#### Opción 1: importar con MySQL Workbench

1. Abrir MySQL Workbench.
2. Iniciar en su instancia local.
3. En el panel izquierdo, seleccionar la pestaña **Administration**.
4. En la secci'on 'MANAGEMENT' seleccionar **Data import/restore**.
5. Seleccionar el radio button **Import from Self-Contained File**.
6. Buscar y elegir el archivo `metalurgica.sql` de este proyecto [aquí](docs/metalurgica.sql).
7. Presionar el boton **Start Import**.
8. En el panel izquierdo, ir a la pestaña **Schemas**
9. Click derecho en cualquier lugar del panel y seleccionar **Refresh All**
10. Verificar que aparezca la base de datos `metalurgica` con las tablas `clientes`, `trabajos`, `materiales`, `tipo_operaciones`, `fotos`, `detalles` y `usuarios`.

#### Opción 2: importar con phpMyAdmin

1. Abrir el panel de control de XAMPP.
2. Iniciar los módulos **Apache** y **MySQL**.
3. Presionar **Admin** junto al módulo MySQL para abrir phpMyAdmin.
4. Seleccionar la pestaña **Importar**.
5. Elegir el archivo `metalurgica.sql` del proyecto [aquí](docs/metalurgica.sql).
6. Mantener el formato SQL y presionar **Continuar**.
7. Verificar que aparezca la base de datos `metalurgica` con las tablas `clientes`, `trabajos`, `materiales`, `tipo_operaciones`, `fotos`, `detalles` y `usuarios`.

# 🚀 Guía para ejecutar el proyecto
## 1. Clonar el repositorio
```bash
git clone https://github.com/Manug95/metalurgicaMVC.git
cd metalurgicaMVC
```
## 2. Revisar configuración
En el archivo `appsettings.json` en la raiz del proyecto, asegurarse de tener su usuario y contraseña de su gestor de base de datos en `ConnectionStrings:MySql`.
## 3. Restaurar dependencias, compilar y ejecutar
```bash
dotnet restore
dotnet build
dotnet run
```

Abrir en el navegador en http://localhost:5137

---

## 🖥 Usuarios
### Con rol ADMIN
* username: manuel - contraseña: 12345678
### Con rol EMPLEADO
* username: user2 - contraseña: 12345678