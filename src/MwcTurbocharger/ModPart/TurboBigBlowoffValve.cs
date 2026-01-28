using MwcModApi.Parts;
using MwcTurbocharger.ModPart;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBigBlowoffValve : DerivablePart
	{
		protected override string partId => "turboBig-blowoff-valve";
		protected override string partName => "Racing Turbo Blowoff Valve";
		protected override Vector3 partInstallPosition => new Vector3(0f, 0.086f, 0.154f);
		protected override Vector3 partInstallRotation => new Vector3(0, 90, 0);

		public TurboBigBlowoffValve(TurboBigIntercoolerTube parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.43f);
			AddScrew(new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1f));
		}
	}
}