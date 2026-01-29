using MwcModApi.Caching;
using MwcModApi.Parts;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBigExhaustOutletTube : DerivablePart
	{
		protected override string partId => "turboBig-exhaust-outlet-tube";
		protected override string partName => "Racing Turbo Exhaust Outlet Tube";
		protected override Vector3 partInstallPosition => new Vector3(-0.000678f, -0.165018f, 0.25361f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);

		public TurboBigExhaustOutletTube(TurboBig parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(new Vector3(0, -0.089f, 0.6435f), new Vector3(3, 180, -90), 0.725f, 10);
			AddScrews(
				new[]
				{
					new Screw(new Vector3(0, 0.207f, -0.131f), new Vector3(0, 0, 0), Screw.Type.Normal),
					new Screw(new Vector3(0.041f, 0.166f, -0.131f), new Vector3(0, 0, 0), Screw.Type.Normal),
					new Screw(new Vector3(0, 0.124f, -0.131f), new Vector3(0, 0, 0), Screw.Type.Normal),
					new Screw(new Vector3(-0.041f, 0.166f, -0.131f), new Vector3(0, 0, 0), Screw.Type.Normal),
				}, 0.8f, 10
			);
		}
	}
}