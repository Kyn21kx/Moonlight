using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Auxiliars {

	public enum RoundingMode {
		UP,
		DOWN,
		NEAREST
	}

	public static class SpartanMath {

		public static void Clamp(ref float n, float min, float max) {
			n = n > max ? max : n;
			n = n < min ? min : n;
		}

		public static float Clamp(float n, float min, float max) {
			n = n > max ? max : n;
			return n < min ? min : n;
		}

		public static Vector3 RemapUpToForward(in Vector3 toRemap, bool preserveZ = false) {
			float zValue = preserveZ ? toRemap.z : 0f;
			return new Vector3(toRemap.x, zValue, toRemap.y);
		}

		public static Vector3 RemapUpToForward(in Vector2 toRemap) => new Vector3(toRemap.x, 0f, toRemap.y);

		public static float Exp(float power) => (float)System.Math.Exp(power);

		public static float Ln(float value) => (float)System.Math.Log(value);

		public static float Log(float value, float n) => Ln(value) / Ln(n);

		public static float Pow(float value, float power) => Exp(power * Ln(value));

		public static float SmoothStart(float from, float to, float t, float n) {
			float powerValue = Pow(t, n);
			return Lerp(from, to, powerValue);
		}
		public static Vector3 SmoothStart(Vector3 from, Vector3 to, float t, float n) {
			float powerValue = Pow(t, n);
			Clamp(ref powerValue, 0f, 1f);
			return new Vector3(
				LerpUnclamped(from.x, to.x, powerValue),
				LerpUnclamped(from.y, to.y, powerValue),
				LerpUnclamped(from.z, to.z, powerValue)
			);
		}

		public static float SmoothStop(float from, float to, float t, float n) {
			float powerValue = 1f - Pow(1f - t, n);
			return Lerp(from, to, powerValue);
		}

		public static Vector3 SmoothStop(Vector3 from, Vector3 to, float t, float n) {
			float powerValue = 1f - Pow(1f - t, n);
			Clamp(ref powerValue, 0f, 1f);
			return new Vector3(
				LerpUnclamped(from.x, to.x, powerValue),
				LerpUnclamped(from.y, to.y, powerValue),
				LerpUnclamped(from.z, to.z, powerValue)
			);
		}

		public static float Lerp(float from, float to, float t) {
			Clamp(ref t, 0f, 1f);
			return from + (to - from) * t;
		}


		public static Vector3 Lerp(Vector3 from, Vector3 to, float t) {
			Clamp(ref t, 0f, 1f);
			return new Vector3(
				LerpUnclamped(from.x, to.x, t),
				LerpUnclamped(from.y, to.y, t),
				LerpUnclamped(from.z, to.z, t)
			);
		}

		public static float LerpUnclamped(float from, float to, float t) {
			return from + (to - from) * t;
		}

		public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float t) {
			return new Vector3(
				LerpUnclamped(from.x, to.x, t),
				LerpUnclamped(from.y, to.y, t),
				LerpUnclamped(from.z, to.z, t)
			);
		}

		public static float InverseLerp(float from, float to, float t) {
			Clamp(ref t, 0f, 1f);
			return (t - from) / to - from;
		}
		
		public static int Sign(float x) => x >= 0f ? 1 : -1;


		public static float DistanceSqr(Vector3 a, Vector3 b) {
			//So, let's substract, right?
			Vector3 towards = a - b;
			return towards.sqrMagnitude;
		}

		public static bool ArrivedAt(Vector3 from, Vector3 to, float threshold = 0.2f) {
			//Get the distance
			float sqrDis = DistanceSqr(from, to);
			if (sqrDis <= threshold * threshold) {
				return true;
			}
			return false;
		}

		public static bool ArrivedAt(float from, float to, float threshold = 0.2f) {
			//Get the distance
			float dis = Mathf.Abs(to - from);
			return dis <= threshold;
		}

		public static bool IsInLayerMask(int value, LayerMask mask) {
			return (mask & 1 << value) == 1 << value;
		}

		/// <summary>
		/// Rounds a float number to the int specified through roundingMode
		/// </summary>
		/// <param name="value">Number to be rounded</param>
		/// <param name="roundingMode">Wether we should round up, down, or to the nearest int</param>
		/// <param name="extraThreshold">A value that either passed or below the 
		/// half point will trigger the rounding, by default 0.01, so that 0.51 or 0.49 triggers the rounding</param>
		/// <returns></returns>
		/// <exception cref="System.Exception"></exception>
		public static int RoundToInt(float value, RoundingMode roundingMode, float extraThreshold = 0.01f) {
			const float HALF = 0.5f;
			int integerPart = (int)value;
			float decimalPart = value - integerPart;
			switch (roundingMode) {
				case RoundingMode.UP:
					if (decimalPart >= HALF + extraThreshold) return integerPart + 1;
					break;
				case RoundingMode.DOWN:
					if (decimalPart <= HALF - extraThreshold) return integerPart - 1;
					break;
				case RoundingMode.NEAREST:
					if (decimalPart >= HALF + extraThreshold) return integerPart + 1;
					if (decimalPart <= HALF - extraThreshold) return integerPart - 1;
					break;
				default:
					throw new System.Exception($"The selected rounding mode {roundingMode}, does not exist!");
			}
			return integerPart;
		}

		public static bool IsEven(int number) => (number & 1) == 0;

	}
}
