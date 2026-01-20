using HutongGames.PlayMaker;
using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.PaintingSystem;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Parts.Game;
using MwcModApi.Parts.PartBox;
using MwcModApi.Shopping;
using MwcModApi.Tools;
using MwcTurbocharger.Gui;
using MwcTurbocharger.ModPart;
using MwcTurbocharger.Turbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
		public override MSCLoader.Game SupportedGames => MSCLoader.Game.MyWinterCar;

		public const string assetBundleName = "MwcTurbocharger.Assets.turbocharger.unity3d";

		//saveFiles
		private const string boostSaveFile = "boost_saveFile.json";

		public GuiDebug guiDebug;
		public Dictionary<string, float> boostSave;



		//Mod Settings
		public static SettingsCheckBox debugGuiSetting;
		public static SettingsCheckBox rotateTurbineSetting;
		public static SettingsCheckBox backfireEffectSetting;
		public static SettingsSliderInt turboVolumeSetting;
		public static SettingsSliderInt blowoffVolumeSetting;
		public static SettingsSliderInt backfireVolumeSetting;

		internal static PartBaseInfo partBaseInfo;
		internal static List<Part> partsList = new List<Part>();

		public AssetBundle assetsBundle;
		//ECU-Mod Communication
		private bool ecuModInstalled = false;


		public override void ModSetup()
		{
			SetupFunction(Setup.OnNewGame, OnNewGame);
			SetupFunction(Setup.OnLoad, () =>
			{
				ModConsole.Print($"{Name} [v{Version} started loading");
				OnLoad();
				ModConsole.Print($"{Name} [v{Version} finished loading");
			});
			//SetupFunction(Setup.ModSettings, ModSettings);
			//SetupFunction(Setup.Update, Update);
			//SetupFunction(Setup.OnSave, OnSave);
			//SetupFunction(Setup.OnGUI, OnGUI);
		}

		public void OnNewGame()
		{
			MwcModApi.MwcModApi.NewGameCleanUp(this);
			TurboPart.Save(this, boostSaveFile, new TurboPart[0]);
		}

		public void OnLoad()
		{
			
			Logger.InitLogger(this);

			ecuModInstalled = ModLoader.IsModPresent("DonnerTech_ECU_Mod");


			guiDebug = new GuiDebug(Screen.width - 310, 50, 300, "TURBO MOD DEBUG", new[]
			{
				new GuiDebugElement("DEBUG")
			});

			assetsBundle = Helper.LoadAssetBundle(this, "turbocharger.unity3d");
			TurboPart.LoadAssets(assetsBundle);
			partBaseInfo = new PartBaseInfo(this, assetsBundle, partsList);

			GamePart weberCarb = new GamePart("VINP_Carburettor", "4 Barrell Racing Carb(VINXX)");
			assetsBundle.Unload(false);
		}
	}
}
