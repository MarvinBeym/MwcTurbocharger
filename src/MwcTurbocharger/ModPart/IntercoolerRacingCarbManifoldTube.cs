using MwcModApi.Parts;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class IntercoolerRacingCarbManifoldTube : DerivablePart
	{
		protected override string partId => "intercooler-manifold-racingCarb-tube";
		protected override string partName => "Intercooler Racing Carb Manifold Tube";
		protected override Vector3 partInstallPosition => new Vector3(0.08508901f, 0.040259f, -0.29815f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public IntercoolerRacingCarbManifoldTube(RacingCarbManifold parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(
				new Vector3(-0.121f, -0.035f, -0.209f),
				new Vector3(0, 90, 0), 
				0.51f,
				10
			);
			AddClampModel(
				new Vector3(-0.04f, 0.002f, 0.206f),
				new Vector3(-45, 90, 0),
				0.51f,
				10
			);
		}
	}
}