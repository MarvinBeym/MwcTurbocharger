using MwcModApi.Parts;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class IntercoolerRacingCarbManifoldTube : DerivablePart
	{
		protected override string partId => "intercooler-manifold-racingCarb-tube";
		protected override string partName => "Intercooler Racing Carb Manifold Tube";
		protected override Vector3 partInstallPosition => new Vector3(0.07608899f, 0.037259f, -0.29815f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);

		public IntercoolerRacingCarbManifoldTube(RacingCarbManifold parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(
				new Vector3(0, 0, 0),
				new Vector3(0, 0, 0), 0.65f
			);
			AddClampModel(
				new Vector3(0, 0, 0),
				new Vector3(0, 0, 0), 0.65f
			);
			AddScrews(
				new[]
				{
					new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0)),
					new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0)),
				}, 0.4f, 8
			);
		}
	}
}