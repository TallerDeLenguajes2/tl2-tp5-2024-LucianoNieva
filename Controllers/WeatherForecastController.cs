using Microsoft.AspNetCore.Mvc;
using producto;
using presupuestos;
using repoProduct;
using repoPresupuesto;
using presupuestosDetalles;
using System.Collections.Generic;

namespace tl2_tp5_2024_LucianoNieva.Controllers;

// Controlador de Producto
[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{
    private readonly RepoProduct _repoProduct;

    public ProductoController()
    {
        _repoProduct = new RepoProduct();
    }

    // POST /api/Producto - Crear un nuevo producto
    [HttpPost]
    public IActionResult CrearProducto([FromBody] Producto producto)
    {
        _repoProduct.CrearNuevo(producto);
        return CreatedAtAction(nameof(CrearProducto), new { id = producto.IdProducto }, producto);
    }

    // GET /api/Producto - Listar todos los productos
    [HttpGet]
    public ActionResult<List<Producto>> ListarProductos()
    {
        return Ok(_repoProduct.ListarProducto());
    }

    // PUT /api/Producto/{id} - Modificar un producto
    [HttpPut("{id}")]
    public IActionResult ModificarProducto(int id, [FromBody] Producto producto)
    {
        var prodExistente = _repoProduct.ObtenerProductoPorId(id);
        if (prodExistente == null)
        {
            return NotFound();
        }
        prodExistente.Descripcion = producto.Descripcion;
        prodExistente.Precio = producto.Precio;
        _repoProduct.ModificarProducto(id, prodExistente);
        return NoContent();
    }
}

// Controlador de Presupuestos
[ApiController]
[Route("api/[controller]")]
public class PresupuestosController : ControllerBase
{
    private readonly PresupuestosRepository _presupuestosRepo;

    public PresupuestosController()
    {
        _presupuestosRepo = new PresupuestosRepository("Data Source=Tienda.db");
    }

    // POST /api/Presupuesto - Crear un nuevo presupuesto
    [HttpPost]
    public IActionResult CrearPresupuesto([FromBody] Presupuestos presupuesto)
    {
        _presupuestosRepo.CrearNuevo(presupuesto);
        return CreatedAtAction(nameof(CrearPresupuesto), new { id = presupuesto.IdPresupuesto }, presupuesto);
    }

    // POST /api/Presupuesto/{id}/ProductoDetalle - Agregar un producto a un presupuesto
    [HttpPost("{id}/ProductoDetalle")]
    public IActionResult AgregarProductoDetalle(int id, [FromBody] PresupuestosDetalles detalle)
    {
        var presupuesto = _presupuestosRepo.ObtenerPresupuestoPorId(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        _presupuestosRepo.AgregarProductoAPresupuesto(id, detalle.Producto, detalle.Cantidad);
        return NoContent();
    }

    // GET /api/Presupuesto - Listar todos los presupuestos
    [HttpGet]
    public ActionResult<List<Presupuestos>> ListarPresupuestos()
    {
        return Ok(_presupuestosRepo.ListarPresupuestos());
    }

    // GET /api/Presupuesto/{id} - Obtener detalles de un presupuesto por ID
    [HttpGet("{id}")]
    public ActionResult<Presupuestos> ObtenerPresupuestoPorId(int id)
    {
        var presupuesto = _presupuestosRepo.ObtenerPresupuestoPorId(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return Ok(presupuesto);
    }
}


