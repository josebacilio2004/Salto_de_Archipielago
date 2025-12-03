# 🏝️ Saltos del Archipiélago

> **Examen Final - Desarrollo de Videojuegos**
> Un plataformas 2D retro de aventura y restauración ecológica.


## 📖 Descripción

**Saltos del Archipiélago** es un videojuego de plataformas 2D estilo pixel art (16-bits). El jugador controla a **Alex Rivera**, un joven biólogo que debe recorrer la Isla de Kora para detener un derrame de energía mecánica que está secando la fuente sagrada.

El juego combina mecánicas clásicas de salto y exploración con un mensaje ecológico, enfrentando autómatas corruptos y resolviendo puzzles para restaurar la naturaleza.

## 🎮 Características Principales

* **Estilo Visual Retro:** Sprites y Tilesets Pixel Art de 16-bits con paletas de colores temáticas.
* **Nivel Continuo (3 Zonas):** Transición fluida entre Playa (Tutorial), Selva (Plataformeo) y Laguna Contaminada (Jefe/Puzzle).
* **Mecánicas de Movimiento:** Sistema de física ajustada con *Coyote Time*, *Jump Buffer* y *Salto Variable*.
* **Combate:** Sistema de "Stomp" (Pisotón) para eliminar enemigos rebotando sobre ellos.
* **IA Enemiga:** Autómata "Guardia Roca" con máquina de estados (Patrulla, Alerta, Persecución, Muerte).
* **Sistema de Restauración:** Mini-puzzle final para activar nodos de vegetación y limpiar el mapa.
* **Efectos Visuales:** Parallax Scrolling en los fondos para dar profundidad.

## 🕹️ Controles

| Acción | Teclado |
| :--- | :--- |
| **Moverse** | Flechas Izquierda/Derecha o `A` / `D` |
| **Saltar** | Barra Espaciadora o Flecha Arriba |
| **Pausa/Menú** | `Esc` |

## 🛠️ Aspectos Técnicos (Unity)

Este proyecto fue desarrollado en **Unity 6** utilizando el flujo de trabajo 2D.

### Scripts Clave
* `PlayerController.cs`: Lógica física del jugador, manejo de inputs y comunicación con Animator.
* `EnemyAI.cs`: Máquina de estados finita (FSM) que controla el comportamiento del Guardia Roca.
* `PuzzleManager.cs`: Sistema de eventos para detectar la activación de los 3 nodos y liberar el fragmento final.
* `ParallaxEffect.cs`: Script para el movimiento relativo de los fondos respecto
