using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace FED
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Método 1: mayor_ganancia: ");
            Console.WriteLine("====================================================================================");
            Dictionary<int, Libro> diccionarioLibros = obtenerListaLibros();
            Libro libroMayorGanacia= mayor_ganancia(diccionarioLibros);
            Console.WriteLine("Producto Ganador: ");
            Console.WriteLine(getStringObject(libroMayorGanacia));
            Console.WriteLine("====================================================================================");

            Console.WriteLine("Método 2: hacer_pedido: ");
            Console.WriteLine("====================================================================================");

            Console.WriteLine("Digite el nombre del libro: ");
            var nombreLibro = Console.ReadLine();

            bool pedidoLibro= hacer_pedido(nombreLibro,diccionarioLibros);
            Console.WriteLine("PEDIDO DEL PRODUCTO: "+nombreLibro);
            Console.WriteLine(pedidoLibro == true ? "RESULTADO: Verdadero" : "RESULTADO: Falso" );

            Console.WriteLine("====================================================================================");


            Console.WriteLine("Método 3: publicacion_antes_anio ");
            Console.WriteLine("====================================================================================");
            
            Console.WriteLine("Digite el año: ");
            var año = int.Parse(Console.ReadLine());
            Console.WriteLine("Lista de libros publicados antes del año " + año);
            Dictionary<string, int> librosEnontrados= publicacion_antes_anio(año,diccionarioLibros);
            foreach (var item in librosEnontrados)
            {
                Console.WriteLine(String.Format("LLAVE: {0} , VALOR: {1}", item.Key, item.Value));
            }

            Console.WriteLine("====================================================================================");


            Console.WriteLine("Método 4: ganancias_venta_libro  ");
            Console.WriteLine("====================================================================================");

            Console.WriteLine("Digite el nombre libro: ");
            var nombreLibroVenta = Console.ReadLine();
            Console.WriteLine("Digite unidades a vender ");
            var unidadesVenta= int.Parse(Console.ReadLine());

            Console.WriteLine("LIBRO: "+nombreLibroVenta);
            Console.WriteLine("UNIDADES VENTA: "+unidadesVenta);

            Dictionary<string, double> ventaLibro= ganancias_venta_libro(diccionarioLibros, nombreLibroVenta, unidadesVenta);
            foreach (var item in ventaLibro)
            {
                Console.WriteLine(String.Format("LIBRO: {0} , GANACIA: {1}", item.Key, item.Value.ToString("N0", CultureInfo.CurrentCulture) ));
            }

            Console.WriteLine("====================================================================================");



            Console.WriteLine("Método 5: venta_por_mayor  ");
            Console.WriteLine("====================================================================================");

            Console.WriteLine("Digite el nombre libro: ");
            var nombreLibroVenta2 = Console.ReadLine();
            Console.WriteLine("Digite unidades a vender ");
            var unidadesVenta2= int.Parse(Console.ReadLine());


            Dictionary<string, VentaLibro> ventaLibroMayor= venta_por_mayor(diccionarioLibros, nombreLibroVenta2, unidadesVenta2);
            foreach (var item in ventaLibroMayor)
            {
                Console.WriteLine("VENTA REALIZADA");
                Console.WriteLine(String.Format("LIBRO: {0}", item.Key));
                Console.WriteLine(String.Format("TOTAL: {0}", item.Value.costoTotal.ToString("N0", CultureInfo.CurrentCulture) ));
                Console.WriteLine(String.Format("DESCUENTO: {0}",  item.Value.descuentoTotal.ToString("N0", CultureInfo.CurrentCulture) ));
                Console.WriteLine(String.Format("PORCENTAJE: {0}%", item.Value.porcentajeDescuento));

            }

            Console.WriteLine("====================================================================================");
        }
        

        private static Libro mayor_ganancia(Dictionary<int, Libro> diccionario)
        {
            Libro libroGanador = null;
            double ganancia = 0;
            foreach (var item in diccionario.Values.ToList())
            {
                var diferencia = item.precio - item.costoProduccion;
                if(diferencia > ganancia){
                    ganancia = diferencia;
                    libroGanador = item; 
                }
                Console.WriteLine(String.Format("Ganancia {0}: {1}", item.nombre, diferencia));
            }
            return libroGanador;
        }

        private static bool hacer_pedido(string nombreLibro, Dictionary<int, Libro> diccionario){
            Libro buscarLibro= diccionario.Values.Where(a=>a.nombre.ToLower() == nombreLibro.ToLower()).FirstOrDefault();
            if(buscarLibro !=null){
                if(buscarLibro.cantidad <= 50){
                    return true;
                }
            }else{
                Console.WriteLine(String.Format("No se encontro el libro con nombre:  {0}", nombreLibro));
            }
            
            return false;
        }

        private static Dictionary<string, int> publicacion_antes_anio(int anio, Dictionary<int, Libro> diccionario){
            var libros = new Dictionary<string, int>();

            var listaLibrosAntes = diccionario.Values.Where(a=>a.añoPublicacion < anio).ToList();
            foreach (var item in listaLibrosAntes)
            {
                libros.Add(item.nombre, item.añoPublicacion);
                
            }
            return libros;
        }

        private static Dictionary<string, double> ganancias_venta_libro(Dictionary<int, Libro> diccionario,string nombreLibro, int unidadesVenta ){
            var resultado = new Dictionary<string, double>();
            Libro buscarLibro= diccionario.Values.Where(a=>a.nombre.ToLower() == nombreLibro.ToLower()).FirstOrDefault();
             if(buscarLibro !=null){
                if(buscarLibro.cantidad >= unidadesVenta) {
                    var ganaciaPorLibro = buscarLibro.precio - buscarLibro.costoProduccion; // halla ganacia de un libro
                    var gananciaTotal = ganaciaPorLibro * unidadesVenta; // se multiplica la ganancia de cada  libro por la cantidad de unidades a vender
                    Console.WriteLine("VENTA REALIZADA" );
                    resultado.Add(buscarLibro.nombre, gananciaTotal);
                }else {
                    Console.WriteLine(String.Format("La cantidad supera el numero de unidades : " + buscarLibro.nombre));
                    Console.WriteLine(String.Format("Cantidad Actual : " + buscarLibro.cantidad));
                    Console.WriteLine(String.Format("Cantidad Solicitada: " + unidadesVenta));
                }
            }else{
                Console.WriteLine(String.Format("No se encontro el libro con nombre:  {0}", nombreLibro));
            }
            return resultado;
        }

        private static Dictionary<string, VentaLibro>  venta_por_mayor(Dictionary<int, Libro> diccionario,string nombreLibro, int unidadesVenta){
            var resultado = new Dictionary<string, VentaLibro>();
            Libro buscarLibro= diccionario.Values.Where(a=>a.nombre.ToLower() == nombreLibro.ToLower()).FirstOrDefault();
             if(buscarLibro !=null){
                if(buscarLibro.cantidad >= unidadesVenta) {
                    var ganaciaPorLibro = buscarLibro.precio - buscarLibro.costoProduccion; // halla ganacia de un libro
                    var gananciaTotal = ganaciaPorLibro * unidadesVenta; // se multiplica la ganancia de cada  libro por la cantidad de unidades a vender

                    var porcentajeCantidad= (unidadesVenta*buscarLibro.cantidad)/ 100;
                    var descuento = 0;
                    var porcentajeDescuento= 0;
                    // 10% de descuento
                    if(porcentajeCantidad> 25 && porcentajeCantidad <= 75){
                        descuento = ((int)gananciaTotal * 10 ) / 100;
                        porcentajeDescuento= 10;
                    }
                    //  20% de descuento si compra más del 50% de la cantidad, pero menos del 75% 
                    else if(porcentajeCantidad> 50 && porcentajeCantidad< 50){
                        descuento = ((int)gananciaTotal * 20 ) / 100;
                        porcentajeDescuento = 20;
                    }
                    //  recibe el 30% de descuento si compra más del 75%
                    else{
                        descuento = ((int)gananciaTotal * 30 ) / 100;
                        porcentajeDescuento = 30;
                    }
                
                    resultado.Add(buscarLibro.nombre, new VentaLibro{
                        costoTotal = gananciaTotal,
                        descuentoTotal = descuento,
                        porcentajeDescuento = porcentajeDescuento
                    }
                    );
                }else {
                    Console.WriteLine(String.Format("La cantidad supera el numero de unidades : " + buscarLibro.nombre));
                    Console.WriteLine(String.Format("Cantidad Actual : " + buscarLibro.cantidad));
                    Console.WriteLine(String.Format("Cantidad Solicitada: " + unidadesVenta));
                }
            }else{
                Console.WriteLine(String.Format("No se encontro el libro con nombre:  {0}", nombreLibro));
            }


            return resultado;
        }

        
        private static Dictionary<int, Libro> obtenerListaLibros()
        {
            var libros = new Dictionary<int, Libro>()
            {
                { 1, new Libro { nombre="Harry Potter y la piedra Filosofal", codigo="HPJK1997", autor="J.K.Rowling", añoPublicacion= 1997, cantidad= 200, precio=25000, costoProduccion= 9000} },
                { 2, new Libro { nombre="Los Juegos del Hambre", codigo="JHSC2008", autor="SuzanneCollins", añoPublicacion= 2008, cantidad= 1000, precio=27000, costoProduccion= 12000} },
                { 3, new Libro { nombre="El Hobbit", codigo="EHJR1937", autor="J.R.R. Tolkien", añoPublicacion= 1937, cantidad= 50, precio=35000, costoProduccion= 15000} },
                { 4, new Libro { nombre="Hamlet", codigo="HWS1589", autor="William Shakespeare", añoPublicacion= 1589, cantidad= 20, precio=26000, costoProduccion= 13000} },
            };
            return libros;
        }
 
        private static string getStringObject(Libro obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json;
        }
    }
}
