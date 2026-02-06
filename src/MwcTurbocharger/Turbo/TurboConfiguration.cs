using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MwcTurbocharger.Turbo
{
	public class TurboConfiguration
	{
		/// <summary>
		/// The base boost conditions are either added or substracted from that resulting in the calculated max boost.
		/// </summary>
		public float boostBase;

		/// <summary>
		/// 
		/// </summary>
		public float boostOffset;

		/// <summary>
		/// The rpm at which boost should start to be generated.
		/// </summary>
		public float boostStartingRpm;

		/// <summary>
		/// When increasing/decreasing userSetBoost, this value is used to add or substract
		/// </summary>
		public float boostSettingSteps;

		/// <summary>
		/// The minimum possible boost that the user should be able to define when setting the boost on the blowoff valve
		/// </summary>
		public float minSettableBoost;

		/// <summary>
		/// Used for calculation of the boost. Defines how steep the graph rises".
		/// </summary>
		public float boostSteepness;

		/// <summary>
		/// Added to the boost starting rpm to get the starting point of the graph (~zero y position) closer to the starting rpm
		/// </summary>
		public float boostStartingRpmOffset;

		/// <summary>
		/// How long to wait after a blowoff has happened until new boost can be produced.
		/// </summary>
		public float blowoffDelay;

		/// <summary>
		/// Above how much boost the blowoff can happen.
		/// </summary>
		public float blowoffTriggerBoost;

		/// <summary>
		/// This defines at which rpm it is possible for a backfire to happen / no longer happen
		/// </summary>
		public float backfireThreshold;

		/// <summary>
		/// This is the number used to find if a backfire should happen (ex. 20 would mean if the random value between 0 and 20 is == 1) -> backfire
		/// </summary>
		public int backfireRandomRange;

		/// <summary>
		/// Multiplier used for calculating the turbo rpm.
		/// </summary>
		public float rpmMultiplier;

		/// <summary>
		/// By how much to multiply the boost that is later applied to the engines power multiplier.
		/// </summary>
		public float extraPowerMultiplicator;

		public float soundboostMinVolume;
		public float soundboostMaxVolume;
		public float soundboostPitchMultiplicator;
		public float backfireDelay;
	}
}