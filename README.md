# 🛠 metalurgicaMVC

Es un sistema para la gestión de los trabajos realizados por una empresa metalurgica.
Permite administrar via web a los clientes y los trabajo solicitados por estos.
También permite administrar los detalles de estos trabajos y los materiales utilizados en estos.
Tiene un sistema de autenticación de usuarios y restricción de funcionalidades en base al rol de estos.

---

## 📌 Tabla de Evidencias de Requerimientos Mínimos

| Requerimiento Mínimo | Estado | Enlace al Código / Controladores | Evidencia (Captura / GIF / Video) |
| :--- | :---: | :--- | :--- |
| **1. 4 Tablas y Relación 1:N** | ✅ Cumplido | [DataContext.cs](URL_LINEA_CODIGO) | [DER](docs/DER.jpg) |
| **2. Login, Roles y Avatar** | ✅ Cumplido | [UsuarioController.cs](Controllers/UsuarioController.cs) | Captura de pantalla de perfil con avatar |
| **3. Manejo de Archivos (Extra)** | ✅ Cumplido | [ProductosController.cs](Api/FotoController.cs#L19) | Video corto / GIF mostrando subida de comprobante |
| **4. ABM en Vue.js + AJAX** | ✅ Cumplido | [VueComponent.js](URL) | Video corto / GIF del ABM dinámico |
| **5. Paginado por Servidor** | ✅ Cumplido | [Paginador.cs](URL) | Captura del inspector de red enviando `page=2` |
| **6. Selección Asíncrona (AJAX)** | ✅ Cumplido | [Create.cshtml](Views/Trabajo/Create.cshtml) | GIF buscando entidades en desplegable |
| **7. Web API + JWT** | ✅ Cumplido | [ApiController.cs](URL) | Captura de Postman con Bearer Token |