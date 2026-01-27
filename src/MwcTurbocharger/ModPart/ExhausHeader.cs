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
		protected override Vector3 partInstallPosition => new Vector3(-0.08804f, 0.07182f, -0.056702f);
		protected override Vector3 partInstallRotation => new Vector3(90, 0, 0);

		public ExhaustHeader(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{ 
			AddScrews(new[]
			{
				new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Nut),
				new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Nut),
				new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Nut),
				new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Nut),
				new Screw(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Screw.Type.Nut),
			}, 0.7f, 8);
		}
	}
}