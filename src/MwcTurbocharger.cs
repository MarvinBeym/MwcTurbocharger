using MSCLoader;
using UnityEngine;

namespace MwcTurbocharger
{
	public class MwcTurbocharger : Mod
	{
		public override string ID => "MwcTurbocharger";
		public override string Name => "Turbocharger";
		public override string Author => "DonnerPlays";
		public override string Version => "1.0";
		public override string Description => "A Turbocharger for the main car (corris)";
		public override Game SupportedGames => Game.MyWinterCar;

		public override void ModSetup()
		{
		}
	}
}
