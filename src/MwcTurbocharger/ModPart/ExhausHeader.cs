using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Parts.Game;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class ExhaustHeader : DerivablePart
	{
		protected override string partId => "exhaust-header";
		protected override string partName => "Turbo Exhaust Header";
		protected override Vector3 partInstallPosition => new Vector3(-0.089f, 0.07182f, -0.0557f);
		protected override Vector3 partInstallRotation => new Vector3(90, 0, 0);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public ExhaustHeader(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddScrews(
				new[]
				{
					new Screw(new Vector3(-0.0088f, 0.035f, 0.217f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, 0.035f, 0.116f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, 0.035f, 0.014f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, 0.035f, -0.0865f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, -0.031f, 0.181f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, -0.031f, 0.0785f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, -0.031f, -0.023f), new Vector3(0, -90, 0), Screw.Type.Nut),
					new Screw(new Vector3(-0.0088f, -0.031f, -0.123f), new Vector3(0, -90, 0), Screw.Type.Nut),
				}, 0.8f, 9
			);
		}
	}
}