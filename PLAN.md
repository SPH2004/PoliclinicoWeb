# 🌐 Plan Web Integral – Hospital Establecido en Perú

**⚙️ Plataforma**: WordPress (versión 6.5 o superior, mayo 2025).  
**💰 Plugins**: Gratuitos para la fase inicial; APIs y suscripciones reservadas para futuras funcionalidades.  
**🔗 Objetivo**: Diseñar y desarrollar una página web profesional, funcional, atractiva, centrada en el paciente, integrada con el sistema interno del hospital (HIS), y en cumplimiento con las leyes peruanas y normativas de salud.  
**🏥 Contexto**: Hospital establecido en Perú con sistema de gestión interno (HIS) y base de datos propia, ofreciendo 29 servicios médicos.  
**📅 Fecha de revisión**: 31 de mayo de 2025.  
**🩺 Servicios disponibles**: Anatomía Patológica, Cardiología, Cirugía General, Densitometría, Dermatología, Ecografía, Endocrinología, Gastroenterología, Ginecología, Laboratorio, Mamografía, Medicina General, Neumología, Neurología, Nutrición, Obstetricia, Odontología, Oftalmología, Otorrinolaringología, Pediatría, Podología, Psicología, Rayos X, Reumatología, Scanner Podológico, Terapia Física, Tomografía, Traumatología, Urología.  
**📌 Nota sobre médicos**: La sección de médicos es opcional, pendiente de confirmación, pero se incluye como propuesta detallada para su posible implementación.  
**📝 Enfoque**: Este plan es exhaustivo, detallado al milímetro, y será el documento guía para el desarrollo, sin omitir ningún aspecto relevante.

---

## 📜 Cumplimiento Legal en Perú

Para garantizar que la web cumpla con las leyes peruanas y normativas del sector salud:

### 1. **Ley N° 29733 – Protección de Datos Personales**
- **Aviso de Privacidad**:
  - Ubicación: Footer (enlace `/politica-de-privacidad`) y en cada formulario.
  - Contenido:
    ```markdown
    Aviso de Privacidad  
    En [Nombre del Hospital], protegemos sus datos personales conforme a la Ley N° 29733. Los datos recopilados (nombre, DNI, correo, teléfono) se utilizan únicamente para gestionar citas, consultas y reclamos. Puede ejercer sus derechos ARCO (Acceso, Rectificación, Cancelación, Oposición) enviando su solicitud a protecciondedatos@hospital.com.  
    ```
  - Formato: Open Sans, 12px, fondo blanco, enlace clicable.
- **Consentimiento explícito**:
  - Checkbox obligatorio en todos los formularios: “Acepto la política de privacidad” (enlace a `/politica-de-privacidad`).
  - Registro de consentimientos: Almacenar en base de datos (fecha, IP, formulario).
- **Seguridad de datos**:
  - SSL obligatorio (*Let’s Encrypt*, gratuito, renovable cada 90 días).
  - Encriptación: AES-256 para datos sensibles en el servidor (consultar con proveedor de hosting).
  - Acceso restringido: Solo personal autorizado del hospital accede a los datos.
- **Derechos ARCO**:
  - Formulario dedicado en `/contacto/derechos-arco`:
    - Campos: Nombre, DNI, correo, tipo de solicitud (Acceso, Rectificación, Cancelación, Oposición), detalle.
    - Respuesta en 20 días hábiles (según Ley N° 29733).
  - Correo: `protecciondedatos@hospital.com`.
- **Retención de datos**:
  - Almacenar datos de citas por 5 años (mínimo legal).
  - Eliminar datos tras 5 años, salvo autorización del paciente.

### 2. **Código de Protección y Defensa del Consumidor (Ley N° 29571)**
- **Libro de Reclamaciones**:
  - Ubicación: Subsección `/contacto/libro-de-reclamaciones`.
  - Formulario:
    - Campos: Nombre, DNI/CE, correo, teléfono, fecha, motivo (dropdown: Servicio, Atención, Facturación, Otros), detalle (textarea, máx. 1000 caracteres).
    - Generar código único: `REC-2025-XXXX` (formato: REC-año-número secuencial).
    - Enviar copia al usuario por correo (*WPForms Lite*).
    - Almacenar quejas por 2 años (cumplir con INDECOPI).
  - Texto legal:
    ```markdown
    Conforme a la Ley N° 29571, este formulario permite registrar sus quejas o reclamos. Recibirá una copia en su correo en un plazo de 24 horas. Tiempo de respuesta: 30 días calendario.
    ```
- **Términos y Condiciones**:
  - Página: `/terminos-y-condiciones`.
  - Contenido:
    ```markdown
    Términos y Condiciones  
    - Las citas deben cancelarse con 24 horas de aviso.  
    - No se realizan reembolsos por citas no asistidas sin notificación previa.  
    - Los horarios están sujetos a disponibilidad del hospital.  
    - [Nombre del Hospital] no se responsabiliza por errores en los datos proporcionados por el usuario.  
    ```
  - Formato: Open Sans, 14px, secciones numeradas.

### 3. **Normativas del Ministerio de Salud (MINSA)**
- **Contenido médico**:
  - No publicar información engañosa o sin respaldo científico.
  - Incluir disclaimer en el blog y páginas de especialidades:
    ```markdown
    La información proporcionada no sustituye una consulta médica profesional. Consulte a un especialista para un diagnóstico y tratamiento adecuados.
    ```
  - Validar contenido con personal médico del hospital antes de publicar.
- **Médicos** (si aplica):
  - Verificar registro en el **Colegio Médico del Perú (CMP)** para cada médico listado.
  - Incluir en página de médicos: “Todos nuestros médicos están registrados en el CMP” (Open Sans, 12px).
- **Publicidad**:
  - Evitar términos como “curamos” o “100% efectivo” (prohibidos por MINSA).
  - Usar frases como “mejoramos tu calidad de vida” o “atención especializada”.

### 4. **Ley N° 29973 – Ley General de la Persona con Discapacidad**
- **Accesibilidad**:
  - Contraste alto: 4.5:1 para texto, 3:1 para gráficos (*WebAIM Contrast Checker*).
  - Navegación por teclado: Compatible con tabuladores y atajos (Enter, Espacio).
  - Texto alternativo: Obligatorio en todas las imágenes (ej: “Equipo médico atendiendo en consulta”).
  - Opciones de accesibilidad:
    - Aumentar/reducir texto (+/- 200%).
    - Modo alto contraste.
    - Modo oscuro.
  - Plugin: *One Click Accessibility* (gratuito).

### 5. **Otras normativas**
- **Propiedad intelectual**: Usar imágenes con licencia (hospital o *Unsplash*/*Pexels*).
- **Publicidad en línea**: Cumplir con el Reglamento de Publicidad del MINSA (evitar promesas no verificables).
- **Cookies**: Incluir banner de cookies (*CookieYes* gratuito) para cumplir con estándares internacionales (aplicable si hay usuarios extranjeros).

---

## 🎨 Diseño Visual y Experiencia de Usuario (UX)

### 1. **Paleta de Colores**
- **Primario**: Azul claro (#4A90E2) – transmite confianza y profesionalismo, común en salud.
- **Secundario**: Blanco (#FFFFFF) – limpieza, claridad.
- **Acentos**: Naranja cálido (#F5A623) – CTAs, calidez, urgencia.
- **Complementario**: Gris claro (#F5F6F5) – fondos alternados para secciones.
- **Validación**: Usar *WebAIM Contrast Checker* para garantizar accesibilidad.
- **Herramienta**: *Coolors.co* para generar y ajustar paleta.

### 2. **Tipografía**
- **Primaria**: *Roboto* (Google Fonts, sans-serif, legible en pantallas).
  - **Tamaños**:
    - H1 (títulos principales): 32px, negrita.
    - H2 (subtítulos): 24px, negrita.
    - H3 (subsecciones): 20px, regular.
    - Texto base: 16px, regular.
    - Notas legales/footer: 12-14px, regular.
  - **Espaciado**: 1.5 para párrafos, 1.2 para títulos.
- **Secundaria**: *Open Sans* (para párrafos largos, alta legibilidad).
- **Accesibilidad**:
  - Contraste mínimo: 4.5:1 (texto), 3:1 (gráficos).
  - Evitar fuentes decorativas o cursivas prolongadas.

### 3. **Imágenes**
- **Fotos reales**:
  - Fachada del hospital, recepción, salas de espera, equipos médicos (con consentimiento para publicación).
  - Resolución: 1200x800px (banners), 600x400px (miniaturas), 300x200px (tarjetas).
- **Fotos complementarias**:
  - *Unsplash* o *Pexels* (temática: médicos, pacientes felices, familias).
  - Ejemplo: “Médico sonriendo con paciente” para banner de Inicio.
- **Optimización**:
  - Formato: JPG (<200 KB por imagen, usar *ShortPixel* gratuito).
  - Compresión: 80% calidad para reducir tiempos de carga.
- **Alt text**:
  - Obligatorio para SEO y accesibilidad.
  - Ejemplo: “Fachada del hospital [Nombre] en Lima, Perú” | “Médico realizando ecografía en consultorio”.
- **Consentimiento**:
  - Obtener autorización escrita para fotos de pacientes o personal (Ley N° 29733).
  - Si no hay fotos reales, usar ilustraciones minimalistas (*Flaticon* gratuito, con atribución).

### 4. **Tema WordPress**
- **Recomendado**: *Astra* (gratuito, ligero, personalizable, SEO-friendly).
- **Configuración**:
  - Ancho de contenido: 1200px (centrado, márgenes 20px).
  - Encabezado: Fondo blanco (#FFFFFF), sombra sutil (0 2px 4px rgba(0,0,0,0.1)).
  - Menú hamburguesa en móviles: Ícono 40x40px, color #4A90E2.
  - Footer: Fondo gris claro (#F5F6F5), enlaces a políticas legales.
- **Alternativa**: *Neve* (optimizado para móviles, ligero).
- **Personalización**:
  - Eliminar bordes decorativos o sombras excesivas.
  - Usar *Elementor* (gratuito) para layouts personalizados.

### 5. **Responsividad**
- **Prioridad móvil**: 70%+ de usuarios en Perú acceden desde smartphones (basado en tendencias 2025).
- **Configuración**:
  - Botones: Mín. 48x48px para toque fácil.
  - Texto: Mín. 16px en móviles.
  - Formularios: Campos apilados (stacked), teclado numérico para DNI/teléfono.
  - Imágenes: Escalar automáticamente (usar `srcset` en WordPress).
- **Herramientas**:
  - Diseñar layouts responsivos con *Elementor*.
  - Testear con *Google Mobile-Friendly Test* y *BrowserStack* (prueba gratuita).
- **Pruebas**:
  - Resoluciones: 360x640px (móviles básicos), 414x896px (smartphones modernos), 768x1024px (tablets), 1920x1080px (escritorio).

### 6. **Tono y lenguaje**
- **Tono**: Cálido, empático, profesional.
  - Ejemplo: “Estamos aquí para cuidar tu salud” en lugar de “Servicios médicos de calidad”.
- **Lenguaje**:
  - Claro y accesible, evitar jerga médica compleja.
  - Ejemplo: “Ecografía para un diagnóstico rápido” en lugar de “Ultrasonografía diagnóstica”.
- **Multilenguaje**: Español por defecto, inglés opcional (*Polylang* gratuito) para pacientes extranjeros.

---

## 🧱 Estructura del Sitio

### 📌 HEADER
- **Logo**:
  - Ubicación: Alineado a la izquierda, enlace a `/inicio`.
  - Tamaño: 150px ancho, 50px alto (SVG o PNG, resolución mínima 1200px).
  - Alt text: “Logo de [Nombre del Hospital]”.
- **Menú principal**:
  - Fijo en scroll (plugin *Sticky Menu & Sticky Header*).
  - Fondo: #FFFFFF, sombra: 0 2px 4px rgba(0,0,0,0.1).
  - Enlaces (Roboto, 16px, color #4A90E2):
    - Inicio (`/inicio`)
    - Especialidades (`/especialidades`)
    - [Opcional] Médicos (`/medicos`)
    - Reservar Cita (`/reservar-cita`)
    - Contacto (`/contacto`)
    - Blog (`/blog`)
  - Submenú para idioma: 🇪🇸 Español | 🇬🇧 Inglés (*Polylang*).
- **CTA destacado**:
  - Botón: `Reserva tu cita` (#F5A623, 16px, borde redondeado 8px, padding 12px).
  - Ícono de teléfono: `tel:+51XXXXXXXXX` (click-to-call, ícono FontAwesome `fa-phone`, 24px).
  - Ícono de WhatsApp (si aplica): `https://wa.me/+51XXXXXXXXX` (ícono FontAwesome `fa-whatsapp`, 24px).
- **Diseño móvil**:
  - Menú hamburguesa: Ícono 40x40px, fondo #4A90E2.
  - CTA visible en móviles: Botón reducido (48x48px).

### 📄 PÁGINAS PRINCIPALES

#### 🏠 INICIO
- **Banner principal**:
  - Imagen: Foto real del hospital (1200x400px, ej: fachada o recepción iluminada).
  - Degradado: Negro al 50% opacidad para legibilidad.
  - Texto:
    ```markdown
    Cuidarte es nuestra prioridad.  
    ¡Reserva tu cita en minutos!
    ```
    - Fuente: Roboto, 28px (H1), 18px (subtítulo), blanco, sombra 1px.
  - Formulario breve:
    - Dropdown: “Selecciona una especialidad” (29 opciones dinámicas desde HIS).
    - Botón: `Buscar horarios` (#F5A623, 16px, enlace a `/reservar-cita`).
  - Diseño: *Elementor*, sección centrada, altura máxima 400px.
- **Especialidades destacadas**:
  - Grid de 4 tarjetas (2 en móviles, *Elementor*).
  - Selección sugerida (basada en demanda común en Perú):
    1. **Cardiología**: “Cuidamos tu corazón con tecnología avanzada” | Ícono: Corazón (FontAwesome `fa-heart`) | Botón: `Ver más` (#4A90E2).
    2. **Pediatría**: “Atención especializada para tus pequeños” | Ícono: Bebé (`fa-baby`) | Botón: `Ver más`.
    3. **Ginecología**: “Cuidamos tu salud en cada etapa” | Ícono: Símbolo femenino (`fa-venus`) | Botón: `Ver más`.
    4. **Traumatología**: “Recupérate con nuestros expertos” | Ícono: Hueso (`fa-bone`) | Botón: `Ver más`.
  - Diseño: Tarjetas con borde redondeado (8px), sombra sutil, fondo #F5F6F5.
- **Sobre nosotros**:
  - Título: `¿Por qué elegirnos?` (Roboto, 24px).
  - 3 columnas (1 en móviles):
    - “Médicos certificados por el CMP” | Ícono: Estetoscopio (`fa-stethoscope`).
    - “Tecnología de punta” | Ícono: Monitor médico (`fa-heartbeat`).
    - “Atención cálida y personalizada” | Ícono: Corazón (`fa-hand-holding-heart`).
  - Imagen lateral: Foto del equipo o sala (500x300px, alt text: “Equipo médico de [Nombre del Hospital]”).
- **Testimonios**:
  - Carrusel (*Strong Testimonials*, 3-4 testimonios, 50 palabras cada uno).
  - Formato: Nombre, cita, 5 estrellas (sin fotos hasta confirmar consentimiento).
  - Ejemplo: “La atención en pediatría fue rápida y profesional” – María G.
  - Fondo: #F5F6F5, texto Open Sans, 16px.
- **CTA inferior**:
  - Fondo: Azul claro (#E6F0FA).
  - Texto: `Agenda tu cita hoy mismo` (Roboto, 20px).
  - Botones:
    - `Ver Especialidades` (#4A90E2, enlace a `/especialidades`).
    - `Reservar Cita` (#F5A623, enlace a `/reservar-cita`).

#### 🧑‍⚕️ MÉDICOS (Opcional, pendiente de confirmación)
- **Estado**: Propuesta en, no confirmada. Si no se implementa, redirigir a especialidades.
- **Grid principal**:
  - Tarjetas (300x400px, *Elementor*):
    - Foto: Circular, 100x100px, fondo blanco (alt text: “Foto del Dr. [Nombre]”).
    - Nombre: Ej. “Dr. Juan Pérez” (Roboto, 18px).
    - Especialidad: Ej. “Cardiología” (Open Sans, 14px).
    - Botón: `Ver horarios` (#4A90E2, enlace a página individual).
  - Filtros (*Search & Filter* gratuito):
    - Dropdown: Especialidad (29 opciones).
    - Dropdown: Turno (Mañana: 8:00-12:00, Tarde: 14:00-18:00).
    - Buscador: Input “Busca por nombre o especialidad” (ícono lupa, FontAwesome `fa-search`).
  - Paginación: 9 médicos por página (*WP-PageNavi*).
- **Página individual de médico**:
  - Diseño en 2 columnas (1 en móviles):
    - **Izquierda**:
      - Foto: 200x200px (alt text: “Dr. [Nombre] en consultorio”).
      - Bio: 100 palabras (ej: “Especialista en cardiología con 10 años de experiencia”).
      - Especialidades: Lista (ej: Cardiología, Ecografía).
    - **Derecha**:
      - Tabla de horarios:
        ```
        | Día         | Horario         |
        |-------------|-----------------|
        | Lunes       | 8:00-12:00      |
        | Martes      | 14:00-18:00     |
        | ...         | ...             |
        ```
      - Botón: `Agendar cita` (#F5A623, enlace a `/reservar-cita?medico=[ID]`).
  - **Disclaimer**: “Médico registrado en el CMP” (Open Sans, 12px).
- **Plugins**:
  - *Custom Post Type UI*: Crear tipo de post “Médicos”.
  - *Advanced Custom Fields (ACF)*: Campos personalizados (nombre, especialidad, horarios, bio).
- **Integración**:
  - Sincronizar datos de médicos con HIS (si aplica) mediante CSV o API.

#### 🏥 ESPECIALIDADES
- **Grid principal**:
  - 8 especialidades por página (paginación con *WP-PageNavi*).
  - Tarjetas (300x200px, *Elementor*):
    - Ícono: FontAwesome (ej: corazón para Cardiología).
    - Título: Nombre de la especialidad (Roboto, 18px).
    - Descripción: 30-50 palabras (Open Sans, 14px).
    - Botón: `Reservar cita` (#F5A623, enlace a `/reservar-cita?especialidad=[ID]`).
  - Diseño: Mampostería, borde redondeado (8px), fondo #FFFFFF.
- **Textos de especialidades** (optimizados para pacientes, tono empático):
  1. **Anatomía Patológica**: “Diagnósticos precisos mediante análisis de tejidos para un tratamiento efectivo.” | Ícono: Microscopio (`fa-microscope`).
  2. **Cardiología**: “Cuidamos tu corazón con exámenes avanzados y tratamientos personalizados.” | Ícono: Corazón (`fa-heart`).
  3. **Cirugía General**: “Procedimientos quirúrgicos seguros realizados por expertos con tecnología moderna.” | Ícono: Bisturí (`fa-scalpel`).
  4. **Densitometría**: “Evaluamos tu salud ósea para prevenir y tratar osteoporosis con precisión.” | Ícono: Hueso (`fa-bone`).
  5. **Dermatología**: “Cuidamos la salud y estética de tu piel con tratamientos especializados.” | Ícono: Piel (`fa-hand-holding-medical`).
  6. **Ecografía**: “Diagnósticos por imágenes no invasivos, rápidos y confiables para tu salud.” | Ícono: Ultrasonido (`fa-procedures`).
  7. **Endocrinología**: “Tratamos trastornos hormonales y metabólicos con atención personalizada.” | Ícono: Glándula (`fa-thyroid`).
  8. **Gastroenterología**: “Soluciones integrales para tu sistema digestivo con diagnósticos precisos.” | Ícono: Estómago (`fa-stomach`).
  9. **Ginecología**: “Cuidamos tu salud femenina en cada etapa con atención cercana.” | Ícono: Símbolo femenino (`fa-venus`).
  10. **Laboratorio**: “Análisis clínicos rápidos y confiables para un diagnóstico oportuno.” | Ícono: Tubo de ensayo (`fa-vial`).
  11. **Mamografía**: “Detección temprana del cáncer de mama con tecnología de punta.” | Ícono: Pecho (`fa-breast`).
  12. **Medicina General**: “Atención primaria para toda la familia, cálida y profesional.” | Ícono: Estetoscopio (`fa-stethoscope`).
  13. **Neumología**: “Cuidamos tu salud respiratoria con tratamientos y diagnósticos especializados.” | Ícono: Pulmones (`fa-lungs`).
  14. **Neurología**: “Atención integral para trastornos neurológicos con enfoque personalizado.” | Ícono: Cerebro (`fa-brain`).
  15. **Nutrición**: “Planes personalizados para una alimentación saludable y equilibrada.” | Ícono: Plato saludable (`fa-utensils`).
  16. **Obstetricia**: “Acompañamiento experto para un embarazo y parto seguros.” | Ícono: Embarazo (`fa-baby-carriage`).
  17. **Odontología**: “Cuidamos tu sonrisa con tratamientos dentales integrales.” | Ícono: Diente (`fa-tooth`).
  18. **Oftalmología**: “Protege tu visión con exámenes y tratamientos especializados.” | Ícono: Ojo (`fa-eye`).
  19. **Otorrinolaringología**: “Soluciones para problemas de oído, nariz y garganta.” | Ícono: Oreja (`fa-ear`).
  20. **Pediatría**: “Cuidamos la salud de tus hijos con atención cálida y profesional.” | Ícono: Bebé (`fa-baby`).
  21. **Podología**: “Tratamos afecciones de los pies para mejorar tu calidad de vida.” | Ícono: Pie (`fa-foot`).
  22. **Psicología**: “Apoyo emocional y mental para tu bienestar integral.” | Ícono: Cabeza (`fa-head-side`).
  23. **Rayos X**: “Diagnósticos por imágenes precisos y rápidos para tu salud.” | Ícono: Radiografía (`fa-x-ray`).
  24. **Reumatología**: “Tratamos enfermedades articulares y autoinmunes con cuidado especializado.” | Ícono: Articulación (`fa-joint`).
  25. **Scanner Podológico**: “Análisis avanzado de los pies para tratamientos personalizados.” | Ícono: Escáner (`fa-scanner`).
  26. **Terapia Física**: “Recupera tu movilidad con terapias efectivas y personalizadas.” | Ícono: Movimiento (`fa-running`).
  27. **Tomografía**: “Imágenes detalladas para diagnósticos precisos y oportunos.” | Ícono: Escáner (`fa-ct-scan`).
  28. **Traumatología**: “Tratamos lesiones óseas y musculares con enfoque integral.” | Ícono: Hueso (`fa-bone`).
  29. **Urología**: “Cuidamos tu salud urológica con diagnósticos y tratamientos especializados.” | Ícono: Riñón (`fa-kidney`).
- **Página individual de especialidad**:
  - Título: Ej. “Cardiología” (Roboto, 24px).
  - Descripción: 150 palabras (ej: “Nuestros cardiólogos ofrecen ecocardiogramas, pruebas de esfuerzo y monitoreo para cuidar tu corazón”).
  - Servicios: Lista (ej: Ecocardiograma, Holter, Prueba de esfuerzo).
  - [Opcional] Médicos: Enlace a médicos de la especialidad (si aplica).
  - Botón: `Agendar cita` (#F5A623, enlace a `/reservar-cita?especialidad=[ID]`).
  - Imagen: 600x400px (ej: equipo de cardiología, alt text: “Consultorio de cardiología en [Nombre del Hospital]”).

#### 📆 RESERVAR CITA
- **Objetivo**: Simplificar el proceso de reserva, integrar con HIS, cumplir con Ley N° 29733.
- **Flujo del formulario** (multistep, *WPForms Lite*):
  1. **Paso 1**: 
     - Dropdown: “Especialidad” (29 opciones, sincronizadas con HIS).
     - Ejemplo: “Cardiología”, “Pediatría”, etc.
  2. **Paso 2**:
     - [Opcional] Dropdown: “Médico” (si aplica, dinámico según especialidad).
     - Calendario: Fecha y hora (sincronizado con horarios del HIS, plugin *Amelia Lite*).
     - Formato: Slots de 30 minutos (ej: 8:00, 8:30, 9:00).
  3. **Paso 3**:
     - Campos:
       - Nombre completo (texto, obligatorio, máx. 100 caracteres).
       - DNI/CE (texto, obligatorio, validación 8-12 dígitos).
       - Correo electrónico (email, obligatorio, validación formato).
       - Teléfono (texto, obligatorio, formato +51XXXXXXXXX).
       - Motivo de consulta (textarea, opcional, máx. 200 caracteres).
       - Checkbox: “Acepto la política de privacidad” (enlace a `/politica-de-privacidad`).
  4. **Confirmación**:
     - Mensaje en pantalla:
       ```markdown
       ¡Cita reservada! Recibirás una confirmación por correo en las próximas 24 horas.  
       Código de cita: CITA-2025-XXXX
       ```
     - Enlace: “Añadir a Google Calendar” (formato .ics).
- **Integración con HIS**:
  - **Fase inicial**: Exportar citas a CSV diario (*WPForms* a Google Sheets via *Zapier* gratuito).
    - Campos exportados: Nombre, DNI, correo, teléfono, especialidad, médico (si aplica), fecha, hora, motivo.
  - **API**: Usar *WP Webhooks* (gratuito) para enviar datos al HIS (requiere documentación de endpoints).
  - **Tabla temporal**: *TablePress* (gratuito) para almacenar citas en WordPress si la API no está lista.
  - **Formato CSV**:
    ```
    Nombre,DNI,Correo,Teléfono,Especialidad,Médico,Fecha,Hora,Motivo
    Juan Pérez,12345678,juan@email.com,+51987654321,Cardiología,Dr. Gómez,2025-06-05,10:00,Chequeo
    ```
- **Seguridad**:
  - SSL (*Let’s Encrypt*).
  - reCAPTCHA v3 (*WPForms*) para evitar spam.
  - Encriptar datos en el servidor (AES-256).
  - No almacenar datos sensibles en Google Sheets a largo plazo.
- **Correo de confirmación** (*WP Mail SMTP*, configurado con Gmail SMTP):
  - Asunto: “Confirmación de cita – [Nombre del Hospital]”.
  - Contenido:
    ```markdown
    Estimado/a [Nombre],  
    Su cita en [Especialidad] ha sido reservada para el [Fecha] a las [Hora].  
    Código de cita: CITA-2025-XXXX  
    Por favor, llegue 15 minutos antes.  
    Gracias por confiar en [Nombre del Hospital].  
    Contacto: +51XXXXXXXXX | protecciondedatos@hospital.com
    ```
- **Plugins**:
  - *Amelia Lite* o *Simply Schedule Appointments* para calendarios.
  - *WPForms Lite* para formularios personalizados.
  - *WP Mail SMTP* para correos confiables.

#### 📍 CONTACTO
- **Formulario principal** (*WPForms Lite*):
  - Campos:
    - Nombre (texto, obligatorio, máx. 100 caracteres).
    - DNI/CE (texto, obligatorio, 8-12 dígitos).
    - Correo (email, obligatorio).
    - Teléfono (texto, obligatorio, +51XXXXXXXXX).
    - Asunto (dropdown: Consulta, Queja, Sugerencia, Otros).
    - Mensaje (textarea, obligatorio, máx. 500 caracteres).
    - Checkbox: “Acepto la política de privacidad” (enlace a `/politica-de-privacidad`).
  - Validación en tiempo real: Mensaje de error en rojo (#FF0000).
  - Confirmación: “Gracias por tu mensaje. Responderemos en 24 horas” (Roboto, 16px).
- **Libro de Reclamaciones**:
  - Subsección: `/contacto/libro-de-reclamaciones`.
  - Campos:
    - Nombre (texto, obligatorio).
    - DNI/CE (texto, obligatorio).
    - Correo (email, obligatorio).
    - Teléfono (texto, obligatorio).
    - Fecha del incidente (calendario, obligatorio).
    - Motivo (dropdown: Servicio, Atención, Facturación, Otros).
    - Detalle (textarea, máx. 1000 caracteres).
  - Generar código: `REC-2025-XXXX` (almacenar en *TablePress*).
  - Enviar copia al usuario (*WPForms*).
  - Cumplir con INDECOPI: Almacenar por 2 años.
- **Mapa**:
  - Google Maps embebido (API gratuita, clave desde Google Cloud Console).
  - Pin: Ubicación exacta del hospital (coordenadas GPS).
  - Botón: `Cómo llegar` (enlace a Google Maps, ej: `https://maps.google.com/?q=-12.0464,-77.0428`).
  - Alt text: “Mapa de ubicación de [Nombre del Hospital] en Lima”.
- **Información de contacto**:
  - Tabla de horarios:
    ```
    | Día         | Horario         |
    |-------------|-----------------|
    | Lunes-Viernes | 8:00-20:00    |
    | Sábado      | 8:00-14:00      |
    | Emergencias | 24/7            |
    ```
  - Teléfonos:
    - Línea principal: +51XXXXXXXXX (click-to-call).
    - Emergencias: +51XXXXXXXXX.
    - WhatsApp (si aplica): `https://wa.me/+51XXXXXXXXX`.
  - Correo: `contacto@hospital.com`.
  - Texto: “Estacionamiento gratuito disponible” | “Acceso para personas con discapacidad” (Open Sans, 14px).

#### 📰 BLOG DE SALUD
- **Objetivo**: Informar, educar y atraer pacientes con contenido relevante.
- **Contenido**:
  - Frecuencia: 2 artículos semanales (400-500 palabras).
  - Categorías: Medicina General, Pediatría, Prevención, Campañas, Especialidades.
  - Ejemplos de artículos:
    - “5 consejos para cuidar tu salud cardiovascular” (Cardiología, palabras clave: “salud cardíaca Lima”).
    - “¿Cuándo realizar tu primera mamografía?” (Mamografía, palabras clave: “mamografía Perú”).
    - “Cómo preparar a tu hijo para una consulta pediátrica” (Pediatría).
    - “Beneficios de la terapia física para lesiones deportivas” (Terapia Física).
  - Disclaimer en cada artículo:
    ```markdown
    La información proporcionada no sustituye una consulta médica profesional. Consulte a un especialista.
    ```
- **Diseño**:
  - Grid de 3 columnas (2 en móviles, *Elementor*).
  - Miniaturas: 600x400px (alt text: “Consejos de salud cardiovascular”).
  - Título: Roboto, 20px, negrita.
  - Extracto: 50 palabras, Open Sans, 14px.
  - Botón: `Leer más` (#4A90E2).
  - Botón compartir: *AddToAny* (Facebook, WhatsApp, Twitter).
  - CTA final: `¿Necesitas una consulta? Reserva aquí` (#F5A623, enlace a `/reservar-cita`).
- **SEO** (*Yoast SEO* gratuito):
  - Títulos: Máx. 60 caracteres (ej: “Cuidar tu corazón en Lima”).
  - Meta descripciones: Máx. 160 caracteres (ej: “Consejos prácticos para tu salud cardiovascular en [Nombre del Hospital]. Reserva tu cita hoy.”).
  - Palabras clave: Localizadas (ej: “cardiología Lima”, “pediatría Perú”).
  - Enlaces internos: Vincular a páginas de especialidades (ej: `/especialidades/cardiologia`).

---

## 🔐 Seguridad y Accesibilidad

### Seguridad
- **SSL**:
  - Activar *Let’s Encrypt* (gratuito, configurar en hosting).
  - Renovar cada 90 días automáticamente.
- **Firewall y monitoreo**:
  - *Wordfence* (gratuito): Bloquear ataques, monitorear accesos sospechosos.
  - Alertas por correo: Configurar para administrador (`admin@hospital.com`).
- **Backups**:
  - *UpdraftPlus* (gratuito): Respaldos diarios a Google Drive.
  - Almacenar: 7 días de copias, eliminar automáticamente las más antiguas.
- **Formularios**:
  - reCAPTCHA v3 (*WPForms*) para evitar spam.
  - Validación de entradas: Evitar inyecciones SQL (configurar en *Wordfence*).
- **Base de datos**:
  - Encriptar datos sensibles (nombres, DNI, correo) con AES-256.
  - Acceso restringido: Usar credenciales seguras (mín. 16 caracteres, alfanuméricas).
- **Cumplimiento**:
  - Ley N° 29733: No compartir datos con terceros sin consentimiento.
  - Auditoría trimestral: Revisar accesos y backups (responsable: equipo IT del hospital).

### Accesibilidad
- **Plugin**: *One Click Accessibility* (gratuito):
  - Aumentar/reducir texto: +/- 200% (16px a 32px/8px).
  - Modo alto contraste: Fondo negro, texto blanco.
  - Modo oscuro: Fondo #333333, texto #FFFFFF.
  - Subrayar enlaces para daltonismo.
- **Navegación**:
  - *WP Accessibility* (gratuito): Soporte para teclado (Tab, Enter, Espacio).
  - Etiquetas ARIA en formularios, imágenes y menús.
- **Imágenes**:
  - Texto alternativo obligatorio (ej: “Mapa de ubicación de [Nombre del Hospital]”).
- **Validación**:
  - Usar *WAVE Web Accessibility Tool* (gratuito) para detectar errores.
  - Cumplir con WCAG 2.1 (nivel AA) y Ley N° 29973.

---

## 🎯 Futuras Funcionalidades (Con Inversión o Suscripción)

1. **Área privada para pacientes**:
   - **Funcionalidad**:
     - Login para pacientes (usuario: correo, contraseña).
     - Mostrar historial de citas, resultados de laboratorio (PDF cifrado).
     - Recordatorios de citas (correo/WhatsApp).
   - **Plugin**: *ProfilePress* Premium (~$99/año).
   - **API**: Integración con HIS para datos en tiempo real.
   - **Costo estimado**: Desarrollo personalizado (~$1000-$2000, única vez).
   - **Seguridad**: Autenticación de dos factores (*miniOrange*, ~$50/año).
2. **Notificaciones por WhatsApp**:
   - **Funcionalidad**: Confirmaciones, recordatorios, mensajes personalizados.
   - **API**: WhatsApp Business API (~$0.01-$0.05 por mensaje).
   - **Costo**: ~$50-$100/mes (1000-5000 mensajes).
   - **Ejemplo**: “Recordatorio: Su cita en Cardiología es el 5/6/2025 a las 10:00”.
3. **Chatbot avanzado**:
   - **Funcionalidad**: Respuestas automáticas a preguntas comunes (horarios, citas, ubicación).
   - **Servicio**: *Dialogflow* (Google) o *Botpress* (~$30-$100/mes).
   - **Costo**: ~$50-$150/mes según volumen.
4. **Integración avanzada con HIS**:
   - **Funcionalidad**: Sincronización bidireccional (citas, resultados, facturación).
   - **Costo**: Desarrollo personalizado (~$2000-$5000, única vez).
5. **Campañas de salud dinámicas**:
   - **Funcionalidad**: Calendario interactivo para vacunaciones, despistajes, eventos.
   - **Plugin**: *The Events Calendar Pro* (~$89/año).
   - **Ejemplo**: “Día de despistaje gratuito: 15/6/2025, registra tu cita”.

---

## 📥 Plan de Implementación

### Semana 1-2: Preparación
1. **Recolección de datos**:
   - **Logo**: SVG o PNG, mínimo 1200px ancho, alt text: “Logo de [Nombre del Hospital]”.
   - **Especialidades**: Lista completa (29 servicios), descripciones, íconos (FontAwesome).
   - **[Opcional] Médicos**: Nombre, especialidad, horarios, foto (100x100px), bio (100 palabras).
   - **Documentos legales**: 
     - Política de privacidad (redactada por abogado, incluir derechos ARCO).
     - Términos y condiciones (políticas de citas, cancelaciones).
   - **HIS**: Documentación de API (endpoints, formato JSON/CSV, autenticación).
   - **Fotos**: Fachada, salas, equipos (mín. 10 imágenes, consentimiento firmado).
2. **Configuración técnica**:
   - Instalar WordPress (hosting recomendado: *Hostinger* o *SiteGround*, soporte PHP 8.2+).
   - Activar SSL (*Let’s Encrypt*, gratuito).
   - Instalar tema: *Astra* (configurar paleta, tipografía, responsividad).
   - Instalar plugins:
     - *Elementor* (diseño).
     - *WPForms Lite* (formularios).
     - *Amelia Lite* o *Simply Schedule Appointments* (citas).
     - *Custom Post Type UI* + *ACF* ([opcional] médicos).
     - *Yoast SEO* (SEO).
     - *Wordfence* (seguridad).
     - *UpdraftPlus* (backups).
     - *Strong Testimonials* (testimonios).
     - *One Click Accessibility* (accesibilidad).
     - *Polylang* (multilenguaje).
     - *ShortPixel* (imágenes).
     - *AddToAny* (redes sociales).
     - *WP-PageNavi* (paginación).
     - *TablePress* (tablas temporales).

### Semana 3-4: Desarrollo
1. **Diseño de páginas** (*Elementor*):
   - **Inicio**: Banner, especialidades destacadas, sobre nosotros, testimonios, CTA.
   - **Especialidades**: Grid, páginas individuales para 29 servicios.
   - **[Opcional] Médicos**: Grid, páginas individuales, filtros.
   - **Reservar Cita**: Formulario multistep, integración con HIS.
   - **Contacto**: Formulario, Libro de Reclamaciones, mapa, horarios.
   - **Blog**: Grid, 5 artículos iniciales.
   - **Páginas legales**: `/politica-de-privacidad`, `/terminos-y-condiciones`, `/contacto/derechos-arco`.
2. **Integración con HIS**:
   - Configurar exportación a CSV diario (*WPForms* a Google Sheets via *Zapier*).
   - Probar *WP Webhooks* con API del HIS (enviar datos en JSON).
   - Crear tabla temporal en WordPress (*TablePress*) para citas.
3. **Contenido**:
   - Redactar textos para todas las páginas (Inicio, Especialidades, Sobre Nosotros).
   - Preparar 5 artículos iniciales para el blog:
     - “5 consejos para tu salud cardiovascular” (Cardiología).
     - “¿Cuándo realizar una mamografía?” (Mamografía).
     - “Cómo cuidar la salud de tus hijos” (Pediatría).
     - “Beneficios de la terapia física” (Terapia Física).
     - “Detección temprana de problemas neurológicos” (Neurología).
   - Optimizar imágenes (*ShortPixel*, <200 KB, alt text completo).
4. **SEO inicial**:
   - Configurar *Yoast SEO*: Títulos, meta descripciones, palabras clave.
   - Crear sitemap XML y enviarlo a Google Search Console.
   - Enlaces internos: Vincular especialidades y blog.

### Semana 5: Pruebas
1. **Funcionalidad**:
   - Testear formulario de citas: 10 reservas simuladas (Cardiología, Pediatría, etc.).
   - Verificar integración con HIS: Exportación a CSV, envío por API.
   - Probar formularios de contacto y Libro de Reclamaciones.
   - [Opcional] Testear filtros de médicos.
2. **Rendimiento**:
   - Optimizar tiempos de carga (<3 segundos, *GTmetrix*).
   - Comprimir imágenes (*ShortPixel*).
   - Habilitar caché (*WP Fastest Cache*, gratuito).
   - Minificar CSS/JS (*Autoptimize*, gratuito).
3. **Responsividad**:
   - Testear en resoluciones: 360x640px, 414x896px, 768x1024px, 1920x1080px (*BrowserStack*).
   - Verificar botones, formularios y menús en móviles.
4. **Accesibilidad**:
   - Usar *WAVE Web Accessibility Tool* para detectar errores (faltan alt text, etiquetas ARIA, etc.).
   - Corregir para cumplir WCAG 2.1 y Ley N° 29973.
5. **Seguridad**:
   - Probar reCAPTCHA v3 en formularios.
   - Simular ataques básicos con *Wordfence* (SQL injection, XSS).
   - Verificar backups automáticos (*UpdraftPlus*).

### Semana 6: Lanzamiento
1. **Publicación**:
   - Subir la web a `www.hospitalnombre.pe`.
   - Configurar *Google Analytics* (seguimiento de visitas, formularios).
   - Enviar sitemap a Google Search Console (*Yoast SEO*).
2. **Capacitación**:
   - Entrenar al personal del hospital (2-3 horas):
     - Gestión de citas (*Amelia Lite* o *WPForms*).
     - Publicación de artículos (*WordPress Editor*).
     - Respuesta a quejas (*Libro de Reclamaciones*).
     - Monitoreo de seguridad (*Wordfence*).
   - Entregar manual básico en PDF (crear con *Google Docs*).
3. **Monitoreo inicial**:
   - *Google Analytics*: Seguimiento de visitas, clics en CTAs, reservas.
   - *Wordfence*: Alertas de seguridad en tiempo real.
   - *UpdraftPlus*: Verificar backups diarios.

---

## 📊 Flujo de Paciente Ejemplo
1. **Acceso**: Paciente ingresa a `www.hospitalnombre.pe`.
2. **Exploración**: Ve banner en Inicio: `Reserva tu cita` → Selecciona “Cardiología”.
3. **Reserva**: Elige fecha/hora (ej: 5/6/2025, 10:00) → Completa formulario (Nombre, DNI, correo, teléfono) → Acepta política de privacidad.
4. **Confirmación**: Recibe mensaje en pantalla: “Cita reservada, código CITA-2025-XXXX” → Correo con detalles.
5. **Seguimiento**: [Futuro] Accede a área privada para ver historial o resultados.

---

## 💡 Libertades Creativas
1. **Chat en vivo**:
   - Implementar *Tawk.to* (gratuito).
   - Horario: Lunes a viernes, 8:00-18:00.
   - Respuestas predefinidas: “¿En qué te ayudamos?” | “Consulta nuestros horarios en /contacto”.
2. **Campañas de salud**:
   - Página: `/campanas` (*The Events Calendar*, gratuito).
   - Ejemplo: “Día de despistaje gratuito: 15/6/2025, registra tu cita”.
   - Diseño: Calendario interactivo, botón `Inscribirse` (#F5A623).
3. **Consejos dinámicos**:
   - Banner rotativo en Inicio (*Elementor*):
     ```markdown
     ¿Sabías que caminar 30 minutos al día reduce el riesgo cardiovascular?  
     Descubre más consejos en nuestro blog.
     ```
   - Animación: Desvanecimiento suave (2 segundos).
4. **FAQ dinámico**:
   - Sección en `/contacto/preguntas-frecuentes`:
     - “¿Cómo reservo una cita?” → “Visita /reservar-cita y selecciona tu especialidad”.
     - “¿Aceptan seguros?” → “Consulta con nuestro equipo al +51XXXXXXXXX”.
     - Diseño: Acordeón (*Elementor*), fondo #F5F6F5.

---

## 📋 Plugins Gratuitos
1. **Astra**: Tema ligero y personalizable.
2. **Elementor**: Diseño visual responsivo.
3. **WPForms Lite**: Formularios de contacto, citas, Libro de Reclamaciones.
4. **Amelia Lite** o **Simply Schedule Appointments**: Gestión de citas.
5. **Custom Post Type UI** + **Advanced Custom Fields (ACF)**: [Opcional] Fichas de médicos.
6. **Yoast SEO**: Optimización SEO (títulos, meta descripciones, sitemap).
7. **Wordfence**: Seguridad (firewall, monitoreo).
8. **UpdraftPlus**: Backups diarios.
9. **Strong Testimonials**: Testimonios de pacientes.
10. **One Click Accessibility**: Accesibilidad (texto, contraste, modo oscuro).
11. **Polylang**: Multilenguaje (español/inglés).
12. **ShortPixel**: Compresión de imágenes.
13. **AddToAny**: Compartir en redes sociales.
14. **WP-PageNavi**: Paginación de especialidades/médicos.
15. **TablePress**: Tablas temporales para citas/quejas.
16. **WP Mail SMTP**: Correos confiables (configuración con Gmail).
17. **CookieYes**: Banner de cookies (cumplir estándares internacionales).

---

## 📅 Cronograma
- **Semana 1-2: Preparación** (7-14 días):
  - Recolectar datos: Logo, especialidades, médicos (si aplica), documentos legales, fotos, API del HIS.
  - Configurar WordPress, SSL, tema, plugins.
- **Semana 3-4: Desarrollo** (14-21 días):
  - Diseñar páginas con *Elementor*.
  - Configurar formulario de citas e integración con HIS.
  - Redactar contenido, subir artículos, optimizar imágenes.
- **Semana 5: Pruebas** (7-10 días):
  - Testear funcionalidad (citas, formularios, filtros).
  - Optimizar rendimiento (<3 segundos).
  - Verificar responsividad y accesibilidad.
- **Semana 6: Lanzamiento** (5-7 días):
  - Publicar en `www.ndgserviciosdesalud.com`.
  - Configurar *Google Analytics*, *Yoast SEO*.
  - Capacitar al personal.
  - Iniciar monitoreo (visitas, seguridad, backups).

---

## 📋 Resumen de Entregables
1. **Páginas completas**: Inicio, Especialidades, [Médicos], Reservar Cita, Contacto, Blog, Políticas legales.
2. **Formularios funcionales**: Citas, contacto, Libro de Reclamaciones, derechos ARCO.
3. **Integración inicial**: Exportación a CSV/Google Sheets, conexión con HIS (si aplica).
4. **Contenido inicial**: 29 páginas de especialidades, 5 artículos de blog, imágenes optimizadas.
5. **SEO**: Sitemap, títulos, meta descripciones, palabras clave localizadas.
6. **Seguridad**: SSL, firewall, backups, reCAPTCHA.
7. **Accesibilidad**: WCAG 2.1, Ley N° 29973.
8. **Capacitación**: Manual y sesión para personal.

---

## 📊 Flujo de Desarrollo
1. **Recolección**: Obtener todos los datos (logo, especialidades, médicos, HIS).
2. **Diseño**: Crear layouts con *Elementor*, priorizar Inicio y Reservar Cita.
3. **Integración**: Configurar exportación de citas, probar API del HIS.
4. **Contenido**: Redactar textos, optimizar imágenes, publicar artículos.
5. **Pruebas**: Verificar funcionalidad, rendimiento, accesibilidad, seguridad.
6. **Lanzamiento**: Publicar, capacitar, monitorear.

---
