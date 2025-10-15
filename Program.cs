using System;
using System.Collections.Generic;
using System.Globalization;

namespace InventarioElectroPlus
{
    internal class Producto
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }

    internal class Program
    {
        static readonly List<Producto> inventario = new List<Producto>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string opcion;
            do
            {
                MostrarMenu();
                opcion = Console.ReadLine()?.Trim().ToLower() ?? "";

                switch (opcion)
                {
                    case "a":
                        AgregarProducto();
                        break;
                    case "b":
                        ListarProductos();
                        break;
                    case "c":
                        BuscarPorCodigo();
                        break;
                    case "d":
                        MostrarSinStock();
                        break;
                    case "S":
                        Console.WriteLine("Saliendo... ¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Intente de nuevo.");
                        break;
                }

                if (opcion != "S")
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (opcion != "S");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("==== Inventario Rápido - ElectroPlus ====");
            Console.WriteLine("a) Agregar producto");
            Console.WriteLine("b) Lista de productos");
            Console.WriteLine("c) Buscar producto por Código");
            Console.WriteLine("d) Mostrar productos con Cantidad = 0");
            Console.WriteLine("S) Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void AgregarProducto()
        {
            Console.WriteLine("\n-- Agregar producto --");

            
            string codigo;
            while (true)
            {
                Console.Write("Código: ");
                codigo = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    Console.WriteLine("El código no puede estar vacío.");
                    continue;
                }
                if (inventario.Exists(p => string.Equals(p.Codigo, codigo, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Codigo de producto existente. Intente con otro codigo.");
                    continue;
                }
                break;
            }

            
            string nombre;
            while (true)
            {
                Console.Write("Nombre: ");
                nombre = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.WriteLine("El nombre no puede estar vacío.");
                    continue;
                }
                break;
            }

            
            decimal precio = LeerDecimalConTryCatch("Precio (use coma o punto): ");

            
            int cantidad = LeerEnteroConTryCatch("Cantidad: ");

            inventario.Add(new Producto
            {
                Codigo = codigo,
                Nombre = nombre,
                Precio = precio,
                Cantidad = cantidad
            });

            Console.WriteLine("Producto agregado correctamente al inventario.");
        }

        static void ListarProductos()
        {
            Console.WriteLine("\n-- Lista de productos --");
            if (inventario.Count == 0)
            {
                Console.WriteLine("No hay productos cargados en inventario.");
                return;
            }

            Console.WriteLine("{0,-10} | {1,-20} | {2,10} | {3,10}", "Código", "Nombre", "Precio", "Cantidad");
            Console.WriteLine(new string('-', 60));

            
            foreach (var p in inventario)
            {
                Console.WriteLine("{0,-10} | {1,-20} | {2,10:F2} | {3,10}",
                    p.Codigo, p.Nombre, p.Precio, p.Cantidad);
            }
        }

        static void BuscarPorCodigo()
        {
            Console.Write("\nIngrese el código a buscar: ");
            var cod = (Console.ReadLine() ?? "").Trim();

            var prod = inventario.Find(p => string.Equals(p.Codigo, cod, StringComparison.OrdinalIgnoreCase));
            if (prod == null)
            {
                Console.WriteLine("No se encontró un producto con ese código.");
                return;
            }

            Console.WriteLine("Codigo|Nombre|Precio|Cantidad");
            Console.WriteLine($"{prod.Codigo}|{prod.Nombre}|{prod.Precio:F2}|{prod.Cantidad}");
        }

        static void MostrarSinStock()
        {
            Console.WriteLine("\n-- Productos con Cantidad = 0 --");
            var sinStock = inventario.FindAll(p => p.Cantidad == 0);

            if (sinStock.Count == 0)
            {
                Console.WriteLine("No hay productos sin stock.");
                return;
            }

            Console.WriteLine("Codigo|Nombre|Precio|Cantidad");
            foreach (var p in sinStock)
            {
                Console.WriteLine($"{p.Codigo}|{p.Nombre}|{p.Precio:F2}|{p.Cantidad}");
            }
        }

        
        static int LeerEnteroConTryCatch(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var texto = Console.ReadLine();

                try
                {
                    if (texto is null) throw new FormatException();
                    return int.Parse(texto.Trim());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Entrada inválida. Debe ingresar un número entero (ej.: 10).");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Número fuera de rango para 'int'. Intente nuevamente.");
                }
            }
        }

        static decimal LeerDecimalConTryCatch(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var texto = Console.ReadLine();

                try
                {
                    if (texto is null) throw new FormatException();
                    if (decimal.TryParse(texto.Trim(),
                                         NumberStyles.Number,
                                         CultureInfo.CurrentCulture,
                                         out var dec))
                        return dec;

                    if (decimal.TryParse(texto.Trim(),
                                         NumberStyles.Number,
                                         CultureInfo.InvariantCulture,
                                         out dec))
                        return dec;

                    throw new FormatException();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Entrada inválida. Debe ingresar un número decimal (ej.: 10.99 o 10,99).");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Número fuera de rango para 'decimal'. Intente nuevamente.");
                }
            }
        }
    }
}

