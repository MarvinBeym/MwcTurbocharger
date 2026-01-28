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
		protected override Vector3 partInstallPosition => new Vector3(0.140818f, -0.00382f, -0.077606f);
		protected override Vector3 partInstallRotation => new Vector3(90f, 0f, 0f);

		public RacingCarbManifold(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.59f);
			AddClampModel(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.59f);
			AddClampModel(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.59f);
			AddClampModel(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.59f);
			AddScrews(
				new[]
				{
					new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Normal),
					new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Normal),
					new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Normal),
					new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Normal),
				}, 1f
			);
		}
	}
}