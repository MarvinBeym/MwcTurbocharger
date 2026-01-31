using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Parts.Game;
using MwcTurbocharger.Turbo;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class BoostGauge : DerivablePart
	{
		protected override string partId => "boost-gauge";
		protected override string partName => "Boost Gauge";
		protected override Vector3 partInstallPosition => new Vector3(0.13f, -0.141f, 0.165f);
		protected override Vector3 partInstallRotation => new Vector3(80, 180, 180);

		protected BoostGaugeLogic logic;

		public BoostGaugeLogic.GaugeMode gaugeMode => logic.gaugeMode;

		protected bool lastElectricityState = false;

		protected TextMesh digitalText;

		public BoostGauge(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddScrew(new Screw(new Vector3(0, -0.0369f, 0), new Vector3(-90, 0, 0), 0.4f, 10, Screw.Type.Normal, Screw.transformStep / 2));

			logic = AddEventBehaviour<BoostGaugeLogic>(PartEvent.Type.InstallOnCar);
			logic.Init(this);

			CarH.electricity.FsmInject("Power", "ON", delegate ()
			{
				if (lastElectricityState != false)
				{
					return;
				}

				lastElectricityState = true;
				logic.SwitchedElectricityOn();
			});
			CarH.electricity.FsmInject("Power", "OFF", delegate ()
			{
				if (!lastElectricityState)
				{
					return;
				}

				lastElectricityState = false;
				logic.SwitchedElectricityOff();
			});

			CloneLcdText();
		}

		private void CloneLcdText()
		{
			GameObject digitalTextObject = this.transform.FindChild("boost-gauge-digital-text").gameObject;

			MeshRenderer meshRenderer = digitalTextObject.GetComponent<MeshRenderer>();
			digitalText = digitalTextObject.GetComponent<TextMesh>();


			GameObject lcd = Cache.Find("CORRIS/AssembliesTuning/VINP_AFRgauge/Functions/LCD");
			MeshRenderer lcdMeshRenderer = lcd.GetComponent<MeshRenderer>();
			TextMesh lcdTextMesh = lcd.GetComponent<TextMesh>();

			//Coping values from af ratio gauge.
			meshRenderer.material = lcdMeshRenderer.material;

			digitalText.transform.localPosition = new Vector3(0f, -0.0135f, 0.0135f);
			digitalText.font = lcdTextMesh.font;
			digitalText.fontSize = 0;
			digitalText.characterSize = 0.0035f;
			digitalText.transform.localScale = lcdTextMesh.transform.localScale;
		}

		public void SetBoost(float boostGaugeTarget, float boostBeforeRelease, TurboConfiguration config)
		{
			logic.SetBoost(boostGaugeTarget, boostBeforeRelease, config);
		}

		public void SetDigitalText(string text)
		{
			digitalText.text = text;
		}

		public void SetDigitalText(float text)
		{
			digitalText.text = text.ToString("0.00");
		}

		public bool error
		{
			get => digitalText.text == "ERR";
			set
			{
				SetDigitalText(value ? "ERR" : "");
			}
		}
	}
}