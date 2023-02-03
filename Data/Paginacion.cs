using Microsoft.EntityFrameworkCore;

namespace MVCCRUD.Data
{
    public class Paginacion<T> : List<T>
    {
        public int PaginaInicio { get; private set; }
        public int PaginasTotales { get; private set; }

        public Paginacion(List<T> items, int contador, int paginaInicio, int cantidadregistros)
        {
            PaginaInicio = paginaInicio;
            PaginasTotales = (int)Math.Ceiling(contador / (double)cantidadregistros);

            this.AddRange(items);
        }

        public bool PaginasAnteriores => PaginaInicio > 1;
        public bool PaginasPosteriores => PaginaInicio < PaginasTotales;

        public static async Task<Paginacion<T>> CrearPaginacion(IQueryable<T> fuente, int paginaInicio, int cantidadregistros)
        {
            var contador = await fuente.CountAsync();
            var items = await fuente.Skip((paginaInicio - 1) * cantidadregistros).Take(cantidadregistros).ToListAsync();
            return new Paginacion<T>(items, contador, paginaInicio, cantidadregistros);
        }
    }
}