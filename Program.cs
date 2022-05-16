using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using CoreEscuela.App;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;

            var engine = new EscuelaEngine();
            
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
            //Printer.Beep(10000, cantidad:10);
            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
                WriteLine("1-Listar Evaluaciones \n2-Listar asignaturas \n3-Lista de Diccionario de Evaluaciones por asignatura \n4-Listar promedio \n");
                WriteLine("Ingrese una opción");
                int opcion = Convert.ToInt32(Console.ReadLine());
                
                //opcion = Convert.ToInt32(Console.ReadLine());
                switch(opcion){
                    case 1:
                    Printer.WriteTitle("Lista de evaluaciones");
                    reporteador.GetListaEvaluaciones();
                    break;
                    case 2:
                    Console.WriteLine("Lista de asignaturas");
                    reporteador.GetListaAsignaturas();
                    break;
                    case 3:
                    Console.WriteLine("Lista de Diccionario de Evaluaciones por asignatura");
                    reporteador.GetDicEvalXAsig();
                    break;
                    case 4:
                    Console.WriteLine("Lista de promedios");
                    reporteador.GetPromeAlumnPorAsignatura(); 
                    break;
                    default:
                    Console.WriteLine("Ingrese una opción correcta");
                    break;
                }
            /*
            reporteador.GetListaEvaluaciones();
            reporteador.GetListaAsignaturas();
            var listaEvalXAsifg = reporteador.GetDicEvalXAsig();
            var listaPromXAsig = reporteador.GetPromeAlumnPorAsignatura();
            */
            
            Printer.WriteTitle("Captura de una Evaluación por Consola");
            var newEval = new Evaluacion();
            string nombre, notastring;
            //float nota;
            
            WriteLine("Ingrese el nombre de la evaluación");
            Printer.PresioneENTER();
            nombre = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Printer.WriteTitle("El valor del nombre no puede ser vacio");
                WriteLine("Saliendo del programa");
            }
            else
            {
                newEval.Nombre = nombre.ToLower();
                WriteLine("El nombre de la evaluacion ha sido ingresado correctamente");
            }


            WriteLine("Ingrese la nota de la evaluación");
            Printer.PresioneENTER();
            notastring = Console.ReadLine()!;


            try
            {
                newEval.Nota = float.Parse(notastring!);
                if (newEval.Nota < 0 || newEval.Nota > 5)
                {
                    throw new ArgumentOutOfRangeException("La nota debe estar entre 0 y 5");
                }
                WriteLine("La nota de la evaluacion ha sido ingresada correctamente");
                return;
            }
            catch (ArgumentOutOfRangeException arge)
            {
                Printer.WriteTitle(arge.Message);
                WriteLine("Saliendo del programa");
            }
            finally
            {
                Printer.WriteTitle("FINALLY");
                Printer.Beep(2500, 500, 3);
            }
            // catch(Exception)
            // {
            //     Printer.WriteTitle("El valor de la nota no es un número válido");
            //     WriteLine("Saliendo del programa");
        }       

            //ImpimirCursosEscuela(engine!.Escuela!);
            //var dictmp = engine.GetDiccionarioObjetos();

            //engine.imprimirDiccionario(dictmp);

            /*int dummy = 0;
            var listaObjetos = engine.GetObjetosEscuela(
                out int conteoEvaluaciones,
                out int conteoCursos,
                out int conteoAsignaturas,
                out int conteoAlumnos
            );*/
            /*
            Dictionary<int , string> diccionario = new Dictionary<int, string>();

            diccionario.Add(10, "Juank");
            diccionario.Add(23, "Loren Ipsum");

            foreach (var keyValPair in diccionario)
            {
                WriteLine($"Key: {keyValPair.Key} Valor: {keyValPair.Value}");
            }

            Printer.WriteTitle("Acceso a diccionario");
            diccionario[0] = "Pekerman";
            WriteLine(diccionario[0]);

            Printer.WriteTitle("Otro diccionario.");
            var dic = new Dictionary<string, string>();
            dic["Luna"] = "Cuerpo celeste que gira al rededor del planeta tierra.";
            WriteLine($"{"Luna: "}{dic["Luna"]}");
            //La llave en los diccionarios nunca se repiten, se puede cambiar los valores de las llaves.
            */



            /*var listaLugar = from obj in listaObjetos
                             where obj is Alumno
                             select (Alumno) obj;*/

            //engine.Escuela!.LimpiarLugar();
        

        private static void AccionDelEvento(object? sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO...");
            Printer.Beep(3000,  1000, 1);
            Printer.WriteTitle("SALIO.");
        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {

            Printer.WriteTitle("Cursos de la Escuela");


            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre}, Id  {curso.UniqueId}");
                }
            }
        }
    }
}
