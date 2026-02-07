using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Tools;
using System;
using MwcTurbocharger.ModPart;
using MwcTurbocharger.Turbo;
using UnityEngine;

namespace MwcTurbocharger
{
	public class BoostGaugeLogic : MonoBehaviour
	{
		private BoostGauge part;
		private GameObject analogDigitalSwitch;


		public void Init(BoostGauge part)
		{
			this.part = part;

			analogDigitalSwitch = transform.FindChild("boost-gauge-button").gameObject;
		}

		void Update()
		{
			if (!CarH.hasPower || !analogDigitalSwitch.IsLookingAt()) {
				return;
			}

			BoostGauge.GaugeMode nextGaugeMode = part.gaugeMode == BoostGauge.GaugeMode.Analog 
				? BoostGauge.GaugeMode.Digital 
				: BoostGauge.GaugeMode.Analog;

			UserInteraction.GuiInteraction(
				string.Format(
					$"[Left mouse] or [{cInput.GetText("Use")}] to switch to {nextGaugeMode}\n" +
					$"[SCROLL UP/Down] to change color"
				)
			);

			if (UserInteraction.MouseScrollWheel.Up) {
				part.NextAnalogColor();
			}

			if (UserInteraction.MouseScrollWheel.Down) {
				part.PreviousAnalogColor();
			}

			if (UserInteraction.UseButtonDown || UserInteraction.LeftMouseDown) {
				part.SetGaugeMode(nextGaugeMode);
			}
		}
	}
}