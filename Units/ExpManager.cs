﻿using System;
using System.Collections.Generic;
using Units.Recursos;
using System.Linq;
using System.Diagnostics;

namespace Units
{
	/// <summary>
	/// Manejador de experiencia
	/// </summary>
	public class ExpManager
	{
		readonly Dictionary<IParámetroRecurso, float> _distribuciónExp;

		/// <summary>
		/// Devuelve la <see cref="Unidad"/> cuya experiencia está manejada por esta clase
		/// </summary>
		public IUnidad Unidad { get; }

		float _expAcum;

		/// <summary>
		/// Devuelve o establece la experiencia acumulada desde el último <see cref="Flush"/>
		/// </summary>
		public float ExperienciaAcumulada
		{
			get
			{
				return _expAcum;
			}
			set
			{
				
				_expAcum = value;
				if (Autoflush)
					Flush ();
			}
		}

		void _normalizeDistDict ()
		{
			var suma = 0f;
			foreach (var x in _distribuciónExp.Values)
				suma += x;

			if (suma == 0)
				throw new Exception ("Cannot normalize vector zero.");

			foreach (var x in _distribuciónExp.Keys.ToArray ())
				_distribuciónExp [x] /= suma;
		}

		/// <summary>
		/// Determina si se debe invocar <see cref="Flush"/> cada vez que exista experiencia por asignar
		/// </summary>
		public bool Autoflush;

		/// <summary>
		/// Recibe la experiencia acumulada, según la asignación
		/// </summary>
		public void Flush ()
		{
			Debug.WriteLine (
				string.Format (
					"{0} está recibiendo {1} puntos de experiencia de este nivel.",
					Unidad,
					ExperienciaAcumulada));
			if (ExperienciaAcumulada == 0)
				return;
			_normalizeDistDict ();
			foreach (var x in _distribuciónExp)
				x.Key.ReceiveExperience (ExperienciaAcumulada * x.Value);
			_distribuciónExp.Clear ();
			ExperienciaAcumulada = 0;
		}

		/// <summary>
		/// Asigna proporción de experiencia a un parámetro
		/// </summary>
		/// <param name="par">Parámetro de asignación</param>
		/// <param name="cant">Peso de la asignación</param>
		public void AddAssignation (IParámetroRecurso par, float cant)
		{
			if (_distribuciónExp.ContainsKey (par))
				_distribuciónExp [par] += cant;
			else
				_distribuciónExp.Add (par, cant);
		}

		/// <summary>
		/// Asigna proporción de experiencia a todos los parámetros
		/// </summary>
		/// <param name="rec">Recurso de asignación</param>
		/// <param name="cant">Peso de la asignación</param>
		public void AddAssignation (IRecurso rec, float cant)
		{
			foreach (var x in rec.EnumerateParameters ())
				AddAssignation (x, cant);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.ExpManager"/> class.
		/// </summary>
		/// <param name="unid">Unidad</param>
		public ExpManager (IUnidad unid)
		{
			Unidad = unid;
			_distribuciónExp = new Dictionary<IParámetroRecurso, float> ();
		}
	}
}