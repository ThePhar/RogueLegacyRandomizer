using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Randomchaos2DGodRays
{
	public class CrepuscularRays : BasePostProcessingEffect
	{
		public LightSourceMask lsMask;
		public LightRay rays;
		public Vector2 lightSource
		{
			get
			{
				return this.rays.lighScreenSourcePos;
			}
			set
			{
				this.lsMask.lighScreenSourcePos = value;
				this.rays.lighScreenSourcePos = value;
			}
		}
		public float X
		{
			get
			{
				return this.rays.lighScreenSourcePos.X;
			}
			set
			{
				this.lsMask.lighScreenSourcePos.X = value;
				this.rays.lighScreenSourcePos.X = value;
			}
		}
		public float Y
		{
			get
			{
				return this.rays.lighScreenSourcePos.Y;
			}
			set
			{
				this.lsMask.lighScreenSourcePos.Y = value;
				this.rays.lighScreenSourcePos.Y = value;
			}
		}
		public Texture lightTexture
		{
			get
			{
				return this.lsMask.lishsourceTexture;
			}
			set
			{
				this.lsMask.lishsourceTexture = value;
			}
		}
		public float LightSourceSize
		{
			get
			{
				return this.lsMask.lightSize;
			}
			set
			{
				this.lsMask.lightSize = value;
			}
		}
		public float Density
		{
			get
			{
				return this.rays.Density;
			}
			set
			{
				this.rays.Density = value;
			}
		}
		public float Decay
		{
			get
			{
				return this.rays.Decay;
			}
			set
			{
				this.rays.Decay = value;
			}
		}
		public float Weight
		{
			get
			{
				return this.rays.Weight;
			}
			set
			{
				this.rays.Weight = value;
			}
		}
		public float Exposure
		{
			get
			{
				return this.rays.Exposure;
			}
			set
			{
				this.rays.Exposure = value;
			}
		}
		public CrepuscularRays(Game game, Vector2 lightScreenSourcePos, string lightSourceImage, float lightSourceSize, float density, float decay, float weight, float exposure) : base(game)
		{
			this.lsMask = new LightSourceMask(game, lightScreenSourcePos, lightSourceImage, lightSourceSize);
			this.rays = new LightRay(game, lightScreenSourcePos, density, decay, weight, exposure);
			base.AddPostProcess(this.lsMask);
			base.AddPostProcess(this.rays);
		}
	}
}
