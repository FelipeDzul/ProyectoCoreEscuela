using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;

namespace CoreEscuela.App

{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>>? _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if(dicObsEsc == null)
                throw new ArgumentException(nameof(dicObsEsc));
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluacion> GetListaEvaluaciones()
        {
            if(_diccionario!.TryGetValue(LlaveDiccionario.Escuela, 
                                        out IEnumerable<ObjetoEscuelaBase>? lista))
            {
                return lista.Cast<Evaluacion>();
            }
            {
                return new List<Evaluacion>();
            }
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluacion> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();
            return (from Evaluacion ev in listaEvaluaciones
                   select ev.Asignatura!.Nombre).Distinct(); ;
        }
        
        public Dictionary<string, IEnumerable<Evaluacion>> GetDicEvalXAsig()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluacion>>();
            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalAsig = from eval in listaEval
                               where eval.Asignatura!.Nombre == asig
                               select eval;

                dicRta.Add(asig, evalAsig);
            }
            return dicRta;
        }

        public Dictionary<string, IEnumerable<object>> GetPromeAlumnPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();
            var dicEvaxAsig = GetDicEvalXAsig();

            foreach (var asigConEval in dicEvaxAsig)
            {
                var promsAlumn = from eval in asigConEval.Value
                            group eval by new {
                                eval.Alumno!.UniqueId,
                                eval.Alumno.Nombre
                                }
                            into grupoEvalAlumno
                            select new AlumnoPromedio
                            {
                                alumnoid = grupoEvalAlumno.Key.UniqueId,
                                alumnoNombre = grupoEvalAlumno.Key.Nombre,
                                promedio = grupoEvalAlumno.Average(evaluacion => evaluacion.Nota)
                                /*
                                eval.Alumno!.UniqueId,
                                AlumnoNombre = eval.Alumno.Nombre,
                                NombreEval = eval.Nombre,
                                eval.Nota*/
                            };
                            rta.Add(asigConEval.Key, promsAlumn);
            }
            return rta;
        }
        
         public Dictionary<string, IEnumerable<object>> GetTopPromedioAsignatura(int num, string asignatura)
        {
            var datos = GetPromeAlumnPorAsignatura();
            var dicTop = new Dictionary<string, IEnumerable<object>>();


            foreach (var item in datos)
            {
                var top = (from datatop in item.Value
                            orderby ((AlumnoPromedio)datatop).promedio descending
                            select datatop).Take(num);
                
                dicTop.Add(item.Key, top);
            }
            return dicTop;
        }
    }
}