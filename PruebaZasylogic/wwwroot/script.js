let orden = 'fecha';
let asc = false;

// Evento al enviar formulario
document.getElementById("formulario").addEventListener("submit", async (e) => {
    e.preventDefault();

    const cliente = {
        nombre: document.getElementById("nombre").value,
        apellidos: document.getElementById("apellidos").value,
        celular: document.getElementById("celular").value,
        email: document.getElementById("email").value,
        sexo: document.getElementById("sexo").value
    };

    const motivoId = parseInt(document.getElementById("motivo").value);

    try {
        // Verificar incidencia
        const verificarRes = await fetch("https://localhost:7236/api/atencioncliente/verificar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(cliente.celular)
        });

        const verificacion = await verificarRes.json();
        if (verificacion.length > 0) {
            alert("❌ Ya hay una incidencia registrada hoy para este número.");
            return;
        }

        // Crear cliente
        await fetch("https://localhost:7236/api/cliente", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(cliente)
        });

        // Obtener cliente insertado
        const clientes = await fetch("https://localhost:7236/api/cliente").then(r => r.json());
        const clienteCreado = clientes.find(c => c.celular === cliente.celular);
        if (!clienteCreado) throw new Error("Cliente no encontrado después del insert");

        // Registrar atención
        await fetch("https://localhost:7236/api/atencioncliente/registrar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ clienteId: clienteCreado.id, motivoId })
        });

        alert("✅ Atención registrada correctamente.");
        renderTabla();
    } catch (err) {
        alert("❌ Error: " + err.message);
    }
});

// Filtro y renderizado de la tabla
async function renderTabla() {
    const tbody = document.getElementById("tablaBody");
    const desde = document.getElementById("fechaInicio").value;
    const hasta = document.getElementById("fechaFin").value;

    if (!desde || !hasta) {
        tbody.innerHTML = `<tr><td colspan="3">Seleccione un rango de fechas.</td></tr>`;
        return;
    }

    try {
        const response = await fetch("https://localhost:7236/api/atencioncliente/listado", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ desde, hasta })
        });

        const datos = await response.json();

        const ordenados = datos.sort((a, b) => {
            if (a[orden] < b[orden]) return asc ? -1 : 1;
            if (a[orden] > b[orden]) return asc ? 1 : -1;
            return 0;
        });

        tbody.innerHTML = "";
        ordenados.forEach(d => {
            const tr = document.createElement("tr");
            tr.innerHTML = `<td>${d.nombre}</td><td>${d.motivo}</td><td>${d.fecha}</td>`;
            tbody.appendChild(tr);
        });

        if (ordenados.length === 0) {
            tbody.innerHTML = `<tr><td colspan="3">No hay registros en ese rango.</td></tr>`;
        }
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="3">❌ Error: ${err.message}</td></tr>`;
    }
}

// Cambiar orden al hacer clic en los encabezados
function ordenar(campo) {
    if (orden === campo) asc = !asc;
    else {
        orden = campo;
        asc = true;
    }
    renderTabla();
}

// ✅ Ejecutar cuando cambia alguna fecha
document.getElementById("fechaInicio").addEventListener("change", renderTabla);
document.getElementById("fechaFin").addEventListener("change", renderTabla);

// ✅ Ejecutar automáticamente cada 30 segundos (solo si hay fechas)
setInterval(renderTabla, 30000);

// ✅ Ejecutar una vez al cargar la página si ya hay fechas
window.onload = () => {
    const desde = document.getElementById("fechaInicio").value;
    const hasta = document.getElementById("fechaFin").value;
    if (desde && hasta) renderTabla();
};