﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Items.Declarations
{
	public abstract class CommonItemBase : IItem
	{
		public string TextureName { get; protected set; }

		Texture2D _texture;

		public Texture2D Texture
		{
			get{ return _texture; }
			protected set
			{
				if (IsInitialized)
					throw new InvalidOperationException ("Cannot set texture after initialization.");
				_texture = value;
			}
		}

		public Color Color { get; set; }

		public bool IsInitialized { get; private set; }

		protected virtual void LoadContent (ContentManager manager)
		{
			_texture = manager.Load<Texture2D> (TextureName);
		}

		protected virtual void UnloadContent ()
		{
		}

		protected virtual void Initialize ()
		{
			Debug.WriteLineIf (IsInitialized,
				string.Format ("Object {0} initialized multiple times.", this), "Init");
			IsInitialized = true;
		}

		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (Texture, rect, Color);
		}

		#region IComponent implementation

		void Moggle.Controles.IComponent.LoadContent (ContentManager manager)
		{
			LoadContent (manager);
		}

		void Moggle.Controles.IComponent.UnloadContent ()
		{
			UnloadContent ();
		}

		#endregion

		#region IDisposable implementation

		void IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		#endregion

		#region IGameComponent implementation

		void IGameComponent.Initialize ()
		{
			Initialize ();
		}

		#endregion

		#region IItem implementation

		public string Nombre { get; }

		string IItem.DefaultTextureName { get { return TextureName; } }

		Color IItem.DefaultColor { get { return Color; } }

		#endregion

		protected CommonItemBase (string nombre)
		{
			Nombre = nombre;
		}
	}
}