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
			AddScrews(new[]
			{
				new Screw(new Vector3(0f, 0.206f, -0.0420f), new Vector3(0, 0, 0)),
				new Screw(new Vector3(-0.042f, 0.164f, -0.0420f), new Vector3(0, 0, 0)),
				new Screw(new Vector3(0f, 0.122f, -0.0420f), new Vector3(0, 0, 0)),
				new Screw(new Vector3(0.041f, 0.164f, -0.0420f), new Vector3(0, 0, 0)),
			}, 0.6f, 8);
		}
	}
}