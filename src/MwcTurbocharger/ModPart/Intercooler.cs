using MwcModApi.Caching;
using MwcModApi.PaintingSystem;
using MwcModApi.Parts;
using MwcModApi.Parts.Game;
using MwcModApi.Tools;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class Intercooler : DerivablePart
	{
		protected override string partId => "intercooler";
		protected override string partName => "Intercooler";
		protected override Vector3 partInstallPosition => new Vector3(0f, 0.213819f, 1.79515f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);

		public Intercooler() : base(CarGamePart.GetInstance(), MwcTurbocharger.partBaseInfo)
		{
			AddScrews(new[]
			{
				new Screw(new Vector3(-0.2210f, 0.081f, 0.0260f), new Vector3(180, 0, 0)),
				new Screw(new Vector3(0.2390f, 0.081f, 0.0260f), new Vector3(180, 0, 0)),
			}, 0.6f, 8);
			
			//PaintingSystem
			//	.Setup(partBaseInfo.mod, this, gameObject.FindChild("intercooler-main"))
			//	.SetMetallic(0.8f, 0.5f);
		}

		
		
	}
}