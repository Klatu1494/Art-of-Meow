using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cells.Collision;
using Moggle.Controles;

namespace Cells.CellObjects
{
	/// <summary>
	/// Representa un muro de Grid
	/// </summary>
	public class GridWall : ICollidableGridObject
	{
		/// <summary>
		/// Devuelve el Grid
		/// </summary>
		/// <value>The grid.</value>
		public Grid Grid { get; }

		/// <summary>
		/// Nombre de la textura
		/// </summary>
		public readonly string StringTexture;

		/// <summary>
		/// La profundidad de dibujo.
		/// </summary>
		public float Depth { get { return Depths.Foreground; } }

		/// <summary>
		/// La textura de dibujo
		/// </summary>
		public Texture2D Texture { get; private set; }

		System.Collections.Generic.IEnumerable<ICollisionRule> ICollidableGridObject.GetCollisionRules ()
		{
			yield return new DescriptCollitionRule (z => true);
		}

		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (Texture, destinationRectangle: rect, layerDepth: Depth);
		}

		void IComponent.AddContent (Moggle.BibliotecaContenido manager)
		{
			manager.AddContent (StringTexture);
		}

		void IComponent.InitializeContent (Moggle.BibliotecaContenido manager)
		{
			Texture = manager.GetContent<Texture2D> (StringTexture);
		}

		void IDisposable.Dispose ()
		{
		}

		void IGameComponent.Initialize ()
		{
		}

		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		/// <value>The location.</value>
		public Point Location { get; set; }

		IComponentContainerComponent<IControl> IControl.Container
		{ get { return Grid as IComponentContainerComponent<IControl>; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.CellObjects.GridWall"/> class.
		/// </summary>
		/// <param name="stringTexture">Name of the texture</param>
		/// <param name="grid">Grid.</param>
		public GridWall (string stringTexture, Grid grid)
		{
			StringTexture = stringTexture;
			Grid = grid;
		}
	}
}