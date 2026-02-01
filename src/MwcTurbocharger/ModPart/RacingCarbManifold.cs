using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Parts.Game;
using MwcModApi.Tools;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class RacingCarbManifold : DerivablePart
	{
		protected override string partName => "Racing Carb Manifold";
		protected override string partId => "racingCarb-manifold";
		protected override Vector3 partInstallPosition => new Vector3(0.138818f, -0.00382f, -0.078606f);
		protected override Vector3 partInstallRotation => new Vector3(90f, 0f, 0f);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public RacingCarbManifold(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(new Vector3(-0.025f, 0.06375f, -0.13975f), new Vector3(1, 90, 0), 0.6f, 10);
			AddClampModel(new Vector3(-0.025f, 0.06775f, -0.05475f), new Vector3(1, 90, 0), 0.6f, 10);
			AddClampModel(new Vector3(-0.025f, 0.07375f, 0.05525f), new Vector3(1, 90, 0), 0.6f, 10);
			AddClampModel(new Vector3(-0.025f, 0.07775f, 0.13925f), new Vector3(1, 90, 0), 0.6f, 10);
		}
	}
}