using System;
using System.Linq;
using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Parts.Game;
using MwcModApi.Tools;
using MwcTurbocharger.Turbo;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class BoostGauge : DerivablePart
	{
		public enum GaugeMode
		{
			Analog,
			Digital,
		};

		protected override string partId => "boost-gauge";
		protected override string partName => "Boost Gauge";
		protected override Vector3 partInstallPosition => new Vector3(0.13f, -0.141f, 0.165f);
		protected override Vector3 partInstallRotation => new Vector3(80, 180, 180);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public const float minAngle = 45;
		public const float maxAngle = 315;
		public const float analogDigitsBrightnessOff = 0.2f;
		public const float analogDigitsBrightnessOn = 1f;

		private readonly Color[] availableColors = {
			Color.white,
			Color.blue,
			Color.red,
			Color.green,
			Color.yellow,
			Color.magenta,
		};


		protected BoostGaugeLogic logic;

		protected int selectedColor;

		protected TextMesh digitalText;

		private GameObject analogNeedle;
		private Animation analogNeedleAnimation;
		private Material analogDigitsMaterial;

		private Material analogNeedleTipMaterial;
		private Shader analogNeedleTipLightsOffShader;
		private Shader analogNeedleTipLightsOnShader;


		private bool dashLightsOn;

		public GaugeMode gaugeMode { get; protected set; } = GaugeMode.Analog;

		public BoostGauge(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddScrew(new Screw(new Vector3(0, -0.0369f, 0), new Vector3(-90, 0, 0), 0.4f, 6, Screw.Type.Normal, Screw.transformStep / 2));

			logic = AddEventBehaviour<BoostGaugeLogic>(PartEvent.Type.InstallOnCar);
			logic.Init(this);


			analogNeedle = this.transform.FindChild("boost-gauge-needle").gameObject;

			if (analogNeedle == null)
			{
				Logger.Error("Failed to find analog needle mesh on boost gauge");
			}

			analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
			analogNeedleAnimation = analogNeedle.GetComponent<Animation>();

			if (analogNeedleAnimation == null)
			{
				Logger.Error("Failed to find analog needle animation component on analog needle mesh");
			}

			analogDigitsMaterial = GetAnalogDigitsMaterial();
			analogNeedleTipMaterial = GetAnalogNeedleTipMaterial();
			if (analogDigitsMaterial == null) {
				Logger.Error("Failed to find analog digits material");
			}

			analogNeedleTipLightsOnShader = Shader.Find("Unlit/Color");
			analogNeedleTipLightsOffShader = Shader.Find("Standard");

			SetupElectricityDetection();
			SetupDashLightDetection();
			CloneLcdText();
			OnSwitchedElectricityOff();
		}

		private Material GetAnalogDigitsMaterial()
		{
			return transform.FindChild("boost-gauge-main")
				.GetComponent<Renderer>().materials
				.FirstOrDefault(material => material.name.Contains("boost-gauge-foreground"));
		}

		private Material GetAnalogNeedleTipMaterial()
		{
			return analogNeedle
				.GetComponent<Renderer>().materials
				.FirstOrDefault(material => material.name.Contains("RedPlastic-Needle"));
		}

		private void SetupDashLightDetection()
		{
			GameObject electricitySystems = Cache.Find("CORRIS/Simulation/Electricity/PowerON/Systems");

			electricitySystems.FsmInject("LightModes", "Off", OnDashLightsOff);
			electricitySystems.FsmInject("LightModes", "Park", OnDashLightsOn);
			electricitySystems.FsmInject("LightModes", "Driving", OnDashLightsOn);
			electricitySystems.FsmInject("LightModes", "Hi Beam", OnDashLightsOn);

		}

		private void OnDashLightsOn()
		{
			if (analogNeedleTipMaterial.shader == analogNeedleTipLightsOnShader) {
				return;
			}

			dashLightsOn = true;

			analogNeedleTipMaterial.shader = analogNeedleTipLightsOnShader;
			ChangeAnalogColor(selectedColor);
		}

		private void OnDashLightsOff()
		{
			if (analogNeedleTipMaterial.shader == analogNeedleTipLightsOffShader)
			{
				return;
			}
			
			dashLightsOn = false;

			analogNeedleTipMaterial.shader = analogNeedleTipLightsOffShader;
			ChangeAnalogColor(selectedColor);
		}

		private void SetupElectricityDetection()
		{
			CarH.electricity.FsmInject("Power", "ON", delegate ()
			{
				OnSwitchedElectricityOn();
			});

			CarH.electricity.FsmInject("Power", "OFF", delegate ()
			{
				OnSwitchedElectricityOff();
			});
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

		public void SetBoost(float boost)
		{
			if (!CarH.hasPower || analogNeedleAnimation.isPlaying)
			{
				return;
			}

			if (error)
			{
				analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
				SetDigitalText("ERR");
				return;
			}

			switch (gaugeMode)
			{
				case BoostGauge.GaugeMode.Analog:
					analogNeedle.transform.localEulerAngles = new Vector3(0, 0, GetNeedleAngle(boost));
					SetDigitalText("");
					break;
				case BoostGauge.GaugeMode.Digital:
					SetDigitalText(boost);
					analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
					break;
			}
		}

		private void SetDigitalText(string text)
		{
			digitalText.text = text;
		}

		private void SetDigitalText(float text)
		{
			digitalText.text = text.ToString("0.00");
		}

		public void SetGaugeMode(BoostGauge.GaugeMode newGaugeMode)
		{
			gameObject.PlayTouch();
			gaugeMode = newGaugeMode;
			switch (gaugeMode)
			{
				case BoostGauge.GaugeMode.Analog:
					SetDigitalText("");
					break;
				case BoostGauge.GaugeMode.Digital:
					SetDigitalText(0);
					analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
					break;
			}
		}

		private void OnSwitchedElectricityOn()
		{
			if (gaugeMode == GaugeMode.Analog)
			{
				analogNeedleAnimation.Play();
			}
			else
			{
				SetDigitalText(0);
			}

			if (dashLightsOn)
			{
				OnDashLightsOn();
			}

			ChangeAnalogColor(selectedColor);
		}

		private void OnSwitchedElectricityOff()
		{
			if (analogNeedleAnimation.isPlaying)
			{
				analogNeedleAnimation.Stop();
			}

			analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
			SetDigitalText("");
			ChangeAnalogColor(0);
			OnDashLightsOff();
		}

		private float GetNeedleAngle(float valueMap, float minMap = 0f, float maxMap = 3)
		{
			return minAngle + (maxAngle - minAngle) * valueMap.Map(minMap, maxMap, 0, 1);
		}

		public void NextAnalogColor()
		{
			selectedColor += 1;
			ChangeAnalogColor(selectedColor);
		}

		public void PreviousAnalogColor()
		{
			selectedColor -= 1;
			ChangeAnalogColor(selectedColor);
		}

		protected void ChangeAnalogColor(int newColorIndex)
		{
			newColorIndex = newColorIndex > availableColors.Length - 1 ? 0 : newColorIndex;
			newColorIndex = newColorIndex < 0 ? availableColors.Length - 1 : newColorIndex;
			selectedColor = newColorIndex;
			Color color = availableColors[newColorIndex];

			float brightness;

			if (CarH.hasPower) {
				brightness = dashLightsOn ? analogDigitsBrightnessOn : analogDigitsBrightnessOff;
			} else {
				brightness = analogDigitsBrightnessOff;
			}

			color.a = brightness;

			analogDigitsMaterial.SetColor("_Color", color);
		}

		public bool error
		{
			get;
			set;
		}
	}
}